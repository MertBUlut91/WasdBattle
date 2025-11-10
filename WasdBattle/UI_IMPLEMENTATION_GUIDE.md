# 🎨 UI Implementation Guide - Unity Editor Kurulum Rehberi

Bu rehber, yeni UI sistemini Unity Editor'de adım adım kurmanız için hazırlanmıştır.

---

## 📋 İçindekiler

1. [3D Character Display Setup](#1-3d-character-display-setup)
2. [Main Menu UI Güncellemesi](#2-main-menu-ui-güncellemesi)
3. [Character Panel Oluşturma](#3-character-panel-oluşturma)
4. [Inventory Panel (Equipment) Oluşturma](#4-inventory-panel-equipment-oluşturma)
5. [UI Prefab'ları Oluşturma](#5-ui-prefabları-oluşturma)
6. [Final Bağlantılar ve Test](#6-final-bağlantılar-ve-test)

---

## 1. 3D Character Display Setup

### Adım 1.1: CharacterDisplayRoot Oluşturma

1. **Hierarchy'de** sağ tık → **Create Empty** → `CharacterDisplayRoot` olarak adlandır
2. **Transform** pozisyonunu sıfırla (0, 0, 0)
3. **Layer'ı** `UI` olarak ayarla (veya yeni bir `CharacterDisplay` layer'ı oluştur)

### Adım 1.2: Camera Oluşturma

1. **CharacterDisplayRoot** altında sağ tık → **Camera** → `CharacterDisplayCamera` olarak adlandır
2. **Transform** ayarları:
   - Position: (0, 1.5, 3)
   - Rotation: (0, 180, 0)
3. **Camera** ayarları:
   - Clear Flags: **Solid Color**
   - Background: **Siyah** veya **Transparan** (Alpha: 0)
   - Culling Mask: Sadece `CharacterDisplay` layer'ı (veya UI)
   - Depth: **-2** (Main Camera'dan önce render olsun)
   - Target Display: **Display 1**

### Adım 1.3: RenderTexture Oluşturma

1. **Project** → **Assets** → **Create** → **Render Texture** → `CharacterDisplayRT` olarak adlandır
2. **RenderTexture** ayarları:
   - Size: **1024 x 1024**
   - Depth Buffer: **24 bit**
   - Anti-aliasing: **4x**
   - Color Format: **Default**
3. **CharacterDisplayCamera**'yı seç
4. **Inspector**'da **Target Texture** alanına `CharacterDisplayRT`'yi sürükle

### Adım 1.4: CharacterDisplayController Script Ekleme

1. **CharacterDisplayRoot**'u seç
2. **Add Component** → `CharacterDisplayController`
3. **Inspector**'da referansları bağla:
   - Display Camera: `CharacterDisplayCamera`
   - Render Texture: `CharacterDisplayRT`
   - Character Root: `CharacterDisplayRoot` (kendisi)
4. **Camera Positions** ayarları (varsayılan değerler iyi):
   - Main Menu Camera Position: (0, 1.5, 3)
   - Character Panel Camera Position: (-1.5, 1.5, 3)
   - Inventory Panel Camera Position: (1.5, 1.5, 3)

---

## 2. Main Menu UI Güncellemesi

### Adım 2.1: Currency Display Güncelleme

1. **MainMenuScene** → **Canvas** → **CurrencyPanel**'i aç
2. **Gem** ve **Diamond** UI elementlerini **SİL** veya **deaktif et**
3. Sadece **Gold** gösterimi kalsın

### Adım 2.2: Bottom Buttons Güncelleme

1. **Canvas** → **BottomButtonsPanel**'i bul (yoksa oluştur)
2. **Horizontal Layout Group** ekle:
   - Spacing: 20
   - Child Alignment: Middle Center
   - Child Force Expand: Width ✓, Height ✓
3. **4 Button** oluştur:
   - `CharacterButton` → Text: "CHARACTER"
   - `InventoryButton` → Text: "INVENTORY"
   - `CraftShopButton` → Text: "CRAFT & SHOP"
   - `SettingsButton` → Text: "SETTINGS"
4. **Quit Button**'u **SİL** (artık gerekli değil)

### Adım 2.3: 3D Character Display Area

1. **Canvas** altında **UI** → **Raw Image** → `CharacterDisplayImage` oluştur
2. **RectTransform** ayarları:
   - Anchor: Sağ taraf (veya ortada, tasarıma göre)
   - Width: 800, Height: 1000 (veya istediğiniz boyut)
3. **Raw Image** component:
   - Texture: `CharacterDisplayRT` (RenderTexture)
4. **Opsiyonel:** Bir **Panel** veya **Image** ile çerçeve ekleyin

### Adım 2.4: MainMenuUI Script Güncelleme

1. **Canvas**'ı seç (MainMenuUI script'i burada)
2. **Inspector**'da yeni referansları bağla:
   - Character Display Controller: `CharacterDisplayRoot`
   - Character Button: `CharacterButton`
   - Inventory Button: `InventoryButton`
   - Craft Shop Button: `CraftShopButton`
   - Settings Button: `SettingsButton`
3. **Eski referansları kaldır:**
   - Essence Text (artık yok)
   - Character Select Button (artık Character Button)
   - Quit Button (artık yok)

---

## 3. Character Panel Oluşturma

### Adım 3.1: Panel Oluşturma

1. **Canvas** altında **UI** → **Panel** → `CharacterPanel` oluştur
2. **RectTransform**: Fullscreen (Stretch/Stretch, Offset: 0)
3. **Image** component: Color: Siyah, Alpha: 0.8 (yarı saydam)
4. **Active**: **FALSE** (başta kapalı)

### Adım 3.2: Content Panel

1. **CharacterPanel** altında **UI** → **Panel** → `ContentPanel`
2. **RectTransform**:
   - Anchor: Center
   - Width: 1600, Height: 900
3. **Image** component: Color: Koyu gri (#2D2D2D)

### Adım 3.3: Close Button

1. **ContentPanel** altında **UI** → **Button** → `CloseButton`
2. **RectTransform**:
   - Anchor: Top-Right
   - Width: 60, Height: 60
   - Pos X: -10, Pos Y: -10
3. **Text**: "X" (Font Size: 36, Color: Red)

### Adım 3.4: Left Panel - Character List

1. **ContentPanel** altında **UI** → **Panel** → `LeftPanel`
2. **RectTransform**:
   - Anchor: Left-Stretch
   - Width: 400
   - Offset Left: 20, Top: -100, Bottom: 20
3. **Scroll View** ekle:
   - **UI** → **Scroll View** → `CharacterListScrollView`
   - **Content** → **Vertical Layout Group**:
     - Spacing: 10
     - Padding: 10
   - **Content** → **Content Size Fitter**:
     - Vertical Fit: Preferred Size

### Adım 3.5: Center Panel - 3D Display

1. **ContentPanel** altında **UI** → **Raw Image** → `CenterPanel`
2. **RectTransform**:
   - Anchor: Center
   - Width: 600, Height: 800
3. **Raw Image**: Texture: `CharacterDisplayRT`

### Adım 3.6: Right Panel - Character Info & Skills

1. **ContentPanel** altında **UI** → **Panel** → `RightPanel`
2. **RectTransform**:
   - Anchor: Right-Stretch
   - Width: 500
   - Offset Right: -20, Top: -100, Bottom: 20
3. **Vertical Layout Group**:
   - Spacing: 20
   - Padding: 10
   - Child Force Expand: Width ✓

#### 3.6.1: Basic Info Panel

1. **RightPanel** altında **UI** → **Panel** → `BasicInfoPanel`
2. **Layout Element**: Min Height: 150
3. İçinde:
   - **Text (TMP)** → `CharacterNameText` (Font Size: 36, Bold)
   - **Text (TMP)** → `CharacterLevelText` (Font Size: 24)
   - **Text (TMP)** → `CharacterDescriptionText` (Font Size: 18, Word Wrap)

#### 3.6.2: Stats Panel

1. **RightPanel** altında **UI** → **Panel** → `StatsPanel`
2. **Layout Element**: Min Height: 150
3. **Vertical Layout Group**: Spacing: 5
4. İçinde 4 Text:
   - `HPText` → "HP: 4500"
   - `StaminaText` → "Stamina: 320"
   - `ArmorText` → "Armor: 280"
   - `MagicResistText` → "Magic Resist: 380"

#### 3.6.3: Skill Category Buttons

1. **RightPanel** altında **UI** → **Panel** → `SkillCategoryPanel`
2. **Layout Element**: Min Height: 60
3. **Horizontal Layout Group**: Spacing: 10
4. İçinde 4 Button:
   - `LightSkillButton` → "LIGHT"
   - `NormalSkillButton` → "NORMAL"
   - `HeavySkillButton` → "HEAVY"
   - `UltimateSkillButton` → "ULTIMATE"

#### 3.6.4: Skill List Scroll View

1. **RightPanel** altında **UI** → **Scroll View** → `SkillListScrollView`
2. **Layout Element**: Min Height: 200
3. **Scroll Rect**: Horizontal ✓, Vertical ✗
4. **Content** → **Horizontal Layout Group**:
   - Spacing: 10
   - Padding: 10

#### 3.6.5: Skill Details Panel

1. **RightPanel** altında **UI** → **Panel** → `SkillDetailsPanel`
2. **Layout Element**: Min Height: 150
3. **Active**: FALSE (başta gizli)
4. İçinde:
   - **Image** → `SkillDetailIcon` (120x120)
   - **Text (TMP)** → `SkillDetailNameText`
   - **Text (TMP)** → `SkillDetailDamageText`
   - **Text (TMP)** → `SkillDetailEffectText`

### Adım 3.7: CharacterPanelUI Script Ekleme

1. **CharacterPanel**'i seç
2. **Add Component** → `CharacterPanelUI`
3. **Inspector**'da tüm referansları bağla (yukarıda oluşturduğunuz elementler)

---

## 4. Inventory Panel (Equipment) Oluşturma

### Adım 4.1: Panel Oluşturma

1. **Canvas** altında **UI** → **Panel** → `InventoryPanel`
2. **RectTransform**: Fullscreen (Stretch/Stretch)
3. **Image**: Color: Siyah, Alpha: 0.8
4. **Active**: **FALSE**

### Adım 4.2: Content Panel

1. **InventoryPanel** altında **UI** → **Panel** → `ContentPanel`
2. **RectTransform**: Center, Width: 1600, Height: 900
3. **Image**: Color: #2D2D2D

### Adım 4.3: Close Button

(Character Panel ile aynı)

### Adım 4.4: Left Panel - Item List

1. **ContentPanel** altında **UI** → **Panel** → `LeftPanel`
2. **RectTransform**: Left-Stretch, Width: 400
3. **Tab Buttons** (üstte):
   - **Panel** → `TabButtonsPanel` (Horizontal Layout Group)
   - İçinde 4 Button:
     - `AllTabButton` → "ALL"
     - `WeaponsTabButton` → "WEAPONS"
     - `ArmorTabButton` → "ARMOR"
     - `ConsumablesTabButton` → "CONSUMABLES"
4. **Scroll View** → `ItemListScrollView`
   - **Content** → **Grid Layout Group**:
     - Cell Size: 100x100
     - Spacing: 10
     - Constraint: Fixed Column Count (3)

### Adım 4.5: Center Panel - 3D Display

(Character Panel ile aynı - RenderTexture kullan)

### Adım 4.6: Right Panel - Equipment & Stats

1. **ContentPanel** altında **UI** → **Panel** → `RightPanel`
2. **RectTransform**: Right-Stretch, Width: 500
3. **Vertical Layout Group**: Spacing: 20

#### 4.6.1: Equipment Slots Panel

1. **RightPanel** altında **UI** → **Panel** → `EquipmentSlotsPanel`
2. **Layout Element**: Min Height: 500
3. **9 Equipment Slot** oluştur (her biri için):
   - **Panel** → `HelmetSlot` (Width: 120, Height: 120)
   - İçinde:
     - **Image** → `ItemIcon` (100x100)
     - **Text (TMP)** → `SlotName` ("Helmet")
     - **Button** → `UnequipButton` ("X", Top-Right, gizli)
     - **Image** → `EmptySlotIndicator` (gri, başta aktif)

**Slot Pozisyonları** (Manual placement):
```
Helmet:    (X: 190, Y: -80)
Chest:     (X: 190, Y: -220)
Gloves:    (X: 65, Y: -360)    Legs: (X: 315, Y: -360)
Weapon:    (X: 190, Y: -500)
Ring1:     (X: 65, Y: -620)    Ring2: (X: 190, Y: -620)
Necklace:  (X: 315, Y: -620)
Bracelet:  (X: 190, Y: -740)
```

#### 4.6.2: Stats Panel

1. **RightPanel** altında **UI** → **Panel** → `StatsPanel`
2. **Layout Element**: Min Height: 200
3. **Vertical Layout Group**: Spacing: 10
4. İçinde 4 Text (stat comparison için):
   - `HPStatText` → "HP: 4500"
   - `StaminaStatText` → "Stamina: 320"
   - `ArmorStatText` → "Armor: 280"
   - `MagicResistStatText` → "Magic Resist: 380"

### Adım 4.7: EquipmentUI Script

1. **InventoryPanel**'i seç
2. **Add Component** → `EquipmentUI`
3. **Inspector**'da tüm referansları bağla
4. **Equipment Slots** array'ini 9 elemanlı yap ve her slot için:
   - Item Icon
   - Slot Name Text
   - Unequip Button
   - Empty Slot Indicator

---

## 5. UI Prefab'ları Oluşturma

### 5.1: CharacterListItem Prefab

1. **Hierarchy'de** **UI** → **Panel** → `CharacterListItem` oluştur
2. **RectTransform**: Width: 380, Height: 100
3. **Horizontal Layout Group**: Spacing: 10, Padding: 10
4. İçinde:
   - **Image** → `Icon` (80x80)
   - **Panel** → `InfoPanel` (Vertical Layout Group)
     - **Text (TMP)** → `Name`
     - **Text (TMP)** → `Level`
   - **Image** → `SelectedIndicator` (Border, başta gizli)
5. **Add Component** → `Button`
6. **Add Component** → `CharacterListItemUI`
7. **Drag to Project** → `Assets/Prefabs/UI/CharacterListItem.prefab`

### 5.2: ItemListCard Prefab

1. **UI** → **Panel** → `ItemListCard` (100x100)
2. İçinde:
   - **Image** → `Icon` (80x80)
   - **Image** → `RarityBorder` (Border, 100x100)
   - **Text (TMP)** → `Name` (Bottom, small)
   - **Image** → `EquippedIndicator` (Top-Right, başta gizli)
3. **Add Component** → `Button`
4. **Add Component** → `ItemCardUI`
5. **Drag to Project** → `Assets/Prefabs/UI/ItemListCard.prefab`

### 5.3: SkillCard Prefab

1. **UI** → **Panel** → `SkillCard` (120x150)
2. İçinde:
   - **Image** → `Icon` (100x100)
   - **Text (TMP)** → `Name`
   - **Text (TMP)** → `Damage`
   - **Text (TMP)** → `Cooldown`
   - **Image** → `SelectedIndicator` (Border, başta gizli)
3. **Add Component** → `Button`
4. **Add Component** → `SkillCardUI`
5. **Drag to Project** → `Assets/Prefabs/UI/SkillCard.prefab`

---

## 6. Final Bağlantılar ve Test

### 6.1: MainMenuUI Referansları

1. **Canvas** (MainMenuUI script) → **Inspector**'da bağla:
   - Character Display Controller: `CharacterDisplayRoot`
   - Character Panel: `CharacterPanel`
   - Character Panel UI: `CharacterPanel` (CharacterPanelUI component)
   - Inventory Panel: `InventoryPanel`
   - Equipment UI: `InventoryPanel` (EquipmentUI component)

### 6.2: CharacterPanelUI Referansları

1. **CharacterPanel** → **Inspector**'da bağla:
   - Character Display Controller: `CharacterDisplayRoot`
   - Main Menu UI: `Canvas` (MainMenuUI component)
   - Character List Item Prefab: `CharacterListItem` prefab
   - Skill Card Prefab: `SkillCard` prefab

### 6.3: EquipmentUI Referansları

1. **InventoryPanel** → **Inspector**'da bağla:
   - Character Display Controller: `CharacterDisplayRoot`
   - Main Menu UI: `Canvas`
   - Item Card Prefab: `ItemListCard` prefab

### 6.4: Test

1. **Play Mode**'a gir
2. **Ana ekranda** seçili karakterin 3D modelini görmelisin
3. **CHARACTER** butonuna tıkla:
   - Panel açılmalı
   - Karakter listesi görünmeli
   - Kamera pozisyonu değişmeli
4. **INVENTORY** butonuna tıkla:
   - Panel açılmalı
   - Item listesi görünmeli (class-filtered)
   - Equipment slotları görünmeli
   - Stat comparison çalışmalı
5. **Close** butonlarına tıkla:
   - Paneller kapanmalı
   - Kamera main menu pozisyonuna dönmeli

---

## 🔧 Troubleshooting

### 3D Karakter Görünmüyor
- RenderTexture'ın Camera'ya bağlı olduğundan emin olun
- Camera'nın Culling Mask'inin doğru olduğundan emin olun
- CharacterData.characterPrefab'ın null olmadığından emin olun

### Paneller Açılmıyor
- Button onClick event'lerinin bağlı olduğundan emin olun
- Script referanslarının null olmadığından emin olun
- Console'da hata olup olmadığını kontrol edin

### Item Listesi Boş
- PlayerData.ownedItems'ın dolu olduğundan emin olun
- ItemData ScriptableObject'lerinin Resources/ScriptableObjects/Items'da olduğundan emin olun
- Class filtering'in doğru çalıştığından emin olun

### Stat Comparison Çalışmıyor
- EquipmentSlotUI array'inin doğru boyutta olduğundan emin olun
- Her slot'un referanslarının bağlı olduğundan emin olun
- ItemData bonuslarının doğru değerlere sahip olduğundan emin olun

---

## 📝 Notlar

- **Prefab'lar** Unity Editor'de manuel oluşturulmalı (yukarıdaki adımları takip edin)
- **RenderTexture** boyutunu performansa göre ayarlayabilirsiniz (512x512 - 2048x2048)
- **Lighting** ve **Equipment Visual** sistemi sonra eklenecek
- **Craft & Shop** panelleri şimdilik placeholder

---

**Başarılar!** 🎉

