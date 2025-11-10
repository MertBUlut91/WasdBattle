using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using WasdBattle.Core;
using WasdBattle.Data;

namespace WasdBattle.UI
{
    /// <summary>
    /// Equipment ve inventory yönetim paneli
    /// Sol: Item listesi (class-filtered), Orta: 3D karakter, Sağ: Equipment slots + Stats
    /// </summary>
    public class EquipmentUI : MonoBehaviour
    {
        [Header("Panel")]
        [SerializeField] private GameObject _panel;
        [SerializeField] private Button _closeButton;
        
        [Header("Item List (Sol Panel)")]
        [SerializeField] private Transform _itemListContent;
        [SerializeField] private GameObject _itemCardPrefab;
        
        [Header("Item Filter Buttons")]
        [SerializeField] private Button _allTabButton;
        [SerializeField] private Button _weaponsTabButton;
        [SerializeField] private Button _helmetTabButton;
        [SerializeField] private Button _chestTabButton;
        [SerializeField] private Button _glovesTabButton;
        [SerializeField] private Button _legsTabButton;
        [SerializeField] private Button _ringTabButton;
        [SerializeField] private Button _necklaceTabButton;
        [SerializeField] private Button _braceletTabButton;
        
        [Header("Equipment Slots (Sağ Panel - Üst)")]
        [SerializeField] private EquipmentSlotUI _helmetSlot;
        [SerializeField] private EquipmentSlotUI _chestSlot;
        [SerializeField] private EquipmentSlotUI _glovesSlot;
        [SerializeField] private EquipmentSlotUI _legsSlot;
        [SerializeField] private EquipmentSlotUI _weaponSlot;
        [SerializeField] private EquipmentSlotUI _ring1Slot;
        [SerializeField] private EquipmentSlotUI _ring2Slot;
        [SerializeField] private EquipmentSlotUI _necklaceSlot;
        [SerializeField] private EquipmentSlotUI _braceletSlot;
        
        [Header("Stats Display (Sağ Panel - Alt)")]
        [SerializeField] private TextMeshProUGUI _hpStatText;
        [SerializeField] private TextMeshProUGUI _staminaStatText;
        [SerializeField] private TextMeshProUGUI _armorStatText;
        [SerializeField] private TextMeshProUGUI _magicResistStatText;
        
        [Header("References")]
        [SerializeField] private CharacterDisplayController _characterDisplayController;
        [SerializeField] private MainMenuUI _mainMenuUI;
        
        private string _selectedCharacterId;
        private CharacterLoadout _currentLoadout;
        private ItemFilter _currentFilter = ItemFilter.All;
        private ItemData _hoveredItem; // Mouse üzerindeki item
        private List<GameObject> _itemCards = new List<GameObject>();
        
        private void Start()
        {
            SetupButtons();
            SetupEquipmentSlots();
        }
        
        private void SetupButtons()
        {
            if (_closeButton != null)
                _closeButton.onClick.AddListener(ClosePanel);
            
            // Item filter buttons
            if (_allTabButton != null)
                _allTabButton.onClick.AddListener(() => OnFilterChanged(ItemFilter.All));
            
            if (_weaponsTabButton != null)
                _weaponsTabButton.onClick.AddListener(() => OnFilterChanged(ItemFilter.Weapons));
            
            if (_helmetTabButton != null)
                _helmetTabButton.onClick.AddListener(() => OnFilterChanged(ItemFilter.Helmet));
            
            if (_chestTabButton != null)
                _chestTabButton.onClick.AddListener(() => OnFilterChanged(ItemFilter.Chest));
            
            if (_glovesTabButton != null)
                _glovesTabButton.onClick.AddListener(() => OnFilterChanged(ItemFilter.Gloves));
            
            if (_legsTabButton != null)
                _legsTabButton.onClick.AddListener(() => OnFilterChanged(ItemFilter.Legs));
            
            if (_ringTabButton != null)
                _ringTabButton.onClick.AddListener(() => OnFilterChanged(ItemFilter.Ring));
            
            if (_necklaceTabButton != null)
                _necklaceTabButton.onClick.AddListener(() => OnFilterChanged(ItemFilter.Necklace));
            
            if (_braceletTabButton != null)
                _braceletTabButton.onClick.AddListener(() => OnFilterChanged(ItemFilter.Bracelet));
        }
        
        private void SetupEquipmentSlots()
        {
            // Her slot için unequip button event'i
            if (_helmetSlot != null)
                _helmetSlot.SetUnequipCallback(() => OnUnequipItem(EquipmentSlot.Helmet));
            
            if (_chestSlot != null)
                _chestSlot.SetUnequipCallback(() => OnUnequipItem(EquipmentSlot.Chest));
            
            if (_glovesSlot != null)
                _glovesSlot.SetUnequipCallback(() => OnUnequipItem(EquipmentSlot.Gloves));
            
            if (_legsSlot != null)
                _legsSlot.SetUnequipCallback(() => OnUnequipItem(EquipmentSlot.Legs));
            
            if (_weaponSlot != null)
                _weaponSlot.SetUnequipCallback(() => OnUnequipItem(EquipmentSlot.Weapon));
            
            if (_ring1Slot != null)
                _ring1Slot.SetUnequipCallback(() => OnUnequipItem(EquipmentSlot.Ring1));
            
            if (_ring2Slot != null)
                _ring2Slot.SetUnequipCallback(() => OnUnequipItem(EquipmentSlot.Ring2));
            
            if (_necklaceSlot != null)
                _necklaceSlot.SetUnequipCallback(() => OnUnequipItem(EquipmentSlot.Necklace));
            
            if (_braceletSlot != null)
                _braceletSlot.SetUnequipCallback(() => OnUnequipItem(EquipmentSlot.Bracelet));
        }
        
        public void OpenPanel()
        {
            if (_panel != null)
                _panel.SetActive(true);
            
            // Seçili karakteri al
            var playerData = GameManager.Instance?.CurrentPlayerData;
            if (playerData != null && !string.IsNullOrEmpty(playerData.selectedCharacterId))
            {
                _selectedCharacterId = playerData.selectedCharacterId;
                _currentLoadout = playerData.GetLoadoutForCharacter(_selectedCharacterId);
                
                LoadItemList();
                UpdateEquipmentSlots();
                UpdateStats(null); // Mevcut statları göster
            }
        }
        
        public void ClosePanel()
        {
            if (_panel != null)
                _panel.SetActive(false);
            
            // Main menu'ye kamerayı döndür
            if (_mainMenuUI != null)
                _mainMenuUI.OnPanelClosed();
        }
        
        /// <summary>
        /// Inventory listesini yenile (salvage sonrası gibi durumlarda)
        /// </summary>
        public void RefreshInventoryList()
        {
            LoadItemList();
        }
        
        private void LoadItemList()
        {
            // Mevcut listeyi temizle
            foreach (var card in _itemCards)
            {
                if (card != null)
                    Destroy(card);
            }
            _itemCards.Clear();
            
            var playerData = GameManager.Instance?.CurrentPlayerData;
            if (playerData == null) return;
            
            // Seçili karakterin class'ını al
            CharacterData characterData = LoadCharacterData(_selectedCharacterId);
            if (characterData == null) return;
            
            ItemClass characterClass = ConvertCharacterClassToItemClass(characterData.characterClass);
            
            // Owned items'ı yükle ve filtrele
            foreach (var itemId in playerData.ownedItems)
            {
                ItemData itemData = LoadItemData(itemId);
                if (itemData != null)
                {
                    // Class kontrolü
                    if (!itemData.CanBeEquippedBy(characterClass))
                        continue;
                    
                    // Filter kontrolü
                    if (!PassesFilter(itemData))
                        continue;
                    
                    // Equipped itemleri inventory'den gizle
                    // Sadece available (unequipped) itemleri göster
                    int totalCount = playerData.GetItemCount(itemId);
                    int equippedCount = GetEquippedItemCount(itemId);
                    int availableCount = totalCount - equippedCount;
                    
                    // Eğer tüm itemler equipped ise, inventory'de gösterme
                    if (availableCount <= 0)
                        continue;
                    
                    CreateItemCard(itemData);
                }
            }
        }
        
        private bool PassesFilter(ItemData itemData)
        {
            switch (_currentFilter)
            {
                case ItemFilter.All:
                    return true;
                
                case ItemFilter.Weapons:
                    return itemData.slot == EquipmentSlot.Weapon;
                
                case ItemFilter.Helmet:
                    return itemData.slot == EquipmentSlot.Helmet;
                
                case ItemFilter.Chest:
                    return itemData.slot == EquipmentSlot.Chest;
                
                case ItemFilter.Gloves:
                    return itemData.slot == EquipmentSlot.Gloves;
                
                case ItemFilter.Legs:
                    return itemData.slot == EquipmentSlot.Legs;
                
                case ItemFilter.Ring:
                    return itemData.slot == EquipmentSlot.Ring1 || itemData.slot == EquipmentSlot.Ring2;
                
                case ItemFilter.Necklace:
                    return itemData.slot == EquipmentSlot.Necklace;
                
                case ItemFilter.Bracelet:
                    return itemData.slot == EquipmentSlot.Bracelet;
                
                default:
                    return true;
            }
        }
        
        private void CreateItemCard(ItemData itemData)
        {
            if (_itemCardPrefab == null || _itemListContent == null)
                return;
            
            GameObject card = Instantiate(_itemCardPrefab, _itemListContent);
            _itemCards.Add(card);
            
            // ItemCardUI script'ini kullan
            var cardUI = card.GetComponent<ItemCardUI>();
            if (cardUI != null)
            {
                // Item equipped mi kontrol et (inventory'deki itemler asla equipped gösterilmez artık)
                bool isEquipped = false;
                
                // Available count'u hesapla (total - equipped)
                var playerData = GameManager.Instance?.CurrentPlayerData;
                int totalCount = playerData != null ? playerData.GetItemCount(itemData.itemId) : 1;
                int equippedCount = GetEquippedItemCount(itemData.itemId);
                int availableCount = totalCount - equippedCount;
                
                // Setup ile tüm UI'ı ayarla (artık double-click için)
                cardUI.Setup(
                    itemData.icon,
                    itemData.itemName,
                    GetRarityColor(itemData.rarity),
                    isEquipped,
                    availableCount,  // Sadece available count göster
                    () => OnItemDoubleClicked(itemData)  // Double-click callback
                );
                
                // ItemData'yı set et (drag-and-drop için)
                cardUI.SetItemData(itemData);
            }
            else
            {
                Debug.LogWarning("[EquipmentUI] ItemCardUI component not found on prefab!");
                
                // Fallback: Manuel setup
                var iconImage = card.transform.Find("Icon")?.GetComponent<Image>();
                if (iconImage != null && itemData.icon != null)
                {
                    iconImage.sprite = itemData.icon;
                }
                
                var nameText = card.transform.Find("Name")?.GetComponent<TextMeshProUGUI>();
                if (nameText != null)
                    nameText.text = itemData.itemName;
                
                var button = card.GetComponent<Button>();
                if (button != null)
                {
                    button.onClick.AddListener(() => OnItemDoubleClicked(itemData));
                }
            }
            
            // Hover events (stat comparison için)
            var eventTrigger = card.GetComponent<UnityEngine.EventSystems.EventTrigger>();
            if (eventTrigger == null)
                eventTrigger = card.AddComponent<UnityEngine.EventSystems.EventTrigger>();
            
            // Pointer Enter
            var pointerEnter = new UnityEngine.EventSystems.EventTrigger.Entry();
            pointerEnter.eventID = UnityEngine.EventSystems.EventTriggerType.PointerEnter;
            pointerEnter.callback.AddListener((data) => { OnItemHoverEnter(itemData); });
            eventTrigger.triggers.Add(pointerEnter);
            
            // Pointer Exit
            var pointerExit = new UnityEngine.EventSystems.EventTrigger.Entry();
            pointerExit.eventID = UnityEngine.EventSystems.EventTriggerType.PointerExit;
            pointerExit.callback.AddListener((data) => { OnItemHoverExit(); });
            eventTrigger.triggers.Add(pointerExit);
        }
        
        private bool IsItemEquipped(ItemData itemData)
        {
            if (_currentLoadout == null)
                return false;
            
            // Item'in slot'una göre kontrol et
            switch (itemData.slot)
            {
                case EquipmentSlot.Helmet:
                    return _currentLoadout.equippedHelmet == itemData.itemId;
                case EquipmentSlot.Chest:
                    return _currentLoadout.equippedChest == itemData.itemId;
                case EquipmentSlot.Gloves:
                    return _currentLoadout.equippedGloves == itemData.itemId;
                case EquipmentSlot.Legs:
                    return _currentLoadout.equippedLegs == itemData.itemId;
                case EquipmentSlot.Weapon:
                    return _currentLoadout.equippedWeapon == itemData.itemId;
                case EquipmentSlot.Ring1:
                case EquipmentSlot.Ring2:
                    return _currentLoadout.equippedRing1 == itemData.itemId || 
                           _currentLoadout.equippedRing2 == itemData.itemId;
                case EquipmentSlot.Necklace:
                    return _currentLoadout.equippedNecklace == itemData.itemId;
                case EquipmentSlot.Bracelet:
                    return _currentLoadout.equippedBracelet == itemData.itemId;
                default:
                    return false;
            }
        }
        
        /// <summary>
        /// Belirtilen item'ın TÜM karakterlerde kaç adet equipped olduğunu sayar
        /// (Global inventory'den düşmek için)
        /// </summary>
        private int GetEquippedItemCount(string itemId)
        {
            var playerData = GameManager.Instance?.CurrentPlayerData;
            if (playerData == null)
                return 0;
            
            int totalEquippedCount = 0;
            
            // TÜM karakterlerin loadout'larını kontrol et
            foreach (var loadout in playerData.characterLoadouts)
            {
                if (loadout.equippedHelmet == itemId) totalEquippedCount++;
                if (loadout.equippedChest == itemId) totalEquippedCount++;
                if (loadout.equippedGloves == itemId) totalEquippedCount++;
                if (loadout.equippedLegs == itemId) totalEquippedCount++;
                if (loadout.equippedWeapon == itemId) totalEquippedCount++;
                if (loadout.equippedRing1 == itemId) totalEquippedCount++;
                if (loadout.equippedRing2 == itemId) totalEquippedCount++;
                if (loadout.equippedNecklace == itemId) totalEquippedCount++;
                if (loadout.equippedBracelet == itemId) totalEquippedCount++;
            }
            
            return totalEquippedCount;
        }
        
        /// <summary>
        /// Double-click ile item equip etme
        /// </summary>
        private void OnItemDoubleClicked(ItemData itemData)
        {
            Debug.Log($"[EquipmentUI] Item double-clicked: {itemData.itemName} ({itemData.slot})");
            EquipItem(itemData, itemData.slot);
        }
        
        /// <summary>
        /// Drag-and-drop ile item equip etme
        /// </summary>
        public void EquipItemFromDrag(ItemData itemData, EquipmentSlot targetSlot)
        {
            Debug.Log($"[EquipmentUI] Item dragged: {itemData.itemName} to slot {targetSlot}");
            EquipItem(itemData, targetSlot);
        }
        
        /// <summary>
        /// Item'i belirtilen slot'a equip eder (ortak method)
        /// </summary>
        private void EquipItem(ItemData itemData, EquipmentSlot targetSlot)
        {
            var playerData = GameManager.Instance?.CurrentPlayerData;
            if (playerData == null || _currentLoadout == null)
            {
                Debug.LogError("[EquipmentUI] PlayerData or CurrentLoadout is null!");
                return;
            }
            
            // Ring için özel kontrol: Aynı ring'den 2 tane equipped mi?
            if (targetSlot == EquipmentSlot.Ring1 || targetSlot == EquipmentSlot.Ring2)
            {
                int equippedCount = _currentLoadout.GetEquippedRingCount(itemData.itemId);
                int inventoryCount = playerData.GetItemCount(itemData.itemId);
                
                // Eğer zaten 2 tane equipped ise ve inventory'de daha fazla yoksa, equip etme
                if (equippedCount >= 2)
                {
                    Debug.LogWarning($"[EquipmentUI] Both ring slots already have {itemData.itemName}");
                    return;
                }
                
                // Eğer inventory'de yeterli yoksa, equip etme
                if (inventoryCount < equippedCount + 1)
                {
                    Debug.LogWarning($"[EquipmentUI] Not enough {itemData.itemName} in inventory (have: {inventoryCount}, need: {equippedCount + 1})");
                    return;
                }
            }
            
            // Item'i equip et (targetSlot kullanarak)
            _currentLoadout.EquipItem(targetSlot, itemData.itemId);
            
            Debug.Log($"[EquipmentUI] Item equipped: {itemData.itemId} to slot {targetSlot}");
            
            // UI'ı güncelle
            UpdateEquipmentSlots();
            UpdateStats(null);
            LoadItemList(); // Item listesini yenile (equipped indicator için)
            
            // Cloud Save
            if (GameManager.Instance != null && GameManager.Instance.DataManager != null)
            {
                GameManager.Instance.DataManager.SavePlayerDataAsync(playerData);
            }
        }
        
        private void OnItemHoverEnter(ItemData itemData)
        {
            _hoveredItem = itemData;
            UpdateStats(itemData);
        }
        
        private void OnItemHoverExit()
        {
            _hoveredItem = null;
            UpdateStats(null);
        }
        
        private void OnUnequipItem(EquipmentSlot slot)
        {
            if (_currentLoadout != null)
            {
                _currentLoadout.UnequipItem(slot);
                
                UpdateEquipmentSlots();
                UpdateStats(null);
                LoadItemList(); // Inventory'yi yenile (unequipped item tekrar görünsün)
                
                // Cloud Save
                GameManager.Instance?.SavePlayerData();
            }
        }
        
        private void UpdateEquipmentSlots()
        {
            if (_currentLoadout == null) return;
            
            UpdateSlot(_helmetSlot, EquipmentSlot.Helmet);
            UpdateSlot(_chestSlot, EquipmentSlot.Chest);
            UpdateSlot(_glovesSlot, EquipmentSlot.Gloves);
            UpdateSlot(_legsSlot, EquipmentSlot.Legs);
            UpdateSlot(_weaponSlot, EquipmentSlot.Weapon);
            UpdateSlot(_ring1Slot, EquipmentSlot.Ring1);
            UpdateSlot(_ring2Slot, EquipmentSlot.Ring2);
            UpdateSlot(_necklaceSlot, EquipmentSlot.Necklace);
            UpdateSlot(_braceletSlot, EquipmentSlot.Bracelet);
        }
        
        private void UpdateSlot(EquipmentSlotUI slotUI, EquipmentSlot slot)
        {
            if (slotUI == null) return;
            
            string itemId = _currentLoadout.GetEquippedItem(slot);
            
            if (!string.IsNullOrEmpty(itemId))
            {
                ItemData itemData = LoadItemData(itemId);
                if (itemData != null)
                {
                    slotUI.SetItem(itemData);
                }
            }
            else
            {
                slotUI.Clear();
            }
        }
        
        private void UpdateStats(ItemData previewItem)
        {
            CharacterData characterData = LoadCharacterData(_selectedCharacterId);
            if (characterData == null) return;
            
            // Base stats
            int baseHP = characterData.baseHealth;
            int baseStamina = characterData.baseStamina;
            int baseArmor = 0;
            int baseMagicResist = 0;
            
            // Equipped items bonusları
            int equippedHP = 0;
            int equippedStamina = 0;
            int equippedArmor = 0;
            int equippedMagicResist = 0;
            
            if (_currentLoadout != null)
            {
                var equippedItems = _currentLoadout.GetAllEquippedItems();
                foreach (var itemId in equippedItems)
                {
                    ItemData itemData = LoadItemData(itemId);
                    if (itemData != null)
                    {
                        equippedHP += itemData.healthBonus;
                        equippedStamina += itemData.staminaBonus;
                        equippedArmor += itemData.armorBonus;
                        equippedMagicResist += itemData.magicResistanceBonus;
                    }
                }
            }
            
            int currentHP = baseHP + equippedHP;
            int currentStamina = baseStamina + equippedStamina;
            int currentArmor = baseArmor + equippedArmor;
            int currentMagicResist = baseMagicResist + equippedMagicResist;
            
            // Preview item varsa, stat değişimini göster
            if (previewItem != null)
            {
                // Eğer aynı slot'ta başka bir item varsa, onu çıkar
                string currentItemId = _currentLoadout?.GetEquippedItem(previewItem.slot);
                if (!string.IsNullOrEmpty(currentItemId))
                {
                    ItemData currentItem = LoadItemData(currentItemId);
                    if (currentItem != null)
                    {
                        equippedHP -= currentItem.healthBonus;
                        equippedStamina -= currentItem.staminaBonus;
                        equippedArmor -= currentItem.armorBonus;
                        equippedMagicResist -= currentItem.magicResistanceBonus;
                    }
                }
                
                // Yeni item'i ekle
                int newHP = baseHP + equippedHP + previewItem.healthBonus;
                int newStamina = baseStamina + equippedStamina + previewItem.staminaBonus;
                int newArmor = baseArmor + equippedArmor + previewItem.armorBonus;
                int newMagicResist = baseMagicResist + equippedMagicResist + previewItem.magicResistanceBonus;
                
                // Stat değişimini göster (ok ile)
                if (_hpStatText != null)
                    _hpStatText.text = GetStatChangeText("HP", currentHP, newHP);
                
                if (_staminaStatText != null)
                    _staminaStatText.text = GetStatChangeText("Stamina", currentStamina, newStamina);
                
                if (_armorStatText != null)
                    _armorStatText.text = GetStatChangeText("Armor", currentArmor, newArmor);
                
                if (_magicResistStatText != null)
                    _magicResistStatText.text = GetStatChangeText("Magic Resist", currentMagicResist, newMagicResist);
            }
            else
            {
                // Sadece mevcut statları göster
                if (_hpStatText != null)
                    _hpStatText.text = $"HP: {currentHP}";
                
                if (_staminaStatText != null)
                    _staminaStatText.text = $"Stamina: {currentStamina}";
                
                if (_armorStatText != null)
                    _armorStatText.text = $"Armor: {currentArmor}";
                
                if (_magicResistStatText != null)
                    _magicResistStatText.text = $"Magic Resist: {currentMagicResist}";
            }
        }
        
        private string GetStatChangeText(string statName, int current, int newValue)
        {
            if (newValue > current)
            {
                // Artış: sadece yeşil toplam göster
                return $"{statName}: <color=green>{newValue}</color>";
            }
            else if (newValue < current)
            {
                // Azalış: sadece kırmızı toplam göster
                return $"{statName}: <color=red>{newValue}</color>";
            }
            else
            {
                // Değişim yok: normal göster
                return $"{statName}: {current}";
            }
        }
        
        private void OnFilterChanged(ItemFilter filter)
        {
            Debug.Log($"[EquipmentUI] Filter changed to: {filter}");
            _currentFilter = filter;
            LoadItemList();
        }
        
        private Color GetRarityColor(ItemRarity rarity)
        {
            switch (rarity)
            {
                case ItemRarity.Common:
                    return new Color(0.6f, 0.6f, 0.6f); // Gri
                case ItemRarity.Uncommon:
                    return new Color(0.2f, 0.8f, 0.2f); // Yeşil
                case ItemRarity.Rare:
                    return new Color(0.2f, 0.4f, 1f); // Mavi
                case ItemRarity.Epic:
                    return new Color(0.6f, 0.2f, 0.8f); // Mor
                case ItemRarity.Legendary:
                    return new Color(1f, 0.5f, 0f); // Turuncu
                default:
                    return Color.white;
            }
        }
        
        private ItemClass ConvertCharacterClassToItemClass(CharacterClass characterClass)
        {
            switch (characterClass)
            {
                case CharacterClass.Mage:
                    return ItemClass.Mage;
                case CharacterClass.Warrior:
                    return ItemClass.Warrior;
                case CharacterClass.Ninja:
                    return ItemClass.Ninja;
                default:
                    return ItemClass.All;
            }
        }
        
        private CharacterData LoadCharacterData(string characterId)
        {
            CharacterData[] allCharacters = Resources.LoadAll<CharacterData>("Characters");
            foreach (var character in allCharacters)
            {
                if (character.characterId == characterId)
                    return character;
            }
            return null;
        }
        
        private ItemData LoadItemData(string itemId)
        {
            ItemData[] allItems = Resources.LoadAll<ItemData>("Items");
            foreach (var item in allItems)
            {
                if (item.itemId == itemId)
                    return item;
            }
            return null;
        }
    }
    
    /// <summary>
    /// Equipment slot UI helper class
    /// </summary>
    [System.Serializable]
    public class EquipmentSlotUI
    {
        public Image itemIcon;
        public TextMeshProUGUI slotNameText;
        public Button unequipButton;
        public GameObject emptySlotIndicator;
        
        private System.Action _unequipCallback;
        
        public void SetUnequipCallback(System.Action callback)
        {
            _unequipCallback = callback;
            if (unequipButton != null)
            {
                unequipButton.onClick.RemoveAllListeners();
                unequipButton.onClick.AddListener(() => _unequipCallback?.Invoke());
            }
        }
        
        public void SetItem(ItemData item)
        {
            if (itemIcon != null)
            {
                itemIcon.sprite = item.icon;
                itemIcon.enabled = true;
            }
            
            if (unequipButton != null)
                unequipButton.gameObject.SetActive(true);
            
            if (emptySlotIndicator != null)
                emptySlotIndicator.SetActive(false);
        }
        
        public void Clear()
        {
            if (itemIcon != null)
            {
                itemIcon.sprite = null;
                itemIcon.enabled = false;
            }
            
            if (unequipButton != null)
                unequipButton.gameObject.SetActive(false);
            
            if (emptySlotIndicator != null)
                emptySlotIndicator.SetActive(true);
        }
    }
    
    public enum ItemFilter
    {
        All,
        Weapons,
        Helmet,
        Chest,
        Gloves,
        Legs,
        Ring,
        Necklace,
        Bracelet
    }
}
