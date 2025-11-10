# Craft & Shop Sistemi - Hızlı Başlangıç

## 🚀 5 Dakikada Kurulum

### 1. Script'leri Ekle ✅

Tüm script'ler oluşturuldu:
- `NPCDisplayController.cs`
- `CraftShopPanelUI.cs`
- `ItemCraftUI.cs`
- `ItemShopUI.cs`

### 2. Unity Scene Setup

#### A. NPC Display Root Oluştur

```
1. Hierarchy → Create Empty → "NPCDisplayRoot"
2. Add Component → NPCDisplayController
3. Create Child → Camera → "NPCDisplayCamera"
4. Create Child → Empty → "NPCRoot"
```

**NPCDisplayCamera Ayarları:**
- Clear Flags: Solid Color
- Background: Black
- Culling Mask: NPCLayer (yeni layer oluştur)
- Target Texture: NPCDisplayRT (yeni RenderTexture oluştur)

**RenderTexture Oluştur:**
```
Assets → Create → Render Texture → "NPCDisplayRT"
Size: 1024x1024
Depth: 24
Anti-aliasing: 4x
```

#### B. NPC Prefab'ları Hazırla

```
Assets/Prefabs/NPCs/
├── CraftNPC.prefab (Basit 3D model - Cube ile başlayabilirsin)
└── ShopNPC.prefab (Basit 3D model - Sphere ile başlayabilirsin)
```

**Geçici NPC Oluşturma:**
```
1. Hierarchy → 3D Object → Cube → "CraftNPC"
2. Scale: (0.5, 1, 0.3)
3. Add Material → Color: Orange
4. Drag to Assets/Prefabs/NPCs/
5. Delete from Hierarchy

6. Hierarchy → 3D Object → Sphere → "ShopNPC"
7. Scale: (0.5, 1, 0.5)
8. Add Material → Color: Blue
9. Drag to Assets/Prefabs/NPCs/
10. Delete from Hierarchy
```

#### C. UI Panel Oluştur

```
Canvas → Create Empty → "CraftShopPanel"
├── Background (Image - Dark overlay, Alpha: 200)
│
├── NPCDisplayPanel (Panel)
│   ├── NPCRenderImage (RawImage)
│   │   └── Texture: NPCDisplayRT
│   ├── LeftButton (Button) → "CraftNPCButton"
│   ├── RightButton (Button) → "ShopNPCButton"
│   ├── CraftLabel (TextMeshProUGUI) - "Craft Master"
│   └── ShopLabel (TextMeshProUGUI) - "Shop Keeper"
│
├── CraftMenuPanel (Panel) → Add ItemCraftUI
│   ├── [Craft UI yapısı - detay için CRAFT_SHOP_SYSTEM_GUIDE.md]
│
├── ShopMenuPanel (Panel) → Add ItemShopUI
│   ├── [Shop UI yapısı - detay için CRAFT_SHOP_SYSTEM_GUIDE.md]
│
└── CloseButton (Button)
```

#### D. Inspector Referansları

**NPCDisplayController:**
```
Display Camera: [NPCDisplayCamera]
Render Texture: [NPCDisplayRT]
NPC Root: [NPCRoot]
Craft NPC Prefab: [CraftNPC]
Shop NPC Prefab: [ShopNPC]
```

**CraftShopPanelUI:**
```
NPC Display Controller: [NPCDisplayController]
NPC Display Image: [NPCRenderImage]
Craft NPC Button: [LeftButton]
Shop NPC Button: [RightButton]
Craft NPC Label: [CraftLabel]
Shop NPC Label: [ShopLabel]
Craft Menu Panel: [CraftMenuPanel]
Item Craft UI: [ItemCraftUI Component]
Shop Menu Panel: [ShopMenuPanel]
Item Shop UI: [ItemShopUI Component]
Main Panel: [CraftShopPanel]
Close Button: [CloseButton]
```

**MainMenuUI:**
```
Craft Shop Panel: [CraftShopPanel GameObject]
Craft Shop Panel UI: [CraftShopPanelUI Component]
```

### 3. Item Data Oluştur

```
Assets/Resources/Items/ klasörü oluştur

Create → WasdBattle → Item Data

Örnek Item:
├── Item Name: "Simple Helmet"
├── Item ID: "simple_helmet"
├── Slot: Helmet
├── Required Class: All
├── Rarity: Common
├── Level: 1
├── Health Bonus: 10
├── Can Be Crafted: ☑
├── Crafting Materials:
│   └── Metal: 50
├── Can Be Bought: ☑
└── Shop Price: 100
```

### 4. Test Et!

```
1. Play Mode'a gir
2. Ana menüde "Craft/Shop" butonuna tıkla
3. ✅ Panel açılmalı
4. ✅ İki NPC görünmeli
5. Sol NPC'ye tıkla → Craft menüsü açılmalı
6. Sağ NPC'ye tıkla → Shop menüsü açılmalı
```

---

## 🎯 Minimum UI Layout

### Basit Craft Menu

```
CraftMenuPanel
├── Title (Text) - "Item Crafting"
├── ClassDropdown (TMP_Dropdown)
├── TypeDropdown (TMP_Dropdown)
├── ScrollView
│   └── Content (Vertical Layout)
├── DetailPanel
│   ├── ItemName (Text)
│   ├── ItemStats (Text)
│   ├── CraftCost (Text)
│   └── CraftButton (Button)
└── CloseButton (Button)
```

### Basit Shop Menu

```
ShopMenuPanel
├── Title (Text) - "Item Shop"
├── ClassDropdown (TMP_Dropdown)
├── TypeDropdown (TMP_Dropdown)
├── ScrollView
│   └── Content (Vertical Layout)
├── DetailPanel
│   ├── ItemName (Text)
│   ├── ItemStats (Text)
│   ├── ShopPrice (Text)
│   └── PurchaseButton (Button)
└── CloseButton (Button)
```

### Item Card Prefab

```
ItemCard (Button)
├── Background (Image)
├── IconImage (Image)
├── NameText (TextMeshProUGUI)
└── InfoText (TextMeshProUGUI) - "Lv.1" veya "100 Gold"
```

---

## 📝 Hızlı Test Item'ları

### 3 Test Item Oluştur

**1. Mage Helmet (Common)**
```
Item ID: mage_helmet_common
Class: Mage
Slot: Helmet
Rarity: Common
Level: 1
HP: +10
Craft: Metal 50
Shop: 100 Gold
```

**2. Warrior Sword (Rare)**
```
Item ID: warrior_sword_rare
Class: Warrior
Slot: Weapon
Rarity: Rare
Level: 5
Damage: +30
Craft: Metal 200, Rune 50
Shop: 500 Gold
```

**3. Ninja Ring (Uncommon)**
```
Item ID: ninja_ring_uncommon
Class: Ninja
Slot: Ring1
Rarity: Uncommon
Level: 3
Crit Chance: +0.1
Craft: Metal 100, GemStone 10
Shop: 300 Gold
```

---

## 🐛 Hızlı Troubleshooting

### NPC'ler görünmüyor?
```
1. NPCDisplayCamera aktif mi?
2. RenderTexture atanmış mı?
3. NPC Prefab'ları atanmış mı?
4. NPCRoot pozisyonu (0,0,0) mı?
```

### Item listesi boş?
```
1. Items klasörü Resources içinde mi?
2. Item'ların canBeCrafted/canBeBought true mu?
3. Filter'lar doğru seçilmiş mi?
```

### Butonlar çalışmıyor?
```
1. Script referansları verilmiş mi?
2. Button onClick event'leri bağlı mı?
3. Console'da hata var mı?
```

---

## 📚 Daha Fazla Bilgi

Detaylı setup için:
- [CRAFT_SHOP_SYSTEM_GUIDE.md](CRAFT_SHOP_SYSTEM_GUIDE.md) - Tam rehber
- [ITEM_SYSTEM_SETUP.md](ITEM_SYSTEM_SETUP.md) - Item setup
- [EQUIPMENT_SYSTEM_GUIDE.md](EQUIPMENT_SYSTEM_GUIDE.md) - Equipment sistemi

---

**Tarih:** 2025-11-10  
**Versiyon:** 1.0

