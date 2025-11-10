# Equipment Drag & Drop + Double-Click System

## 📋 Genel Bakış

Bu rehber, envanter sistemindeki ekipmanları **çift tıklama** ve **sürükle-bırak** yöntemleriyle karaktere giydirme sistemini açıklar.

### Özellikler

✅ **Çift Tıklama (Double-Click)**: Item'a çift tıklayarak otomatik olarak uygun slot'a giydirme  
✅ **Sürükle-Bırak (Drag & Drop)**: Item'ı sürükleyip istediğiniz slot'a bırakma  
✅ **Görsel Geri Bildirim**: Sürükleme sırasında slot highlight'ı ve item transparanlığı  
✅ **Slot Validasyonu**: Sadece uygun slot'lara item bırakılabilir (örn: yüzük sadece ring slot'larına)  
✅ **Ring Özel Mantığı**: İki ring slot'u için akıllı yerleştirme

---

## 🎯 Kullanım

### Çift Tıklama ile Giydirme

1. Envanter listesinde bir item'a **çift tıklayın**
2. Item otomatik olarak uygun slot'a giydirilir
3. Ring'ler için: Boş slot varsa oraya, yoksa ilk slot'a giydirilir

**Örnek:**
- Helmet'e çift tıkla → Helmet slot'una giydirilir
- Ring'e çift tıkla → Ring1 boşsa oraya, değilse Ring2'ye giydirilir

### Sürükle-Bırak ile Giydirme

1. Envanter listesinde bir item'ı **tıklayın ve basılı tutun**
2. Mouse'u hareket ettirerek item'ı **sürükleyin**
3. Uygun slot'un üzerine geldiğinizde slot **sarı renkte highlight** olur
4. Mouse'u bırakarak item'ı **o slot'a giydirin**

**Örnek:**
- Ring'i sürükle → Ring1 veya Ring2 slot'una bırak
- Helmet'i sürükle → Helmet slot'una bırak
- Weapon'ı sürükle → Weapon slot'una bırak

---

## 🔧 Teknik Detaylar

### Yeni/Güncellenen Scriptler

#### 1. `ItemCardUI.cs` (Güncellendi)

**Yeni Özellikler:**
- `IPointerClickHandler`: Double-click detection
- `IBeginDragHandler`, `IDragHandler`, `IEndDragHandler`: Drag-and-drop
- `SetItemData()`: ItemData referansını saklar (drag için gerekli)
- `GetItemData()`: Drop zone'un item bilgisine erişmesi için

**Double-Click Mantığı:**
```csharp
private float _lastClickTime;
private const float DOUBLE_CLICK_TIME = 0.3f;

public void OnPointerClick(PointerEventData eventData)
{
    float timeSinceLastClick = Time.time - _lastClickTime;
    
    if (timeSinceLastClick <= DOUBLE_CLICK_TIME)
    {
        // Double-click detected!
        _onDoubleClick?.Invoke();
        _lastClickTime = 0f;
    }
    else
    {
        // Single click - sadece zamanı kaydet
        _lastClickTime = Time.time;
    }
}
```

**Drag-and-Drop Mantığı:**
```csharp
public void OnBeginDrag(PointerEventData eventData)
{
    // Orijinal pozisyon ve parent'ı kaydet
    _originalPosition = _rectTransform.anchoredPosition;
    _originalParent = transform.parent;
    
    // Canvas'ın en üstüne taşı
    transform.SetParent(_canvas.transform, true);
    
    // Yarı saydam yap
    _canvasGroup.alpha = 0.6f;
    _canvasGroup.blocksRaycasts = false;
}

public void OnDrag(PointerEventData eventData)
{
    // Mouse pozisyonunu takip et
    _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
}

public void OnEndDrag(PointerEventData eventData)
{
    // Raycast ile drop target'ı bul
    var results = new List<RaycastResult>();
    EventSystem.current.RaycastAll(eventData, results);
    
    foreach (var result in results)
    {
        var dropZone = result.gameObject.GetComponent<EquipmentSlotDropZone>();
        if (dropZone != null)
        {
            dropZone.OnItemDropped(_itemData);
            break;
        }
    }
    
    // Orijinal duruma döndür
    _canvasGroup.alpha = 1f;
    transform.SetParent(_originalParent, true);
    _rectTransform.anchoredPosition = _originalPosition;
}
```

#### 2. `EquipmentSlotDropZone.cs` (YENİ)

Equipment slot'larına eklenen drop zone component.

**Özellikler:**
- `IDropHandler`: Drop event'ini yakalar
- `IPointerEnterHandler`, `IPointerExitHandler`: Hover highlight
- Slot type validation (sadece uygun itemler kabul edilir)
- Visual feedback (highlight rengi)

**Kullanım:**
```csharp
public class EquipmentSlotDropZone : MonoBehaviour
{
    [SerializeField] private EquipmentSlot _slotType;
    [SerializeField] private Image _highlightImage;
    [SerializeField] private Color _highlightColor = new Color(1f, 1f, 0f, 0.3f);
    
    public void OnItemDropped(ItemData itemData)
    {
        // Slot type kontrolü
        if (!IsValidSlot(itemData))
            return;
        
        // EquipmentUI'a equip isteği gönder
        _equipmentUI.EquipItemFromDrag(itemData, _slotType);
    }
    
    private bool IsValidSlot(ItemData itemData)
    {
        // Ring slotları için özel kontrol
        if (_slotType == EquipmentSlot.Ring1 || _slotType == EquipmentSlot.Ring2)
            return itemData.slot == EquipmentSlot.Ring1 || itemData.slot == EquipmentSlot.Ring2;
        
        return itemData.slot == _slotType;
    }
}
```

#### 3. `EquipmentUI.cs` (Güncellendi)

**Yeni Methodlar:**
- `OnItemDoubleClicked()`: Double-click callback
- `EquipItemFromDrag()`: Drag-and-drop callback (public)
- `EquipItem()`: Ortak equip mantığı (private)

**Değişiklikler:**
- `OnItemClicked()` → `OnItemDoubleClicked()` (renamed)
- `CreateItemCard()` içinde `cardUI.SetItemData(itemData)` eklendi
- Drag-and-drop için `EquipItemFromDrag()` public method eklendi

---

## 🎨 Unity Setup

### 1. ItemCardUI Prefab Setup

ItemCardPrefab üzerinde:

1. **CanvasGroup Component Ekle** (otomatik eklenir ama manuel de eklenebilir)
   - Alpha: 1
   - Interactable: true
   - Block Raycasts: true

2. **ItemCardUI Script**
   - Canvas: (otomatik bulunur, ama referans verilebilir)
   - Drag Alpha: 0.6

3. **Button Component'i Kaldır veya Disable Et**
   - ItemCardUI artık IPointerClickHandler kullanıyor
   - Button'a gerek yok (ama varsa interactable = false yapılır)

### 2. Equipment Slot Setup

Her equipment slot GameObject'ine:

1. **EquipmentSlotDropZone Component Ekle**
   ```
   Add Component → EquipmentSlotDropZone
   ```

2. **Inspector'da Ayarla:**
   - Slot Type: (Helmet, Chest, Weapon, Ring1, Ring2, vb.)
   - Highlight Image: Slot'un background image'ı
   - Highlight Color: Sarı (1, 1, 0, 0.3)

3. **Image Component** (highlight için)
   - Slot'un background image'ı olmalı
   - Raycast Target: ✅ (AÇIK olmalı)

**Örnek Hierarchy:**
```
HelmetSlot (GameObject)
├── Image (Background) → Highlight Image olarak kullanılır
├── Image (ItemIcon)
├── TextMeshPro (SlotName)
├── Button (UnequipButton)
└── EquipmentSlotDropZone (Component) ← YENİ
    - Slot Type: Helmet
    - Highlight Image: Background Image
    - Highlight Color: (1, 1, 0, 0.3)
```

### 3. Canvas Setup

Equipment Panel'in Canvas'ı:
- Render Mode: Screen Space - Overlay (veya Camera)
- Graphic Raycaster: ✅ (olmalı)

---

## 🧪 Test Senaryoları

### Test 1: Double-Click Equip
1. Envanter'de bir helmet'e çift tıkla
2. ✅ Helmet slot'una giydirilmeli
3. ✅ Envanter'den kaybolmalı (veya count azalmalı)

### Test 2: Drag-and-Drop Equip
1. Envanter'den bir ring'i sürükle
2. ✅ Ring1 slot'u üzerine gelince sarı highlight olmalı
3. ✅ Bırakınca ring giydirilmeli
4. ✅ Envanter'den kaybolmalı

### Test 3: Invalid Drop
1. Helmet'i sürükle
2. ✅ Ring slot'u üzerine gelince highlight OLMAMALI
3. ✅ Ring slot'una bırakınca giydirilMEMELI
4. ✅ Item orijinal pozisyonuna dönmeli

### Test 4: Ring Double Equip
1. Aynı ring'den 2 tane var
2. İlkine çift tıkla → Ring1'e giydirilmeli
3. İkincisine çift tıkla → Ring2'ye giydirilmeli
4. ✅ Her iki slot da dolu olmalı

### Test 5: Drag Visual Feedback
1. Item'ı sürükle
2. ✅ Item yarı saydam olmalı (alpha = 0.6)
3. ✅ Mouse'u takip etmeli
4. ✅ Bırakınca normal alpha'ya dönmeli

---

## 🐛 Troubleshooting

### Problem: Double-click çalışmıyor

**Çözüm:**
- ItemCardUI'da Button component'i varsa `interactable = false` yapın
- Veya Button component'ini tamamen kaldırın
- ItemCardUI script'inin `IPointerClickHandler` implement ettiğinden emin olun

### Problem: Drag başlamıyor

**Çözüm:**
- ItemCardUI'da `CanvasGroup` component'i olduğundan emin olun
- Canvas referansı doğru mu kontrol edin
- Raycast Target'ların açık olduğundan emin olun

### Problem: Drop çalışmıyor

**Çözüm:**
- Equipment slot'larında `EquipmentSlotDropZone` component'i var mı?
- Highlight Image'ın Raycast Target'ı açık mı?
- Canvas'ta Graphic Raycaster var mı?

### Problem: Slot highlight olmuyor

**Çözüm:**
- `EquipmentSlotDropZone` Inspector'da Highlight Image referansı verilmiş mi?
- Highlight Color alpha değeri 0'dan büyük mü? (örn: 0.3)
- Item'ın slot type'ı ile drop zone'un slot type'ı uyumlu mu?

### Problem: Item yanlış slot'a giydirildi

**Çözüm:**
- `EquipmentSlotDropZone`'da Slot Type doğru ayarlanmış mı?
- Ring slotları için Ring1 veya Ring2 olarak ayarlanmış mı?
- `IsValidSlot()` method'u doğru çalışıyor mu?

---

## 📝 Kod Örnekleri

### Örnek 1: Custom Highlight Effect

```csharp
// EquipmentSlotDropZone.cs içinde
private void ShowHighlight()
{
    if (_highlightImage != null)
    {
        // Pulse effect
        _highlightImage.color = _highlightColor;
        LeanTween.alpha(_highlightImage.rectTransform, 0.5f, 0.3f).setLoopPingPong();
    }
}

private void ResetHighlight()
{
    if (_highlightImage != null)
    {
        LeanTween.cancel(_highlightImage.gameObject);
        _highlightImage.color = _originalColor;
    }
}
```

### Örnek 2: Sound Effects

```csharp
// ItemCardUI.cs içinde
public void OnBeginDrag(PointerEventData eventData)
{
    // ... existing code ...
    
    // Sound effect
    AudioManager.Instance?.PlaySFX("ItemPickup");
}

public void OnEndDrag(PointerEventData eventData)
{
    // ... existing code ...
    
    if (droppedOnSlot)
    {
        AudioManager.Instance?.PlaySFX("ItemEquip");
    }
    else
    {
        AudioManager.Instance?.PlaySFX("ItemDrop");
    }
}
```

### Örnek 3: Particle Effects

```csharp
// EquipmentSlotDropZone.cs içinde
public void OnItemDropped(ItemData itemData)
{
    if (!IsValidSlot(itemData))
        return;
    
    // Particle effect
    if (_equipParticles != null)
    {
        _equipParticles.Play();
    }
    
    _equipmentUI.EquipItemFromDrag(itemData, _slotType);
}
```

---

## 🎯 Gelecek Geliştirmeler

### Öneri 1: Swap Equipment
Equipped item'ı sürükleyip başka bir item ile yer değiştirme:
```csharp
// EquipmentSlotDropZone.cs içinde
public void OnItemDropped(ItemData itemData)
{
    // Eğer slot doluysa, swap yap
    string currentItemId = _currentLoadout.GetEquippedItem(_slotType);
    if (!string.IsNullOrEmpty(currentItemId))
    {
        _equipmentUI.SwapItems(itemData, _slotType);
    }
    else
    {
        _equipmentUI.EquipItemFromDrag(itemData, _slotType);
    }
}
```

### Öneri 2: Tooltip on Hover
Drag sırasında item tooltip'i gösterme:
```csharp
// ItemCardUI.cs içinde
public void OnDrag(PointerEventData eventData)
{
    _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
    
    // Tooltip güncelle
    TooltipManager.Instance?.ShowTooltip(_itemData, eventData.position);
}
```

### Öneri 3: Multi-Item Drag
Aynı item'dan birden fazla sürükleme (stackable items için):
```csharp
// ItemCardUI.cs içinde
[SerializeField] private int _dragCount = 1;

public void OnBeginDrag(PointerEventData eventData)
{
    // Shift basılıysa tüm stack'i sürükle
    if (Input.GetKey(KeyCode.LeftShift))
    {
        _dragCount = _itemData.count;
    }
    else
    {
        _dragCount = 1;
    }
}
```

---

## 📚 İlgili Dökümanlar

- [ITEM_SYSTEM_SETUP.md](ITEM_SYSTEM_SETUP.md) - Item sistemi genel bakış
- [EQUIPMENT_SYSTEM_GUIDE.md](EQUIPMENT_SYSTEM_GUIDE.md) - Equipment sistemi detayları
- [UI_IMPLEMENTATION_GUIDE.md](UI_IMPLEMENTATION_GUIDE.md) - UI implementasyon rehberi

---

## ✅ Checklist

### Development
- [x] ItemCardUI double-click implementasyonu
- [x] ItemCardUI drag-and-drop implementasyonu
- [x] EquipmentSlotDropZone component oluşturuldu
- [x] EquipmentUI drag-and-drop entegrasyonu
- [x] Slot validation mantığı
- [x] Visual feedback (highlight, transparency)

### Unity Setup
- [ ] ItemCardPrefab'a CanvasGroup eklendi
- [ ] Her equipment slot'a EquipmentSlotDropZone eklendi
- [ ] Slot Type'lar doğru ayarlandı
- [ ] Highlight Image referansları verildi
- [ ] Canvas Graphic Raycaster kontrol edildi

### Testing
- [ ] Double-click equip test edildi
- [ ] Drag-and-drop equip test edildi
- [ ] Invalid drop test edildi
- [ ] Ring double equip test edildi
- [ ] Visual feedback test edildi

---

**Son Güncelleme:** 2025-11-10  
**Versiyon:** 1.0

