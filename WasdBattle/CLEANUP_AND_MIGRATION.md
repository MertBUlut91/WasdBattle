# 🧹 Cleanup and Migration Guide

Bu dosya, eski UI sisteminden yeni sisteme geçiş için temizlenmesi gereken dosyaları ve yapılması gereken değişiklikleri listeler.

---

## 🗑️ Kaldırılacak/Güncellenecek Dosyalar

### Opsiyonel Silme (Yedek Olarak Tutulabilir)

Bu dosyalar artık kullanılmıyor ama yedek olarak tutmak isteyebilirsiniz:

1. **Assets/Scripts/UI/CharacterSelectUI.cs**
   - **Sebep:** Artık `CharacterPanelUI.cs` kullanılıyor
   - **Aksiyon:** Sil veya `_OLD` suffix'i ekle

### Scene'de Kaldırılacak/Güncellenecek Elementler

**MainMenuScene** içinde:

1. **CharacterSelectPanel** (GameObject)
   - **Sebep:** Yeni `CharacterPanel` ile değiştirildi
   - **Aksiyon:** Sil veya deaktif et

2. **Currency Panel** içinde:
   - **GemRow** (GameObject)
   - **DiamondRow** (GameObject)
   - **Sebep:** Sadece Gold gösteriliyor
   - **Aksiyon:** Sil

3. **Bottom Buttons:**
   - **QuitButton** (GameObject)
   - **Sebep:** Artık kullanılmıyor
   - **Aksiyon:** Sil

4. **Eski InventoryPanel** yapısı
   - **Sebep:** Tamamen yeni tasarıma göre yeniden yapıldı
   - **Aksiyon:** Sil ve yeni `InventoryPanel`'i oluştur

---

## 🔄 Güncellenmesi Gerekenler

### MainMenuUI.cs

✅ **Tamamlandı** - Aşağıdaki değişiklikler yapıldı:
- `_essenceText` referansı kaldırıldı
- `_characterSelectButton` → `_characterButton` olarak değiştirildi
- `_shopButton` kaldırıldı, `_craftShopButton` eklendi
- `_quitButton` kaldırıldı
- `_characterDisplayController` referansı eklendi
- `_characterPanelUI` ve `_equipmentUI` referansları eklendi
- `OnPanelClosed()` metodu eklendi

### EquipmentUI.cs

✅ **Tamamlandı** - Tamamen yeniden yazıldı:
- Class-filtered item listesi
- Stat comparison sistemi
- Equipment slot yönetimi
- Hover events (stat preview)

### CharacterPanelUI.cs

✅ **Yeni Dosya** - Oluşturuldu:
- Character selection
- Skill kategorilendirme
- Skill detayları
- 3D character display entegrasyonu

### CharacterDisplayController.cs

✅ **Yeni Dosya** - Oluşturuldu:
- 3D karakter gösterimi
- Otomatik + manuel rotasyon
- Kamera pozisyon kontrolü
- RenderTexture yönetimi

---

## 📦 Yeni Dosyalar

### Scripts

1. ✅ **Assets/Scripts/UI/CharacterDisplayController.cs**
2. ✅ **Assets/Scripts/UI/CharacterPanelUI.cs**
3. ✅ **Assets/Scripts/UI/CharacterListItemUI.cs**
4. ✅ **Assets/Scripts/UI/ItemCardUI.cs**
5. ✅ **Assets/Scripts/UI/SkillCardUI.cs**

### Prefabs (Unity Editor'de oluşturulacak)

1. **Assets/Prefabs/UI/CharacterListItem.prefab**
2. **Assets/Prefabs/UI/ItemListCard.prefab**
3. **Assets/Prefabs/UI/SkillCard.prefab**

### Documentation

1. ✅ **UI_IMPLEMENTATION_GUIDE.md** - Detaylı Unity Editor kurulum rehberi
2. ✅ **CLEANUP_AND_MIGRATION.md** - Bu dosya

---

## ⚠️ Breaking Changes

### MainMenuUI

**Eski Sistem:**
```csharp
[SerializeField] private Button _characterSelectButton;
[SerializeField] private Button _shopButton;
[SerializeField] private Button _quitButton;
[SerializeField] private TextMeshProUGUI _essenceText;
```

**Yeni Sistem:**
```csharp
[SerializeField] private Button _characterButton;
[SerializeField] private Button _craftShopButton;
[SerializeField] private CharacterDisplayController _characterDisplayController;
// _essenceText kaldırıldı
// _quitButton kaldırıldı
```

### EquipmentUI

**Eski Sistem:**
- Basit item listesi
- Manuel equip/unequip
- Stat hesaplama yok

**Yeni Sistem:**
- Class-filtered item listesi
- Otomatik stat hesaplama
- Stat comparison (hover ile preview)
- Tab sistemi (All, Weapons, Armor, Consumables)

---

## 🎯 Migration Checklist

### Adım 1: Yedekleme
- [ ] MainMenuScene'i yedekle (Duplicate)
- [ ] Eski script'leri `_OLD` klasörüne taşı

### Adım 2: Scene Temizliği
- [ ] Eski CharacterSelectPanel'i sil
- [ ] Currency Panel'den Gem ve Diamond'ı sil
- [ ] Quit Button'u sil
- [ ] Eski InventoryPanel'i sil

### Adım 3: Yeni Sistemleri Kurma
- [ ] `UI_IMPLEMENTATION_GUIDE.md`'yi takip et
- [ ] CharacterDisplayRoot oluştur
- [ ] RenderTexture oluştur
- [ ] CharacterPanel oluştur
- [ ] InventoryPanel oluştur

### Adım 4: Prefab'ları Oluşturma
- [ ] CharacterListItem prefab
- [ ] ItemListCard prefab
- [ ] SkillCard prefab

### Adım 5: Referansları Bağlama
- [ ] MainMenuUI referansları
- [ ] CharacterPanelUI referansları
- [ ] EquipmentUI referansları

### Adım 6: Test
- [ ] 3D karakter gösterimi
- [ ] Character panel açılma/kapanma
- [ ] Inventory panel açılma/kapanma
- [ ] Stat comparison
- [ ] Cloud Save entegrasyonu

---

## 🐛 Known Issues & Solutions

### Issue 1: RenderTexture Siyah Görünüyor
**Sebep:** Camera Culling Mask yanlış ayarlanmış
**Çözüm:** Camera'nın Culling Mask'ini kontrol et, doğru layer'ı seç

### Issue 2: Karakter Prefab Yüklenmiyor
**Sebep:** CharacterData.characterPrefab null
**Çözüm:** ScriptableObject'te characterPrefab'ı ata

### Issue 3: Item Listesi Boş
**Sebep:** PlayerData.ownedItems boş veya class filtering çalışmıyor
**Çözüm:** 
- PlayerData'ya test itemleri ekle
- ItemData.requiredClass'ı kontrol et
- CharacterClass → ItemClass conversion'ı kontrol et

### Issue 4: Stat Comparison Çalışmıyor
**Sebep:** EquipmentSlotUI array'i boş veya yanlış boyutta
**Çözüm:** Inspector'da 9 elemanlı array oluştur ve her slot'u bağla

---

## 📊 Performance Notes

### Optimizasyon İpuçları

1. **RenderTexture Boyutu:**
   - Desktop: 1024x1024 veya 2048x2048
   - Mobile: 512x512 veya 1024x1024

2. **Character Instance:**
   - Karakter instance'ı cache'leniyor
   - Panel değişiminde yeniden instantiate edilmiyor
   - Sadece kamera pozisyonu değişiyor

3. **Item Filtering:**
   - Class filtering her item için yapılıyor
   - Çok sayıda item varsa (>100) cache düşünülebilir

4. **Stat Calculation:**
   - Her hover'da hesaplanıyor
   - Optimize edilebilir (cache equipped items stats)

---

## 🔮 Future Enhancements

Şimdilik dahil edilmeyenler (sonra eklenecek):

1. **Lighting System:**
   - Karakter için özel lighting
   - Dynamic shadows

2. **Equipment Visual:**
   - Equipped item'ları karakterde göster
   - Item prefab'larını bone'lara attach et

3. **Animations:**
   - Panel açılma/kapanma animasyonları
   - Character idle animations
   - Skill preview animations

4. **Craft & Shop Panels:**
   - Benzer yapıda yeni paneller
   - Crafting sistemi UI
   - Shop sistemi UI

---

**Migration tamamlandığında bu dosyayı arşivleyin!** ✅

