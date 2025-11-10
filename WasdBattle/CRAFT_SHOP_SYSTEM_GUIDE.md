# Craft & Shop Sistemi - Tam Rehber

## 🎯 Genel Bakış

Bu sistem, oyuncuların **Craft NPC** ve **Shop NPC** ile etkileşime girerek item craft edip satın almasını sağlar.

### Özellikler
- ✅ **İki NPC Gösterimi**: Yan yana duran Craft ve Shop NPC'leri (RenderTexture ile)
- ✅ **NPC Seçimi**: Tıklanan NPC'nin menüsü açılır
- ✅ **Class Bazlı Filtreleme**: Mage, Warrior, Ninja veya All
- ✅ **Item Type Filtreleme**: Helmet, Chest, Gloves, Legs, Weapon, Ring, Necklace, Bracelet
- ✅ **Craft Sistemi**: Malzeme ile item üretimi
- ✅ **Shop Sistemi**: Gold ile item satın alma
- ✅ **Rarity Renkleri**: Common (Gri), Uncommon (Yeşil), Rare (Mavi), Epic (Mor), Legendary (Turuncu)

---

## 📁 Dosya Yapısı

### Yeni Eklenen Dosyalar

```
Assets/Scripts/UI/
├── NPCDisplayController.cs      # NPC render ve gösterim
├── CraftShopPanelUI.cs          # Ana panel ve NPC seçimi
├── ItemCraftUI.cs               # Craft menüsü
└── ItemShopUI.cs                # Shop menüsü

Assets/Scripts/Economy/
├── CraftingSystem.cs            # (Güncellendi - ItemData desteği)
└── ShopSystem.cs                # (Güncellendi - ItemData desteği)
```

---

## 🎮 Nasıl Çalışır?

### 1. Ana Panel Açılışı

```
MainMenu → Craft/Shop Button → CraftShopPanelUI açılır
```

**CraftShopPanelUI** açıldığında:
- İki NPC yan yana görünür (RenderTexture ile)
- Sol: **Craft NPC** (Craft Master)
- Sağ: **Shop NPC** (Shop Keeper)
- Her NPC'nin altında label vardır

### 2. NPC Seçimi

**Craft NPC'ye tıklandığında:**
1. NPC highlight olur (sarı renk)
2. `ItemCraftUI` menüsü açılır
3. GameState → `Crafting`

**Shop NPC'ye tıklandığında:**
1. NPC highlight olur (sarı renk)
2. `ItemShopUI` menüsü açılır
3. GameState → `Shop`

### 3. Craft Menüsü (ItemCraftUI)

**Filtreleme:**
1. **Class Filter**: All / Mage / Warrior / Ninja
2. **Item Type Filter**: Helmet / Chest / Gloves / Legs / Weapon / Ring / Necklace / Bracelet

**Akış:**
```
1. Class seç (örn: Mage)
2. Item type seç (örn: Helmet)
3. Filtrelenmiş item listesi görünür
4. Item'a tıkla → Detay paneli açılır
5. Craft Cost ve Stats gösterilir
6. "Craft" butonuna tıkla
7. Malzemeler tüketilir
8. Item inventory'ye eklenir
```

**Craft Cost Kontrolü:**
- Yeterli malzeme varsa → Craft butonu aktif (yeşil)
- Yetersiz malzeme varsa → Craft butonu pasif (gri)

### 4. Shop Menüsü (ItemShopUI)

**Filtreleme:**
1. **Class Filter**: All / Mage / Warrior / Ninja
2. **Item Type Filter**: Helmet / Chest / Gloves / Legs / Weapon / Ring / Necklace / Bracelet

**Akış:**
```
1. Class seç (örn: Warrior)
2. Item type seç (örn: Weapon)
3. Filtrelenmiş item listesi görünür
4. Item'a tıkla → Detay paneli açılır
5. Shop Price ve Stats gösterilir
6. "Purchase" butonuna tıkla
7. Gold tüketilir
8. Item inventory'ye eklenir
```

**Purchase Kontrolü:**
- Yeterli Gold varsa → Purchase butonu aktif (yeşil)
- Yetersiz Gold varsa → Purchase butonu pasif (gri)

---

## 🔧 Unity Setup

### 1. Scene Hierarchy

```
Canvas
├── CraftShopPanel (GameObject)
│   ├── Background (Image - Dark overlay)
│   │
│   ├── NPCDisplayPanel (Panel)
│   │   ├── NPCRenderImage (RawImage) ← RenderTexture gösterir
│   │   ├── CraftNPCButton (Button - Sol yarı)
│   │   ├── ShopNPCButton (Button - Sağ yarı)
│   │   ├── CraftNPCLabel (TextMeshProUGUI) - "Craft Master"
│   │   └── ShopNPCLabel (TextMeshProUGUI) - "Shop Keeper"
│   │
│   ├── CraftMenuPanel (GameObject) ← ItemCraftUI
│   │   ├── Header (Panel)
│   │   │   ├── Title (TextMeshProUGUI) - "Item Crafting"
│   │   │   └── CloseButton (Button)
│   │   │
│   │   ├── Filters (Panel)
│   │   │   ├── ClassFilterDropdown (TMP_Dropdown)
│   │   │   └── ItemTypeFilterDropdown (TMP_Dropdown)
│   │   │
│   │   ├── CurrencyDisplay (Panel)
│   │   │   ├── GoldText (TextMeshProUGUI)
│   │   │   ├── MetalText (TextMeshProUGUI)
│   │   │   ├── CrystalText (TextMeshProUGUI)
│   │   │   ├── RuneText (TextMeshProUGUI)
│   │   │   └── EssenceText (TextMeshProUGUI)
│   │   │
│   │   ├── ItemListPanel (Panel)
│   │   │   └── ScrollView (ScrollRect)
│   │   │       └── Content (Vertical Layout Group)
│   │   │           └── ItemCard (Prefab) x N
│   │   │
│   │   └── ItemDetailPanel (Panel)
│   │       ├── ItemIcon (Image)
│   │       ├── ItemName (TextMeshProUGUI)
│   │       ├── ItemDescription (TextMeshProUGUI)
│   │       ├── ItemStats (TextMeshProUGUI)
│   │       ├── CraftCost (TextMeshProUGUI)
│   │       └── CraftButton (Button)
│   │
│   ├── ShopMenuPanel (GameObject) ← ItemShopUI
│   │   ├── Header (Panel)
│   │   │   ├── Title (TextMeshProUGUI) - "Item Shop"
│   │   │   └── CloseButton (Button)
│   │   │
│   │   ├── Filters (Panel)
│   │   │   ├── ClassFilterDropdown (TMP_Dropdown)
│   │   │   └── ItemTypeFilterDropdown (TMP_Dropdown)
│   │   │
│   │   ├── CurrencyDisplay (Panel)
│   │   │   └── GoldText (TextMeshProUGUI)
│   │   │
│   │   ├── ItemListPanel (Panel)
│   │   │   └── ScrollView (ScrollRect)
│   │   │       └── Content (Vertical Layout Group)
│   │   │           └── ItemCard (Prefab) x N
│   │   │
│   │   └── ItemDetailPanel (Panel)
│   │       ├── ItemIcon (Image)
│   │       ├── ItemName (TextMeshProUGUI)
│   │       ├── ItemDescription (TextMeshProUGUI)
│   │       ├── ItemStats (TextMeshProUGUI)
│   │       ├── ShopPrice (TextMeshProUGUI)
│   │       └── PurchaseButton (Button)
│   │
│   └── CloseButton (Button)

NPCDisplayRoot (GameObject) ← Scene'de ayrı
├── NPCDisplayCamera (Camera)
│   └── Target Texture: NPCDisplayRT
├── NPCRoot (Transform)
│   ├── CraftNPC (Instantiated)
│   └── ShopNPC (Instantiated)
└── Lighting
```

### 2. ItemCard Prefab

```
ItemCard (GameObject)
├── Background (Image)
├── IconImage (Image)
├── NameText (TextMeshProUGUI)
├── LevelText (TextMeshProUGUI) ← Craft için
└── PriceText (TextMeshProUGUI) ← Shop için
```

### 3. Inspector Ayarları

#### CraftShopPanelUI

```
CraftShopPanelUI (Script)
├── NPC Display
│   ├── NPC Display Controller: [NPCDisplayController]
│   └── NPC Display Image: [NPCRenderImage RawImage]
│
├── NPC Click Areas
│   ├── Craft NPC Button: [CraftNPCButton]
│   └── Shop NPC Button: [ShopNPCButton]
│
├── NPC Labels
│   ├── Craft NPC Label: [CraftNPCLabel]
│   └── Shop NPC Label: [ShopNPCLabel]
│
├── Menu Panels
│   ├── Craft Menu Panel: [CraftMenuPanel GameObject]
│   ├── Item Craft UI: [ItemCraftUI Component]
│   ├── Shop Menu Panel: [ShopMenuPanel GameObject]
│   └── Item Shop UI: [ItemShopUI Component]
│
└── Main Panel
    ├── Main Panel: [CraftShopPanel GameObject]
    └── Close Button: [CloseButton]
```

#### NPCDisplayController

```
NPCDisplayController (Script)
├── Camera & Render
│   ├── Display Camera: [NPCDisplayCamera]
│   ├── Render Texture: [NPCDisplayRT]
│   └── NPC Root: [NPCRoot Transform]
│
├── NPC Prefabs
│   ├── Craft NPC Prefab: [CraftNPC Prefab]
│   └── Shop NPC Prefab: [ShopNPC Prefab]
│
├── NPC Positions
│   ├── Craft NPC Position: (-1.5, 0, 0)
│   └── Shop NPC Position: (1.5, 0, 0)
│
├── Rotation Settings
│   ├── Auto Rotation Speed: 20
│   └── Enable Auto Rotation: ☑ ON
│
└── Highlight Settings
    ├── Highlight Color: Yellow
    └── Normal Color: White
```

#### ItemCraftUI

```
ItemCraftUI (Script)
├── Filter Dropdowns
│   ├── Class Filter Dropdown: [ClassFilterDropdown]
│   └── Item Type Filter Dropdown: [ItemTypeFilterDropdown]
│
├── Item List
│   ├── Item Scroll View: [ScrollView]
│   ├── Item List Content: [Content Transform]
│   └── Item Card Prefab: [ItemCard Prefab]
│
├── Selected Item Display
│   ├── Item Detail Panel: [ItemDetailPanel]
│   ├── Item Name Text: [ItemName TextMeshProUGUI]
│   ├── Item Description Text: [ItemDescription TextMeshProUGUI]
│   ├── Item Icon Image: [ItemIcon Image]
│   ├── Item Stats Text: [ItemStats TextMeshProUGUI]
│   └── Craft Cost Text: [CraftCost TextMeshProUGUI]
│
├── Buttons
│   ├── Craft Button: [CraftButton]
│   └── Close Button: [CloseButton]
│
└── Currency Display
    ├── Gold Text: [GoldText]
    ├── Metal Text: [MetalText]
    ├── Crystal Text: [CrystalText]
    ├── Rune Text: [RuneText]
    └── Essence Text: [EssenceText]
```

#### ItemShopUI

```
ItemShopUI (Script)
├── Filter Dropdowns
│   ├── Class Filter Dropdown: [ClassFilterDropdown]
│   └── Item Type Filter Dropdown: [ItemTypeFilterDropdown]
│
├── Item List
│   ├── Item Scroll View: [ScrollView]
│   ├── Item List Content: [Content Transform]
│   └── Item Card Prefab: [ItemCard Prefab]
│
├── Selected Item Display
│   ├── Item Detail Panel: [ItemDetailPanel]
│   ├── Item Name Text: [ItemName TextMeshProUGUI]
│   ├── Item Description Text: [ItemDescription TextMeshProUGUI]
│   ├── Item Icon Image: [ItemIcon Image]
│   ├── Item Stats Text: [ItemStats TextMeshProUGUI]
│   └── Shop Price Text: [ShopPrice TextMeshProUGUI]
│
├── Buttons
│   ├── Purchase Button: [PurchaseButton]
│   └── Close Button: [CloseButton]
│
└── Currency Display
    └── Gold Text: [GoldText]
```

---

## 💾 ItemData Setup

### Craft Item Örneği

```csharp
// Resources/Items/MageHelmet_Common.asset

Item Name: Mage Helmet
Item ID: mage_helmet_common
Description: A simple helmet for mages
Slot: Helmet
Required Class: Mage
Rarity: Common
Level: 1

Stats:
├── Health Bonus: 10
├── Stamina Bonus: 5
├── Magic Resistance Bonus: 5

Crafting:
├── Can Be Crafted: ☑ ON
└── Crafting Materials:
    ├── Metal: 50
    └── Cloth: 20

Shop:
├── Can Be Bought: ☑ ON
└── Shop Price: 100 Gold
```

### Shop Item Örneği

```csharp
// Resources/Items/WarriorSword_Rare.asset

Item Name: Warrior Sword
Item ID: warrior_sword_rare
Description: A powerful sword for warriors
Slot: Weapon
Required Class: Warrior
Rarity: Rare
Level: 5

Stats:
├── Health Bonus: 20
├── Damage Bonus: 30
├── Armor Bonus: 10

Crafting:
├── Can Be Crafted: ☑ ON
└── Crafting Materials:
    ├── Metal: 200
    ├── Rune: 50
    └── GemStone: 10

Shop:
├── Can Be Bought: ☑ ON
└── Shop Price: 500 Gold
```

---

## 🎨 UI Layout Örnekleri

### Ana Panel (NPC Seçimi)

```
┌─────────────────────────────────────────────────────┐
│                 Craft & Shop                        │
├─────────────────────────────────────────────────────┤
│                                                     │
│     ┌─────────────────────────────────────┐        │
│     │                                     │        │
│     │   [Craft NPC]     [Shop NPC]       │        │
│     │       👨‍🔧            👨‍💼            │        │
│     │                                     │        │
│     └─────────────────────────────────────┘        │
│                                                     │
│   Craft Master              Shop Keeper            │
│  (Click to Craft)         (Click to Shop)          │
│                                                     │
│                    [Close]                          │
└─────────────────────────────────────────────────────┘
```

### Craft Menüsü

```
┌─────────────────────────────────────────────────────┐
│  Item Crafting                           [X]        │
├─────────────────────────────────────────────────────┤
│                                                     │
│  Class: [All ▼]    Item Type: [Helmet ▼]          │
│                                                     │
│  Gold: 1000  Metal: 500  Crystal: 200              │
│  Rune: 100   Essence: 50                           │
│                                                     │
├──────────────────────┬──────────────────────────────┤
│  Item List           │  Item Details                │
│                      │                              │
│  ┌────────────────┐  │  [Icon]                      │
│  │ Mage Helmet    │  │  Mage Helmet (Common)        │
│  │ Lv.1           │  │  A simple helmet for mages   │
│  └────────────────┘  │                              │
│                      │  Stats:                      │
│  ┌────────────────┐  │  +10 HP                      │
│  │ Mage Robe      │  │  +5 Stamina                  │
│  │ Lv.2           │  │  +5 Magic Resist             │
│  └────────────────┘  │                              │
│                      │  Craft Cost:                 │
│  ┌────────────────┐  │  • Metal: 50                 │
│  │ Mage Staff     │  │  • Cloth: 20                 │
│  │ Lv.3           │  │                              │
│  └────────────────┘  │  [Craft]                     │
│                      │                              │
└──────────────────────┴──────────────────────────────┘
```

### Shop Menüsü

```
┌─────────────────────────────────────────────────────┐
│  Item Shop                               [X]        │
├─────────────────────────────────────────────────────┤
│                                                     │
│  Class: [Warrior ▼]    Item Type: [Weapon ▼]      │
│                                                     │
│  Gold: 1000                                        │
│                                                     │
├──────────────────────┬──────────────────────────────┤
│  Item List           │  Item Details                │
│                      │                              │
│  ┌────────────────┐  │  [Icon]                      │
│  │ Warrior Sword  │  │  Warrior Sword (Rare)        │
│  │ 500 Gold       │  │  A powerful sword            │
│  └────────────────┘  │                              │
│                      │  Stats:                      │
│  ┌────────────────┐  │  +20 HP                      │
│  │ Warrior Axe    │  │  +30 Damage                  │
│  │ 800 Gold       │  │  +10 Armor                   │
│  └────────────────┘  │                              │
│                      │  Price: 500 Gold             │
│  ┌────────────────┐  │                              │
│  │ Warrior Shield │  │  [Purchase]                  │
│  │ 600 Gold       │  │                              │
│  └────────────────┘  │                              │
│                      │                              │
└──────────────────────┴──────────────────────────────┘
```

---

## 🔄 Kod Akışı

### Craft İşlemi

```csharp
1. ItemCraftUI.OnCraftClicked()
   ↓
2. CanCraftItem(selectedItem) kontrolü
   ├── Malzeme yeterli mi?
   └── Item craftable mı?
   ↓
3. CraftItem(selectedItem)
   ├── Malzemeleri tüket (RemoveMaterial)
   ├── Item'i inventory'ye ekle (AddItem)
   └── PlayerData'yı kaydet
   ↓
4. RefreshUI()
   ├── Currency display güncelle
   ├── Item list yenile
   └── Detail panel güncelle
```

### Shop İşlemi

```csharp
1. ItemShopUI.OnPurchaseClicked()
   ↓
2. CanPurchaseItem(selectedItem) kontrolü
   ├── Gold yeterli mi?
   └── Item purchasable mı?
   ↓
3. PurchaseItem(selectedItem)
   ├── Gold tüket (playerData.gold -= price)
   ├── Item'i inventory'ye ekle (AddItem)
   └── PlayerData'yı kaydet
   ↓
4. RefreshUI()
   ├── Currency display güncelle
   ├── Item list yenile
   └── Detail panel güncelle
```

---

## 🧪 Test Senaryoları

### Test 1: Craft NPC Seçimi
1. Ana menüden "Craft/Shop" butonuna tıkla
2. ✅ CraftShopPanel açılmalı
3. ✅ İki NPC görünmeli
4. Sol NPC'ye (Craft) tıkla
5. ✅ NPC highlight olmalı (sarı)
6. ✅ Craft menüsü açılmalı

### Test 2: Class Filtreleme (Craft)
1. Craft menüsünde "Class: Mage" seç
2. "Item Type: Helmet" seç
3. ✅ Sadece Mage helmet'ları görünmeli
4. ✅ All class itemlar da görünmeli
5. "Class: All" seç
6. ✅ Tüm class'ların helmet'ları görünmeli

### Test 3: Item Craft
1. Yeterli malzemesi olan bir item seç
2. ✅ Craft butonu aktif olmalı
3. ✅ Craft cost gösterilmeli
4. "Craft" butonuna tıkla
5. ✅ Malzemeler azalmalı
6. ✅ Item inventory'ye eklenmeli
7. ✅ Currency display güncellenmiş olmalı

### Test 4: Yetersiz Malzeme
1. Yetersiz malzemesi olan bir item seç
2. ✅ Craft butonu pasif olmalı (gri)
3. ✅ Craft'a tıklanamaz olmalı

### Test 5: Shop NPC Seçimi
1. Ana panelde sağ NPC'ye (Shop) tıkla
2. ✅ NPC highlight olmalı (sarı)
3. ✅ Shop menüsü açılmalı
4. ✅ Craft menüsü kapanmalı

### Test 6: Item Purchase
1. Shop menüsünde yeterli Gold'u olan item seç
2. ✅ Purchase butonu aktif olmalı
3. ✅ Shop price gösterilmeli
4. "Purchase" butonuna tıkla
5. ✅ Gold azalmalı
6. ✅ Item inventory'ye eklenmeli

### Test 7: Yetersiz Gold
1. Yetersiz Gold'u olan bir item seç
2. ✅ Purchase butonu pasif olmalı (gri)
3. ✅ Purchase'a tıklanamaz olmalı

### Test 8: Rarity Renkleri
1. Craft veya Shop menüsünde item listesine bak
2. ✅ Common itemlar gri renkte
3. ✅ Uncommon itemlar yeşil renkte
4. ✅ Rare itemlar mavi renkte
5. ✅ Epic itemlar mor renkte
6. ✅ Legendary itemlar turuncu renkte

---

## 🎯 Önemli Notlar

### 1. ItemData Gereksinimleri

**Craft için:**
- `canBeCrafted = true`
- `craftingMaterials` dolu olmalı
- `requiredClass` ayarlanmalı

**Shop için:**
- `canBeBought = true`
- `shopPrice > 0` olmalı
- `requiredClass` ayarlanmalı

### 2. Resources Klasörü

Item'lar `Resources/Items/` klasöründe olmalı:

```
Assets/Resources/Items/
├── MageHelmet_Common.asset
├── MageRobe_Uncommon.asset
├── MageStaff_Rare.asset
├── WarriorHelmet_Common.asset
├── WarriorSword_Rare.asset
└── ...
```

### 3. NPC Prefab'ları

NPC prefab'ları hazırlanmalı:

```
Assets/Prefabs/NPCs/
├── CraftNPC.prefab (3D model)
└── ShopNPC.prefab (3D model)
```

### 4. RenderTexture

`NPCDisplayRT` RenderTexture oluşturulmalı:

```
Assets/
└── NPCDisplayRT.renderTexture
    ├── Size: 1024x1024
    ├── Depth: 24
    └── Anti-aliasing: 4x
```

---

## 🐛 Troubleshooting

### Problem: NPC'ler görünmüyor

**Çözüm:**
- `NPCDisplayController` referansları verilmiş mi?
- `CraftNPCPrefab` ve `ShopNPCPrefab` atanmış mı?
- `RenderTexture` oluşturulmuş mu?
- `NPCDisplayCamera` aktif mi?

### Problem: Item listesi boş

**Çözüm:**
- `Resources/Items/` klasöründe item'lar var mı?
- Item'ların `canBeCrafted` veya `canBeBought` true mu?
- Filter'lar doğru seçilmiş mi?

### Problem: Craft butonu çalışmıyor

**Çözüm:**
- `ItemCraftUI` script'i atanmış mı?
- `CraftButton` referansı verilmiş mi?
- Yeterli malzeme var mı?
- `craftingMaterials` dolu mu?

### Problem: Purchase butonu çalışmıyor

**Çözüm:**
- `ItemShopUI` script'i atanmış mı?
- `PurchaseButton` referansı verilmiş mi?
- Yeterli Gold var mı?
- `shopPrice > 0` mı?

### Problem: Filter çalışmıyor

**Çözüm:**
- Dropdown'lar doğru atanmış mı?
- `onValueChanged` event'leri bağlı mı?
- Item'ların `requiredClass` doğru mu?

---

## 📚 İlgili Dokümantasyon

- [EQUIPMENT_SYSTEM_GUIDE.md](EQUIPMENT_SYSTEM_GUIDE.md) - Equipment sistemi
- [EQUIPMENT_DRAG_DROP_GUIDE.md](EQUIPMENT_DRAG_DROP_GUIDE.md) - Drag-drop
- [SALVAGE_SYSTEM_GUIDE.md](SALVAGE_SYSTEM_GUIDE.md) - Salvage sistemi
- [ITEM_SYSTEM_SETUP.md](ITEM_SYSTEM_SETUP.md) - Item setup
- [GAME_DATA_EDITOR_GUIDE.md](GAME_DATA_EDITOR_GUIDE.md) - Editor tools

---

## 🚀 Gelecek Geliştirmeler

- [ ] Multiple currency support (Gem, Diamond, etc.)
- [ ] Craft confirmation dialog
- [ ] Purchase confirmation dialog
- [ ] Item preview (3D model)
- [ ] Craft queue system
- [ ] Bulk crafting
- [ ] Shop discount system
- [ ] Daily deals
- [ ] Limited stock items

---

**Tarih:** 2025-11-10  
**Versiyon:** 1.0  
**Yazar:** AI Assistant

