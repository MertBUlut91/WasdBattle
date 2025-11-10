# Salvage Drag & Drop System

## 📋 Genel Bakış

Envanter sistemine **çöp kutusu (salvage zone)** eklendi! Artık itemleri sürükleyip çöp kutusuna bırakarak salvage edebilirsiniz.

### 🎯 Özellikler

✅ **Drag & Drop Salvage**: Item'ı çöp kutusuna sürükleyip bırakarak erit  
✅ **Görsel Geri Bildirim**: Kırmızı highlight, pulse animasyon  
✅ **Validation**: Sadece salvage edilebilir itemler kabul edilir  
✅ **Otomatik Güncelleme**: Salvage sonrası inventory otomatik yenilenir  
✅ **Confirmation (Opsiyonel)**: İsteğe bağlı onay penceresi  
✅ **Animasyonlar**: Success/fail animasyonları

---

## 🎮 Kullanım

### Salvage Nasıl Yapılır?

1. Envanter'den bir item'ı **tıklayın ve basılı tutun**
2. Item'ı **çöp kutusuna sürükleyin**
3. Çöp kutusu **kırmızı highlight** olur ve **pulse** animasyonu oynar
4. Mouse'u **bırakın**
5. Item salvage edilir ve materyaller kazanılır
6. Inventory otomatik güncellenir

### Görsel Geri Bildirim

- 🔴 **Normal Durum**: Açık kırmızı (standby)
- 🔥 **Valid Item Üzerinde**: Parlak kırmızı + pulse animasyon
- ⚫ **Invalid Item Üzerinde**: Gri (salvage edilemez)
- ✅ **Başarılı Salvage**: Scale up animasyon
- ❌ **Başarısız Salvage**: Shake animasyon

---

## 🔧 Unity Setup

### 1. Salvage Zone (Çöp Kutusu) Oluşturma

#### Hierarchy:
```
EquipmentPanel
└── LeftPanel
    └── SalvageZone (GameObject) ← YENİ
        ├── Background (Image)
        ├── Icon (Image) - Çöp kutusu ikonu
        └── Label (TextMeshProUGUI) - "Salvage"
```

#### SalvageZone GameObject Setup:

1. **GameObject Oluştur**
   ```
   Right-click LeftPanel → UI → Panel
   Rename: "SalvageZone"
   ```

2. **RectTransform Ayarla**
   ```
   Anchor: Bottom-Center
   Width: 150
   Height: 150
   Pos X: 0
   Pos Y: 50
   ```

3. **Background Image**
   ```
   Color: (0.8, 0.2, 0.2, 0.5) - Açık kırmızı
   Raycast Target: ✅ ON (ÖNEMLİ!)
   ```

4. **Icon Image**
   ```
   Sprite: Çöp kutusu ikonu
   Size: 80x80
   Anchor: Center
   ```

5. **SalvageDropZone Component Ekle**
   ```
   Add Component → SalvageDropZone
   ```

#### Inspector Settings:

```
SalvageDropZone (Script)
├── Visual Feedback
│   ├── Highlight Image: [Background Image referansı]
│   ├── Normal Color: (0.8, 0.2, 0.2, 0.5)
│   ├── Valid Highlight Color: (1, 0.3, 0.3, 0.8)
│   └── Invalid Highlight Color: (0.5, 0.5, 0.5, 0.5)
│
├── Icon Animation
│   ├── Icon Transform: [Icon Image Transform]
│   ├── Pulse Scale: 1.2
│   └── Pulse Duration: 0.3
│
└── Confirmation (Optional)
    ├── Require Confirmation: ☐ (false - opsiyonel)
    ├── Confirmation Panel: (boş bırakılabilir)
    ├── Confirmation Text: (boş bırakılabilir)
    ├── Confirm Button: (boş bırakılabilir)
    └── Cancel Button: (boş bırakılabilir)
```

### 2. Confirmation Panel (Opsiyonel)

Eğer salvage için onay penceresi istiyorsanız:

```
SalvageZone
└── ConfirmationPanel (GameObject)
    ├── Background (Image - Dark overlay)
    ├── Panel (Image - Dialog box)
    │   ├── Title (TextMeshProUGUI) - "Confirm Salvage"
    │   ├── Message (TextMeshProUGUI) - Dinamik mesaj
    │   ├── ConfirmButton (Button) - "Salvage"
    │   └── CancelButton (Button) - "Cancel"
```

Inspector'da:
```
Require Confirmation: ☑ ON
Confirmation Panel: [ConfirmationPanel referansı]
Confirmation Text: [Message TextMeshProUGUI]
Confirm Button: [ConfirmButton]
Cancel Button: [CancelButton]
```

---

## 💻 Kod Detayları

### SalvageDropZone.cs

**Özellikler:**
- `IDropHandler`: Drop event'ini yakalar
- `IPointerEnterHandler`, `IPointerExitHandler`: Hover highlight
- Salvage validation (canBeSalvaged kontrolü)
- Visual feedback (highlight, animations)
- Optional confirmation dialog

**Ana Methodlar:**

```csharp
// Item drop edildiğinde
public void OnItemDropped(ItemData itemData)
{
    // Validation
    if (!SalvageManager.Instance.CanSalvageItem(itemData, 1))
        return;
    
    // Confirmation veya direkt salvage
    if (_requireConfirmation)
        ShowConfirmation(itemData);
    else
        PerformSalvage(itemData);
}

// Salvage işlemi
private void PerformSalvage(ItemData itemData)
{
    bool success = SalvageManager.Instance.SalvageItem(itemData, 1);
    
    if (success)
    {
        PlaySalvageAnimation();
        _equipmentUI.RefreshInventoryList();
    }
}
```

### ItemCardUI.cs (Güncellendi)

**Değişiklik:**
- `OnEndDrag()` içinde SalvageDropZone kontrolü eklendi
- Hem equipment slot hem de salvage zone destekleniyor

```csharp
public void OnEndDrag(PointerEventData eventData)
{
    // Raycast ile drop target'ı bul
    var results = new List<RaycastResult>();
    EventSystem.current.RaycastAll(eventData, results);
    
    foreach (var result in results)
    {
        // Equipment slot?
        var equipmentDropZone = result.gameObject.GetComponent<EquipmentSlotDropZone>();
        if (equipmentDropZone != null)
        {
            equipmentDropZone.OnItemDropped(_itemData);
            break;
        }
        
        // Salvage zone?
        var salvageDropZone = result.gameObject.GetComponent<SalvageDropZone>();
        if (salvageDropZone != null)
        {
            salvageDropZone.OnItemDropped(_itemData);
            break;
        }
    }
}
```

### EquipmentUI.cs (Güncellendi)

**Yeni Method:**
```csharp
public void RefreshInventoryList()
{
    LoadItemList();
}
```

---

## 🎨 Animasyonlar

### Pulse Animation (Hover)
```csharp
LeanTween.scale(iconObject, Vector3.one * 1.2f, 0.3f)
    .setEase(LeanTweenType.easeInOutSine)
    .setLoopPingPong();
```

### Success Animation (Salvage)
```csharp
LeanTween.scale(iconObject, originalScale * 1.5f, 0.2f)
    .setEase(LeanTweenType.easeOutQuad)
    .setOnComplete(() => {
        LeanTween.scale(iconObject, originalScale, 0.2f);
    });
```

### Shake Animation (Invalid)
```csharp
LeanTween.moveLocalX(iconObject, originalPos.x + 10f, 0.05f)
    .setEase(LeanTweenType.easeShake)
    .setLoopCount(4);
```

---

## 🧪 Test Senaryoları

### Test 1: Valid Salvage
1. Salvage edilebilir bir item sürükle (örn: Common Helmet)
2. ✅ Çöp kutusu parlak kırmızı highlight olmalı
3. ✅ Icon pulse animasyonu oynamalı
4. ✅ Bırakınca item salvage edilmeli
5. ✅ Materyaller kazanılmalı
6. ✅ Inventory güncellenip item kaybolmalı

### Test 2: Invalid Salvage
1. Salvage edilemeyen bir item sürükle (canBeSalvaged = false)
2. ✅ Çöp kutusu gri highlight olmalı
3. ✅ Pulse animasyonu OLMAMALI
4. ✅ Bırakınca salvage edilMEMELI
5. ✅ Shake animasyonu oynamalı

### Test 3: Equipped Item
1. Equipped bir item'ı unequip et
2. Inventory'ye düşmeli
3. Şimdi salvage edilebilmeli
4. ✅ Çöp kutusuna bırakınca salvage edilmeli

### Test 4: Confirmation Dialog
1. `Require Confirmation` = true yap
2. Item'ı çöp kutusuna bırak
3. ✅ Confirmation dialog açılmalı
4. ✅ Preview gösterilmeli (ne kadar materyal kazanılacak)
5. ✅ Confirm → Salvage edilmeli
6. ✅ Cancel → Salvage edilmemeli

### Test 5: Multiple Items
1. Aynı item'dan 3 tane var
2. Birini salvage et
3. ✅ Count 3'ten 2'ye düşmeli
4. ✅ Item hala inventory'de görünmeli
5. Diğer 2'sini de salvage et
6. ✅ Item inventory'den tamamen kaybolmalı

---

## 🐛 Troubleshooting

### Problem: Çöp kutusu highlight olmuyor

**Çözüm:**
- Background Image'ın `Raycast Target` açık mı?
- `Highlight Image` referansı verilmiş mi?
- SalvageDropZone component'i eklenmiş mi?

### Problem: Drop çalışmıyor

**Çözüm:**
- Canvas'ta `Graphic Raycaster` var mı?
- SalvageZone GameObject'i aktif mi?
- ItemCardUI güncel mi? (SalvageDropZone kontrolü var mı?)

### Problem: Animasyon oynanmıyor

**Çözüm:**
- `Icon Transform` referansı verilmiş mi?
- LeanTween package'ı yüklü mü?
- Icon GameObject aktif mi?

### Problem: Salvage sonrası inventory güncellenmiyor

**Çözüm:**
- `EquipmentUI.RefreshInventoryList()` çağrılıyor mu?
- SalvageDropZone'da `_equipmentUI` referansı doğru mu?

### Problem: Equipped item salvage ediliyor

**Çözüm:**
- Bu normal! Inventory'deki itemler zaten equipped olanları göstermiyor
- Eğer equipped item görünüyorsa, EquipmentUI'daki filtreleme kontrol edilmeli

---

## 📊 Salvage Materyalleri

### Örnek Salvage Returns

```
Common Helmet (Crafting: 100 Metal, 50 Crystal)
└── Salvage (50% return):
    ├── Metal: 50
    └── Crystal: 25

Rare Sword (Crafting: 200 Metal, 100 Crystal, 50 Rune)
└── Salvage (50% return):
    ├── Metal: 100
    ├── Crystal: 50
    └── Rune: 25

Legendary Armor (Crafting: 500 Metal, 300 Crystal, 100 Rune, 50 Essence)
└── Salvage (75% return):
    ├── Metal: 375
    ├── Crystal: 225
    ├── Rune: 75
    └── Essence: 37
```

### Salvage Return Rate Önerileri

| Rarity | Return Rate | Açıklama |
|--------|-------------|----------|
| Common | 25-40% | Düşük geri dönüş |
| Uncommon | 40-50% | Orta geri dönüş |
| Rare | 50-60% | İyi geri dönüş |
| Epic | 60-75% | Yüksek geri dönüş |
| Legendary | 75-90% | Çok yüksek geri dönüş |

---

## 🎯 Gelecek Geliştirmeler

### Öneri 1: Bulk Salvage
Aynı item'dan birden fazla salvage:
```csharp
// Shift basılıysa tüm stack'i salvage et
if (Input.GetKey(KeyCode.LeftShift))
{
    int count = playerData.GetItemCount(itemData.itemId);
    SalvageManager.Instance.SalvageItem(itemData, count);
}
```

### Öneri 2: Salvage Particles
Salvage sonrası particle effect:
```csharp
private void PlaySalvageParticles()
{
    if (_salvageParticles != null)
    {
        _salvageParticles.Play();
    }
}
```

### Öneri 3: Sound Effects
```csharp
// Salvage başarılı
AudioManager.Instance?.PlaySFX("ItemSalvage");

// Invalid drop
AudioManager.Instance?.PlaySFX("ErrorBeep");
```

### Öneri 4: Material Preview Popup
Hover sırasında ne kadar materyal kazanılacağını göster:
```csharp
public void OnPointerEnter(PointerEventData eventData)
{
    var itemCard = eventData.pointerDrag.GetComponent<ItemCardUI>();
    if (itemCard != null)
    {
        string preview = SalvageManager.Instance.GetSalvagePreview(itemData, 1);
        TooltipManager.Instance?.ShowTooltip(preview, eventData.position);
    }
}
```

---

## 📚 İlgili Dokümantasyon

- [SALVAGE_SYSTEM_GUIDE.md](SALVAGE_SYSTEM_GUIDE.md) - Salvage sistemi detayları
- [EQUIPMENT_DRAG_DROP_GUIDE.md](EQUIPMENT_DRAG_DROP_GUIDE.md) - Equipment drag-drop
- [ITEM_SYSTEM_SETUP.md](ITEM_SYSTEM_SETUP.md) - Item sistemi kurulum

---

## ✅ Checklist

### Unity Setup
- [ ] SalvageZone GameObject oluşturuldu
- [ ] Background Image eklendi (Raycast Target ON)
- [ ] Icon Image eklendi
- [ ] SalvageDropZone component eklendi
- [ ] Inspector referansları verildi
- [ ] (Opsiyonel) Confirmation panel oluşturuldu

### Test
- [ ] Valid item salvage test edildi
- [ ] Invalid item test edildi
- [ ] Visual feedback (highlight, animations) test edildi
- [ ] Inventory güncelleme test edildi
- [ ] (Opsiyonel) Confirmation dialog test edildi

---

## 🎉 Özet

Artık itemleri çöp kutusuna sürükleyip bırakarak salvage edebilirsiniz!

**Kullanım:**
1. Item'ı sürükle
2. Çöp kutusuna bırak
3. Materyalleri kazan!

**Özellikler:**
- ✅ Drag & drop
- ✅ Visual feedback
- ✅ Animasyonlar
- ✅ Validation
- ✅ Otomatik güncelleme

---

**Son Güncelleme:** 2025-11-10  
**Versiyon:** 1.0

