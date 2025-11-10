# 🛠️ Editor Scripts - WasdBattle

Bu klasör, Unity Editor için özel araçlar içerir.

## 📋 Mevcut Editörler

### 1. GameDataEditor.cs
**Amaç:** Karakter ve Item oluşturma, düzenleme ve yönetme

**Özellikler:**
- ✅ Karakter oluşturma ve düzenleme
- ✅ Item oluşturma ve düzenleme
- ✅ Arama ve filtreleme
- ✅ Kopyalama (duplicate)
- ✅ Silme işlemleri
- ✅ Otomatik ID oluşturma
- ✅ Görsel önizleme

**Nasıl Açılır:**
```
Window > WasdBattle > Game Data Editor
```

**Dokümantasyon:**
[GAME_DATA_EDITOR_GUIDE.md](../../../GAME_DATA_EDITOR_GUIDE.md)

---

### 2. SkillCreator.cs
**Amaç:** Temel skill'leri otomatik oluşturma

**Özellikler:**
- ✅ Default skill'ler oluşturur (Light Strike, Heavy Blow, Stamina Drain)
- ✅ Combo data oluşturur
- ✅ Otomatik klasör yapısı

**Nasıl Kullanılır:**
```
WasdBattle > Create Default Skills
```

---

## 🎯 Kullanım Önerileri

### Yeni Karakter Oluşturma
1. `GameDataEditor` açın
2. "Oluştur" sekmesine gidin
3. Formu doldurun
4. "Karakteri Oluştur" tıklayın

### Yeni Item Oluşturma
1. `GameDataEditor` açın
2. "Oluştur" sekmesine gidin
3. "Yeni Item" seçin
4. Formu doldurun
5. "Item'i Oluştur" tıklayın

### Mevcut Veriyi Düzenleme
1. `GameDataEditor` açın
2. "Karakterler" veya "Itemler" sekmesine gidin
3. Düzenlemek istediğiniz veriyi seçin
4. Değişiklikleri yapın
5. "Kaydet" tıklayın

---

## 📁 Oluşturulan Dosyalar

### Karakterler
```
Assets/ScriptableObjects/Characters/
├── FireMage.asset
├── IceWarrior.asset
└── ShadowNinja.asset
```

### Itemler
```
Assets/ScriptableObjects/Items/
├── LegendarySword.asset
├── MysticRobe.asset
└── DragonHelmet.asset
```

### Skill'ler (SkillCreator)
```
Assets/_Project/ScriptableObjects/Skills/
├── LightStrike.asset
├── HeavyBlow.asset
├── StaminaDrain.asset
└── Combos/
    ├── FastCombo.asset
    ├── HeavyCombo.asset
    └── SpecialCombo.asset
```

---

## 💡 İpuçları

### ID Oluşturma
- Karakter: `char_fire_mage`
- Item: `item_legendary_sword`
- Skill: `skill_light_strike`

### Organizasyon
- Karakterleri class'larına göre gruplandırın
- Itemleri slot ve rarity'ye göre organize edin
- Açıklayıcı isimler kullanın

### Performans
- Büyük değişikliklerden sonra "Yenile" butonunu kullanın
- Değişiklikleri kaydetmeyi unutmayın
- Icon'ları optimize edin (max 512x512)

---

## 🐛 Sorun Giderme

### "Asset kaydedilemedi"
**Çözüm:** `Assets/ScriptableObjects/` klasörünün var olduğundan emin olun

### "ID zaten kullanılıyor"
**Çözüm:** Benzersiz bir ID kullanın veya "ID'yi Otomatik Oluştur" butonunu kullanın

### "Değişiklikler kaydedilmiyor"
**Çözüm:** "Kaydet" butonuna tıklamayı unutmayın

---

## 🔗 İlgili Dökümanlar

- [GAME_DATA_EDITOR_GUIDE.md](../../../GAME_DATA_EDITOR_GUIDE.md) - Detaylı kullanım kılavuzu
- [CHARACTER_UNLOCK_GUIDE.md](../../../CHARACTER_UNLOCK_GUIDE.md) - Karakter sistemi
- [ITEM_SYSTEM_SETUP.md](../../../ITEM_SYSTEM_SETUP.md) - Item sistemi
- [EQUIPMENT_SYSTEM_GUIDE.md](../../../EQUIPMENT_SYSTEM_GUIDE.md) - Ekipman sistemi

---

**Keyifli geliştirmeler! 🎮✨**

