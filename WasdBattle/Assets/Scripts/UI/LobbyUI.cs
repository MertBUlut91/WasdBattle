using UnityEngine;
using UnityEngine.UI;
using TMPro;
using WasdBattle.Matchmaking;
using WasdBattle.Core;
using UnityEngine.SceneManagement;

namespace WasdBattle.UI
{
    /// <summary>
    /// Matchmaking lobby UI controller
    /// </summary>
    public class LobbyUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI _searchingText;
        [SerializeField] private TextMeshProUGUI _eloRangeText;
        [SerializeField] private TextMeshProUGUI _timerText;
        [SerializeField] private Button _cancelButton;
        
        [Header("Settings")]
        [SerializeField] private string _combatSceneName = "CombatScene";
        [SerializeField] private string _mainMenuSceneName = "MainMenuScene";
        
        private float _startTime;
        private bool _matchFound = false;
        
        private void Start()
        {
            _startTime = Time.time;
            
            // Button listener
            if (_cancelButton != null)
            {
                _cancelButton.onClick.AddListener(OnCancelClicked);
            }
            
            // Matchmaking event'lerine abone ol
            if (SimpleMatchmakingManager.Instance != null)
            {
                SimpleMatchmakingManager.Instance.OnMatchFound += OnMatchFound;
                SimpleMatchmakingManager.Instance.OnMatchmakingFailed += OnMatchFailed;
                SimpleMatchmakingManager.Instance.OnMatchmakingCancelled += OnMatchCancelled;
            }
            
            // ELO range'i göster
            UpdateELORange();
        }
        
        private void Update()
        {
            if (_matchFound)
                return;
            
            // Timer'ı güncelle
            if (_timerText != null)
            {
                float elapsed = Time.time - _startTime;
                int minutes = (int)(elapsed / 60);
                int seconds = (int)(elapsed % 60);
                _timerText.text = $"{minutes:00}:{seconds:00}";
            }
            
            // Searching text animasyonu (opsiyonel)
            if (_searchingText != null)
            {
                int dots = ((int)(Time.time * 2)) % 4;
                _searchingText.text = "Searching for opponent" + new string('.', dots);
            }
        }
        
        private void UpdateELORange()
        {
            if (_eloRangeText != null && GameManager.Instance != null)
            {
                var playerData = GameManager.Instance.CurrentPlayerData;
                if (playerData != null)
                {
                    int eloTolerance = 200; // SimpleMatchmakingManager'dan al
                    int minElo = playerData.elo - eloTolerance;
                    int maxElo = playerData.elo + eloTolerance;
                    _eloRangeText.text = $"ELO Range: {minElo} - {maxElo}";
                }
            }
        }
        
        private void OnMatchFound(MatchmakingResult result)
        {
            _matchFound = true;
            Debug.Log("[LobbyUI] Match found! Loading combat scene...");
            
            if (_searchingText != null)
            {
                _searchingText.text = "Match Found!";
            }
            
            // Combat scene'e geç
            SceneManager.LoadScene(_combatSceneName);
        }
        
        private void OnMatchFailed()
        {
            Debug.Log("[LobbyUI] Matchmaking failed, returning to main menu");
            
            if (_searchingText != null)
            {
                _searchingText.text = "Matchmaking Failed";
            }
            
            // Ana menüye dön
            Invoke(nameof(ReturnToMainMenu), 2f);
        }
        
        private void OnMatchCancelled()
        {
            Debug.Log("[LobbyUI] Matchmaking cancelled");
            ReturnToMainMenu();
        }
        
        private void OnCancelClicked()
        {
            Debug.Log("[LobbyUI] Cancel button clicked");
            
            if (SimpleMatchmakingManager.Instance != null)
            {
                SimpleMatchmakingManager.Instance.CancelMatchmaking();
            }
            
            ReturnToMainMenu();
        }
        
        private void ReturnToMainMenu()
        {
            SceneManager.LoadScene(_mainMenuSceneName);
        }
        
        private void OnDestroy()
        {
            // Event'lerden ayrıl
            if (SimpleMatchmakingManager.Instance != null)
            {
                SimpleMatchmakingManager.Instance.OnMatchFound -= OnMatchFound;
                SimpleMatchmakingManager.Instance.OnMatchmakingFailed -= OnMatchFailed;
                SimpleMatchmakingManager.Instance.OnMatchmakingCancelled -= OnMatchCancelled;
            }
            
            // Button listener'ı temizle
            if (_cancelButton != null)
            {
                _cancelButton.onClick.RemoveListener(OnCancelClicked);
            }
        }
    }
}

