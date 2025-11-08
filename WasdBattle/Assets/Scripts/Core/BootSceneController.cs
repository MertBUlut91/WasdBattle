using UnityEngine;
using UnityEngine.SceneManagement;
using WasdBattle.Core;

namespace WasdBattle.Core
{
    /// <summary>
    /// Boot scene controller - servislerin başlamasını bekler ve ana menüye geçer
    /// </summary>
    public class BootSceneController : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float _minLoadTime = 2f;
        [SerializeField] private string _nextSceneName = "MainMenuScene";
        
        [Header("UI References (Optional)")]
        [SerializeField] private UnityEngine.UI.Text _loadingText;
        [SerializeField] private TMPro.TextMeshProUGUI _loadingTextTMP;
        
        private async void Start()
        {
            Debug.Log("[BootScene] Starting boot sequence...");
            
            float startTime = Time.time;
            
            // Loading text'i güncelle
            UpdateLoadingText("Initializing services...");
            
            // GameManager'ın başlamasını bekle
            int waitCount = 0;
            while (GameManager.Instance == null || GameManager.Instance.CurrentPlayerData == null)
            {
                await System.Threading.Tasks.Task.Yield();
                waitCount++;
                
                // 10 saniye timeout
                if (waitCount > 10000)
                {
                    Debug.LogError("[BootScene] Timeout waiting for GameManager!");
                    UpdateLoadingText("Failed to initialize. Please restart.");
                    return;
                }
            }
            
            UpdateLoadingText("Loading complete!");
            Debug.Log("[BootScene] Services initialized successfully");
            
            // Minimum yükleme süresi (kullanıcı loading screen'i görsün diye)
            float elapsed = Time.time - startTime;
            if (elapsed < _minLoadTime)
            {
                float remainingTime = _minLoadTime - elapsed;
                Debug.Log($"[BootScene] Waiting {remainingTime:F1}s before scene transition...");
                await System.Threading.Tasks.Task.Delay((int)(remainingTime * 1000));
            }
            
            // Ana menüye geç
            Debug.Log($"[BootScene] Loading {_nextSceneName}...");
            SceneManager.LoadScene(_nextSceneName);
        }
        
        private void UpdateLoadingText(string message)
        {
            if (_loadingText != null)
            {
                _loadingText.text = message;
            }
            
            if (_loadingTextTMP != null)
            {
                _loadingTextTMP.text = message;
            }
            
            Debug.Log($"[BootScene] {message}");
        }
    }
}

