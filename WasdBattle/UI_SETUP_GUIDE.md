# 🎨 UI Setup Guide - Detaylı Kurulum Rehberi

Bu rehber, yeni sistemler için gerekli tüm UI elementlerini **adım adım** nasıl kuracağınızı gösterir.

---

## 📋 İçindekiler

1. [MainMenuScene UI Setup](#1-mainmenuscene-ui-setup)
2. [Equipment System UI](#2-equipment-system-ui)
3. [Skill System UI](#3-skill-system-ui)
4. [Character Selection UI](#4-character-selection-ui)
5. [Inventory UI](#5-inventory-ui)
6. [Crafting UI](#6-crafting-ui)
7. [Shop UI](#7-shop-ui)
8. [Matchmaking UI](#8-matchmaking-ui-inline)

---

## 1. MainMenuScene UI Setup

### 📐 Genel Yapı

```
MainMenuScene
├── Canvas (Main)
│   ├── Background
│   ├── PlayerInfoPanel (Üst Sol)
│   ├── CurrencyPanel (Üst Sağ)
│   ├── FindMatchButton (Ortada Büyük)
│   ├── MatchmakingPanel (Başta Gizli)
│   ├── BottomButtonsPanel
│   ├── CharacterPanel (Başta Gizli)
│   ├── InventoryPanel (Başta Gizli)
│   ├── CraftPanel (Başta Gizli)
│   └── ShopPanel (Başta Gizli)
```

---

### Step 1: Canvas Setup

```
Hierarchy → UI → Canvas → "MainCanvas"

Inspector:
- Canvas Scaler:
  - UI Scale Mode: Scale With Screen Size
  - Reference Resolution: 1920x1080
  - Match: 0.5
```

---

### Step 2: Background

```
MainCanvas → UI → Image → "Background"

Inspector:
- Anchor: Stretch/Stretch (Alt+Shift+Click)
- Color: #1A1A2E (Koyu mavi-gri)
```

---

### Step 3: Player Info Panel (Üst Sol)

```
MainCanvas → UI → Panel → "PlayerInfoPanel"

Inspector:
- Anchor: Top-Left
- Width: 400, Height: 200
- Pos X: 220, Pos Y: -120
```

**İçindekiler:**

```
PlayerInfoPanel altına:

1. Text (TMP) → "UsernameText"
   - Anchor: Top-Center
   - Pos Y: -20
   - Text: "Player Name"
   - Font Size: 32
   - Alignment: Center

2. Text (TMP) → "LevelText"
   - Anchor: Top-Left
   - Pos X: 20, Pos Y: -60
   - Text: "Level: 1"
   - Font Size: 24

3. Text (TMP) → "ELOText"
   - Anchor: Top-Left
   - Pos X: 20, Pos Y: -90
   - Text: "ELO: 1000"
   - Font Size: 24

4. Text (TMP) → "RankText"
   - Anchor: Top-Right
   - Pos X: -20, Pos Y: -60
   - Text: "Bronze"
   - Font Size: 24
   - Alignment: Right

5. Image → "XPBarBackground"
   - Anchor: Bottom-Center
   - Width: 360, Height: 20
   - Pos Y: 20
   - Color: #333333

6. Image → "XPBarFill" (XPBarBackground altına)
   - Anchor: Left-Center
   - Width: 360, Height: 20
   - Image Type: Filled
   - Fill Method: Horizontal
   - Fill Amount: 0.5
   - Color: #4CAF50 (Yeşil)
```

---

### Step 4: Currency Panel (Üst Sağ)

```
MainCanvas → UI → Panel → "CurrencyPanel"

Inspector:
- Anchor: Top-Right
- Width: 300, Height: 150
- Pos X: -170, Pos Y: -95
- Add Component: Vertical Layout Group
  - Spacing: 10
  - Child Alignment: Upper Center
  - Padding: 10
```

**İçindekiler:**

```
CurrencyPanel altına:

1. Panel → "GoldRow"
   - Height: 40
   - Add Component: Horizontal Layout Group
   - Child Force Expand: Width ✓
   
   GoldRow altına:
   - Image → "GoldIcon" (Width: 32, Height: 32)
   - Text (TMP) → "GoldText" (Text: "1000", Font Size: 24)

2. Panel → "GemRow" (Aynı yapı)
   - Image → "GemIcon"
   - Text (TMP) → "GemText"

3. Panel → "DiamondRow" (Aynı yapı)
   - Image → "DiamondIcon"
   - Text (TMP) → "DiamondText"
```

---

### Step 5: Find Match Button (Ortada Büyük)

```
MainCanvas → UI → Button (TMP) → "FindMatchButton"

Inspector:
- Anchor: Center
- Width: 500, Height: 120
- Pos Y: 0

Button altındaki Text (TMP):
- Text: "Find Match"
- Font Size: 56
- Alignment: Center
- Color: White
```

---

### Step 6: Matchmaking Panel (Başta Gizli)

```
MainCanvas → UI → Panel → "MatchmakingPanel"

Inspector:
- Anchor: Center
- Width: 600, Height: 200
- Pos Y: -150
- Active: FALSE (başta kapalı)
```

**İçindekiler:**

```
MatchmakingPanel altına:

1. Text (TMP) → "MatchmakingTimerText"
   - Anchor: Top-Center
   - Pos Y: -40
   - Text: "Searching: 00:00"
   - Font Size: 36
   - Alignment: Center

2. Text (TMP) → "ELORangeText"
   - Anchor: Center
   - Pos Y: 0
   - Text: "ELO Range: 800 - 1200"
   - Font Size: 24
   - Alignment: Center

3. Button (TMP) → "CancelMatchButton"
   - Anchor: Bottom-Center
   - Width: 300, Height: 60
   - Pos Y: 20
   - Text: "Cancel"
   - Color: Red (#FF5252)
```

---

### Step 7: Bottom Buttons Panel

```
MainCanvas → UI → Panel → "BottomButtonsPanel"

Inspector:
- Anchor: Bottom-Center
- Width: 1400, Height: 100
- Pos Y: 70
- Add Component: Horizontal Layout Group
  - Spacing: 20
  - Child Alignment: Middle Center
  - Child Force Expand: Width ✓, Height ✓
```

**İçindekiler:**

```
BottomButtonsPanel altına (4 buton):

1. Button (TMP) → "CharacterButton"
   - Text: "Characters"
   - Font Size: 28

2. Button (TMP) → "InventoryButton"
   - Text: "Inventory"
   - Font Size: 28

3. Button (TMP) → "CraftButton"
   - Text: "Craft"
   - Font Size: 28

4. Button (TMP) → "ShopButton"
   - Text: "Shop"
   - Font Size: 28
```

---

### Step 8: MainMenuUI Script Bağlama

```
MainCanvas → Add Component → Main Menu UI

Inspector'da referansları bağla:
- Username Text: UsernameText
- Level Text: LevelText
- ELO Text: ELOText
- Rank Text: RankText
- XP Bar: XPBarFill
- Gold Text: GoldText (CurrencyPanel içindeki)
- Gem Text: GemText
- Diamond Text: DiamondText
- Play Button: FindMatchButton
- Play Button Text: FindMatchButton içindeki Text
- Matchmaking Panel: MatchmakingPanel
- Matchmaking Timer Text: MatchmakingTimerText
- Cancel Match Button: CancelMatchButton
- Character Button: CharacterButton
- Inventory Button: InventoryButton
- Craft Button: CraftButton
- Shop Button: ShopButton
```

---

## 2. Equipment System UI

### Character Panel (Karakter + Equipment)

```
MainCanvas → UI → Panel → "CharacterPanel"

Inspector:
- Anchor: Stretch/Stretch
- Offset: 0, 0, 0, 0
- Active: FALSE (başta kapalı)
- Color: #000000AA (Yarı saydam siyah)
```

**İçindekiler:**

```
CharacterPanel altına:

1. Panel → "ContentPanel"
   - Anchor: Center
   - Width: 1600, Height: 900
   - Color: #2D2D2D

2. Button (TMP) → "CloseButton" (ContentPanel içinde)
   - Anchor: Top-Right
   - Width: 60, Height: 60
   - Pos X: -10, Pos Y: -10
   - Text: "X"
   - Font Size: 36
   - Color: Red

3. Text (TMP) → "TitleText" (ContentPanel içinde)
   - Anchor: Top-Center
   - Pos Y: -30
   - Text: "Character Selection"
   - Font Size: 48
```

---

### Character Display Area (Sol Taraf)

```
ContentPanel → Panel → "CharacterDisplayArea"

Inspector:
- Anchor: Left-Stretch
- Width: 500
- Offset Left: 20, Top: -100, Bottom: 20
```

**İçindekiler:**

```
CharacterDisplayArea altına:

1. Scroll View → "CharacterListScrollView"
   - Anchor: Stretch/Stretch
   - Content → Add Component: Vertical Layout Group
     - Spacing: 10
     - Padding: 10
   - Content → Add Component: Content Size Fitter
     - Vertical Fit: Preferred Size

Content içine (prefab olarak oluşturulacak):
- Panel → "CharacterItemPrefab" (120 height)
  - Image → "CharacterIcon" (80x80)
  - Text (TMP) → "CharacterName"
  - Text (TMP) → "CharacterLevel"
  - Image → "LockedIcon" (Kilitli ise)
  - Button → "SelectButton"
```

---

### Equipment Slots Area (Sağ Taraf)

```
ContentPanel → Panel → "EquipmentArea"

Inspector:
- Anchor: Right-Stretch
- Width: 1000
- Offset Right: -20, Top: -100, Bottom: 20
```

**İçindekiler:**

```
EquipmentArea altına:

1. Panel → "EquipmentSlotsPanel"
   - Anchor: Top-Left
   - Width: 450, Height: 700
   - Pos X: 20, Pos Y: -20

EquipmentSlotsPanel içine (9 slot):

Slot Template (her biri için):
Panel → "HelmetSlot"
- Width: 120, Height: 120
- Add Component: Image (Border)
- Color: #444444

İçinde:
- Image → "ItemIcon" (100x100, başta boş)
- Text (TMP) → "SlotName" (Bottom, "Helmet")
- Button → "UnequipButton" (Top-Right, "X", başta gizli)

9 Slot Pozisyonları:
┌─────────────┐
│   Helmet    │  (X: 165, Y: -80)
│             │
│   Chest     │  (X: 165, Y: -220)
│             │
│   Gloves    │  (X: 40, Y: -360)  Legs (X: 290, Y: -360)
│             │
│   Weapon    │  (X: 165, Y: -500)
│             │
│   Ring1     │  (X: 40, Y: -620)  Ring2 (X: 165, Y: -620)
│             │
│   Necklace  │  (X: 290, Y: -620)
│             │
│   Bracelet  │  (X: 165, Y: -740)
└─────────────┘
```

---

### Item List Panel (Sağ Alt)

```
EquipmentArea → Panel → "ItemListPanel"

Inspector:
- Anchor: Bottom-Right
- Width: 500, Height: 700
- Pos X: -20, Pos Y: 20
```

**İçindekiler:**

```
ItemListPanel altına:

1. Text (TMP) → "ItemListTitle"
   - Anchor: Top-Center
   - Text: "Available Items"
   - Font Size: 32

2. Dropdown (TMP) → "FilterDropdown"
   - Anchor: Top-Center
   - Pos Y: -50
   - Options: "All", "Helmet", "Chest", "Weapon", vb.

3. Scroll View → "ItemScrollView"
   - Anchor: Stretch/Stretch
   - Offset Top: -100, Bottom: 10
   - Content → Grid Layout Group
     - Cell Size: 100x100
     - Spacing: 10
     - Constraint: Fixed Column Count (4)

Content içine (prefab):
- Panel → "ItemCardPrefab"
  - Image → "ItemIcon"
  - Image → "RarityBorder" (Rarity'ye göre renk)
  - Text (TMP) → "ItemName"
  - Button → "EquipButton"
```

---

### Equipment UI Script

```csharp
// Yeni script oluştur: EquipmentUI.cs

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using WasdBattle.Data;
using WasdBattle.Core;
using System.Collections.Generic;

namespace WasdBattle.UI
{
    public class EquipmentUI : MonoBehaviour
    {
        [Header("Character List")]
        [SerializeField] private ScrollRect _characterScrollView;
        [SerializeField] private Transform _characterListContent;
        [SerializeField] private GameObject _characterItemPrefab;
        
        [Header("Equipment Slots")]
        [SerializeField] private EquipmentSlotUI[] _equipmentSlots; // 9 slot
        
        [Header("Item List")]
        [SerializeField] private ScrollRect _itemScrollView;
        [SerializeField] private Transform _itemListContent;
        [SerializeField] private GameObject _itemCardPrefab;
        [SerializeField] private TMP_Dropdown _filterDropdown;
        
        [Header("Panels")]
        [SerializeField] private GameObject _characterPanel;
        [SerializeField] private Button _closeButton;
        
        private string _selectedCharacterId;
        private CharacterLoadout _currentLoadout;
        
        private void Start()
        {
            _closeButton.onClick.AddListener(ClosePanel);
            _filterDropdown.onValueChanged.AddListener(OnFilterChanged);
            
            LoadCharacterList();
        }
        
        public void OpenPanel()
        {
            _characterPanel.SetActive(true);
            LoadCharacterList();
        }
        
        public void ClosePanel()
        {
            _characterPanel.SetActive(false);
        }
        
        private void LoadCharacterList()
        {
            // Clear existing
            foreach (Transform child in _characterListContent)
            {
                Destroy(child.gameObject);
            }
            
            var playerData = GameManager.Instance.CurrentPlayerData;
            
            // Load all characters (owned + locked)
            // TODO: Load from Resources or AssetDatabase
            // For now, just show owned characters
            foreach (var characterId in playerData.ownedCharacters)
            {
                CreateCharacterItem(characterId);
            }
        }
        
        private void CreateCharacterItem(string characterId)
        {
            GameObject item = Instantiate(_characterItemPrefab, _characterListContent);
            
            // TODO: Set character info
            // item.GetComponent<CharacterItemUI>().Setup(characterData, OnCharacterSelected);
        }
        
        private void OnCharacterSelected(string characterId)
        {
            _selectedCharacterId = characterId;
            
            // Load character's loadout
            var playerData = GameManager.Instance.CurrentPlayerData;
            _currentLoadout = playerData.GetLoadoutForCharacter(characterId);
            
            // Update equipment slots
            UpdateEquipmentSlots();
            
            // Update item list
            UpdateItemList();
        }
        
        private void UpdateEquipmentSlots()
        {
            // Update each slot with equipped item
            for (int i = 0; i < _equipmentSlots.Length; i++)
            {
                EquipmentSlot slot = (EquipmentSlot)i;
                string itemId = _currentLoadout.GetEquippedItem(slot);
                
                if (!string.IsNullOrEmpty(itemId))
                {
                    // TODO: Load ItemData and display
                    // _equipmentSlots[i].SetItem(itemData);
                }
                else
                {
                    _equipmentSlots[i].Clear();
                }
            }
        }
        
        private void UpdateItemList()
        {
            // Clear existing
            foreach (Transform child in _itemListContent)
            {
                Destroy(child.gameObject);
            }
            
            var playerData = GameManager.Instance.CurrentPlayerData;
            
            // Filter items by selected character's class
            // TODO: Load ItemData for each ownedItem
            // Filter by class and current filter dropdown
            
            foreach (var itemId in playerData.ownedItems)
            {
                // TODO: Load ItemData
                // if (itemData.CanBeEquippedBy(characterClass))
                // {
                //     CreateItemCard(itemData);
                // }
            }
        }
        
        private void OnFilterChanged(int filterIndex)
        {
            UpdateItemList();
        }
        
        public void OnEquipItem(ItemData item, EquipmentSlot slot)
        {
            _currentLoadout.EquipItem(slot, item.itemId);
            UpdateEquipmentSlots();
            
            // Save to cloud
            GameManager.Instance.SavePlayerData();
        }
        
        public void OnUnequipItem(EquipmentSlot slot)
        {
            _currentLoadout.UnequipItem(slot);
            UpdateEquipmentSlots();
            
            // Save to cloud
            GameManager.Instance.SavePlayerData();
        }
    }
    
    [System.Serializable]
    public class EquipmentSlotUI
    {
        public EquipmentSlot slotType;
        public Image itemIcon;
        public TextMeshProUGUI slotName;
        public Button unequipButton;
        
        public void SetItem(ItemData item)
        {
            itemIcon.sprite = item.icon;
            itemIcon.enabled = true;
            unequipButton.gameObject.SetActive(true);
        }
        
        public void Clear()
        {
            itemIcon.sprite = null;
            itemIcon.enabled = false;
            unequipButton.gameObject.SetActive(false);
        }
    }
}
```

---

## 3. Skill System UI

### Skill Panel (Equipment Panel'in yanında)

```
EquipmentArea → Panel → "SkillPanel"

Inspector:
- Anchor: Top-Center
- Width: 900, Height: 300
- Pos Y: -50
```

**İçindekiler:**

```
SkillPanel altına:

1. Text (TMP) → "SkillPanelTitle"
   - Text: "Skills"
   - Font Size: 32

2. Panel → "SkillSlotsPanel"
   - Anchor: Center
   - Add Component: Horizontal Layout Group
   - Spacing: 20

SkillSlotsPanel içine (5 slot):

Slot Template:
Panel → "SkillSlot_Q" (Width: 150, Height: 200)
- Image → "SlotBackground" (Color: #444444)
- Image → "SkillIcon" (120x120)
- Text (TMP) → "SkillName"
- Text (TMP) → "KeyBinding" ("Q", "E", "R", "P", "U")
- Button → "UnequipButton" ("X")

5 Slot:
- Skill Slot Q (Active 1)
- Skill Slot E (Active 2)
- Skill Slot R (Active 3)
- Skill Slot P (Passive)
- Skill Slot U (Ultimate)
```

---

### Available Skills List

```
SkillPanel → Scroll View → "AvailableSkillsScrollView"

Inspector:
- Anchor: Bottom-Stretch
- Height: 150
- Content → Horizontal Layout Group
  - Spacing: 10
  - Padding: 10

Content içine (prefab):
- Panel → "SkillCardPrefab" (Width: 120, Height: 120)
  - Image → "SkillIcon"
  - Text (TMP) → "SkillName"
  - Drag & Drop Component (IBeginDragHandler, IDragHandler, IEndDragHandler)
```

---

## 4. Character Selection UI (Unlock System)

### Character Select Panel (Unlock ile)

```
MainCanvas → UI → Panel → "CharacterSelectPanel"

Inspector:
- Anchor: Stretch/Stretch
- Active: FALSE
```

**İçindekiler:**

```
CharacterSelectPanel altına:

1. Scroll View → "CharacterGridScrollView"
   - Content → Grid Layout Group
     - Cell Size: 300x400
     - Spacing: 20
     - Constraint: Fixed Column Count (3)

Content içine (prefab):
Panel → "CharacterCardPrefab"
├── Image → "CharacterPortrait" (300x300)
├── Text (TMP) → "CharacterName"
├── Text (TMP) → "CharacterClass"
├── Panel → "StatsPanel"
│   ├── Text → "HP: 100"
│   ├── Text → "Stamina: 100"
│   └── Text → "Defense: 10%"
├── Button → "SelectButton" (Owned ise)
├── Panel → "LockedPanel" (Locked ise)
│   ├── Image → "LockIcon"
│   ├── Text → "Required Level: 5"
│   ├── Text → "Price: 500 Gold"
│   └── Button → "UnlockButton"
```

---

### Character Card Script

```csharp
// CharacterCardUI.cs

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using WasdBattle.Data;
using WasdBattle.Core;

namespace WasdBattle.UI
{
    public class CharacterCardUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Image _characterPortrait;
        [SerializeField] private TextMeshProUGUI _characterName;
        [SerializeField] private TextMeshProUGUI _characterClass;
        [SerializeField] private TextMeshProUGUI _hpText;
        [SerializeField] private TextMeshProUGUI _staminaText;
        [SerializeField] private TextMeshProUGUI _defenseText;
        
        [Header("Buttons")]
        [SerializeField] private Button _selectButton;
        [SerializeField] private GameObject _lockedPanel;
        [SerializeField] private TextMeshProUGUI _requiredLevelText;
        [SerializeField] private TextMeshProUGUI _priceText;
        [SerializeField] private Button _unlockButton;
        
        private CharacterData _characterData;
        private bool _isOwned;
        
        public void Setup(CharacterData character, bool isOwned)
        {
            _characterData = character;
            _isOwned = isOwned;
            
            // Basic info
            _characterPortrait.sprite = character.characterIcon;
            _characterName.text = character.characterName;
            _characterClass.text = character.characterClass.ToString();
            
            // Stats
            _hpText.text = $"HP: {character.baseHealth}";
            _staminaText.text = $"Stamina: {character.baseStamina}";
            _defenseText.text = $"Defense: {character.baseDefense * 100}%";
            
            // Owned or Locked
            if (_isOwned)
            {
                _selectButton.gameObject.SetActive(true);
                _lockedPanel.SetActive(false);
                _selectButton.onClick.AddListener(OnSelectClicked);
            }
            else
            {
                _selectButton.gameObject.SetActive(false);
                _lockedPanel.SetActive(true);
                
                // Unlock requirements
                _requiredLevelText.text = $"Level {character.requiredLevel}";
                
                if (character.unlockPrices != null && character.unlockPrices.Length > 0)
                {
                    // Show first price option
                    var price = character.unlockPrices[0];
                    _priceText.text = $"{price.amount} {price.currencyType}";
                }
                
                _unlockButton.onClick.AddListener(OnUnlockClicked);
                
                // Can unlock?
                var playerData = GameManager.Instance.CurrentPlayerData;
                _unlockButton.interactable = character.CanUnlock(playerData);
            }
        }
        
        private void OnSelectClicked()
        {
            var playerData = GameManager.Instance.CurrentPlayerData;
            playerData.selectedCharacterId = _characterData.characterId;
            GameManager.Instance.SavePlayerData();
            
            Debug.Log($"[CharacterCard] Selected: {_characterData.characterName}");
        }
        
        private void OnUnlockClicked()
        {
            var playerData = GameManager.Instance.CurrentPlayerData;
            
            // Find affordable price
            ShopPrice selectedPrice = null;
            foreach (var price in _characterData.unlockPrices)
            {
                if (playerData.HasCurrency(price.currencyType, price.amount))
                {
                    selectedPrice = price;
                    break;
                }
            }
            
            if (selectedPrice != null)
            {
                // Spend currency
                playerData.ModifyCurrency(selectedPrice.currencyType, -selectedPrice.amount);
                
                // Add character
                playerData.ownedCharacters.Add(_characterData.characterId);
                
                // Create default loadout
                var loadout = new CharacterLoadout(_characterData.characterId);
                playerData.characterLoadouts.Add(loadout);
                
                // Save
                GameManager.Instance.SavePlayerData();
                
                Debug.Log($"[CharacterCard] Unlocked: {_characterData.characterName}");
                
                // Refresh UI
                Setup(_characterData, true);
            }
        }
    }
}
```

---

## 5. Inventory UI

*(Mevcut InventoryUI.cs'i genişlet)*

```
InventoryPanel → Panel → "MaterialsPanel"

Inspector:
- Anchor: Top-Stretch
- Height: 200
```

**İçindekiler:**

```
MaterialsPanel altına (Grid Layout):

Material Item Template (10 material için):
Panel → "MaterialItem" (Width: 150, Height: 80)
├── Image → "MaterialIcon"
├── Text (TMP) → "MaterialName" ("Metal")
└── Text (TMP) → "MaterialAmount" ("50")

10 Material:
- Metal
- Energy Crystal
- Rune
- Essence
- Leather
- Cloth
- Wood
- Gem Stone
- Dark Essence
- Light Essence
```

---

## 6. Crafting UI

### Craft Panel

```
MainCanvas → UI → Panel → "CraftPanel"

Inspector:
- Anchor: Stretch/Stretch
- Active: FALSE
```

**İçindekiler:**

```
CraftPanel altına:

1. Panel → "CraftContentPanel"
   - Anchor: Center
   - Width: 1600, Height: 900

2. Panel → "RecipeListPanel" (Sol)
   - Anchor: Left-Stretch
   - Width: 500
   
   Scroll View → "RecipeScrollView"
   - Content → Vertical Layout Group
   
   Recipe Item Prefab:
   Panel → "RecipeItem" (Height: 120)
   ├── Image → "ResultIcon"
   ├── Text → "RecipeName"
   ├── Text → "RequiredMaterials"
   └── Button → "SelectButton"

3. Panel → "CraftDetailsPanel" (Sağ)
   - Anchor: Right-Stretch
   - Width: 1000
   
   İçinde:
   - Image → "ResultPreview" (300x300)
   - Text → "ResultName"
   - Text → "ResultDescription"
   - Panel → "MaterialRequirementsPanel"
     - Text → "Metal: 50 / 100" (Kırmızı ise yetersiz)
     - Text → "Crystal: 30 / 30" (Yeşil ise yeterli)
   - Button → "CraftButton" (Width: 400, Height: 80)
     - Text: "CRAFT"
     - Color: Green (yeterli ise), Gray (yetersiz ise)
```

---

## 7. Shop UI

### Shop Panel

```
MainCanvas → UI → Panel → "ShopPanel"

Inspector:
- Anchor: Stretch/Stretch
- Active: FALSE
```

**İçindekiler:**

```
ShopPanel altına:

1. Panel → "ShopContentPanel"
   - Anchor: Center
   - Width: 1600, Height: 900

2. Dropdown (TMP) → "CategoryDropdown"
   - Anchor: Top-Center
   - Options: "All", "Characters", "Items", "Materials", "Currency"

3. Scroll View → "ShopItemScrollView"
   - Content → Grid Layout Group
     - Cell Size: 350x450
     - Spacing: 20

Shop Item Prefab:
Panel → "ShopItemCard" (350x450)
├── Image → "ItemPreview" (300x300)
├── Text → "ItemName"
├── Text → "ItemDescription"
├── Panel → "PricePanel"
│   ├── Image → "CurrencyIcon"
│   ├── Text → "PriceAmount" ("500 Gold")
│   └── Text → "OrText" ("OR")
│   ├── Image → "Currency2Icon"
│   └── Text → "Price2Amount" ("100 Gem")
├── Panel → "BadgesPanel"
│   ├── Image → "NewBadge" (Yeni ise)
│   ├── Image → "SaleBadge" (İndirimde ise)
│   └── Image → "FeaturedBadge" (Öne çıkan ise)
└── Button → "PurchaseButton"
    - Text: "BUY"
    - Color: Green (yeterli ise), Gray (yetersiz ise)
```

---

## 8. Matchmaking UI (Inline)

*(Zaten MainMenuUI'de yapıldı, yukarıda Step 6'da)*

---

## 📊 Script Bağlama Özeti

### MainCanvas'a Eklenecek Script'ler:

1. **MainMenuUI.cs** (Mevcut)
   - Player info
   - Currency display
   - Find Match button
   - Matchmaking panel
   - Bottom buttons

2. **EquipmentUI.cs** (Yeni - yukarıda kod verildi)
   - Character selection
   - Equipment slots (9)
   - Item list
   - Equip/Unequip

3. **SkillUI.cs** (Yeni - oluşturulacak)
   - Skill slots (5)
   - Available skills
   - Drag & drop

4. **CharacterSelectUI.cs** (Mevcut - genişletilecek)
   - Character cards
   - Unlock system
   - Purchase

5. **InventoryUI.cs** (Mevcut - genişletilecek)
   - Materials display (10)
   - Items display

6. **CraftUI.cs** (Yeni - oluşturulacak)
   - Recipe list
   - Material requirements
   - Craft button

7. **ShopUI.cs** (Mevcut - genişletilecek)
   - Shop items
   - Multiple currencies
   - Purchase

---

## 🎨 Prefab'lar Oluşturulacak

1. **CharacterItemPrefab** - Character list'te gösterilecek
2. **ItemCardPrefab** - Item list'te gösterilecek
3. **SkillCardPrefab** - Skill list'te gösterilecek
4. **CharacterCardPrefab** - Character select'te gösterilecek
5. **RecipeItemPrefab** - Craft recipe list'te
6. **ShopItemCardPrefab** - Shop'ta gösterilecek
7. **MaterialItemPrefab** - Inventory'de material gösterimi

---

## 🚀 Sonraki Adımlar

1. MainMenuScene'i aç
2. Bu rehberi takip ederek UI'ları oluştur
3. Script'leri bağla
4. Prefab'ları oluştur
5. Test et

**Başarılar!** 🎉

