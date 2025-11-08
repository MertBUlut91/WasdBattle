using System.Threading.Tasks;
using UnityEngine;
using WasdBattle.Data;
using WasdBattle.Network;

namespace WasdBattle.Core
{
    /// <summary>
    /// Veri yönetimi ve persistance için merkezi sınıf
    /// </summary>
    public class DataManager
    {
        private IFirebaseService _firebaseService;
        private PlayerData _cachedPlayerData;
        
        public DataManager(IFirebaseService firebaseService)
        {
            _firebaseService = firebaseService;
        }
        
        /// <summary>
        /// Oyuncu verisini yükler
        /// </summary>
        public async Task<PlayerData> LoadPlayerDataAsync()
        {
            if (_firebaseService == null || !_firebaseService.IsAuthenticated)
            {
                Debug.LogWarning("[DataManager] Firebase not authenticated!");
                return CreateDefaultPlayerData();
            }
            
            try
            {
                _cachedPlayerData = await _firebaseService.LoadPlayerDataAsync(_firebaseService.CurrentUserId);
                
                if (_cachedPlayerData == null)
                {
                    _cachedPlayerData = CreateDefaultPlayerData();
                    _cachedPlayerData.userId = _firebaseService.CurrentUserId;
                }
                
                Debug.Log($"[DataManager] Player data loaded: {_cachedPlayerData.username}");
                return _cachedPlayerData;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[DataManager] Error loading player data: {e.Message}");
                return CreateDefaultPlayerData();
            }
        }
        
        /// <summary>
        /// Oyuncu verisini kaydeder
        /// </summary>
        public async Task<bool> SavePlayerDataAsync(PlayerData data)
        {
            if (_firebaseService == null || !_firebaseService.IsAuthenticated)
            {
                Debug.LogWarning("[DataManager] Firebase not authenticated!");
                SaveToLocalCache(data);
                return false;
            }
            
            try
            {
                bool success = await _firebaseService.SavePlayerDataAsync(data);
                
                if (success)
                {
                    _cachedPlayerData = data;
                    SaveToLocalCache(data);
                    Debug.Log("[DataManager] Player data saved successfully");
                }
                
                return success;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[DataManager] Error saving player data: {e.Message}");
                SaveToLocalCache(data);
                return false;
            }
        }
        
        /// <summary>
        /// Maç sonucu verilerini günceller
        /// </summary>
        public async Task<bool> UpdateMatchResultAsync(PlayerData playerData, PlayerData opponentData, bool won)
        {
            if (_firebaseService == null || !_firebaseService.IsAuthenticated)
            {
                Debug.LogWarning("[DataManager] Firebase not authenticated!");
                return false;
            }
            
            try
            {
                // ELO ve XP hesapla (bu kısım CombatManager'dan çağrılmalı)
                int eloChange = 0; // ELOSystem'den hesaplanacak
                int xpGained = 0;  // LevelSystem'den hesaplanacak
                
                bool success = await _firebaseService.UpdatePlayerStatsAsync(
                    playerData.userId,
                    eloChange,
                    xpGained,
                    won
                );
                
                return success;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[DataManager] Error updating match result: {e.Message}");
                return false;
            }
        }
        
        /// <summary>
        /// Varsayılan oyuncu verisi oluşturur
        /// </summary>
        private PlayerData CreateDefaultPlayerData()
        {
            PlayerData data = new PlayerData();
            data.username = "Player_" + Random.Range(1000, 9999);
            
            // Başlangıç karakterlerini ekle
            data.ownedCharacters.Add("char_mage");
            data.ownedCharacters.Add("char_warrior");
            data.ownedCharacters.Add("char_ninja");
            data.selectedCharacterId = "char_mage";
            
            Debug.Log($"[DataManager] Created default player data: {data.username}");
            return data;
        }
        
        /// <summary>
        /// Lokal cache'e kaydeder (offline kullanım için)
        /// </summary>
        private void SaveToLocalCache(PlayerData data)
        {
            string json = JsonUtility.ToJson(data);
            PlayerPrefs.SetString("CachedPlayerData", json);
            PlayerPrefs.Save();
            
            Debug.Log("[DataManager] Saved to local cache");
        }
        
        /// <summary>
        /// Lokal cache'den yükler
        /// </summary>
        public PlayerData LoadFromLocalCache()
        {
            if (PlayerPrefs.HasKey("CachedPlayerData"))
            {
                string json = PlayerPrefs.GetString("CachedPlayerData");
                PlayerData data = JsonUtility.FromJson<PlayerData>(json);
                
                Debug.Log("[DataManager] Loaded from local cache");
                return data;
            }
            
            return null;
        }
        
        /// <summary>
        /// Cache'i temizler
        /// </summary>
        public void ClearCache()
        {
            PlayerPrefs.DeleteKey("CachedPlayerData");
            _cachedPlayerData = null;
            
            Debug.Log("[DataManager] Cache cleared");
        }
        
        public PlayerData CachedPlayerData => _cachedPlayerData;
    }
}

