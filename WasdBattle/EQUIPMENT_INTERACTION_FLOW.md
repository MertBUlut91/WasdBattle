# Equipment Interaction Flow Diagram

## 🎯 Kullanıcı Etkileşim Akışı

### Yöntem 1: Çift Tıklama (Double-Click)

```
┌─────────────────────────────────────────────────────────────────┐
│                    ÇIFT TIKLAMA AKIŞI                            │
└─────────────────────────────────────────────────────────────────┘

   Kullanıcı                ItemCardUI              EquipmentUI
      │                         │                         │
      │  Tek Tık (1. tık)       │                         │
      ├────────────────────────>│                         │
      │                         │                         │
      │                         │ _lastClickTime kaydet   │
      │                         │                         │
      │  Tek Tık (2. tık)       │                         │
      │  (< 0.3 saniye içinde)  │                         │
      ├────────────────────────>│                         │
      │                         │                         │
      │                         │ ✅ Double-click algıla  │
      │                         │                         │
      │                         │  OnItemDoubleClicked()  │
      │                         ├────────────────────────>│
      │                         │                         │
      │                         │                         │ EquipItem()
      │                         │                         │ ├─ Slot validation
      │                         │                         │ ├─ Ring count check
      │                         │                         │ ├─ Equip to loadout
      │                         │                         │ └─ Save to cloud
      │                         │                         │
      │                         │  UpdateEquipmentSlots() │
      │                         │<────────────────────────│
      │                         │                         │
      │  UI Güncellendi         │                         │
      │<────────────────────────┤                         │
      │  (Item slot'ta görünür) │                         │
      │                         │                         │
```

---

### Yöntem 2: Sürükle-Bırak (Drag & Drop)

```
┌─────────────────────────────────────────────────────────────────┐
│                   SÜRÜKLE-BIRAK AKIŞI                            │
└─────────────────────────────────────────────────────────────────┘

   Kullanıcı          ItemCardUI         EquipmentSlotDropZone    EquipmentUI
      │                  │                         │                    │
      │  Mouse Down      │                         │                    │
      │  (Basılı Tut)    │                         │                    │
      ├─────────────────>│                         │                    │
      │                  │                         │                    │
      │                  │ OnBeginDrag()           │                    │
      │                  │ ├─ Save original pos    │                    │
      │                  │ ├─ Move to canvas top   │                    │
      │                  │ ├─ Set alpha = 0.6      │                    │
      │                  │ └─ Disable raycasts     │                    │
      │                  │                         │                    │
      │  Mouse Move      │                         │                    │
      │  (Sürükleme)     │                         │                    │
      ├─────────────────>│                         │                    │
      │                  │                         │                    │
      │                  │ OnDrag()                │                    │
      │                  │ └─ Follow mouse         │                    │
      │                  │                         │                    │
      │  Mouse Over Slot │                         │                    │
      ├─────────────────>│                         │                    │
      │                  │                         │                    │
      │                  │  Raycast check          │                    │
      │                  ├────────────────────────>│                    │
      │                  │                         │                    │
      │                  │                         │ OnPointerEnter()   │
      │                  │                         │ ├─ Validate slot   │
      │                  │                         │ └─ 🟡 Highlight    │
      │                  │                         │                    │
      │  Mouse Up        │                         │                    │
      │  (Bırak)         │                         │                    │
      ├─────────────────>│                         │                    │
      │                  │                         │                    │
      │                  │ OnEndDrag()             │                    │
      │                  │ ├─ Raycast all          │                    │
      │                  │ ├─ Find drop zone       │                    │
      │                  │ └─ Restore alpha        │                    │
      │                  │                         │                    │
      │                  │  OnItemDropped()        │                    │
      │                  ├────────────────────────>│                    │
      │                  │                         │                    │
      │                  │                         │ ✅ Validate slot   │
      │                  │                         │                    │
      │                  │                         │ EquipItemFromDrag()│
      │                  │                         ├───────────────────>│
      │                  │                         │                    │
      │                  │                         │                    │ EquipItem()
      │                  │                         │                    │ ├─ Equip
      │                  │                         │                    │ └─ Save
      │                  │                         │                    │
      │                  │  UpdateEquipmentSlots() │                    │
      │                  │<───────────────────────────────────────────│
      │                  │                         │                    │
      │  UI Güncellendi  │                         │                    │
      │<─────────────────┤                         │                    │
      │                  │                         │                    │
```

---

## 🎨 Görsel Geri Bildirim

### Sürükleme Durumları

```
┌────────────────────────────────────────────────────────────────┐
│                    SÜRÜKLEME DURUMLARI                          │
└────────────────────────────────────────────────────────────────┘

1. BAŞLANGIÇ (Idle)
   ┌──────────┐
   │  [Icon]  │  ← Normal görünüm
   │  Helmet  │     Alpha = 1.0
   └──────────┘     Raycast = ON

2. SÜRÜKLEME BAŞLADI (Begin Drag)
   ┌──────────┐
   │  [Icon]  │  ← Yarı saydam
   │  Helmet  │     Alpha = 0.6
   └──────────┘     Raycast = OFF
        ↓
        │ (Mouse'u takip eder)
        ↓

3a. UYGUN SLOT ÜZERİNDE (Valid Slot Hover)
    ┌──────────┐
    │  [Icon]  │  ← Yarı saydam item
    │  Helmet  │
    └──────────┘
         ↓
    ╔══════════╗
    ║ 🟡 SLOT  ║  ← Sarı highlight (Valid!)
    ║  Helmet  ║
    ╚══════════╝

3b. UYGUNSUZ SLOT ÜZERİNDE (Invalid Slot Hover)
    ┌──────────┐
    │  [Icon]  │  ← Yarı saydam item
    │  Helmet  │
    └──────────┘
         ↓
    ┌──────────┐
    │   Ring   │  ← Highlight YOK (Invalid!)
    │   Slot   │
    └──────────┘

4a. BAŞARILI DROP (Valid Drop)
    ╔══════════╗
    ║  [Icon]  ║  ← Item slot'ta görünür
    ║  Helmet  ║     Alpha = 1.0
    ╚══════════╝     ✅ Equipped!

4b. BAŞARISIZ DROP (Invalid Drop)
    ┌──────────┐
    │  [Icon]  │  ← Item orijinal yerine döner
    │  Helmet  │     Alpha = 1.0
    └──────────┘     ❌ Not equipped
```

---

## 🔄 Slot Validation Mantığı

```
┌────────────────────────────────────────────────────────────────┐
│                    SLOT VALIDATION                              │
└────────────────────────────────────────────────────────────────┘

Item Dropped on Slot
        │
        ↓
    Is Valid Slot?
        │
        ├─ YES ─→ EquipmentSlotDropZone.IsValidSlot()
        │            │
        │            ├─ Item.slot == Slot.type? ✅
        │            │   └─> Equip Item
        │            │
        │            └─ Ring Special Case:
        │                 - Item is Ring?
        │                 - Slot is Ring1 or Ring2?
        │                 └─> ✅ Allow
        │
        └─ NO ──→ Reject Drop ❌
                   └─> Item returns to inventory

Örnekler:
┌──────────────┬──────────────┬─────────┐
│ Item Type    │ Target Slot  │ Result  │
├──────────────┼──────────────┼─────────┤
│ Helmet       │ Helmet       │ ✅ OK   │
│ Helmet       │ Chest        │ ❌ NO   │
│ Ring         │ Ring1        │ ✅ OK   │
│ Ring         │ Ring2        │ ✅ OK   │
│ Ring         │ Necklace     │ ❌ NO   │
│ Weapon       │ Weapon       │ ✅ OK   │
│ Weapon       │ Gloves       │ ❌ NO   │
└──────────────┴──────────────┴─────────┘
```

---

## 🎯 Ring Özel Mantığı

```
┌────────────────────────────────────────────────────────────────┐
│                    RING EQUIP MANTIGI                           │
└────────────────────────────────────────────────────────────────┘

Scenario 1: İlk Ring Equip (Çift Tıklama)
    Ring1: [Empty]    Ring2: [Empty]
       ↓
    Double-click on "Silver Ring"
       ↓
    Ring1: [Silver Ring]    Ring2: [Empty]  ✅

Scenario 2: İkinci Ring Equip (Çift Tıklama)
    Ring1: [Silver Ring]    Ring2: [Empty]
       ↓
    Double-click on "Gold Ring"
       ↓
    Ring1: [Silver Ring]    Ring2: [Gold Ring]  ✅

Scenario 3: Aynı Ring'den 2 Tane (Çift Tıklama)
    Ring1: [Empty]    Ring2: [Empty]
    Inventory: Silver Ring x2
       ↓
    Double-click on "Silver Ring" (1. tıklama)
       ↓
    Ring1: [Silver Ring]    Ring2: [Empty]
       ↓
    Double-click on "Silver Ring" (2. tıklama)
       ↓
    Ring1: [Silver Ring]    Ring2: [Silver Ring]  ✅

Scenario 4: Drag to Specific Slot
    Ring1: [Silver Ring]    Ring2: [Empty]
       ↓
    Drag "Gold Ring" to Ring2
       ↓
    Ring1: [Silver Ring]    Ring2: [Gold Ring]  ✅

Scenario 5: Swap Ring (Drag)
    Ring1: [Silver Ring]    Ring2: [Gold Ring]
       ↓
    Drag "Ruby Ring" to Ring1
       ↓
    Ring1: [Ruby Ring]    Ring2: [Gold Ring]  ✅
    (Silver Ring returns to inventory)
```

---

## 📊 Component İlişkileri

```
┌────────────────────────────────────────────────────────────────┐
│                    COMPONENT HIERARCHY                          │
└────────────────────────────────────────────────────────────────┘

EquipmentPanel (GameObject)
│
├── ItemListPanel (Sol)
│   │
│   └── ScrollView
│       │
│       └── Content
│           │
│           ├── ItemCard (Prefab Instance)
│           │   ├── ItemCardUI (Script) ← Drag source
│           │   │   ├── IPointerClickHandler
│           │   │   ├── IBeginDragHandler
│           │   │   ├── IDragHandler
│           │   │   └── IEndDragHandler
│           │   │
│           │   ├── CanvasGroup (Component)
│           │   ├── Image (Icon)
│           │   ├── Image (RarityBorder)
│           │   └── TextMeshProUGUI (Name)
│           │
│           ├── ItemCard (Prefab Instance)
│           └── ItemCard (Prefab Instance)
│
└── EquipmentSlotsPanel (Sağ)
    │
    ├── HelmetSlot (GameObject)
    │   ├── EquipmentSlotDropZone (Script) ← Drop target
    │   │   ├── IDropHandler
    │   │   ├── IPointerEnterHandler
    │   │   └── IPointerExitHandler
    │   │
    │   ├── Image (Background) ← Highlight image
    │   ├── Image (ItemIcon)
    │   └── Button (UnequipButton)
    │
    ├── ChestSlot (GameObject)
    ├── WeaponSlot (GameObject)
    ├── Ring1Slot (GameObject)
    ├── Ring2Slot (GameObject)
    └── ... (diğer slotlar)

EquipmentUI (Script) ← Main controller
├── OnItemDoubleClicked()
├── EquipItemFromDrag()
└── EquipItem() (private)
```

---

## 🔍 Debug Flow

```
┌────────────────────────────────────────────────────────────────┐
│                    DEBUG LOG AKIŞI                              │
└────────────────────────────────────────────────────────────────┘

Çift Tıklama Debug:
[ItemCardUI] Double-click detected on Silver Ring
[EquipmentUI] Item double-clicked: Silver Ring (Ring1)
[EquipmentUI] Item equipped: item_ring_silver to slot Ring1

Sürükle-Bırak Debug:
[ItemCardUI] Begin drag: Silver Ring
[ItemCardUI] End drag: Silver Ring
[EquipmentSlotDropZone] Item Silver Ring dropped on Ring1 slot
[EquipmentUI] Item dragged: Silver Ring to slot Ring1
[EquipmentUI] Item equipped: item_ring_silver to slot Ring1

Invalid Drop Debug:
[ItemCardUI] Begin drag: Helmet
[ItemCardUI] End drag: Helmet
[ItemCardUI] Dropped outside valid slot
(Helmet slot'u dışında bir yere bırakıldı)

veya:

[EquipmentSlotDropZone] Item Helmet cannot be equipped in Ring1 slot
(Helmet'i Ring slot'una bırakmaya çalıştı)
```

---

## 🎮 Kullanıcı Senaryoları

### Senaryo 1: Yeni Oyuncu (İlk Defa Equipment)
```
1. Oyuncu envanter'i açar
2. Helmet'i görür
3. Helmet'e çift tıklar
4. ✅ Helmet karakterde görünür
5. Oyuncu mutlu! 😊
```

### Senaryo 2: Deneyimli Oyuncu (Hızlı Equip)
```
1. Oyuncu 5 item birden equip etmek istiyor
2. Her birine çift tıklar (hızlı)
3. ✅ Tüm itemler giydirilir
4. Oyuncu çok mutlu! 😄
```

### Senaryo 3: Hassas Oyuncu (Specific Slot)
```
1. Oyuncu Ring1'de Silver Ring var
2. Ring2'ye Gold Ring giydirmek istiyor
3. Gold Ring'i sürükleyip Ring2'ye bırakır
4. ✅ Ring2'de Gold Ring görünür
5. Ring1'deki Silver Ring yerinde kalır
6. Oyuncu çok çok mutlu! 🤩
```

### Senaryo 4: Yanlışlık (Invalid Drop)
```
1. Oyuncu Helmet'i sürükler
2. Yanlışlıkla Ring slot'una bırakmaya çalışır
3. ❌ Helmet giydirilmez
4. Helmet orijinal yerine döner
5. Oyuncu "Aa tamam, yanlış yere bırakmışım" der
6. Tekrar dener, doğru slot'a bırakır ✅
```

---

## 📝 Notlar

- Double-click süresi: **0.3 saniye** (ayarlanabilir)
- Drag alpha değeri: **0.6** (ayarlanabilir)
- Highlight rengi: **Sarı (1, 1, 0, 0.3)** (ayarlanabilir)
- Ring slot mantığı: **Akıllı yerleştirme** (boş slot öncelikli)

---

**Son Güncelleme:** 2025-11-10  
**Versiyon:** 1.0

