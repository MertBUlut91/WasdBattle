using UnityEngine;
using UnityEngine.SceneManagement;
using WasdBattle.Data;
using WasdBattle.Network;
using Unity.Services.Core;
using Unity.Services.Authentication;

namespace WasdBattle.Core
{
    /// <summary>
    /// Oyunun merkezi yöneticisi. Singleton pattern kullanır.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;
        public static GameManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject go = new GameObject("GameManager");
                    _instance = go.AddComponent<GameManager>();
                    DontDestroyOnLoad(go);
                }
                return _instance;
            }
        }
        
        [Header("Services")]
        private IFirebaseService _firebaseService;
        private DataManager _dataManager;
        
        [Header("Player Data")]
        private PlayerData _currentPlayerData;
        
        [Header("Game State")]
        private GameState _currentState = GameState.MainMenu;
        
        public IFirebaseService FirebaseService => _firebaseService;
        public DataManager DataManager => _dataManager;
        public PlayerData CurrentPlayerData => _currentPlayerData;
        public GameState CurrentState => _currentState;
        
        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            _instance = this;
            DontDestroyOnLoad(gameObject);
            
            InitializeServices();
        }
        
        private async void InitializeServices()
        {
            Debug.Log("[GameManager] Initializing services...");
            
            try
            {
                // 1. Unity Services'i başlat
                Debug.Log("[GameManager] Initializing Unity Services...");
                await UnityServices.InitializeAsync();
                Debug.Log("[GameManager] Unity Services initialized");
                
                // 2. Anonymous Authentication
                Debug.Log("[GameManager] Signing in anonymously...");
                if (!AuthenticationService.Instance.IsSignedIn)
                {
                    await AuthenticationService.Instance.SignInAnonymouslyAsync();
                }
                Debug.Log($"[GameManager] Signed in as: {AuthenticationService.Instance.PlayerId}");
                
                // 3. Unity Cloud Save servisini başlat
                _firebaseService = new UnityCloudSaveService();
                await _firebaseService.InitializeAsync();
                
                // 4. Data Manager'ı oluştur
                _dataManager = new DataManager(_firebaseService);
                
                // 5. Anonim giriş yap (Cloud Save için)
                bool signedIn = await _firebaseService.SignInAnonymouslyAsync();
                if (signedIn)
                {
                    // Oyuncu verisini yükle
                    _currentPlayerData = await _dataManager.LoadPlayerDataAsync();
                    Debug.Log($"[GameManager] Player data loaded: {_currentPlayerData.username}, Level: {_currentPlayerData.level}, ELO: {_currentPlayerData.elo}");
                }
                else
                {
                    // Giriş başarısız, lokal cache'den yükle
                    _currentPlayerData = _dataManager.LoadFromLocalCache();
                    if (_currentPlayerData == null)
                    {
                        Debug.LogWarning("[GameManager] Failed to load player data, creating default");
                        _currentPlayerData = new PlayerData();
                    }
                }
                
                Debug.Log("[GameManager] Services initialized successfully!");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[GameManager] Service initialization failed: {e.Message}\n{e.StackTrace}");
                
                // Fallback: Offline mode
                _currentPlayerData = new PlayerData();
                Debug.LogWarning("[GameManager] Running in offline mode with default data");
            }
        }
        
        public void SetGameState(GameState newState)
        {
            _currentState = newState;
            Debug.Log($"[GameManager] Game state changed to: {newState}");
        }
        
        public async void SavePlayerData()
        {
            if (_currentPlayerData != null && _dataManager != null)
            {
                await _dataManager.SavePlayerDataAsync(_currentPlayerData);
            }
        }
        
        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
        
        public void QuitGame()
        {
            SavePlayerData();
            Application.Quit();
        }
        
        private void OnApplicationQuit()
        {
            SavePlayerData();
        }
        
        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                SavePlayerData();
            }
        }
    }
    
    public enum GameState
    {
        MainMenu,
        CharacterSelect,
        Matchmaking,
        InLobby,
        InCombat,
        PostMatch,
        Crafting,
        Shop
    }
}

