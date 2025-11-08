using UnityEngine;
using WasdBattle.Core;

namespace WasdBattle
{
    /// <summary>
    /// Oyun başlangıcında gerekli sistemleri başlatır
    /// </summary>
    public class GameInitializer : MonoBehaviour
    {
        [Header("Auto Start")]
        [SerializeField] private bool _autoInitialize = true;
        
        private void Awake()
        {
            if (_autoInitialize)
            {
                Initialize();
            }
        }
        
        public void Initialize()
        {
            Debug.Log("[GameInitializer] Initializing game systems...");
            
            // GameManager'ı başlat (singleton, otomatik oluşturulur)
            var gameManager = GameManager.Instance;
            
            // NetworkManager'ı başlat
            var networkManager = WasdNetworkManager.Instance;
            
            Debug.Log("[GameInitializer] Game systems initialized");
        }
    }
}

