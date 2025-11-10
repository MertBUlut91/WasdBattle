using UnityEngine;
using UnityEngine.UI;
using TMPro;
using WasdBattle.Core;

namespace WasdBattle.UI
{
    /// <summary>
    /// Craft & Shop panel ana controller'ı
    /// İki NPC gösterir ve hangisine tıklanırsa o menüyü açar
    /// </summary>
    public class CraftShopPanelUI : MonoBehaviour
    {
        [Header("NPC Display")]
        [SerializeField] private NPCDisplayController _npcDisplayController;
        [SerializeField] private RawImage _npcDisplayImage; // RenderTexture gösterimi
        
        [Header("NPC Click Areas")]
        [SerializeField] private Button _craftNPCButton; // Sol taraf (Craft NPC)
        [SerializeField] private Button _shopNPCButton;  // Sağ taraf (Shop NPC)
        
        [Header("NPC Labels")]
        [SerializeField] private TextMeshProUGUI _craftNPCLabel;
        [SerializeField] private TextMeshProUGUI _shopNPCLabel;
        
        [Header("Menu Panels")]
        [SerializeField] private GameObject _craftMenuPanel;
        [SerializeField] private ItemCraftUI _itemCraftUI;
        [SerializeField] private GameObject _shopMenuPanel;
        [SerializeField] private ItemShopUI _itemShopUI;
        
        [Header("Main Panel")]
        [SerializeField] private GameObject _mainPanel;
        [SerializeField] private Button _closeButton;
        
        private void Start()
        {
            SetupButtons();
            SetupNPCDisplay();
            
            // Başlangıçta menüleri gizle
            if (_craftMenuPanel != null)
                _craftMenuPanel.SetActive(false);
            
            if (_shopMenuPanel != null)
                _shopMenuPanel.SetActive(false);
        }
        
        private void SetupButtons()
        {
            if (_craftNPCButton != null)
            {
                _craftNPCButton.onClick.AddListener(OnCraftNPCClicked);
            }
            
            if (_shopNPCButton != null)
            {
                _shopNPCButton.onClick.AddListener(OnShopNPCClicked);
            }
            
            if (_closeButton != null)
            {
                _closeButton.onClick.AddListener(OnCloseClicked);
            }
        }
        
        private void SetupNPCDisplay()
        {
            if (_npcDisplayController != null && _npcDisplayImage != null)
            {
                _npcDisplayImage.texture = _npcDisplayController.GetRenderTexture();
            }
            
            // Label'ları ayarla
            if (_craftNPCLabel != null)
                _craftNPCLabel.text = "Craft Master\n(Click to Craft)";
            
            if (_shopNPCLabel != null)
                _shopNPCLabel.text = "Shop Keeper\n(Click to Shop)";
        }
        
        /// <summary>
        /// Craft NPC'ye tıklandı
        /// </summary>
        private void OnCraftNPCClicked()
        {
            Debug.Log("[CraftShop] Craft NPC clicked");
            
            // NPC'yi highlight et
            if (_npcDisplayController != null)
                _npcDisplayController.HighlightNPC(NPCType.Craft);
            
            // Craft menüsünü aç
            OpenCraftMenu();
        }
        
        /// <summary>
        /// Shop NPC'ye tıklandı
        /// </summary>
        private void OnShopNPCClicked()
        {
            Debug.Log("[CraftShop] Shop NPC clicked");
            
            // NPC'yi highlight et
            if (_npcDisplayController != null)
                _npcDisplayController.HighlightNPC(NPCType.Shop);
            
            // Shop menüsünü aç
            OpenShopMenu();
        }
        
        /// <summary>
        /// Craft menüsünü aç
        /// </summary>
        private void OpenCraftMenu()
        {
            // Shop menüsünü kapat
            if (_shopMenuPanel != null)
                _shopMenuPanel.SetActive(false);
            
            // Craft menüsünü aç
            if (_craftMenuPanel != null)
            {
                _craftMenuPanel.SetActive(true);
                
                // ItemCraftUI'yi refresh et
                if (_itemCraftUI != null)
                    _itemCraftUI.RefreshUI();
            }
            
            GameManager.Instance?.SetGameState(GameState.Crafting);
        }
        
        /// <summary>
        /// Shop menüsünü aç
        /// </summary>
        private void OpenShopMenu()
        {
            // Craft menüsünü kapat
            if (_craftMenuPanel != null)
                _craftMenuPanel.SetActive(false);
            
            // Shop menüsünü aç
            if (_shopMenuPanel != null)
            {
                _shopMenuPanel.SetActive(true);
                
                // ItemShopUI'yi refresh et
                if (_itemShopUI != null)
                    _itemShopUI.RefreshUI();
            }
            
            GameManager.Instance?.SetGameState(GameState.Shop);
        }
        
        /// <summary>
        /// Panel'i aç
        /// </summary>
        public void OpenPanel()
        {
            if (_mainPanel != null)
                _mainPanel.SetActive(true);
            
            // Highlight'ı temizle
            if (_npcDisplayController != null)
                _npcDisplayController.HighlightNPC(NPCType.None);
            
            // Menüleri kapat
            if (_craftMenuPanel != null)
                _craftMenuPanel.SetActive(false);
            
            if (_shopMenuPanel != null)
                _shopMenuPanel.SetActive(false);
        }
        
        /// <summary>
        /// Panel'i kapat
        /// </summary>
        private void OnCloseClicked()
        {
            if (_mainPanel != null)
                _mainPanel.SetActive(false);
            
            // Menüleri kapat
            if (_craftMenuPanel != null)
                _craftMenuPanel.SetActive(false);
            
            if (_shopMenuPanel != null)
                _shopMenuPanel.SetActive(false);
            
            GameManager.Instance?.SetGameState(GameState.MainMenu);
        }
    }
}

