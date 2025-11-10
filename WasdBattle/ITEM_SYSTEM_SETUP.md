# Item System Setup Guide

## 📦 Item Sistemi Kurulum Rehberi

Bu rehber, item sistemini kurmak ve test etmek için gereken tüm adımları içerir.

> **🆕 YENİ:** Ekipman sistemi artık **çift tıklama** ve **sürükle-bırak** desteği ile geliştirildi!  
> Detaylar için: [EQUIPMENT_DRAG_DROP_GUIDE.md](EQUIPMENT_DRAG_DROP_GUIDE.md) ve [DEGISIKLIK_OZETI.md](DEGISIKLIK_OZETI.md)

---

## 1️⃣ Test Itemleri Oluşturma

### Unity Editor'da:

1. **WasdBattle** → **Create Test Items** menüsüne tıklayın
2. Console'da şu mesajı göreceksiniz: `[ItemCreator] Test items created successfully!`
3. `Assets/Resources/Items/` klasöründe itemler oluşturulacak

### Oluşturulan Test Itemleri:

**Weapons:**
- `item_warrior_sword` - Warrior's Blade (Common)
- `item_mage_staff` - Mage's Staff (Common)
- `item_rogue_dagger` - Rogue's Dagger (Common)
- `item_legendary_sword` - Legendary Excalibur (Legendary)

**Armor:**
- `item_warrior_helmet` - Iron Helmet (Common)
- `item_warrior_chest` - Iron Chestplate (Common)
- `item_warrior_gloves` - Iron Gauntlets (Common)
- `item_warrior_legs` - Iron Greaves (Common)
- `item_mage_helmet` - Mystic Hood (Common)
- `item_mage_chest` - Mystic Robe (Common)
- `item_rogue_helmet` - Leather Cap (Common)
- `item_rogue_chest` - Leather Armor (Common)

**Accessories:**
- `item_ring_common` - Simple Ring (Common)
- `item_ring_rare` - Enchanted Ring (Rare)
- `item_necklace_common` - Simple Necklace (Common)
- `item_necklace_epic` - Dragon's Pendant (Epic)
- `item_bracelet_common` - Simple Bracelet (Common)

---

## 2️⃣ Karakterlere Başlangıç Itemleri Ekleme

### Unity Editor'da:

1. `Assets/Resources/Characters/` klasöründe bir karakter seçin (örn: `char_mage`)
2. Inspector'da **Starting Equipment** bölümünü bulun
3. **Size** değerini artırın (örn: 5)
4. Her slot'a item sürükleyin:
   - Element 0: `item_mage_staff`
   - Element 1: `item_mage_helmet`
   - Element 2: `item_mage_chest`
   - Element 3: `item_ring_common`
   - Element 4: `item_necklace_common`

### Tüm Karakterler İçin Önerilen Setup:

**Warrior (char_warrior):**
- item_warrior_sword
- item_warrior_helmet
- item_warrior_chest
- item_warrior_gloves
- item_warrior_legs

**Mage (char_mage):**
- item_mage_staff
- item_mage_helmet
- item_mage_chest
- item_ring_common
- item_necklace_common

**Rogue (char_rogue):**
- item_rogue_dagger
- item_rogue_helmet
- item_rogue_chest
- item_bracelet_common

---

## 3️⃣ Debug Menu Kurulumu (Test İçin)

### Unity Editor'da:

1. **MainMenuScene** açın
2. Hierarchy'de **Canvas** altına sağ tık → **Create Empty**
3. İsim: `DebugMenu`
4. **Add Component** → `DebugMenuUI`

### Debug Panel UI Oluşturma:

```
DebugMenu
├── ToggleButton (Button) - "F12 / Debug" yazısı
└── Panel (GameObject)
    ├── Background (Image - Dark semi-transparent)
    ├── Title (TextMeshProUGUI - "Debug Menu")
    ├── CloseButton (Button - "X")
    ├── ItemSection (GameObject)
    │   ├── AddWarriorItemsButton (Button - "Add Warrior Items")
    │   ├── AddMageItemsButton (Button - "Add Mage Items")
    │   ├── AddRogueItemsButton (Button - "Add Rogue Items")
    │   ├── AddAllItemsButton (Button - "Add ALL Items")
    │   └── ClearInventoryButton (Button - "Clear Inventory")
    ├── CurrencySection (GameObject)
    │   ├── Add1000GoldButton (Button - "+1000 Gold")
    │   └── Add100GemsButton (Button - "+100 Gems")
    └── InfoText (TextMeshProUGUI - Inventory count display)
```

### DebugMenuUI Component Referansları:

Inspector'da `DebugMenuUI` component'inde:
- **Panel** → Panel GameObject
- **Toggle Button** → ToggleButton
- **Close Button** → CloseButton
- **Add Warrior Items Button** → AddWarriorItemsButton
- **Add Mage Items Button** → AddMageItemsButton
- **Add Rogue Items Button** → AddRogueItemsButton
- **Add All Items Button** → AddAllItemsButton
- **Clear Inventory Button** → ClearInventoryButton
- **Add 1000 Gold Button** → Add1000GoldButton
- **Add 100 Gems Button** → Add100GemsButton
- **Inventory Count Text** → InfoText

---

## 4️⃣ Test Etme

### Play Mode'da:

1. **F12** tuşuna basın (Debug Menu açılır)
2. **Add Warrior Items** → Warrior itemleri inventory'e eklenir
3. **Add Mage Items** → Mage itemleri inventory'e eklenir
4. **Add All Items** → Tüm itemler eklenir
5. **Clear Inventory** → Inventory temizlenir

### Inventory'i Görüntüleme:

1. Main Menu'de **Inventory** button'a tıklayın
2. Sol panelde item listesi görünmeli
3. Filter buttonları ile filtreleme yapın:
   - **All** → Tüm itemler
   - **Weapons** → Sadece silahlar
   - **Helmet** → Sadece kasklar
   - vb.

### Item Equip Etme:

1. Sol panelden bir item'e tıklayın
2. Item otomatik olarak uygun slot'a equip edilir
3. Sağ panelde equipped item görünür
4. Alt panelde stat değişimi görünür

---

## 5️⃣ Başlangıç Itemlerini Otomatik Ekleme

Karakterin başlangıç itemlerini otomatik olarak inventory'e eklemek için karakter seçim sisteminde şu kodu kullanın:

```csharp
// Karakter unlock edildiğinde
CharacterData characterData = ...; // Unlock edilen karakter
PlayerData playerData = GameManager.Instance.CurrentPlayerData;

// Başlangıç itemlerini ekle
playerData.AddStartingItems(characterData);

// Kaydet
GameManager.Instance.DataManager.SavePlayerDataAsync(playerData);
```

---

## 6️⃣ Yeni Item Oluşturma (Manuel)

### Unity Editor'da:

1. Project'te sağ tık → **Create** → **WasdBattle** → **Item Data**
2. İsim ver (örn: `item_epic_sword`)
3. Inspector'da özellikleri ayarla:
   - **Item Id**: `item_epic_sword`
   - **Item Name**: `Epic Sword`
   - **Slot**: `Weapon`
   - **Required Class**: `Warrior` veya `All`
   - **Rarity**: `Epic`
   - **HP Bonus**: `50`
   - **Stamina Bonus**: `30`
   - **Armor Bonus**: `20`
   - **Magic Resist Bonus**: `10`
   - **Description**: Item açıklaması
   - **Icon**: Item icon sprite
4. `Assets/Resources/Items/` klasörüne taşı

---

## 7️⃣ Console Log'ları

### Item Eklendiğinde:
```
[PlayerData] Item added to inventory: item_warrior_sword
[DataManager] Saved to local cache
[DataManager] Player data saved successfully
```

### Item Zaten Varsa:
```
[PlayerData] Item already owned: item_warrior_sword
```

### Başlangıç Itemleri Eklendiğinde:
```
[PlayerData] Added 5 starting items for Warrior
```

### Filter Değiştiğinde:
```
[EquipmentUI] Filter changed to: Helmet
```

---

## 🎯 Hızlı Test Senaryosu

1. ✅ Unity Editor'ı aç
2. ✅ **WasdBattle** → **Create Test Items** (itemleri oluştur)
3. ✅ Karakterlere başlangıç itemleri ekle (Inspector'da)
4. ✅ Play Mode'a geç
5. ✅ **F12** bas (Debug Menu aç)
6. ✅ **Add Warrior Items** (test itemleri ekle)
7. ✅ **Inventory** button'a tıkla
8. ✅ Sol panelde itemleri gör
9. ✅ Filter buttonları ile filtrele
10. ✅ Item'e tıkla ve equip et
11. ✅ Sağ panelde equipped item'i gör
12. ✅ Alt panelde stat değişimini gör

---

## 🐛 Troubleshooting

### "Item not found" hatası:
- `Assets/Resources/Items/` klasörünü kontrol edin
- Item ID'lerin doğru olduğundan emin olun
- Resources klasörü altında olduğundan emin olun

### Itemler görünmüyor:
- Player inventory'de item var mı kontrol edin (F12 → Info Text)
- Filter doğru mu kontrol edin (All seçili olmalı)
- Character class uyumlu mu kontrol edin

### Item equip edilmiyor:
- Item slot'u doğru mu kontrol edin
- Character class uyumlu mu kontrol edin
- CharacterLoadout doğru mu kontrol edin

---

## 🔥 Salvage (Item Eritme) Sistemi

### ItemData Salvage Ayarları

Her item için salvage sistemi otomatik olarak crafting materyallerinden hesaplanır:

```csharp
[Header("Salvage (Item Eritme)")]
public bool canBeSalvaged = true;
[Range(0f, 1f)]
public float salvageReturnRate = 0.5f; // %50 geri dönüş
```

### Nasıl Çalışır?

1. **ItemData'da Crafting Materials Ekle:**
   - Metal: 100
   - Energy Crystal: 50
   - Rune: 10

2. **Salvage Return Rate Ayarla:**
   - 0.5 = %50 (Varsayılan)
   - 0.75 = %75
   - 0.25 = %25

3. **Otomatik Hesaplama:**
   - Metal: 100 × 0.5 = 50
   - Energy Crystal: 50 × 0.5 = 25
   - Rune: 10 × 0.5 = 5

### Inspector'da Preview

ItemData'yı Inspector'da açtığınızda, salvage preview otomatik olarak gösterilir:

```
Salvage Preview
Bu item eritildiğinde şu materyaller geri dönecek:
(Crafting maliyetinin %50'si)

• Metal: 50
• EnergyCrystal: 25
• Rune: 5

Crafting Cost vs Salvage Return:
Metal: 100 → 50
EnergyCrystal: 50 → 25
Rune: 10 → 5
```

### Kod ile Salvage İşlemi

```csharp
// Tek item salvage
SalvageManager.Instance.SalvageItem(itemData, 1);

// Birden fazla item salvage
SalvageManager.Instance.SalvageItem(itemData, 5);

// Salvage edilebilir mi kontrol
bool canSalvage = SalvageManager.Instance.CanSalvageItem(itemData, 1);

// Preview al
string preview = SalvageManager.Instance.GetSalvagePreview(itemData, 3);
```

### Salvage Kuralları

- ✅ Equipped itemler salvage edilemez (önce unequip etmeli)
- ✅ Inventory'de yeterli item olmalı
- ✅ Crafting materials yoksa salvage edilemez
- ✅ `canBeSalvaged = false` ise salvage edilemez
- ✅ Salvage edilen itemler inventory'den kalkar
- ✅ Materyaller otomatik olarak PlayerData'ya eklenir

---

## 📝 Notlar

- **Debug Menu** sadece test için kullanılmalı, production'da kaldırılmalı
- **Starting Items** karakterin ilk unlock edildiğinde otomatik eklenir
- **Item Icon'ları** henüz eklenmedi, sprite atayın
- **Item Rarity Colors** otomatik olarak uygulanır
- **Stat Comparison** item hover'da çalışır
- **Salvage System** crafting materyallerinden otomatik hesaplanır

---

## ✅ Tamamlandı!

Item sistemi artık çalışıyor! Inventory'e item ekleyebilir, filtreleyebilir, equip edebilir, stat değişimlerini görebilir ve itemleri eritebilirsiniz.

