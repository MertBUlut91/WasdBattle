# Craft & Shop System - Visual Setup Guide

## 🎨 Unity Scene Setup (Görsel Rehber)

### 1. NPC Display Root Setup

```
Scene Hierarchy:
═══════════════════════════════════════════════════════════

📦 NPCDisplayRoot (GameObject)
   ├── 📷 NPCDisplayCamera (Camera)
   │   ├── Clear Flags: Solid Color
   │   ├── Background: Black (0,0,0,255)
   │   ├── Culling Mask: NPCLayer
   │   ├── Projection: Perspective
   │   ├── Field of View: 60
   │   ├── Target Texture: NPCDisplayRT ⭐
   │   └── Transform: (0, 1.5, -3)
   │
   ├── 🎭 NPCRoot (Empty Transform)
   │   ├── Position: (0, 0, 0)
   │   ├── Rotation: (0, 0, 0)
   │   └── Scale: (1, 1, 1)
   │
   └── 💡 Directional Light (Optional)
       ├── Color: White
       ├── Intensity: 1
       └── Rotation: (50, -30, 0)

Component: NPCDisplayController
═══════════════════════════════════════════════════════════
Display Camera: [NPCDisplayCamera]
Render Texture: [NPCDisplayRT]
NPC Root: [NPCRoot]
Craft NPC Prefab: [CraftNPC]
Shop NPC Prefab: [ShopNPC]
Craft NPC Position: (-1.5, 0, 0)
Shop NPC Position: (1.5, 0, 0)
Auto Rotation Speed: 20
Enable Auto Rotation: ☑
Highlight Color: Yellow (1, 1, 0, 1)
Normal Color: White (1, 1, 1, 1)
```

---

### 2. Canvas UI Setup

```
Canvas Hierarchy:
═══════════════════════════════════════════════════════════

📱 Canvas
   └── 🎪 CraftShopPanel (GameObject)
       ├── Anchor: Stretch/Stretch
       ├── Offset: (0, 0, 0, 0)
       └── Active: false (başlangıçta kapalı)

       ├── 🖼️ Background (Image)
       │   ├── Color: Black (0, 0, 0, 200) - Alpha 200
       │   ├── Anchor: Stretch/Stretch
       │   └── Raycast Target: ☑
       │
       ├── 📺 NPCDisplayPanel (Panel)
       │   ├── Size: (800, 600)
       │   ├── Anchor: Top Center
       │   ├── Position: (0, -100, 0)
       │   │
       │   ├── 🖼️ NPCRenderImage (RawImage)
       │   │   ├── Texture: NPCDisplayRT ⭐
       │   │   ├── Size: (800, 600)
       │   │   ├── Anchor: Center
       │   │   └── UV Rect: (0,0,1,1)
       │   │
       │   ├── 🔘 LeftButton (Button) - "CraftNPCButton"
       │   │   ├── Size: (400, 600)
       │   │   ├── Anchor: Left
       │   │   ├── Position: (0, 0, 0)
       │   │   └── Color: Transparent (0,0,0,0)
       │   │
       │   ├── 🔘 RightButton (Button) - "ShopNPCButton"
       │   │   ├── Size: (400, 600)
       │   │   ├── Anchor: Right
       │   │   ├── Position: (0, 0, 0)
       │   │   └── Color: Transparent (0,0,0,0)
       │   │
       │   ├── 📝 CraftLabel (TextMeshProUGUI)
       │   │   ├── Text: "Craft Master\n(Click to Craft)"
       │   │   ├── Font Size: 24
       │   │   ├── Alignment: Center
       │   │   ├── Position: (-200, -320, 0)
       │   │   └── Color: White
       │   │
       │   └── 📝 ShopLabel (TextMeshProUGUI)
       │       ├── Text: "Shop Keeper\n(Click to Shop)"
       │       ├── Font Size: 24
       │       ├── Alignment: Center
       │       ├── Position: (200, -320, 0)
       │       └── Color: White
       │
       ├── 🛠️ CraftMenuPanel (Panel)
       │   ├── Active: false
       │   ├── Size: (1200, 800)
       │   ├── Anchor: Center
       │   └── [See Craft Menu Layout below]
       │
       ├── 🛒 ShopMenuPanel (Panel)
       │   ├── Active: false
       │   ├── Size: (1200, 800)
       │   ├── Anchor: Center
       │   └── [See Shop Menu Layout below]
       │
       └── ❌ CloseButton (Button)
           ├── Size: (100, 50)
           ├── Anchor: Top Right
           ├── Position: (-50, -50, 0)
           └── Text: "X"
```

---

### 3. Craft Menu Layout

```
🛠️ CraftMenuPanel
═══════════════════════════════════════════════════════════

├── 📋 Header (Panel)
│   ├── Size: (1200, 80)
│   ├── Anchor: Top Stretch
│   │
│   ├── 📝 Title (TextMeshProUGUI)
│   │   ├── Text: "Item Crafting"
│   │   ├── Font Size: 36
│   │   └── Position: (0, -40, 0)
│   │
│   └── ❌ CloseButton (Button)
│       ├── Size: (60, 60)
│       └── Position: (570, -40, 0)
│
├── 🎛️ Filters (Panel)
│   ├── Size: (1200, 60)
│   ├── Anchor: Top Stretch
│   ├── Position: (0, -80, 0)
│   │
│   ├── 📝 ClassLabel (Text): "Class:"
│   ├── 🔽 ClassFilterDropdown (TMP_Dropdown)
│   │   ├── Options: All, Mage, Warrior, Ninja
│   │   └── Size: (200, 40)
│   │
│   ├── 📝 TypeLabel (Text): "Item Type:"
│   └── 🔽 ItemTypeFilterDropdown (TMP_Dropdown)
│       ├── Options: Helmet, Chest, Gloves, Legs, Weapon, Ring, Necklace, Bracelet
│       └── Size: (200, 40)
│
├── 💰 CurrencyDisplay (Panel)
│   ├── Size: (1200, 40)
│   ├── Anchor: Top Stretch
│   ├── Position: (0, -140, 0)
│   │
│   ├── 📝 GoldText: "Gold: 1000"
│   ├── 📝 MetalText: "Metal: 500"
│   ├── 📝 CrystalText: "Crystal: 200"
│   ├── 📝 RuneText: "Rune: 100"
│   └── 📝 EssenceText: "Essence: 50"
│
├── 📜 ItemListPanel (Panel)
│   ├── Size: (600, 600)
│   ├── Anchor: Left
│   ├── Position: (300, -470, 0)
│   │
│   └── 📋 ScrollView (ScrollRect)
│       ├── Vertical: ☑
│       ├── Horizontal: ☐
│       │
│       └── 📦 Content (Vertical Layout Group)
│           ├── Spacing: 10
│           ├── Padding: 10
│           └── Child Force Expand: ☑
│
└── 📊 ItemDetailPanel (Panel)
    ├── Size: (500, 600)
    ├── Anchor: Right
    ├── Position: (-250, -470, 0)
    │
    ├── 🖼️ ItemIcon (Image)
    │   ├── Size: (200, 200)
    │   └── Position: (0, 200, 0)
    │
    ├── 📝 ItemName (TextMeshProUGUI)
    │   ├── Font Size: 28
    │   └── Position: (0, 80, 0)
    │
    ├── 📝 ItemDescription (TextMeshProUGUI)
    │   ├── Font Size: 16
    │   └── Position: (0, 20, 0)
    │
    ├── 📝 ItemStats (TextMeshProUGUI)
    │   ├── Font Size: 18
    │   └── Position: (0, -60, 0)
    │
    ├── 📝 CraftCost (TextMeshProUGUI)
    │   ├── Font Size: 16
    │   └── Position: (0, -150, 0)
    │
    └── 🔨 CraftButton (Button)
        ├── Size: (200, 60)
        ├── Position: (0, -240, 0)
        └── Text: "Craft"
```

---

### 4. Shop Menu Layout

```
🛒 ShopMenuPanel
═══════════════════════════════════════════════════════════

├── 📋 Header (Panel)
│   ├── Size: (1200, 80)
│   ├── Anchor: Top Stretch
│   │
│   ├── 📝 Title (TextMeshProUGUI)
│   │   ├── Text: "Item Shop"
│   │   ├── Font Size: 36
│   │   └── Position: (0, -40, 0)
│   │
│   └── ❌ CloseButton (Button)
│       ├── Size: (60, 60)
│       └── Position: (570, -40, 0)
│
├── 🎛️ Filters (Panel)
│   ├── Size: (1200, 60)
│   ├── Anchor: Top Stretch
│   ├── Position: (0, -80, 0)
│   │
│   ├── 📝 ClassLabel (Text): "Class:"
│   ├── 🔽 ClassFilterDropdown (TMP_Dropdown)
│   │   ├── Options: All, Mage, Warrior, Ninja
│   │   └── Size: (200, 40)
│   │
│   ├── 📝 TypeLabel (Text): "Item Type:"
│   └── 🔽 ItemTypeFilterDropdown (TMP_Dropdown)
│       ├── Options: Helmet, Chest, Gloves, Legs, Weapon, Ring, Necklace, Bracelet
│       └── Size: (200, 40)
│
├── 💰 CurrencyDisplay (Panel)
│   ├── Size: (1200, 40)
│   ├── Anchor: Top Stretch
│   ├── Position: (0, -140, 0)
│   │
│   └── 📝 GoldText: "Gold: 1000"
│
├── 📜 ItemListPanel (Panel)
│   ├── Size: (600, 600)
│   ├── Anchor: Left
│   ├── Position: (300, -470, 0)
│   │
│   └── 📋 ScrollView (ScrollRect)
│       ├── Vertical: ☑
│       ├── Horizontal: ☐
│       │
│       └── 📦 Content (Vertical Layout Group)
│           ├── Spacing: 10
│           ├── Padding: 10
│           └── Child Force Expand: ☑
│
└── 📊 ItemDetailPanel (Panel)
    ├── Size: (500, 600)
    ├── Anchor: Right
    ├── Position: (-250, -470, 0)
    │
    ├── 🖼️ ItemIcon (Image)
    │   ├── Size: (200, 200)
    │   └── Position: (0, 200, 0)
    │
    ├── 📝 ItemName (TextMeshProUGUI)
    │   ├── Font Size: 28
    │   └── Position: (0, 80, 0)
    │
    ├── 📝 ItemDescription (TextMeshProUGUI)
    │   ├── Font Size: 16
    │   └── Position: (0, 20, 0)
    │
    ├── 📝 ItemStats (TextMeshProUGUI)
    │   ├── Font Size: 18
    │   └── Position: (0, -60, 0)
    │
    ├── 📝 ShopPrice (TextMeshProUGUI)
    │   ├── Font Size: 16
    │   └── Position: (0, -150, 0)
    │
    └── 💰 PurchaseButton (Button)
        ├── Size: (200, 60)
        ├── Position: (0, -240, 0)
        └── Text: "Purchase"
```

---

### 5. Item Card Prefab

```
📦 ItemCard (Prefab)
═══════════════════════════════════════════════════════════

Size: (560, 100)
Component: Button

├── 🖼️ Background (Image)
│   ├── Color: Dark Gray (0.2, 0.2, 0.2, 1)
│   ├── Anchor: Stretch/Stretch
│   └── Sprite: UI/Skin/UISprite
│
├── 🖼️ IconImage (Image)
│   ├── Size: (80, 80)
│   ├── Anchor: Left
│   ├── Position: (50, 0, 0)
│   └── Preserve Aspect: ☑
│
├── 📝 NameText (TextMeshProUGUI)
│   ├── Font Size: 20
│   ├── Anchor: Left
│   ├── Position: (120, 20, 0)
│   ├── Width: 300
│   └── Alignment: Left
│
└── 📝 InfoText (TextMeshProUGUI)
    ├── Font Size: 16
    ├── Anchor: Left
    ├── Position: (120, -20, 0)
    ├── Width: 300
    ├── Alignment: Left
    └── Text: "Lv.1" or "100 Gold"
```

---

### 6. Color Scheme

```
Rarity Colors:
═══════════════════════════════════════════════════════════

⚪ Common    : RGB(128, 128, 128) - #808080
🟢 Uncommon  : RGB(0, 255, 0)     - #00FF00
🔵 Rare      : RGB(0, 0, 255)     - #0000FF
🟣 Epic      : RGB(153, 51, 204)  - #9933CC
🟠 Legendary : RGB(255, 128, 0)   - #FF8000

UI Colors:
═══════════════════════════════════════════════════════════

Background      : RGB(0, 0, 0, 200)    - Semi-transparent black
Panel           : RGB(40, 40, 40, 255) - Dark gray
Button Normal   : RGB(60, 60, 60, 255) - Medium gray
Button Hover    : RGB(80, 80, 80, 255) - Light gray
Button Pressed  : RGB(40, 40, 40, 255) - Dark gray
Button Disabled : RGB(100, 100, 100, 128) - Transparent gray
Text            : RGB(255, 255, 255, 255) - White
Text Disabled   : RGB(128, 128, 128, 255) - Gray
Highlight       : RGB(255, 255, 0, 255) - Yellow
```

---

### 7. Inspector Setup Checklist

```
✅ NPCDisplayRoot Setup:
═══════════════════════════════════════════════════════════
☐ Create GameObject "NPCDisplayRoot"
☐ Add NPCDisplayController component
☐ Create child Camera "NPCDisplayCamera"
☐ Set Camera Target Texture to NPCDisplayRT
☐ Create child Empty "NPCRoot"
☐ Assign CraftNPC prefab
☐ Assign ShopNPC prefab
☐ Set positions (-1.5, 0, 0) and (1.5, 0, 0)

✅ CraftShopPanel Setup:
═══════════════════════════════════════════════════════════
☐ Create Panel "CraftShopPanel" in Canvas
☐ Add CraftShopPanelUI component
☐ Create NPCDisplayPanel with RawImage
☐ Assign NPCDisplayRT to RawImage
☐ Create LeftButton and RightButton
☐ Create CraftLabel and ShopLabel
☐ Create CraftMenuPanel with ItemCraftUI
☐ Create ShopMenuPanel with ItemShopUI
☐ Create CloseButton
☐ Assign all references in Inspector

✅ ItemCraftUI Setup:
═══════════════════════════════════════════════════════════
☐ Create ClassFilterDropdown
☐ Create ItemTypeFilterDropdown
☐ Create Currency Display texts
☐ Create ScrollView for item list
☐ Create ItemDetailPanel
☐ Create CraftButton
☐ Assign ItemCard prefab
☐ Assign all references in Inspector

✅ ItemShopUI Setup:
═══════════════════════════════════════════════════════════
☐ Create ClassFilterDropdown
☐ Create ItemTypeFilterDropdown
☐ Create Gold Display text
☐ Create ScrollView for item list
☐ Create ItemDetailPanel
☐ Create PurchaseButton
☐ Assign ItemCard prefab
☐ Assign all references in Inspector

✅ MainMenuUI Update:
═══════════════════════════════════════════════════════════
☐ Assign CraftShopPanel GameObject
☐ Assign CraftShopPanelUI component
☐ Test Craft/Shop button
```

---

### 8. Layer Setup

```
Layers:
═══════════════════════════════════════════════════════════

Layer 8: NPCLayer
├── Used for: NPC rendering
├── Culling Mask: NPCDisplayCamera only
└── Purpose: Isolate NPC rendering from main camera

Setup:
1. Edit → Project Settings → Tags and Layers
2. Add "NPCLayer" to Layer 8
3. Set NPCDisplayCamera Culling Mask to "NPCLayer"
4. Set CraftNPC and ShopNPC layer to "NPCLayer"
```

---

### 9. RenderTexture Settings

```
NPCDisplayRT (RenderTexture)
═══════════════════════════════════════════════════════════

Size: 1024 x 1024
Depth Buffer: 24 bit
Anti-aliasing: 4x MSAA
Color Format: Default
Dimension: 2D
sRGB: ☑ (Read/Write)
Filter Mode: Bilinear
Wrap Mode: Clamp

Create:
1. Assets → Create → Render Texture
2. Name: "NPCDisplayRT"
3. Set properties above
4. Assign to NPCDisplayCamera.targetTexture
5. Assign to NPCRenderImage.texture
```

---

### 10. Quick Visual Reference

```
Main Flow:
═══════════════════════════════════════════════════════════

┌─────────────────────────────────────────────────────┐
│                  Main Menu                          │
│                                                     │
│              [Craft/Shop Button]                    │
└─────────────────────────────────────────────────────┘
                        ↓
┌─────────────────────────────────────────────────────┐
│              Craft & Shop Panel                     │
│  ┌───────────────────────────────────────────────┐  │
│  │         NPC Display (RenderTexture)           │  │
│  │                                               │  │
│  │    [Craft NPC 👨‍🔧]    [Shop NPC 👨‍💼]          │  │
│  │                                               │  │
│  └───────────────────────────────────────────────┘  │
│                                                     │
│   Craft Master              Shop Keeper             │
│  (Click to Craft)         (Click to Shop)           │
└─────────────────────────────────────────────────────┘
           ↓                           ↓
    ┌─────────────┐            ┌─────────────┐
    │ Craft Menu  │            │  Shop Menu  │
    └─────────────┘            └─────────────┘
```

---

**Tarih:** 2025-11-10  
**Versiyon:** 1.0  
**Not:** Bu görsel rehber Unity Editor'de setup yaparken kullanılmalıdır.

