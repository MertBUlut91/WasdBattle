# Craft & Shop Sistemi - Türkçe Özet

## 🎯 Ne Yaptık?

İki NPC'li (Craft ve Shop) bir sistem oluşturduk. Oyuncular:
- **Craft NPC**'ye tıklayarak item craft edebilir
- **Shop NPC**'ye tıklayarak item satın alabilir
- Class ve item type bazlı filtreleme yapabilir

---

## 📁 Oluşturulan Dosyalar

### UI Scripts (4 adet)

1. **NPCDisplayController.cs**
   - İki NPC'yi yan yana gösterir (RenderTexture ile)
   - Tıklanan NPC'yi highlight eder
   - Otomatik rotasyon özelliği

2. **CraftShopPanelUI.cs**
   - Ana panel controller
   - NPC seçimi ve menü yönetimi
   - Craft/Shop menüleri arası geçiş

3. **ItemCraftUI.cs**
   - Craft menüsü
   - Class filtreleme (All/Mage/Warrior/Ninja)
   - Item type filtreleme (Helmet/Chest/Weapon/etc.)
   - Malzeme kontrolü ve craft işlemi

4. **ItemShopUI.cs**
   - Shop menüsü
   - Class filtreleme (All/Mage/Warrior/Ninja)
   - Item type filtreleme (Helmet/Chest/Weapon/etc.)
   - Gold kontrolü ve satın alma işlemi

### Güncellenen Dosyalar (3 adet)

1. **CraftingSystem.cs**
   - `CanCraftItem(ItemData)` eklendi
   - `CraftItem(ItemData)` eklendi
   - ItemData ile craft desteği

2. **ShopSystem.cs**
   - `CanPurchaseItem(ItemData)` eklendi
   - `PurchaseItem(ItemData)` eklendi
   - ItemData ile purchase desteği

3. **MainMenuUI.cs**
   - Craft/Shop panel referansları eklendi
   - `OnCraftShopClicked()` güncellendi

### Dokümantasyon (3 adet)

1. **CRAFT_SHOP_SYSTEM_GUIDE.md** (Detaylı rehber)
2. **CRAFT_SHOP_QUICK_START.md** (Hızlı başlangıç)
3. **CRAFT_SHOP_OZET.md** (Bu dosya - Türkçe özet)

---

## 🎮 Nasıl Çalışır?

### Akış Şeması

```
Ana Menü
    ↓
[Craft/Shop Butonu]
    ↓
CraftShopPanel Açılır
    ↓
İki NPC Görünür (Yan Yana)
    ↓
┌─────────────────┬─────────────────┐
│   Craft NPC     │    Shop NPC     │
│   (Sol - 👨‍🔧)    │   (Sağ - 👨‍💼)    │
└─────────────────┴─────────────────┘
    ↓                    ↓
Craft Menüsü         Shop Menüsü
    ↓                    ↓
Class Seç            Class Seç
    ↓                    ↓
Item Type Seç        Item Type Seç
    ↓                    ↓
Item Listesi         Item Listesi
    ↓                    ↓
Item Seç             Item Seç
    ↓                    ↓
Detay Gör            Detay Gör
    ↓                    ↓
[Craft]              [Purchase]
    ↓                    ↓
Malzeme Tüket        Gold Tüket
    ↓                    ↓
Item Al              Item Al
```

---

## 🔧 Unity Setup (Özet)

### 1. NPC Display Root

```
Hierarchy → Create Empty → "NPCDisplayRoot"
├── NPCDisplayCamera (Camera)
│   └── Target Texture: NPCDisplayRT
└── NPCRoot (Empty)
```

### 2. NPC Prefab'lar

```
Assets/Prefabs/NPCs/
├── CraftNPC.prefab (3D model)
└── ShopNPC.prefab (3D model)
```

**Geçici Test İçin:**
- CraftNPC: Turuncu Cube
- ShopNPC: Mavi Sphere

### 3. UI Panel

```
Canvas → CraftShopPanel
├── NPCDisplayPanel
│   ├── NPCRenderImage (RawImage)
│   ├── CraftNPCButton (Sol yarı)
│   └── ShopNPCButton (Sağ yarı)
├── CraftMenuPanel (ItemCraftUI)
├── ShopMenuPanel (ItemShopUI)
└── CloseButton
```

### 4. Item Data

```
Assets/Resources/Items/
├── MageHelmet_Common.asset
├── WarriorSword_Rare.asset
└── NinjaRing_Uncommon.asset
```

**Her Item'da Ayarla:**
- ✅ Can Be Crafted (Craft için)
- ✅ Crafting Materials (Metal, Rune, etc.)
- ✅ Can Be Bought (Shop için)
- ✅ Shop Price (Gold)

---

## 🎯 Özellikler

### Craft Sistemi
- ✅ Class bazlı filtreleme
- ✅ Item type bazlı filtreleme
- ✅ Malzeme kontrolü
- ✅ Craft cost gösterimi
- ✅ Yetersiz malzemede buton pasif
- ✅ Rarity renkleri

### Shop Sistemi
- ✅ Class bazlı filtreleme
- ✅ Item type bazlı filtreleme
- ✅ Gold kontrolü
- ✅ Shop price gösterimi
- ✅ Yetersiz Gold'da buton pasif
- ✅ Rarity renkleri

### NPC Gösterimi
- ✅ RenderTexture ile 3D NPC'ler
- ✅ Yan yana duran iki NPC
- ✅ Tıklanan NPC highlight olur
- ✅ Otomatik rotasyon

---

## 📊 Rarity Renkleri

| Rarity    | Renk      | Hex       |
|-----------|-----------|-----------|
| Common    | Gri       | #808080   |
| Uncommon  | Yeşil     | #00FF00   |
| Rare      | Mavi      | #0000FF   |
| Epic      | Mor       | #9933CC   |
| Legendary | Turuncu   | #FF8000   |

---

## 🧪 Test Adımları

### 1. NPC Seçimi
```
1. Play Mode
2. Craft/Shop butonuna tıkla
3. ✅ Panel açılmalı
4. ✅ İki NPC görünmeli
5. Sol NPC'ye tıkla
6. ✅ NPC sarı olmalı (highlight)
7. ✅ Craft menüsü açılmalı
```

### 2. Craft İşlemi
```
1. Craft menüsünde Class: Mage seç
2. Item Type: Helmet seç
3. ✅ Mage helmet'ları görünmeli
4. Bir item'a tıkla
5. ✅ Detay paneli açılmalı
6. ✅ Stats ve Craft Cost gösterilmeli
7. Craft butonuna tıkla
8. ✅ Malzemeler azalmalı
9. ✅ Item inventory'ye eklenmeli
```

### 3. Shop İşlemi
```
1. Sağ NPC'ye (Shop) tıkla
2. ✅ Shop menüsü açılmalı
3. Class: Warrior seç
4. Item Type: Weapon seç
5. ✅ Warrior weapon'ları görünmeli
6. Bir item'a tıkla
7. ✅ Detay paneli açılmalı
8. ✅ Stats ve Price gösterilmeli
9. Purchase butonuna tıkla
10. ✅ Gold azalmalı
11. ✅ Item inventory'ye eklenmeli
```

---

## 🐛 Sık Karşılaşılan Sorunlar

### NPC'ler görünmüyor
**Çözüm:**
- NPCDisplayCamera aktif mi?
- RenderTexture atanmış mı?
- NPC Prefab'ları atanmış mı?

### Item listesi boş
**Çözüm:**
- Items klasörü Resources içinde mi?
- Item'ların canBeCrafted/canBeBought true mu?

### Craft butonu pasif
**Çözüm:**
- Yeterli malzeme var mı?
- craftingMaterials dolu mu?

### Purchase butonu pasif
**Çözüm:**
- Yeterli Gold var mı?
- shopPrice > 0 mı?

---

## 💡 Önemli Notlar

### ItemData Gereksinimleri

**Craft için:**
```csharp
canBeCrafted = true
craftingMaterials = { Metal: 50, Cloth: 20 }
requiredClass = Mage (veya All)
```

**Shop için:**
```csharp
canBeBought = true
shopPrice = 100
requiredClass = Warrior (veya All)
```

### Resources Klasörü

Item'lar **mutlaka** `Resources/Items/` içinde olmalı!

```
Assets/Resources/Items/
├── MageHelmet_Common.asset
├── MageRobe_Uncommon.asset
└── ...
```

---

## 🚀 Sonraki Adımlar

### Yapılabilecek Geliştirmeler

1. **Multiple Currency**
   - Gem, Diamond ile satın alma
   - Currency dropdown

2. **Confirmation Dialog**
   - Craft onay ekranı
   - Purchase onay ekranı

3. **3D Item Preview**
   - Item'ı 3D olarak göster
   - Rotate ile inceleme

4. **Bulk Crafting**
   - Birden fazla item craft et
   - Slider ile miktar seç

5. **Shop Discount**
   - Daily deals
   - Limited time offers

6. **Craft Queue**
   - Sırayla craft
   - Timer sistemi

---

## 📚 Detaylı Dokümantasyon

- **CRAFT_SHOP_SYSTEM_GUIDE.md** → Tam rehber (İngilizce)
- **CRAFT_SHOP_QUICK_START.md** → Hızlı başlangıç
- **ITEM_SYSTEM_SETUP.md** → Item kurulumu
- **EQUIPMENT_SYSTEM_GUIDE.md** → Equipment sistemi

---

## 📝 Özet

✅ **4 yeni UI script** oluşturuldu  
✅ **3 sistem script'i** güncellendi  
✅ **3 dokümantasyon** hazırlandı  
✅ **Class bazlı filtreleme** eklendi  
✅ **Item type filtreleme** eklendi  
✅ **NPC gösterimi** (RenderTexture)  
✅ **Craft sistemi** (malzeme kontrolü)  
✅ **Shop sistemi** (Gold kontrolü)  
✅ **Rarity renkleri** eklendi  

---

**Tarih:** 2025-11-10  
**Versiyon:** 1.0  
**Durum:** ✅ Tamamlandı

