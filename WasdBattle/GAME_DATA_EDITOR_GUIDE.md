# 🎮 Game Data Editor - Kullanım Kılavuzu

## 📋 Genel Bakış

**Game Data Editor**, WasdBattle oyununuz için karakter ve item oluşturmanızı, düzenlemenizi ve yönetmenizi sağlayan kapsamlı bir Unity Editor aracıdır.

## 🚀 Editörü Açma

Unity Editor'de:
```
Window > WasdBattle > Game Data Editor
```

veya

```
Üst menüden: WasdBattle > Game Data Editor
```

## 📑 Ana Sekmeler

Editör 3 ana sekmeden oluşur:

### 1. 📋 Karakterler Sekmesi
Mevcut karakterleri görüntüleme ve düzenleme

### 2. 📦 Itemler Sekmesi
Mevcut itemleri görüntüleme ve düzenleme

### 3. ➕ Oluştur Sekmesi
Yeni karakter ve item oluşturma

---

## 🎭 Karakter Yönetimi

### Karakter Listesi (Sol Panel)

#### Özellikler:
- **🔄 Yenile**: Tüm karakterleri yeniden yükler
- **➕ Yeni Karakter**: Oluştur sekmesine yönlendirir
- **🔍 Arama**: Karakter ismi veya ID'sine göre arama
- **Class Filter**: Belirli bir class'a göre filtreleme (Mage, Warrior, Ninja, vb.)

#### Karakter Kartı Bilgileri:
- Karakter ikonu (varsa)
- Karakter ismi
- Class bilgisi
- Karakter ID'si
- Seç butonu

### Karakter Detayları (Sağ Panel)

Bir karakter seçtiğinizde aşağıdaki bilgileri düzenleyebilirsiniz:

#### 📝 Temel Bilgiler
- **İsim**: Karakterin görünen adı
- **ID**: Benzersiz karakter kimliği (örn: `char_fire_mage`)
- **Class**: Karakter sınıfı (Mage, Warrior, Ninja, Assassin, Paladin, Ranger)
- **Açıklama**: Karakter hakkında detaylı bilgi

#### 📊 Temel İstatistikler
- **Health**: Can puanı (50-500)
- **Stamina**: Enerji puanı (50-300)
- **Stamina Regen**: Saniyede enerji yenileme hızı (1-50)
- **Defense**: Savunma değeri (0-1 arası hasar azaltma)

#### 🎨 Görsel
- **Icon**: Karakter ikonu (Sprite)
- **Prefab**: 3D karakter modeli (GameObject)
- **Renk**: Karakterin tema rengi

#### 🔓 Unlock Ayarları
- **Başlangıç Karakteri**: Oyun başında açık mı?
- **Unlock Gerekiyor**: Kilit açma gerekiyor mu?
- **Gerekli Level**: Minimum oyuncu seviyesi

#### 🎒 Başlangıç Ekipmanı
- Karakterin başlangıçta sahip olduğu itemler

#### ⚔️ Başlangıç Skillleri
- Karakterin başlangıçta sahip olduğu yetenekler

### Karakter İşlemleri

#### 💾 Kaydet
Yapılan değişiklikleri kaydeder

#### 📋 Kopyala
Seçili karakterin bir kopyasını oluşturur
- Otomatik olarak "_copy" eklenir
- Yeni bir asset olarak kaydedilir

#### 🗑️ Sil
Karakteri kalıcı olarak siler
- Onay penceresi gösterilir
- Geri alınamaz!

---

## 🛡️ Item Yönetimi

### Item Listesi (Sol Panel)

#### Özellikler:
- **🔄 Yenile**: Tüm itemleri yeniden yükler
- **➕ Yeni Item**: Oluştur sekmesine yönlendirir
- **🔍 Arama**: Item ismi veya ID'sine göre arama
- **Class Filter**: Belirli bir class için itemleri filtrele
- **Slot Filter**: Belirli bir ekipman slotuna göre filtrele

#### Item Kartı Bilgileri:
- Item ikonu (varsa)
- Item ismi
- Rarity (nadirlik) seviyesi (emoji ile)
- Slot ve Class bilgisi
- Seç butonu

### Item Detayları (Sağ Panel)

Bir item seçtiğinizde aşağıdaki bilgileri düzenleyebilirsiniz:

#### 📝 Temel Bilgiler
- **İsim**: Item'in görünen adı
- **ID**: Benzersiz item kimliği (örn: `item_fire_sword`)
- **Slot**: Ekipman slotu
  - Helmet (Kask)
  - Chest (Gövdelik)
  - Gloves (Ellik)
  - Legs (Bacaklık)
  - Weapon (Silah)
  - Ring1 (Yüzük 1)
  - Ring2 (Yüzük 2)
  - Necklace (Kolye)
  - Bracelet (Bileklik)
- **Class**: Hangi sınıf giyebilir (All, Mage, Warrior, Ninja)
- **Rarity**: Nadirlik seviyesi
  - ⚪ Common (Gri)
  - 🟢 Uncommon (Yeşil)
  - 🔵 Rare (Mavi)
  - 🟣 Epic (Mor)
  - 🟠 Legendary (Turuncu)
- **Level**: Minimum seviye gereksinimi
- **Açıklama**: Item hakkında detaylı bilgi

#### 📊 İstatistikler
- **Health Bonus**: Can artışı (0-200)
- **Stamina Bonus**: Enerji artışı (0-100)
- **Damage Bonus**: Hasar artışı (0-100)
- **Armor Bonus**: Zırh artışı (0-100)
- **Magic Res Bonus**: Büyü direnci artışı (0-100)
- **Crit Chance**: Kritik vuruş şansı (0-1)
- **Crit Damage**: Kritik vuruş hasarı (0-2)

**Toplam Stat Bonusu** otomatik olarak hesaplanır ve gösterilir.

#### 🎨 Görsel
- **Icon**: Item ikonu (Sprite)
- **Prefab**: 3D item modeli (GameObject)

#### 🔨 Crafting
- **Craft Edilebilir**: Item üretilebilir mi?
- **Crafting Materials**: Gerekli malzemeler (array)
  - Material Type (Metal, EnergyCrystal, Rune, vb.)
  - Amount (Miktar)

#### 🛒 Mağaza
- **Satın Alınabilir**: Mağazadan alınabilir mi?
- **Fiyat**: Altın cinsinden fiyat

#### ♻️ Salvage (Eritme)
- **Eritilebilir**: Item eritilebilir mi?
- **Geri Dönüş Oranı**: Malzemelerin yüzde kaçı geri verilir (0-1)
- **Salvage Ödülleri**: Otomatik hesaplanan geri dönüş malzemeleri

### Item İşlemleri

#### 💾 Kaydet
Yapılan değişiklikleri kaydeder

#### 📋 Kopyala
Seçili item'in bir kopyasını oluşturur
- Otomatik olarak "_copy" eklenir
- Yeni bir asset olarak kaydedilir

#### 🗑️ Sil
Item'i kalıcı olarak siler
- Onay penceresi gösterilir
- Geri alınamaz!

---

## ➕ Yeni Veri Oluşturma

### Yeni Karakter Oluşturma

1. **Oluştur** sekmesine gidin
2. **Yeni Karakter** alt sekmesini seçin
3. Formu doldurun:
   - İsim girin
   - "ID'yi Otomatik Oluştur" butonuna tıklayın (veya manuel girin)
   - Class seçin
   - Açıklama yazın
   - İstatistikleri ayarlayın
   - Görsel elementleri ekleyin
   - Unlock ayarlarını yapın
4. **✨ Karakteri Oluştur** butonuna tıklayın
5. Karakter `Assets/ScriptableObjects/Characters/` klasörüne kaydedilir
6. Otomatik olarak Karakterler sekmesine yönlendirilir

#### 🔄 Formu Temizle
Tüm alanları sıfırlar ve yeni bir karakter için hazırlar

### Yeni Item Oluşturma

1. **Oluştur** sekmesine gidin
2. **Yeni Item** alt sekmesini seçin
3. Formu doldurun:
   - İsim girin
   - "ID'yi Otomatik Oluştur" butonuna tıklayın (veya manuel girin)
   - Slot, Class ve Rarity seçin
   - Açıklama yazın
   - İstatistikleri ayarlayın
   - Görsel elementleri ekleyin
   - Mağaza ve Crafting ayarlarını yapın
4. **✨ Item'i Oluştur** butonuna tıklayın
5. Item `Assets/ScriptableObjects/Items/` klasörüne kaydedilir
6. Otomatik olarak Itemler sekmesine yönlendirilir

#### 🔄 Formu Temizle
Tüm alanları sıfırlar ve yeni bir item için hazırlar

---

## 💡 İpuçları ve En İyi Uygulamalar

### ID Oluşturma
- **Karakter ID'leri**: `char_` öneki kullanın (örn: `char_fire_mage`)
- **Item ID'leri**: `item_` öneki kullanın (örn: `item_legendary_sword`)
- Boşluk yerine alt çizgi kullanın
- Küçük harf kullanın
- Benzersiz olduğundan emin olun

### Karakter Tasarımı
- **Mage**: Yüksek hasar, düşük stamina
- **Warrior**: Yüksek savunma, orta hasar
- **Ninja**: Yüksek hız, düşük savunma
- Her class için dengeli stat dağılımı yapın

### Item Tasarımı
- **Rarity'ye göre stat dağılımı**:
  - Common: 10-30 toplam stat
  - Uncommon: 30-60 toplam stat
  - Rare: 60-100 toplam stat
  - Epic: 100-150 toplam stat
  - Legendary: 150+ toplam stat
- Slot'a uygun stat verin (örn: Weapon'a damage, Helmet'e armor)
- Class'a özgü itemler oluşturun

### Crafting Malzemeleri
- Rarity'ye göre malzeme miktarı ayarlayın
- Salvage oranını %30-70 arasında tutun
- Legendary itemler için nadir malzemeler kullanın

### Organizasyon
- Karakterleri class'larına göre gruplandırın
- Itemleri slot ve rarity'ye göre organize edin
- Açıklayıcı isimler kullanın
- İkonları mutlaka ekleyin (görsel referans için)

---

## 🔍 Arama ve Filtreleme

### Karakter Arama
- İsme göre arama: "Fire", "Mage", vb.
- ID'ye göre arama: "char_", "warrior", vb.
- Class filtresi: Sadece belirli bir class'ı göster

### Item Arama
- İsme göre arama: "Sword", "Legendary", vb.
- ID'ye göre arama: "item_", "weapon", vb.
- Class filtresi: Belirli bir class için itemler
- Slot filtresi: Belirli bir slot için itemler

### Filtreleri Birleştirme
- Arama + Class filtresi
- Arama + Slot filtresi
- Arama + Class + Slot filtresi

---

## 🐛 Sorun Giderme

### "Karakter/Item bulunamadı"
- **Çözüm**: 🔄 Yenile butonuna tıklayın

### "ID zaten kullanılıyor"
- **Çözüm**: Benzersiz bir ID girin veya otomatik oluştur

### "Asset kaydedilemedi"
- **Çözüm**: `Assets/ScriptableObjects/` klasörünün var olduğundan emin olun

### "Icon görünmüyor"
- **Çözüm**: Sprite asset'i doğru şekilde atandığından emin olun

### "Değişiklikler kaydedilmiyor"
- **Çözüm**: 💾 Kaydet butonuna tıklamayı unutmayın

---

## 📁 Dosya Yapısı

```
Assets/
├── ScriptableObjects/
│   ├── Characters/
│   │   ├── FireMage.asset
│   │   ├── IceWarrior.asset
│   │   └── ShadowNinja.asset
│   └── Items/
│       ├── LegendarySword.asset
│       ├── MysticRobe.asset
│       └── DragonHelmet.asset
└── Scripts/
    ├── Data/
    │   ├── CharacterData.cs
    │   └── ItemData.cs
    └── Editor/
        └── GameDataEditor.cs
```

---

## 🎯 Hızlı Başlangıç

### 5 Dakikada İlk Karakterinizi Oluşturun

1. `WasdBattle > Game Data Editor` menüsünden editörü açın
2. **Oluştur** sekmesine gidin
3. **Yeni Karakter** seçin
4. İsim: "Ateş Büyücüsü"
5. "ID'yi Otomatik Oluştur" tıklayın
6. Class: Mage seçin
7. Health: 120, Stamina: 80 yapın
8. **✨ Karakteri Oluştur** tıklayın
9. ✅ İlk karakteriniz hazır!

### 5 Dakikada İlk Item'inizi Oluşturun

1. **Oluştur** sekmesinde **Yeni Item** seçin
2. İsim: "Ateş Kılıcı"
3. "ID'yi Otomatik Oluştur" tıklayın
4. Slot: Weapon seçin
5. Rarity: Rare seçin
6. Damage Bonus: 50 yapın
7. **✨ Item'i Oluştur** tıklayın
8. ✅ İlk item'iniz hazır!

---

## 🔗 İlgili Dökümanlar

- [CHARACTER_UNLOCK_GUIDE.md](CHARACTER_UNLOCK_GUIDE.md) - Karakter unlock sistemi
- [ITEM_SYSTEM_SETUP.md](ITEM_SYSTEM_SETUP.md) - Item sistemi detayları
- [EQUIPMENT_SYSTEM_GUIDE.md](EQUIPMENT_SYSTEM_GUIDE.md) - Ekipman sistemi
- [SALVAGE_SYSTEM_GUIDE.md](SALVAGE_SYSTEM_GUIDE.md) - Salvage sistemi

---

## 📞 Destek

Herhangi bir sorun yaşarsanız:
1. Unity Console'u kontrol edin (hata mesajları için)
2. Asset'lerin doğru klasörlerde olduğundan emin olun
3. Editörü kapatıp yeniden açmayı deneyin
4. 🔄 Yenile butonunu kullanın

---

## ✨ Özellikler Özeti

✅ Karakter oluşturma, düzenleme, silme
✅ Item oluşturma, düzenleme, silme
✅ Gelişmiş arama ve filtreleme
✅ Kopyalama (duplicate) özelliği
✅ Otomatik ID oluşturma
✅ Görsel önizleme (icon)
✅ Stat hesaplayıcılar
✅ Salvage ödül önizlemesi
✅ Kullanıcı dostu arayüz
✅ Onay diyalogları (silme işlemleri için)
✅ Otomatik klasör oluşturma
✅ Scroll desteği (uzun listeler için)

---

**Keyifli geliştirmeler! 🎮✨**

