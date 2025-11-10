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
    /// Item crafting UI - Class ve item type bazlı filtreleme
    /// </summary>
    public class ItemCraftUI : MonoBehaviour
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
        [SerializeField] private TextMeshProUGUI _craftCostText;
        
        [Header("Buttons")]
        [SerializeField] private Button _craftButton;
        [SerializeField] private Button _closeButton;
        
        [Header("Currency Display")]
        [SerializeField] private TextMeshProUGUI _goldText;
        [SerializeField] private TextMeshProUGUI _metalText;
        [SerializeField] private TextMeshProUGUI _crystalText;
        [SerializeField] private TextMeshProUGUI _runeText;
        [SerializeField] private TextMeshProUGUI _essenceText;
        
        private ItemData _selectedItem;
        private ItemClass _selectedClass = ItemClass.All;
        private EquipmentSlot _selectedSlot = EquipmentSlot.Helmet;
        private List<ItemData> _allCraftableItems = new List<ItemData>();
        
        private void Start()
        {
            SetupDropdowns();
            SetupButtons();
            LoadAllCraftableItems();
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
            if (_craftButton != null)
                _craftButton.onClick.AddListener(OnCraftClicked);
            
            if (_closeButton != null)
                _closeButton.onClick.AddListener(OnCloseClicked);
        }
        
        /// <summary>
        /// Tüm craftable item'ları yükle
        /// </summary>
        private void LoadAllCraftableItems()
        {
            _allCraftableItems.Clear();
            
            // Resources'tan tüm ItemData'ları yükle
            ItemData[] allItems = Resources.LoadAll<ItemData>("Items");
            
            foreach (var item in allItems)
            {
                if (item.canBeCrafted)
                {
                    _allCraftableItems.Add(item);
                }
            }
            
            Debug.Log($"[ItemCraft] Loaded {_allCraftableItems.Count} craftable items");
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
            
            if (_metalText != null)
                _metalText.text = $"Metal: {playerData.metal}";
            
            if (_crystalText != null)
                _crystalText.text = $"Crystal: {playerData.energyCrystal}";
            
            if (_runeText != null)
                _runeText.text = $"Rune: {playerData.rune}";
            
            if (_essenceText != null)
                _essenceText.text = $"Essence: {playerData.essence}";
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
            var filteredItems = _allCraftableItems
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
            
            TextMeshProUGUI levelText = card.transform.Find("LevelText")?.GetComponent<TextMeshProUGUI>();
            if (levelText != null)
                levelText.text = $"Lv.{item.level}";
            
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
            
            // Craft cost
            if (_craftCostText != null)
            {
                string cost = "Craft Cost:\n";
                if (_selectedItem.craftingMaterials != null)
                {
                    foreach (var material in _selectedItem.craftingMaterials)
                    {
                        if (material.amount > 0)
                        {
                            cost += $"• {material.materialType}: {material.amount}\n";
                        }
                    }
                }
                _craftCostText.text = cost.TrimEnd('\n');
            }
            
            // Craft button durumu
            if (_craftButton != null)
            {
                bool canCraft = CanCraftItem(_selectedItem);
                _craftButton.interactable = canCraft;
            }
        }
        
        /// <summary>
        /// Item craft edilebilir mi?
        /// </summary>
        private bool CanCraftItem(ItemData item)
        {
            if (item == null || !item.canBeCrafted)
                return false;
            
            var playerData = GameManager.Instance?.CurrentPlayerData;
            if (playerData == null)
                return false;
            
            // Malzeme kontrolü
            if (item.craftingMaterials != null)
            {
                foreach (var material in item.craftingMaterials)
                {
                    int playerAmount = GetMaterialAmount(playerData, material.materialType);
                    if (playerAmount < material.amount)
                        return false;
                }
            }
            
            return true;
        }
        
        /// <summary>
        /// Oyuncunun sahip olduğu materyal miktarı
        /// </summary>
        private int GetMaterialAmount(PlayerData playerData, MaterialType materialType)
        {
            switch (materialType)
            {
                case MaterialType.Metal: return playerData.metal;
                case MaterialType.EnergyCrystal: return playerData.energyCrystal;
                case MaterialType.Rune: return playerData.rune;
                case MaterialType.Essence: return playerData.essence;
                case MaterialType.Leather: return playerData.leather;
                case MaterialType.Cloth: return playerData.cloth;
                case MaterialType.Wood: return playerData.wood;
                case MaterialType.GemStone: return playerData.gemStone;
                case MaterialType.DarkEssence: return playerData.darkEssence;
                case MaterialType.LightEssence: return playerData.lightEssence;
                default: return 0;
            }
        }
        
        /// <summary>
        /// Craft butonuna tıklandı
        /// </summary>
        private void OnCraftClicked()
        {
            if (_selectedItem == null)
                return;
            
            if (!CanCraftItem(_selectedItem))
            {
                Debug.LogWarning("[ItemCraft] Cannot craft item - insufficient materials");
                return;
            }
            
            // Craft işlemini yap
            CraftItem(_selectedItem);
        }
        
        /// <summary>
        /// Item'i craft et
        /// </summary>
        private void CraftItem(ItemData item)
        {
            var playerData = GameManager.Instance?.CurrentPlayerData;
            if (playerData == null)
                return;
            
            // Malzemeleri tüket
            if (item.craftingMaterials != null)
            {
                foreach (var material in item.craftingMaterials)
                {
                    RemoveMaterial(playerData, material.materialType, material.amount);
                }
            }
            
            // Item'i inventory'ye ekle
            playerData.AddItem(item.itemId, 1);
            
            // Save
            GameManager.Instance?.SavePlayerData();
            
            Debug.Log($"[ItemCraft] Crafted: {item.itemName}");
            
            // UI'yi güncelle
            RefreshUI();
        }
        
        /// <summary>
        /// Materyal tüket
        /// </summary>
        private void RemoveMaterial(PlayerData playerData, MaterialType materialType, int amount)
        {
            switch (materialType)
            {
                case MaterialType.Metal: playerData.metal -= amount; break;
                case MaterialType.EnergyCrystal: playerData.energyCrystal -= amount; break;
                case MaterialType.Rune: playerData.rune -= amount; break;
                case MaterialType.Essence: playerData.essence -= amount; break;
                case MaterialType.Leather: playerData.leather -= amount; break;
                case MaterialType.Cloth: playerData.cloth -= amount; break;
                case MaterialType.Wood: playerData.wood -= amount; break;
                case MaterialType.GemStone: playerData.gemStone -= amount; break;
                case MaterialType.DarkEssence: playerData.darkEssence -= amount; break;
                case MaterialType.LightEssence: playerData.lightEssence -= amount; break;
            }
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

