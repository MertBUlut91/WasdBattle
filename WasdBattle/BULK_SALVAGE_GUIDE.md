# Bulk Salvage (Toplu Eritme) Sistemi

## 🎯 Özellik

Confirmation panel'de **slider** ile birden fazla item'ı aynı anda salvage edebilirsiniz!

---

## 🎮 Nasıl Çalışır?

### Tek Item (1 adet)
1. Item'ı çöp kutusuna sürükle
2. Confirmation panel açılır
3. Slider **görünmez** (sadece 1 adet var)
4. Confirm → 1 item salvage edilir

### Birden Fazla Item (2+ adet)
1. Item'ı çöp kutusuna sürükle (örn: Simple Ring x5)
2. Confirmation panel açılır
3. **Slider görünür** (min: 1, max: 5)
4. Slider'ı kaydır → Amount: 3 seç
5. Preview **otomatik güncellenir** (toplam materyal gösterilir)
6. Confirm → 3 item salvage edilir

---

## 🔧 Unity Setup

### Confirmation Panel Hierarchy

```
ConfirmationPanel (GameObject)
├── Background (Image - Dark overlay)
├── DialogBox (Panel)
│   ├── Title (TextMeshProUGUI) - "Confirm Salvage"
│   ├── Message (TextMeshProUGUI) - "Are you sure...?"
│   │
│   ├── SliderPanel (GameObject) ← YENİ
│   │   ├── Label (TextMeshProUGUI) - "Amount:"
│   │   ├── Slider (Slider)
│   │   │   ├── Background
│   │   │   ├── Fill Area → Fill
│   │   │   └── Handle Slide Area → Handle
│   │   └── CountText (TextMeshProUGUI) - "Amount: 1"
│   │
│   ├── TotalPreviewText (TextMeshProUGUI) ← YENİ
│   │   - "• Metal: 150"
│   │   - "• Rune: 75"
│   │
│   ├── ConfirmButton (Button) - "Salvage"
│   └── CancelButton (Button) - "Cancel"
```

### Inspector Ayarları

```
SalvageDropZone (Script)
├── Confirmation (Optional)
│   ├── Require Confirmation: ☑ ON
│   ├── Confirmation Panel: [ConfirmationPanel]
│   ├── Confirmation Text: [Message TextMeshProUGUI]
│   ├── Confirm Button: [ConfirmButton]
│   └── Cancel Button: [CancelButton]
│
└── Bulk Salvage
    ├── Slider Panel: [SliderPanel GameObject]
    ├── Count Slider: [Slider component]
    ├── Count Text: [CountText TextMeshProUGUI]
    └── Total Preview Text: [TotalPreviewText TextMeshProUGUI]
```

### Slider Setup

```
Slider Component:
├── Min Value: 1 (otomatik ayarlanır)
├── Max Value: 5 (otomatik ayarlanır - item count'a göre)
├── Whole Numbers: ☑ ON
└── Value: 1
```

---

## 💻 Kod Detayları

### Yeni Özellikler

**1. Slider Visibility**
```csharp
// 1 item varsa slider gizli
if (availableCount > 1)
    _sliderPanel.SetActive(true);
else
    _sliderPanel.SetActive(false);
```

**2. Dynamic Preview**
```csharp
// Slider değişince preview güncellenir
private void OnSliderValueChanged(float value)
{
    _pendingSalvageCount = Mathf.RoundToInt(value);
    UpdateConfirmationText();
}
```

**3. Total Material Preview**
```csharp
// Toplam materyal hesaplama
string preview = SalvageManager.Instance.GetSalvagePreview(itemData, count);
// Örnek: "• Metal: 150\n• Rune: 75"
```

---

## 🎨 UI Layout Örneği

```
┌─────────────────────────────────────────┐
│         Confirm Salvage                 │
├─────────────────────────────────────────┤
│                                         │
│  Are you sure you want to salvage:     │
│                                         │
│  Simple Ring x3?                        │
│                                         │
│  ┌───────────────────────────────────┐ │
│  │ Amount:                           │ │
│  │ ━━━━━━━●━━━━━━━━━━━━━━━━━━━━━━━  │ │
│  │ Amount: 3                         │ │
│  └───────────────────────────────────┘ │
│                                         │
│  You will receive:                     │
│  • Metal: 150                          │
│  • Rune: 75                            │
│                                         │
│  [Salvage]           [Cancel]          │
│                                         │
└─────────────────────────────────────────┘
```

---

## 🧪 Test Senaryoları

### Test 1: Tek Item
1. Simple Ring x1 var
2. Çöp kutusuna bırak
3. ✅ Slider görünmemeli
4. ✅ "Simple Ring x1" göstermeli
5. Confirm → 1 item salvage edilmeli

### Test 2: Birden Fazla Item
1. Simple Ring x5 var
2. Çöp kutusuna bırak
3. ✅ Slider görünmeli (1-5 arası)
4. ✅ Slider 1'de başlamalı
5. Slider'ı 3'e çek
6. ✅ "Simple Ring x3" göstermeli
7. ✅ Preview güncellenmeli (3x materyal)
8. Confirm → 3 item salvage edilmeli
9. ✅ Inventory'de 2 item kalmalı

### Test 3: Max Salvage
1. Simple Ring x5 var
2. Slider'ı 5'e çek (max)
3. ✅ "Simple Ring x5" göstermeli
4. Confirm → 5 item salvage edilmeli
5. ✅ Inventory'den tamamen kaybolmalı

### Test 4: Preview Güncelleme
1. Simple Ring x10 var (50 Metal, 25 Rune per item)
2. Slider 1: "Metal: 50, Rune: 25"
3. Slider 5: "Metal: 250, Rune: 125"
4. Slider 10: "Metal: 500, Rune: 250"
5. ✅ Her değişimde preview güncellenmeli

---

## 📊 Örnek Senaryolar

### Senaryo 1: Hızlı Temizlik
```
Inventory: Common Helmet x20
1. Çöp kutusuna bırak
2. Slider'ı 20'ye çek (max)
3. Confirm
4. ✅ 20 helmet salvage edildi
5. ✅ 1000 Metal, 500 Crystal kazanıldı
```

### Senaryo 2: Kısmi Salvage
```
Inventory: Rare Sword x3
1. Çöp kutusuna bırak
2. Slider'ı 2'ye çek
3. Confirm
4. ✅ 2 sword salvage edildi
5. ✅ 1 sword inventory'de kaldı
```

### Senaryo 3: Confirmation Cancel
```
Inventory: Epic Ring x5
1. Çöp kutusuna bırak
2. Slider'ı 5'e çek
3. Cancel
4. ✅ Hiçbir şey salvage edilmedi
5. ✅ 5 ring inventory'de kaldı
```

---

## 🎯 Avantajlar

✅ **Hızlı Temizlik**: Birden fazla item'ı tek seferde salvage et  
✅ **Kontrollü**: Kaç adet salvage edeceğini seç  
✅ **Preview**: Toplam kazanılacak materyali gör  
✅ **Güvenli**: Confirmation ile yanlışlık önlenir  
✅ **Esnek**: 1'den max'a kadar herhangi bir sayı seçilebilir

---

## 🐛 Troubleshooting

### Problem: Slider görünmüyor

**Çözüm:**
- `Require Confirmation` açık mı?
- `Slider Panel` referansı verilmiş mi?
- Item count 1'den fazla mı?

### Problem: Preview güncellenmiyor

**Çözüm:**
- `Total Preview Text` referansı verilmiş mi?
- `Count Text` referansı verilmiş mi?
- Slider'ın `onValueChanged` event'i bağlı mı?

### Problem: Yanlış sayıda salvage ediliyor

**Çözüm:**
- Slider'ın `Whole Numbers` açık mı?
- Min/Max değerleri doğru mu?

---

## 📚 İlgili Dokümantasyon

- [SALVAGE_DRAG_DROP_GUIDE.md](SALVAGE_DRAG_DROP_GUIDE.md) - Salvage drag-drop
- [SALVAGE_SYSTEM_GUIDE.md](SALVAGE_SYSTEM_GUIDE.md) - Salvage sistemi
- [COP_KUTUSU_SALVAGE_OZET.md](COP_KUTUSU_SALVAGE_OZET.md) - Türkçe özet

---

**Tarih:** 2025-11-10  
**Versiyon:** 1.1 (Bulk Salvage eklendi)

