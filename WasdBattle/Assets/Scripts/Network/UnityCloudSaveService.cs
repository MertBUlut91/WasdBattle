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
            
            // NOT: Başlangıç itemleri CharacterData'dan otomatik olarak eklenir (MainMenuUI.CheckAndAddStartingItems)
            // Burada hardcoded item eklemeyin!
            
            Debug.Log($"[CloudSave] Created default player data: {data.username}, Level: {data.level}, ELO: {data.elo}, Gold: {data.gold}");
            return data;
        }
        
        // NOT: CreateDefaultLoadouts kaldırıldı
        // Başlangıç loadout'ları CharacterData'daki starting items ile otomatik oluşturulur (MainMenuUI.CheckAndAddStartingItems)
    }
}

