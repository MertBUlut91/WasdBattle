# 🎯 Game Data Editor - Hızlı Referans

## 🚀 Hızlı Başlangıç

### Editörü Açma
```
Window > WasdBattle > Game Data Editor
```

veya

```
Üst menü > WasdBattle > Game Data Editor
```

---

## ⚡ Hızlı İşlemler

### 5 Dakikada Karakter Oluştur
1. Editörü aç
2. **Oluştur** sekmesi → **Yeni Karakter**
3. İsim gir → "ID'yi Otomatik Oluştur"
4. Class seçin (Mage/Warrior/Ninja)
5. Statları ayarla
6. **✨ Karakteri Oluştur**

### 5 Dakikada Item Oluştur
1. Editörü aç
2. **Oluştur** sekmesi → **Yeni Item**
3. İsim gir → "ID'yi Otomatik Oluştur"
4. Slot ve Rarity seçin
5. Statları ayarla
6. **✨ Item'i Oluştur**

### Mevcut Veriyi Düzenle
1. **Karakterler** veya **Itemler** sekmesi
2. Listeden seç
3. Değiştir
4. **💾 Kaydet**

### Veri Kopyala
1. Veriyi seç
2. **📋 Kopyala**
3. Yeni verinin ID ve ismini değiştir
4. **💾 Kaydet**

---

## 🎭 Karakter Özellikleri

### Temel Bilgiler
| Alan | Açıklama | Örnek |
|------|----------|-------|
| İsim | Görünen ad | "Ateş Büyücüsü" |
| ID | Benzersiz kimlik | `char_fire_mage` |
| Class | Karakter sınıfı | Mage, Warrior, Ninja |
| Açıklama | Detaylı bilgi | "Güçlü ateş büyüleri..." |

### İstatistikler
| Stat | Aralık | Önerilen |
|------|--------|----------|
| Health | 50-500 | Mage: 100, Warrior: 150, Ninja: 120 |
| Stamina | 50-300 | Mage: 80, Warrior: 100, Ninja: 120 |
| Stamina Regen | 1-50 | 10-15 |
| Defense | 0-1 | Mage: 0.1, Warrior: 0.3, Ninja: 0.15 |

### Class Özellikleri
| Class | Health | Stamina | Defense | Özellik |
|-------|--------|---------|---------|---------|
| **Mage** | Düşük | Düşük | Düşük | Yüksek hasar |
| **Warrior** | Yüksek | Orta | Yüksek | Yüksek savunma |
| **Ninja** | Orta | Yüksek | Düşük | Yüksek hız |
| **Assassin** | Düşük | Orta | Düşük | Kritik vuruş |
| **Paladin** | Yüksek | Orta | Yüksek | Heal |
| **Ranger** | Orta | Orta | Orta | Uzak saldırı |

---

## 🛡️ Item Özellikleri

### Temel Bilgiler
| Alan | Açıklama | Örnek |
|------|----------|-------|
| İsim | Görünen ad | "Ateş Kılıcı" |
| ID | Benzersiz kimlik | `item_fire_sword` |
| Slot | Ekipman yeri | Weapon, Helmet, vb. |
| Class | Kim giyebilir | All, Mage, Warrior, Ninja |
| Rarity | Nadirlik | Common → Legendary |

### Equipment Slot'ları
| Slot | Türkçe | Önerilen Stat |
|------|--------|---------------|
| **Helmet** | Kask | Armor, Magic Res |
| **Chest** | Gövdelik | Health, Armor |
| **Gloves** | Ellik | Damage, Crit Chance |
| **Legs** | Bacaklık | Stamina, Armor |
| **Weapon** | Silah | Damage, Crit Damage |
| **Ring1/Ring2** | Yüzük | Özel bonuslar |
| **Necklace** | Kolye | Health, Magic Res |
| **Bracelet** | Bileklik | Stamina, Crit Chance |

### Rarity Seviyeleri
| Rarity | Emoji | Toplam Stat | Renk |
|--------|-------|-------------|------|
| **Common** | ⚪ | 10-30 | Gri |
| **Uncommon** | 🟢 | 30-60 | Yeşil |
| **Rare** | 🔵 | 60-100 | Mavi |
| **Epic** | 🟣 | 100-150 | Mor |
| **Legendary** | 🟠 | 150+ | Turuncu |

### İstatistikler
| Stat | Aralık | Slot Önerisi |
|------|--------|--------------|
| Health Bonus | 0-200 | Chest, Necklace |
| Stamina Bonus | 0-100 | Legs, Bracelet |
| Damage Bonus | 0-100 | Weapon, Gloves |
| Armor Bonus | 0-100 | Helmet, Chest, Legs |
| Magic Res Bonus | 0-100 | Helmet, Necklace |
| Crit Chance | 0-1 | Gloves, Ring |
| Crit Damage | 0-2 | Weapon, Ring |

---

## 🔍 Arama ve Filtreleme

### Karakter Arama
- **İsme göre:** "Fire", "Mage"
- **ID'ye göre:** "char_", "warrior"
- **Class filtresi:** ☑️ Class Filter → Mage

### Item Arama
- **İsme göre:** "Sword", "Legendary"
- **ID'ye göre:** "item_", "weapon"
- **Class filtresi:** ☑️ Class Filter → Warrior
- **Slot filtresi:** ☑️ Slot Filter → Weapon

### Filtreleri Birleştirme
```
Arama: "legendary"
+ Class Filter: Warrior
+ Slot Filter: Weapon
= Sadece Warrior için Legendary Weapon'lar
```

---

## 💾 Kaydetme ve Yönetim

### Kaydetme
- **Tek veri:** Veriyi seç → Düzenle → **💾 Kaydet**
- **Otomatik kayıt:** Yeni oluşturma sırasında otomatik

### Kopyalama
- **Karakter:** Seç → **📋 Kopyala** → `_copy` eklenir
- **Item:** Seç → **📋 Kopyala** → `_copy` eklenir
- Kopyadan sonra ID ve ismi değiştirin!

### Silme
- **Uyarı:** Geri alınamaz!
- Seç → **🗑️ Sil** → Onay ver

---

## 🎨 ID Kuralları

### Format
```
[tip]_[açıklama]_[detay]
```

### Örnekler
| Tip | Format | Örnek |
|-----|--------|-------|
| Karakter | `char_[class]_[name]` | `char_fire_mage` |
| Item | `item_[slot]_[name]` | `item_weapon_fire_sword` |
| Skill | `skill_[type]_[name]` | `skill_light_strike` |

### Kurallar
- ✅ Küçük harf kullan
- ✅ Boşluk yerine alt çizgi (`_`)
- ✅ Benzersiz olmalı
- ❌ Özel karakter kullanma
- ❌ Türkçe karakter kullanma

---

## 🎯 Dengeli Stat Dağılımı

### Karakter Stat Örnekleri

#### Mage (Yüksek Hasar)
```
Health: 100
Stamina: 80
Stamina Regen: 12
Defense: 0.1
```

#### Warrior (Yüksek Savunma)
```
Health: 150
Stamina: 100
Stamina Regen: 10
Defense: 0.3
```

#### Ninja (Yüksek Hız)
```
Health: 120
Stamina: 120
Stamina Regen: 15
Defense: 0.15
```

### Item Stat Örnekleri

#### Common Weapon
```
Damage: 10
Crit Chance: 0.05
Toplam: 10
```

#### Rare Weapon
```
Damage: 40
Crit Chance: 0.15
Crit Damage: 0.3
Toplam: 40
```

#### Legendary Weapon
```
Damage: 80
Crit Chance: 0.25
Crit Damage: 0.5
Health: 20
Stamina: 10
Toplam: 110
```

---

## 🔨 Crafting ve Shop

### Crafting Ayarları
| Rarity | Material Count | Salvage Rate |
|--------|----------------|--------------|
| Common | 2-3 | 30-40% |
| Uncommon | 3-4 | 40-50% |
| Rare | 4-5 | 50-60% |
| Epic | 5-6 | 60-70% |
| Legendary | 6-8 | 70-80% |

### Shop Fiyatları
| Rarity | Gold | Gem |
|--------|------|-----|
| Common | 50-100 | - |
| Uncommon | 100-200 | - |
| Rare | 200-500 | 10-20 |
| Epic | 500-1000 | 20-50 |
| Legendary | 1000+ | 50-100 |

---

## ⚠️ Sık Yapılan Hatalar

### ❌ ID Tekrarı
**Hata:** Aynı ID'yi kullanma
**Çözüm:** "ID'yi Otomatik Oluştur" kullan

### ❌ Kaydetmeyi Unutma
**Hata:** Değişiklikler kaybolur
**Çözüm:** Her düzenlemeden sonra **💾 Kaydet**

### ❌ Dengesiz Statlar
**Hata:** Çok yüksek veya düşük statlar
**Çözüm:** Yukarıdaki tablolara göre ayarla

### ❌ Icon Eksikliği
**Hata:** Icon atanmamış
**Çözüm:** Her zaman icon ekle (UI için önemli)

### ❌ Class Uyumsuzluğu
**Hata:** Warrior için Mage item'i
**Çözüm:** Item Class'ı doğru ayarla veya "All" kullan

---

## 🎮 Klavye Kısayolları

| Tuş | İşlem |
|-----|-------|
| **Ctrl + R** | Yenile |
| **Ctrl + S** | Kaydet (seçili veri) |
| **Ctrl + D** | Kopyala (seçili veri) |
| **Delete** | Sil (onay gerekir) |

---

## 📊 Kontrol Listesi

### Yeni Karakter Oluşturma
- [ ] İsim ve ID belirlendi
- [ ] Class seçildi
- [ ] Statlar dengelendi
- [ ] Icon eklendi
- [ ] Açıklama yazıldı
- [ ] Unlock ayarları yapıldı
- [ ] Başlangıç ekipmanı atandı
- [ ] Başlangıç skill'leri atandı
- [ ] Kaydedildi

### Yeni Item Oluşturma
- [ ] İsim ve ID belirlendi
- [ ] Slot ve Class seçildi
- [ ] Rarity belirlendi
- [ ] Statlar rarity'ye uygun
- [ ] Icon eklendi
- [ ] Açıklama yazıldı
- [ ] Crafting materyalleri eklendi
- [ ] Shop fiyatı belirlendi
- [ ] Salvage oranı ayarlandı
- [ ] Kaydedildi

---

## 🔗 Hızlı Linkler

- **[Detaylı Kılavuz](GAME_DATA_EDITOR_GUIDE.md)** - Tam kullanım kılavuzu
- **[Karakter Sistemi](CHARACTER_UNLOCK_GUIDE.md)** - Karakter unlock sistemi
- **[Item Sistemi](ITEM_SYSTEM_SETUP.md)** - Item sistemi detayları
- **[Ekipman Sistemi](EQUIPMENT_SYSTEM_GUIDE.md)** - Ekipman yönetimi
- **[Salvage Sistemi](SALVAGE_SYSTEM_GUIDE.md)** - Item eritme sistemi

---

## 💡 Pro İpuçları

1. **Toplu İşlem:** Benzer itemler için kopyalama kullan
2. **Organizasyon:** Class ve rarity'ye göre grupla
3. **Test:** Her yeni veriyi oyunda test et
4. **Yedekleme:** Önemli değişikliklerden önce kopyala
5. **Tutarlılık:** ID ve isimlendirme kurallarına uy
6. **Denge:** Stat dağılımını dengeli tut
7. **Dokümantasyon:** Karmaşık özellikler için açıklama yaz
8. **Icon:** Her zaman görsel ekle (UX için kritik)

---

**Hızlı ve kolay geliştirmeler! 🚀✨**

