using System;
using System.Threading.Tasks;
using UnityEngine;
using WasdBattle.Data;

namespace WasdBattle.Network
{
    /// <summary>
    /// Firebase SDK kurulana kadar kullanılacak mock servis.
    /// PlayerPrefs ile lokal veri saklama.
    /// </summary>
    public class MockFirebaseService : IFirebaseService
    {
        private const string PLAYER_DATA_KEY = "PlayerData";
        private string _currentUserId;
        private bool _isInitialized;
        private bool _isAuthenticated;
        
        public bool IsInitialized => _isInitialized;
        public bool IsAuthenticated => _isAuthenticated;
        public string CurrentUserId => _currentUserId;
        
        public async Task<bool> InitializeAsync()
        {
            await Task.Delay(100); // Simulate async operation
            _isInitialized = true;
            Debug.Log("[MockFirebase] Initialized");
            return true;
        }
        
        public async Task<bool> SignInAnonymouslyAsync()
        {
            await Task.Delay(100);
            _currentUserId = SystemInfo.deviceUniqueIdentifier;
            _isAuthenticated = true;
            Debug.Log($"[MockFirebase] Signed in anonymously: {_currentUserId}");
            return true;
        }
        
        public async Task<bool> SignInWithEmailAsync(string email, string password)
        {
            await Task.Delay(100);
            _currentUserId = email.GetHashCode().ToString();
            _isAuthenticated = true;
            Debug.Log($"[MockFirebase] Signed in with email: {email}");
            return true;
        }
        
        public async Task<bool> SignUpWithEmailAsync(string email, string password)
        {
            await Task.Delay(100);
            _currentUserId = email.GetHashCode().ToString();
            _isAuthenticated = true;
            Debug.Log($"[MockFirebase] Signed up with email: {email}");
            return true;
        }
        
        public async Task SignOutAsync()
        {
            await Task.Delay(50);
            _currentUserId = null;
            _isAuthenticated = false;
            Debug.Log("[MockFirebase] Signed out");
        }
        
        public async Task<PlayerData> LoadPlayerDataAsync(string userId)
        {
            await Task.Delay(100);
            
            string key = $"{PLAYER_DATA_KEY}_{userId}";
            if (PlayerPrefs.HasKey(key))
            {
                string json = PlayerPrefs.GetString(key);
                PlayerData data = JsonUtility.FromJson<PlayerData>(json);
                Debug.Log($"[MockFirebase] Loaded player data for {userId}");
                return data;
            }
            
            // Yeni oyuncu - varsayılan data oluştur
            PlayerData newData = new PlayerData
            {
                userId = userId,
                username = $"Player_{userId.Substring(0, 6)}"
            };
            
            Debug.Log($"[MockFirebase] Created new player data for {userId}");
            return newData;
        }
        
        public async Task<bool> SavePlayerDataAsync(PlayerData data)
        {
            await Task.Delay(100);
            
            string key = $"{PLAYER_DATA_KEY}_{data.userId}";
            string json = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(key, json);
            PlayerPrefs.Save();
            
            Debug.Log($"[MockFirebase] Saved player data for {data.userId}");
            return true;
        }
        
        public async Task<bool> UpdatePlayerStatsAsync(string userId, int deltaElo, int deltaXp, bool won)
        {
            PlayerData data = await LoadPlayerDataAsync(userId);
            
            data.elo += deltaElo;
            data.experience += deltaXp;
            data.totalMatches++;
            
            if (won)
                data.wins++;
            else
                data.losses++;
            
            return await SavePlayerDataAsync(data);
        }
    }
}

