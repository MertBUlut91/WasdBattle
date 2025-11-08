using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.CloudSave;
using UnityEngine;
using WasdBattle.Data;

namespace WasdBattle.Network
{
    /// <summary>
    /// Unity Cloud Save ile veri persistance
    /// </summary>
    public class UnityCloudSaveService : IFirebaseService
    {
        public bool IsInitialized { get; private set; }
        public bool IsAuthenticated { get; private set; }
        public string CurrentUserId { get; private set; }
        
        public async Task<bool> InitializeAsync()
        {
            try
            {
                // Unity Services zaten GameManager'da başlatılıyor
                IsInitialized = true;
                Debug.Log("[CloudSave] Initialized");
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[CloudSave] Init failed: {e.Message}");
                return false;
            }
        }
        
        public async Task<bool> SignInAnonymouslyAsync()
        {
            try
            {
                // Unity Authentication zaten yapılmış olmalı
                CurrentUserId = Unity.Services.Authentication.AuthenticationService.Instance.PlayerId;
                IsAuthenticated = true;
                Debug.Log($"[CloudSave] Signed in: {CurrentUserId}");
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[CloudSave] Sign in failed: {e.Message}");
                return false;
            }
        }
        
        public async Task<bool> SignInWithEmailAsync(string email, string password)
        {
            // Unity Cloud Save email/password desteklemiyor
            // Bunun yerine Unity Authentication kullanın
            Debug.LogWarning("[CloudSave] Email sign-in not supported, using anonymous");
            return await SignInAnonymouslyAsync();
        }
        
        public async Task<bool> SignUpWithEmailAsync(string email, string password)
        {
            // Unity Cloud Save email/password desteklemiyor
            Debug.LogWarning("[CloudSave] Email sign-up not supported, using anonymous");
            return await SignInAnonymouslyAsync();
        }
        
        public async Task SignOutAsync()
        {
            IsAuthenticated = false;
            CurrentUserId = null;
            Debug.Log("[CloudSave] Signed out");
            await Task.CompletedTask;
        }
        
        public async Task<PlayerData> LoadPlayerDataAsync(string userId)
        {
            try
            {
                var keys = new HashSet<string> { "playerData" };
                var data = await CloudSaveService.Instance.Data.Player.LoadAsync(keys);
                
                if (data.TryGetValue("playerData", out var item))
                {
                    string json = item.Value.GetAsString();
                    PlayerData playerData = JsonUtility.FromJson<PlayerData>(json);
                    Debug.Log($"[CloudSave] Loaded player data for {userId}");
                    return playerData;
                }
                
                // Yeni oyuncu - varsayılan data oluştur
                Debug.Log($"[CloudSave] No data found, creating new player data for {userId}");
                return CreateDefaultPlayerData(userId);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[CloudSave] Load failed: {e.Message}");
                return CreateDefaultPlayerData(userId);
            }
        }
        
        public async Task<bool> SavePlayerDataAsync(PlayerData data)
        {
            try
            {
                var dataToSave = new Dictionary<string, object>
                {
                    { "playerData", JsonUtility.ToJson(data) }
                };
                
                await CloudSaveService.Instance.Data.Player.SaveAsync(dataToSave);
                Debug.Log($"[CloudSave] Saved player data for {data.userId}");
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[CloudSave] Save failed: {e.Message}");
                return false;
            }
        }
        
        public async Task<bool> UpdatePlayerStatsAsync(string userId, int deltaElo, int deltaXp, bool won)
        {
            try
            {
                PlayerData data = await LoadPlayerDataAsync(userId);
                
                data.elo += deltaElo;
                data.experience += deltaXp;
                data.totalMatches++;
                
                if (won)
                    data.wins++;
                else
                    data.losses++;
                
                bool success = await SavePlayerDataAsync(data);
                
                if (success)
                {
                    Debug.Log($"[CloudSave] Updated stats for {userId}: ELO {deltaElo:+#;-#;0}, XP {deltaXp:+#;-#;0}, Won: {won}");
                }
                
                return success;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[CloudSave] Update stats failed: {e.Message}");
                return false;
            }
        }
        
        private PlayerData CreateDefaultPlayerData(string userId)
        {
            // PlayerData constructor'ı zaten default değerleri set ediyor
            var data = new PlayerData();
            
            // User-specific bilgileri override et
            data.userId = userId;
            data.username = "Player_" + userId.Substring(0, System.Math.Min(6, userId.Length));
            
            // Başlangıç karakterleri (3 starter karakter)
            data.ownedCharacters.Clear(); // Constructor'dan gelen boş listeyi temizle
            data.ownedCharacters.Add("char_mage");
            data.ownedCharacters.Add("char_warrior");
            data.ownedCharacters.Add("char_ninja");
            data.selectedCharacterId = "char_mage";
            
            // Başlangıç skill'leri (her karakterin base skill'leri)
            data.ownedSkills.Add("skill_fireball");      // Mage
            data.ownedSkills.Add("skill_slash");         // Warrior
            data.ownedSkills.Add("skill_shuriken");      // Ninja
            
            // Başlangıç itemleri (starter equipment - her class için base itemler)
            // Mage starter items
            data.ownedItems.Add("item_mage_starter_robe");
            data.ownedItems.Add("item_mage_starter_staff");
            
            // Warrior starter items
            data.ownedItems.Add("item_warrior_starter_armor");
            data.ownedItems.Add("item_warrior_starter_sword");
            
            // Ninja starter items
            data.ownedItems.Add("item_ninja_starter_outfit");
            data.ownedItems.Add("item_ninja_starter_daggers");
            
            // Her karakter için başlangıç loadout'ları oluştur
            CreateDefaultLoadouts(data);
            
            Debug.Log($"[CloudSave] Created default player data: {data.username}, Level: {data.level}, ELO: {data.elo}, Gold: {data.gold}");
            return data;
        }
        
        /// <summary>
        /// Her karakter için başlangıç loadout'ları oluştur
        /// </summary>
        private void CreateDefaultLoadouts(PlayerData data)
        {
            // Mage loadout
            var mageLoadout = new CharacterLoadout("char_mage");
            mageLoadout.equippedChest = "item_mage_starter_robe";
            mageLoadout.equippedWeapon = "item_mage_starter_staff";
            mageLoadout.activeSkill1 = "skill_fireball";
            data.characterLoadouts.Add(mageLoadout);
            
            // Warrior loadout
            var warriorLoadout = new CharacterLoadout("char_warrior");
            warriorLoadout.equippedChest = "item_warrior_starter_armor";
            warriorLoadout.equippedWeapon = "item_warrior_starter_sword";
            warriorLoadout.activeSkill1 = "skill_slash";
            data.characterLoadouts.Add(warriorLoadout);
            
            // Ninja loadout
            var ninjaLoadout = new CharacterLoadout("char_ninja");
            ninjaLoadout.equippedChest = "item_ninja_starter_outfit";
            ninjaLoadout.equippedWeapon = "item_ninja_starter_daggers";
            ninjaLoadout.activeSkill1 = "skill_shuriken";
            data.characterLoadouts.Add(ninjaLoadout);
            
            Debug.Log($"[CloudSave] Created default loadouts for 3 characters");
        }
    }
}

