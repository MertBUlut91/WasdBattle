using Unity.Netcode;
using UnityEngine;

namespace WasdBattle.Core
{
    /// <summary>
    /// Network yönetimi için singleton. Unity Netcode'u wrap eder.
    /// </summary>
    public class WasdNetworkManager : MonoBehaviour
    {
        private static WasdNetworkManager _instance;
        public static WasdNetworkManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject go = new GameObject("WasdNetworkManager");
                    _instance = go.AddComponent<WasdNetworkManager>();
                    DontDestroyOnLoad(go);
                }
                return _instance;
            }
        }
        
        private NetworkManager _networkManager;
        
        public NetworkManager NetworkManager => _networkManager;
        public bool IsServer => _networkManager != null && _networkManager.IsServer;
        public bool IsClient => _networkManager != null && _networkManager.IsClient;
        public bool IsHost => _networkManager != null && _networkManager.IsHost;
        public bool IsConnected => _networkManager != null && (_networkManager.IsServer || _networkManager.IsClient);
        
        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            _instance = this;
            DontDestroyOnLoad(gameObject);
            
            InitializeNetworkManager();
        }
        
        private void InitializeNetworkManager()
        {
            // NetworkManager'ı bul veya oluştur
            _networkManager = FindObjectOfType<NetworkManager>();
            
            if (_networkManager == null)
            {
                GameObject nmGo = new GameObject("NetworkManager");
                _networkManager = nmGo.AddComponent<NetworkManager>();
                DontDestroyOnLoad(nmGo);
                
                Debug.Log("[WasdNetworkManager] NetworkManager created");
            }
            
            // Event'lere abone ol
            if (_networkManager != null)
            {
                _networkManager.OnClientConnectedCallback += OnClientConnected;
                _networkManager.OnClientDisconnectCallback += OnClientDisconnected;
                _networkManager.OnServerStarted += OnServerStarted;
            }
        }
        
        public void StartHost()
        {
            if (_networkManager != null)
            {
                _networkManager.StartHost();
                Debug.Log("[WasdNetworkManager] Started as Host");
            }
        }
        
        public void StartServer()
        {
            if (_networkManager != null)
            {
                _networkManager.StartServer();
                Debug.Log("[WasdNetworkManager] Started as Server");
            }
        }
        
        public void StartClient()
        {
            if (_networkManager != null)
            {
                _networkManager.StartClient();
                Debug.Log("[WasdNetworkManager] Started as Client");
            }
        }
        
        public void Shutdown()
        {
            if (_networkManager != null)
            {
                _networkManager.Shutdown();
                Debug.Log("[WasdNetworkManager] Shutdown");
            }
        }
        
        private void OnServerStarted()
        {
            Debug.Log("[WasdNetworkManager] Server started");
        }
        
        private void OnClientConnected(ulong clientId)
        {
            Debug.Log($"[WasdNetworkManager] Client connected: {clientId}");
        }
        
        private void OnClientDisconnected(ulong clientId)
        {
            Debug.Log($"[WasdNetworkManager] Client disconnected: {clientId}");
        }
        
        private void OnDestroy()
        {
            if (_networkManager != null)
            {
                _networkManager.OnClientConnectedCallback -= OnClientConnected;
                _networkManager.OnClientDisconnectCallback -= OnClientDisconnected;
                _networkManager.OnServerStarted -= OnServerStarted;
            }
        }
    }
}

