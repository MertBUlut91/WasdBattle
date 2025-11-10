# Quick Reference - Equipment Drag & Drop

## 🚀 Hızlı Başlangıç

### Unity Setup (5 Dakika)

#### 1. ItemCardPrefab
```
Assets/Prefabs/UI/ItemCardPrefab.prefab
└── Add Component: CanvasGroup (otomatik eklenir)
```

#### 2. Equipment Slots (Her slot için)
```
HelmetSlot GameObject
└── Add Component: EquipmentSlotDropZone
    ├── Slot Type: Helmet
    ├── Highlight Image: [Background Image]
    └── Highlight Color: (1, 1, 0, 0.3)
```

#### 3. Test
- Helmet'e çift tıkla → Giydirilmeli ✅
- Ring'i sürükle → Ring slot'una bırak ✅

---

## 🎮 Kullanım

### Çift Tıklama
```
1. Item'a çift tıkla (< 0.3 saniye)
2. Otomatik olarak uygun slot'a giydirilir
```

### Sürükle-Bırak
```
1. Item'ı tıkla ve basılı tut
2. Sürükle (yarı saydam olur)
3. Slot'un üzerine gel (sarı highlight)
4. Bırak (giydirilir)
```

---

## 🔧 Slot Type'lar

| Slot Type | Açıklama |
|-----------|----------|
| `Helmet` | Kask slot'u |
| `Chest` | Göğüs zırhı slot'u |
| `Gloves` | Eldiven slot'u |
| `Legs` | Bacak zırhı slot'u |
| `Weapon` | Silah slot'u |
| `Ring1` | 1. yüzük slot'u |
| `Ring2` | 2. yüzük slot'u |
| `Necklace` | Kolye slot'u |
| `Bracelet` | Bilezik slot'u |

---

## 🐛 Sorun Giderme

| Sorun | Çözüm |
|-------|-------|
| Çift tıklama çalışmıyor | Button component'i disable et |
| Sürükleme başlamıyor | CanvasGroup var mı kontrol et |
| Drop çalışmıyor | EquipmentSlotDropZone eklendi mi? |
| Highlight yok | Highlight Image referansı verildi mi? |

---

## 📝 Kod Snippet'leri

### EquipmentSlotDropZone Setup (Script)
```csharp
// Slot GameObject'ine ekle
var dropZone = gameObject.AddComponent<EquipmentSlotDropZone>();
dropZone.SetSlotType(EquipmentSlot.Helmet);
```

### ItemCardUI Setup (Script)
```csharp
// ItemCard prefab'ında otomatik
// Sadece ItemData'yı set et
itemCard.SetItemData(itemData);
```

---

## 📚 Detaylı Dokümantasyon

- **Teknik Rehber:** [EQUIPMENT_DRAG_DROP_GUIDE.md](EQUIPMENT_DRAG_DROP_GUIDE.md)
- **Türkçe Özet:** [DEGISIKLIK_OZETI.md](DEGISIKLIK_OZETI.md)
- **Akış Diyagramları:** [EQUIPMENT_INTERACTION_FLOW.md](EQUIPMENT_INTERACTION_FLOW.md)
- **Tüm Değişiklikler:** [CHANGES_SUMMARY.md](CHANGES_SUMMARY.md)

---

## ✅ Checklist

### Unity Setup
- [ ] ItemCardPrefab → CanvasGroup
- [ ] 9 slot → EquipmentSlotDropZone
- [ ] Slot Type'lar ayarlandı
- [ ] Highlight Image referansları verildi

### Test
- [ ] Çift tıklama test edildi
- [ ] Sürükle-bırak test edildi
- [ ] Invalid drop test edildi

---

**Versiyon:** 1.0 | **Tarih:** 2025-11-10

