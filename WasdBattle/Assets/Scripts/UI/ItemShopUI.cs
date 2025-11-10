using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using WasdBattle.Core;
using WasdBattle.Data;
using WasdBattle.Economy;

namespace WasdBattle.UI
{
    /// <summary>
    /// Item shop UI - Class ve item type bazlı filtreleme
    /// </summary>
    public class ItemShopUI : MonoBehaviour
    {
        [Header("Filter Dropdowns")]
        [SerializeField] private TMP_Dropdown _classFilterDropdown;
        [SerializeField] private TMP_Dropdown _itemTypeFilterDropdown;
        
        [Header("Item List")]
        [SerializeField] private ScrollRect _itemScrollView;
        [SerializeField] private Transform _itemListContent;
        [SerializeField] private GameObject _itemCardPrefab;
        
        [Header("Selected Item Display")]
        [SerializeField] private GameObject _itemDetailPanel;
        [SerializeField] private TextMeshProUGUI _itemNameText;
        [SerializeField] private TextMeshProUGUI _itemDescriptionText;
        [SerializeField] private Image _itemIconImage;
        [SerializeField] private TextMeshProUGUI _itemStatsText;
        [SerializeField] private TextMeshProUGUI _shopPriceText;
        
        [Header("Buttons")]
        [SerializeField] private Button _purchaseButton;
        [SerializeField] private Button _closeButton;
        
        [Header("Currency Display")]
        [SerializeField] private TextMeshProUGUI _goldText;
        
        private ItemData _selectedItem;
        private ItemClass _selectedClass = ItemClass.All;
        private EquipmentSlot _selectedSlot = EquipmentSlot.Helmet;
        private List<ItemData> _allShopItems = new List<ItemData>();
        
        private void Start()
        {
            SetupDropdowns();
            SetupButtons();
            LoadAllShopItems();
            RefreshUI();
        }
        
        private void OnEnable()
        {
            RefreshUI();
        }
        
        private void SetupDropdowns()
        {
            // Class filter dropdown
            if (_classFilterDropdown != null)
            {
                _classFilterDropdown.ClearOptions();
                List<string> classOptions = new List<string> { "All", "Mage", "Warrior", "Ninja" };
                _classFilterDropdown.AddOptions(classOptions);
                _classFilterDropdown.onValueChanged.AddListener(OnClassFilterChanged);
            }
            
            // Item type filter dropdown
            if (_itemTypeFilterDropdown != null)
            {
                _itemTypeFilterDropdown.ClearOptions();
                List<string> typeOptions = new List<string> 
                { 
                    "Helmet", "Chest", "Gloves", "Legs", "Weapon", 
                    "Ring", "Necklace", "Bracelet" 
                };
                _itemTypeFilterDropdown.AddOptions(typeOptions);
                _itemTypeFilterDropdown.onValueChanged.AddListener(OnItemTypeFilterChanged);
            }
        }
        
        private void SetupButtons()
        {
            if (_purchaseButton != null)
                _purchaseButton.onClick.AddListener(OnPurchaseClicked);
            
            if (_closeButton != null)
                _closeButton.onClick.AddListener(OnCloseClicked);
        }
        
        /// <summary>
        /// Tüm shop item'larını yükle
        /// </summary>
        private void LoadAllShopItems()
        {
            _allShopItems.Clear();
            
            // Resources'tan tüm ItemData'ları yükle
            ItemData[] allItems = Resources.LoadAll<ItemData>("Items");
            
            foreach (var item in allItems)
            {
                if (item.canBeBought && item.shopPrice > 0)
                {
                    _allShopItems.Add(item);
                }
            }
            
            Debug.Log($"[ItemShop] Loaded {_allShopItems.Count} shop items");
        }
        
        /// <summary>
        /// Class filter değişti
        /// </summary>
        private void OnClassFilterChanged(int index)
        {
            _selectedClass = (ItemClass)index; // 0=All, 1=Mage, 2=Warrior, 3=Ninja
            RefreshItemList();
        }
        
        /// <summary>
        /// Item type filter değişti
        /// </summary>
        private void OnItemTypeFilterChanged(int index)
        {
            _selectedSlot = (EquipmentSlot)index;
            RefreshItemList();
        }
        
        /// <summary>
        /// UI'yi yenile
        /// </summary>
        public void RefreshUI()
        {
            UpdateCurrencyDisplay();
            RefreshItemList();
            UpdateItemDetailPanel();
        }
        
        /// <summary>
        /// Currency gösterimini güncelle
        /// </summary>
        private void UpdateCurrencyDisplay()
        {
            var playerData = GameManager.Instance?.CurrentPlayerData;
            if (playerData == null) return;
            
            if (_goldText != null)
                _goldText.text = $"Gold: {playerData.gold}";
        }
        
        /// <summary>
        /// Item listesini yenile (filtrelere göre)
        /// </summary>
        private void RefreshItemList()
        {
            // Mevcut item'ları temizle
            foreach (Transform child in _itemListContent)
            {
                Destroy(child.gameObject);
            }
            
            // Filtrelenmiş item'ları göster
            var filteredItems = _allShopItems
                .Where(item => (item.requiredClass == _selectedClass || _selectedClass == ItemClass.All || item.requiredClass == ItemClass.All))
                .Where(item => item.slot == _selectedSlot)
                .OrderBy(item => item.rarity)
                .ThenBy(item => item.level);
            
            foreach (var item in filteredItems)
            {
                CreateItemCard(item);
            }
        }
        
        /// <summary>
        /// Item card oluştur
        /// </summary>
        private void CreateItemCard(ItemData item)
        {
            if (_itemCardPrefab == null || _itemListContent == null)
                return;
            
            GameObject card = Instantiate(_itemCardPrefab, _itemListContent);
            
            // Item bilgilerini ayarla
            TextMeshProUGUI nameText = card.transform.Find("NameText")?.GetComponent<TextMeshProUGUI>();
            if (nameText != null)
            {
                nameText.text = item.itemName;
                nameText.color = GetRarityColor(item.rarity);
            }
            
            TextMeshProUGUI priceText = card.transform.Find("PriceText")?.GetComponent<TextMeshProUGUI>();
            if (priceText != null)
                priceText.text = $"{item.shopPrice} Gold";
            
            Image iconImage = card.transform.Find("IconImage")?.GetComponent<Image>();
            if (iconImage != null && item.icon != null)
                iconImage.sprite = item.icon;
            
            // Click event
            Button button = card.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(() => OnItemSelected(item));
            }
        }
        
        /// <summary>
        /// Item seçildi
        /// </summary>
        private void OnItemSelected(ItemData item)
        {
            _selectedItem = item;
            UpdateItemDetailPanel();
        }
        
        /// <summary>
        /// Item detay panelini güncelle
        /// </summary>
        private void UpdateItemDetailPanel()
        {
            if (_itemDetailPanel == null)
                return;
            
            if (_selectedItem == null)
            {
                _itemDetailPanel.SetActive(false);
                return;
            }
            
            _itemDetailPanel.SetActive(true);
            
            // Item bilgileri
            if (_itemNameText != null)
            {
                _itemNameText.text = _selectedItem.itemName;
                _itemNameText.color = GetRarityColor(_selectedItem.rarity);
            }
            
            if (_itemDescriptionText != null)
                _itemDescriptionText.text = _selectedItem.description;
            
            if (_itemIconImage != null && _selectedItem.icon != null)
                _itemIconImage.sprite = _selectedItem.icon;
            
            // Stats
            if (_itemStatsText != null)
            {
                string stats = "";
                if (_selectedItem.healthBonus > 0)
                    stats += $"+{_selectedItem.healthBonus} HP\n";
                if (_selectedItem.staminaBonus > 0)
                    stats += $"+{_selectedItem.staminaBonus} Stamina\n";
                if (_selectedItem.damageBonus > 0)
                    stats += $"+{_selectedItem.damageBonus} Damage\n";
                if (_selectedItem.armorBonus > 0)
                    stats += $"+{_selectedItem.armorBonus} Armor\n";
                if (_selectedItem.magicResistanceBonus > 0)
                    stats += $"+{_selectedItem.magicResistanceBonus} Magic Resist\n";
                if (_selectedItem.critChanceBonus > 0)
                    stats += $"+{(_selectedItem.critChanceBonus * 100):F1}% Crit Chance\n";
                if (_selectedItem.critDamageBonus > 0)
                    stats += $"+{(_selectedItem.critDamageBonus * 100):F1}% Crit Damage\n";
                
                _itemStatsText.text = stats.TrimEnd('\n');
            }
            
            // Shop price
            if (_shopPriceText != null)
            {
                _shopPriceText.text = $"Price: {_selectedItem.shopPrice} Gold";
            }
            
            // Purchase button durumu
            if (_purchaseButton != null)
            {
                bool canPurchase = CanPurchaseItem(_selectedItem);
                _purchaseButton.interactable = canPurchase;
            }
        }
        
        /// <summary>
        /// Item satın alınabilir mi?
        /// </summary>
        private bool CanPurchaseItem(ItemData item)
        {
            if (item == null || !item.canBeBought)
                return false;
            
            var playerData = GameManager.Instance?.CurrentPlayerData;
            if (playerData == null)
                return false;
            
            // Gold kontrolü
            return playerData.gold >= item.shopPrice;
        }
        
        /// <summary>
        /// Purchase butonuna tıklandı
        /// </summary>
        private void OnPurchaseClicked()
        {
            if (_selectedItem == null)
                return;
            
            if (!CanPurchaseItem(_selectedItem))
            {
                Debug.LogWarning("[ItemShop] Cannot purchase item - insufficient gold");
                return;
            }
            
            // Purchase işlemini yap
            PurchaseItem(_selectedItem);
        }
        
        /// <summary>
        /// Item'i satın al
        /// </summary>
        private void PurchaseItem(ItemData item)
        {
            var playerData = GameManager.Instance?.CurrentPlayerData;
            if (playerData == null)
                return;
            
            // Gold tüket
            playerData.gold -= item.shopPrice;
            
            // Item'i inventory'ye ekle
            playerData.AddItem(item.itemId, 1);
            
            // Save
            GameManager.Instance?.SavePlayerData();
            
            Debug.Log($"[ItemShop] Purchased: {item.itemName} for {item.shopPrice} Gold");
            
            // UI'yi güncelle
            RefreshUI();
        }
        
        /// <summary>
        /// Rarity rengini al
        /// </summary>
        private Color GetRarityColor(ItemRarity rarity)
        {
            switch (rarity)
            {
                case ItemRarity.Common: return Color.gray;
                case ItemRarity.Uncommon: return Color.green;
                case ItemRarity.Rare: return Color.blue;
                case ItemRarity.Epic: return new Color(0.6f, 0.2f, 0.8f); // Purple
                case ItemRarity.Legendary: return new Color(1f, 0.5f, 0f); // Orange
                default: return Color.white;
            }
        }
        
        private void OnCloseClicked()
        {
            gameObject.SetActive(false);
        }
    }
}

