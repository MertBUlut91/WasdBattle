using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using WasdBattle.Core;
using WasdBattle.Matchmaking;
using WasdBattle.Progression;

namespace WasdBattle.UI
{
    /// <summary>
    /// Ana menü UI controller'ı
    /// </summary>
    public class MainMenuUI : MonoBehaviour
    {
        [Header("Player Info")]
        [SerializeField] private TextMeshProUGUI _usernameText;
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private TextMeshProUGUI _eloText;
        [SerializeField] private TextMeshProUGUI _rankText;
        [SerializeField] private Image _rankIcon;
        [SerializeField] private Image _xpBar;
        
        [Header("Currency Display")]
        [SerializeField] private TextMeshProUGUI _goldText;
        [SerializeField] private TextMeshProUGUI _essenceText;
        
        [Header("Buttons")]
        [SerializeField] private Button _playButton;
        [SerializeField] private TextMeshProUGUI _playButtonText; // "Find Match" / "Searching..."
        [SerializeField] private Button _cancelMatchButton; // Cancel butonu (başta gizli)
        [SerializeField] private Button _characterSelectButton;
        [SerializeField] private Button _inventoryButton;
        [SerializeField] private Button _shopButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _quitButton;
        
        [Header("Matchmaking UI")]
        [SerializeField] private GameObject _matchmakingPanel; // Timer ve cancel butonu (başta gizli)
        [SerializeField] private TextMeshProUGUI _matchmakingTimerText;
        
        [Header("Panels")]
        [SerializeField] private GameObject _characterSelectPanel;
        [SerializeField] private GameObject _inventoryPanel;
        [SerializeField] private GameObject _shopPanel;
        
        private bool _isSearchingForMatch = false;
        private float _matchmakingStartTime;
        
        private void Start()
        {
            SetupButtons();
            UpdatePlayerInfo();
            SetupMatchmaking();
            
            // Matchmaking UI'yi başta gizle
            if (_matchmakingPanel != null)
                _matchmakingPanel.SetActive(false);
        }
        
        private void Update()
        {
            // Matchmaking timer güncelle
            if (_isSearchingForMatch && _matchmakingTimerText != null)
            {
                float elapsed = Time.time - _matchmakingStartTime;
                int minutes = (int)(elapsed / 60);
                int seconds = (int)(elapsed % 60);
                _matchmakingTimerText.text = $"Searching: {minutes:00}:{seconds:00}";
            }
        }
        
        private void SetupButtons()
        {
            if (_playButton != null)
            {
                _playButton.onClick.AddListener(OnPlayClicked);
                
                // Başlangıç text'i
                if (_playButtonText != null)
                    _playButtonText.text = "Find Match";
            }
            
            if (_cancelMatchButton != null)
                _cancelMatchButton.onClick.AddListener(OnCancelMatchClicked);
            
            if (_characterSelectButton != null)
                _characterSelectButton.onClick.AddListener(OnCharacterSelectClicked);
            
            if (_inventoryButton != null)
                _inventoryButton.onClick.AddListener(OnInventoryClicked);
            
            if (_shopButton != null)
                _shopButton.onClick.AddListener(OnShopClicked);
            
            if (_settingsButton != null)
                _settingsButton.onClick.AddListener(OnSettingsClicked);
            
            if (_quitButton != null)
                _quitButton.onClick.AddListener(OnQuitClicked);
        }
        
        private void SetupMatchmaking()
        {
            // Matchmaking event'lerine abone ol
            if (SimpleMatchmakingManager.Instance != null)
            {
                SimpleMatchmakingManager.Instance.OnMatchFound += OnMatchFound;
                SimpleMatchmakingManager.Instance.OnMatchmakingFailed += OnMatchmakingFailed;
                SimpleMatchmakingManager.Instance.OnMatchmakingCancelled += OnMatchmakingCancelled;
            }
        }
        
        private void UpdatePlayerInfo()
        {
            var playerData = GameManager.Instance.CurrentPlayerData;
            
            if (playerData == null)
                return;
            
            // Player info
            if (_usernameText != null)
                _usernameText.text = playerData.username;
            
            if (_levelText != null)
                _levelText.text = $"Level {playerData.level}";
            
            if (_eloText != null)
                _eloText.text = $"ELO: {playerData.elo}";
            
            // Rank
            Rank rank = ELOSystem.GetRank(playerData.elo);
            if (_rankText != null)
                _rankText.text = ELOSystem.GetRankDisplayName(rank);
            
            if (_rankIcon != null)
                _rankIcon.color = ELOSystem.GetRankColor(rank);
            
            // XP Bar
            if (_xpBar != null)
            {
                float progress = LevelSystem.GetLevelProgress(playerData.experience, playerData.level);
                _xpBar.fillAmount = progress;
            }
            
            // Currency
            if (_goldText != null)
                _goldText.text = playerData.gold.ToString();
            
            if (_essenceText != null)
                _essenceText.text = playerData.essence.ToString();
        }
        
        private void OnPlayClicked()
        {
            if (_isSearchingForMatch)
                return; // Zaten arama yapılıyor
            
            Debug.Log("[MainMenu] Find Match button clicked");
            GameManager.Instance.SetGameState(GameState.Matchmaking);
            
            // UI'yi güncelle
            _isSearchingForMatch = true;
            _matchmakingStartTime = Time.time;
            
            if (_playButtonText != null)
                _playButtonText.text = "Searching...";
            
            if (_playButton != null)
                _playButton.interactable = false; // Butonu disable et
            
            if (_matchmakingPanel != null)
                _matchmakingPanel.SetActive(true); // Timer ve cancel butonunu göster
            
            // Matchmaking'i başlat
            SimpleMatchmakingManager.Instance.StartMatchmaking();
        }
        
        private void OnCancelMatchClicked()
        {
            Debug.Log("[MainMenu] Cancel match button clicked");
            
            // Matchmaking'i iptal et
            if (SimpleMatchmakingManager.Instance != null)
            {
                SimpleMatchmakingManager.Instance.CancelMatchmaking();
            }
            
            ResetMatchmakingUI();
        }
        
        private void OnMatchFound(MatchmakingResult result)
        {
            Debug.Log($"[MainMenu] Match found! Opponent: {result.Players[1].Username}");
            
            // Combat scene'e geç
            SceneManager.LoadScene("CombatScene");
        }
        
        private void OnMatchmakingFailed()
        {
            Debug.LogWarning("[MainMenu] Matchmaking failed (timeout)");
            ResetMatchmakingUI();
        }
        
        private void OnMatchmakingCancelled()
        {
            Debug.Log("[MainMenu] Matchmaking cancelled");
            ResetMatchmakingUI();
        }
        
        private void ResetMatchmakingUI()
        {
            _isSearchingForMatch = false;
            
            if (_playButtonText != null)
                _playButtonText.text = "Find Match";
            
            if (_playButton != null)
                _playButton.interactable = true;
            
            if (_matchmakingPanel != null)
                _matchmakingPanel.SetActive(false);
            
            GameManager.Instance.SetGameState(GameState.MainMenu);
        }
        
        private void OnCharacterSelectClicked()
        {
            Debug.Log("[MainMenu] Character Select clicked");
            
            if (_characterSelectPanel != null)
                _characterSelectPanel.SetActive(true);
        }
        
        private void OnInventoryClicked()
        {
            Debug.Log("[MainMenu] Inventory clicked");
            
            if (_inventoryPanel != null)
                _inventoryPanel.SetActive(true);
        }
        
        private void OnShopClicked()
        {
            Debug.Log("[MainMenu] Shop clicked");
            
            if (_shopPanel != null)
                _shopPanel.SetActive(true);
        }
        
        private void OnSettingsClicked()
        {
            Debug.Log("[MainMenu] Settings clicked");
            // Settings panel açılacak
        }
        
        private void OnQuitClicked()
        {
            Debug.Log("[MainMenu] Quit clicked");
            GameManager.Instance.QuitGame();
        }
        
        private void OnDestroy()
        {
            // Event'lerden ayrıl
            if (SimpleMatchmakingManager.Instance != null)
            {
                SimpleMatchmakingManager.Instance.OnMatchFound -= OnMatchFound;
                SimpleMatchmakingManager.Instance.OnMatchmakingFailed -= OnMatchmakingFailed;
                SimpleMatchmakingManager.Instance.OnMatchmakingCancelled -= OnMatchmakingCancelled;
            }
        }
    }
}

