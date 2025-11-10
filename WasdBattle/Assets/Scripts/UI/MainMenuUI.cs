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
        
        [Header("3D Character Display")]
        [SerializeField] private CharacterDisplayController _characterDisplayController;
        
        [Header("Buttons")]
        [SerializeField] private Button _playButton;
        [SerializeField] private TextMeshProUGUI _playButtonText; // "Find Match" / "Searching..."
        [SerializeField] private Button _cancelMatchButton; // Cancel butonu (başta gizli)
        [SerializeField] private Button _characterButton;
        [SerializeField] private Button _inventoryButton;
        [SerializeField] private Button _craftShopButton;
        [SerializeField] private Button _settingsButton;
        
        [Header("Matchmaking UI")]
        [SerializeField] private GameObject _matchmakingPanel; // Timer ve cancel butonu (başta gizli)
        [SerializeField] private TextMeshProUGUI _matchmakingTimerText;
        
        [Header("Panels")]
        [SerializeField] private GameObject _characterPanel;
        [SerializeField] private CharacterPanelUI _characterPanelUI;
        [SerializeField] private GameObject _inventoryPanel;
        [SerializeField] private EquipmentUI _equipmentUI;
        [SerializeField] private GameObject _craftShopPanel;
        [SerializeField] private CraftShopPanelUI _craftShopPanelUI;
        
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
            
            // 3D karakteri yükle
            if (_characterDisplayController != null)
            {
                _characterDisplayController.LoadSelectedCharacter();
                _characterDisplayController.SetCameraPosition(CameraPosition.MainMenu);
            }
            
            // Başlangıç itemlerini kontrol et
            CheckAndAddStartingItems();
        }
        
        /// <summary>
        /// Karakterlerin başlangıç itemlerini otomatik equip et (sadece ilk kez)
        /// </summary>
        private void CheckAndAddStartingItems()
        {
            var playerData = GameManager.Instance?.CurrentPlayerData;
            if (playerData == null) return;
            
            // Her owned karakter için kontrol et
            var allCharacters = Resources.LoadAll<WasdBattle.Data.CharacterData>("Characters");
            
            foreach (var character in allCharacters)
            {
                // Karakter owned mi?
                if (!playerData.ownedCharacters.Contains(character.characterId))
                    continue;
                
                // Bu karakterin starting itemleri var mı?
                if (character.startingItems == null || character.startingItems.Length == 0)
                    continue;
                
                // Loadout'u al
                var loadout = playerData.GetLoadoutForCharacter(character.characterId);
                
                // Eğer loadout boşsa (ilk kez), starting itemleri equip et
                bool isFirstTime = string.IsNullOrEmpty(loadout.equippedWeapon) && 
                                   string.IsNullOrEmpty(loadout.equippedHelmet) &&
                                   string.IsNullOrEmpty(loadout.equippedChest);
                
                if (isFirstTime)
                {
                    Debug.Log($"[MainMenu] Setting up starting equipment for {character.characterName}");
                    
                    // Starting itemleri inventory'e ekle VE equip et
                    foreach (var item in character.startingItems)
                    {
                        if (item != null)
                        {
                            // Inventory'e ekle (aynı item'den 2 tane varsa count artar)
                            playerData.AddItem(item.itemId, 1);
                            
                            // Otomatik equip et
                            loadout.EquipItem(item.slot, item.itemId);
                            Debug.Log($"[MainMenu] Equipped starting item: {item.itemName} ({item.slot})");
                        }
                    }
                }
            }
            
            // Kaydet
            if (GameManager.Instance != null && GameManager.Instance.DataManager != null)
            {
                GameManager.Instance.DataManager.SavePlayerDataAsync(playerData);
            }
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
            
            if (_characterButton != null)
                _characterButton.onClick.AddListener(OnCharacterClicked);
            
            if (_inventoryButton != null)
                _inventoryButton.onClick.AddListener(OnInventoryClicked);
            
            if (_craftShopButton != null)
                _craftShopButton.onClick.AddListener(OnCraftShopClicked);
            
            if (_settingsButton != null)
                _settingsButton.onClick.AddListener(OnSettingsClicked);
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
            
            // Currency (sadece Gold)
            if (_goldText != null)
                _goldText.text = playerData.gold.ToString();
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
        
        private void OnCharacterClicked()
        {
            Debug.Log("[MainMenu] Character button clicked");
            
            if (_characterPanelUI != null)
            {
                _characterPanelUI.OpenPanel();
                
                // Kamera pozisyonunu değiştir
                if (_characterDisplayController != null)
                    _characterDisplayController.SetCameraPosition(CameraPosition.CharacterPanel);
            }
        }
        
        private void OnInventoryClicked()
        {
            Debug.Log("[MainMenu] Inventory button clicked");
            
            if (_equipmentUI != null)
            {
                _equipmentUI.OpenPanel();
                
                // Kamera pozisyonunu değiştir
                if (_characterDisplayController != null)
                    _characterDisplayController.SetCameraPosition(CameraPosition.InventoryPanel);
            }
        }
        
        private void OnCraftShopClicked()
        {
            Debug.Log("[MainMenu] Craft & Shop button clicked");
            
            if (_craftShopPanelUI != null)
            {
                _craftShopPanelUI.OpenPanel();
            }
        }
        
        private void OnSettingsClicked()
        {
            Debug.Log("[MainMenu] Settings button clicked");
            // TODO: Settings panel açılacak
        }
        
        /// <summary>
        /// Panel kapandığında kamerayı main menu pozisyonuna döndür
        /// </summary>
        public void OnPanelClosed()
        {
            if (_characterDisplayController != null)
                _characterDisplayController.SetCameraPosition(CameraPosition.MainMenu);
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

