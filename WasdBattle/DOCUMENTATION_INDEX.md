# 📚 WasdBattle - Dokümantasyon İndeksi

Bu dosya, projedeki tüm dokümantasyonun hızlı erişim rehberidir.

---

## 🎯 Hangi Rehberi Okumalıyım?

### 🆕 Yeni Başlıyorum
1. **[README.md](README.md)** - Projeye genel bakış
2. **[QUICK_START_GUIDE.md](QUICK_START_GUIDE.md)** - Hızlı başlangıç
3. **[SCENE_SETUP_GUIDE.md](SCENE_SETUP_GUIDE.md)** - Scene kurulumu

### 🔧 Unity Services Kuruyorum
1. **[UNITY_SERVICES_SETUP.md](UNITY_SERVICES_SETUP.md)** - Unity Dashboard ayarları
2. **[QUICK_START_GUIDE.md](QUICK_START_GUIDE.md)** - Test etme

### 💻 Kod Yapısını Anlamak İstiyorum
1. **[Assets/_Project/Scripts/README.md](Assets/_Project/Scripts/README.md)** - Kod yapısı
2. **[IMPLEMENTATION_SUMMARY.md](IMPLEMENTATION_SUMMARY.md)** - Tamamlanan sistemler

### 🎮 Matchmaking Sistemi Hakkında
1. **[MATCHMAKING_UPDATE.md](MATCHMAKING_UPDATE.md)** - Matchmaking detayları

---

## 📖 Tüm Dokümantasyon

### 1. README.md
**Amaç:** Projeye genel bakış  
**İçerik:**
- Oyun özellikleri
- Teknolojiler
- Kurulum adımları (özet)
- Proje yapısı
- Geliştirme durumu
- Sorun giderme (özet)

**Ne zaman oku:** İlk kez projeye baktığında

---

### 2. QUICK_START_GUIDE.md
**Amaç:** Adım adım hızlı başlangıç  
**İçerik:**
- İlk kurulum (detaylı)
- Unity Dashboard ayarları (özet)
- Test etme
- Tüm sistemlerin nasıl kullanılacağı (kod örnekleri)
- Sık sorulan sorular
- Kontrol listesi

**Ne zaman oku:** Projeyi çalıştırmaya hazırlanırken

**Önemli Bölümler:**
- Sayfa 1-3: Kurulum
- Sayfa 4-10: Sistemlerin kullanımı
- Sayfa 11: Sorun giderme

---

### 3. SCENE_SETUP_GUIDE.md
**Amaç:** Tüm scene'lerin detaylı kurulum rehberi  
**İçerik:**
- BootScene kurulumu (adım adım)
- MainMenuScene kurulumu (UI dahil)
- CombatScene kurulumu (tüm UI elementleri)
- LobbyScene kurulumu (opsiyonel)
- Build Settings
- Test etme
- Sorun giderme

**Ne zaman oku:** Scene'leri kurarken

**Önemli Bölümler:**
- BootScene: En önemli, ilk kurulması gereken
- MainMenuScene: UI referansları kritik
- CombatScene: Spawn point'ler ve UI
- Her scene'in sonunda kontrol listesi var

---

### 4. UNITY_SERVICES_SETUP.md
**Amaç:** Unity Gaming Services kurulum rehberi  
**İçerik:**
- Package kurulumu
- Unity Dashboard ayarları (detaylı)
- Authentication setup
- Cloud Save setup
- Kod yapısı
- Test etme
- Troubleshooting
- Maliyet bilgisi

**Ne zaman oku:** Unity Services'i kurarken

**Önemli Bölümler:**
- "Dashboard Configuration": Adım adım dashboard ayarları
- "Testing": Test senaryoları
- "Troubleshooting": Sık karşılaşılan hatalar

---

### 5. IMPLEMENTATION_SUMMARY.md
**Amaç:** Tamamlanan tüm sistemlerin özeti  
**İçerik:**
- Tamamlanan sistemler listesi
- Her sistemin açıklaması
- Dosya istatistikleri (46 script)
- Sonraki adımlar
- Teknik detaylar

**Ne zaman oku:** Projenin genel durumunu öğrenmek istediğinde

**Önemli Bölümler:**
- "Implemented Systems": Sistem listesi
- "Next Steps": Yapılacaklar

---

### 6. MATCHMAKING_UPDATE.md
**Amaç:** Matchmaking sistemi değişikliklerinin açıklaması  
**İçerik:**
- Unity Matchmaker neden kaldırıldı
- Custom matchmaking nasıl çalışır
- ELO ve Level bazlı eşleşme
- Ayarlar
- Gerçek implementasyon için öneriler

**Ne zaman oku:** Matchmaking sistemini anlamak/geliştirmek istediğinde

**Önemli Bölümler:**
- "Why Custom Matchmaking": Karar süreci
- "How It Works": Algoritma açıklaması
- "For Real Implementation": Production için öneriler

---

### 7. Assets/_Project/Scripts/README.md
**Amaç:** Kod yapısının detaylı açıklaması  
**İçerik:**
- Klasör yapısı
- Her klasördeki script'lerin açıklaması
- Oyun akışı diyagramı
- Veri akışı
- Singleton'lar
- ScriptableObject'ler
- Sonraki adımlar

**Ne zaman oku:** Kod yazmaya başlamadan önce

**Önemli Bölümler:**
- "Folder Structure": Klasör organizasyonu
- "Game Flow": Oyun akışı
- "Data Flow": Veri akışı

---

### 8. GAME_DATA_EDITOR_GUIDE.md
**Amaç:** Karakter ve Item oluşturma/düzenleme editörü kullanım kılavuzu  
**İçerik:**
- Editörü açma ve kullanma
- Karakter yönetimi (oluşturma, düzenleme, silme)
- Item yönetimi (oluşturma, düzenleme, silme)
- Arama ve filtreleme özellikleri
- Kopyalama ve toplu işlemler
- İpuçları ve en iyi uygulamalar
- Hızlı başlangıç örnekleri

**Ne zaman oku:** Yeni karakter veya item oluştururken

**Önemli Bölümler:**
- "Karakter Yönetimi": Karakter oluşturma ve düzenleme
- "Item Yönetimi": Item oluşturma ve düzenleme
- "Hızlı Başlangıç": 5 dakikada ilk varlıklarınızı oluşturun

---

## 🔍 Hızlı Arama

### Sorun Giderme
- **CloudSave hatası:** [QUICK_START_GUIDE.md - Sık Sorulan Sorular](QUICK_START_GUIDE.md#-sık-sorulan-sorular)
- **Scene yüklenmiyor:** [SCENE_SETUP_GUIDE.md - Sorun Giderme](SCENE_SETUP_GUIDE.md#-sık-karşılaşılan-sorunlar)
- **Unity Services hatası:** [UNITY_SERVICES_SETUP.md - Troubleshooting](UNITY_SERVICES_SETUP.md#troubleshooting)

### Sistem Kullanımı
- **GameManager:** [QUICK_START_GUIDE.md - Sistemler](QUICK_START_GUIDE.md#1-gamemanager-oyun-yöneticisi)
- **Matchmaking:** [QUICK_START_GUIDE.md - SimpleMatchmakingManager](QUICK_START_GUIDE.md#3-simplematchmakingmanager-eşleşme)
- **Combat:** [QUICK_START_GUIDE.md - CombatManager](QUICK_START_GUIDE.md#7-combatmanager-dövüş)
- **Skills:** [QUICK_START_GUIDE.md - SkillManager](QUICK_START_GUIDE.md#5-skillmanager-skill-yönetimi)
- **Game Data Editor:** [GAME_DATA_EDITOR_GUIDE.md](GAME_DATA_EDITOR_GUIDE.md)

### Kurulum
- **İlk kurulum:** [QUICK_START_GUIDE.md - İlk Kurulum](QUICK_START_GUIDE.md#-ilk-kurulum)
- **Scene kurulum:** [SCENE_SETUP_GUIDE.md](SCENE_SETUP_GUIDE.md)
- **Unity Dashboard:** [UNITY_SERVICES_SETUP.md - Dashboard Configuration](UNITY_SERVICES_SETUP.md#dashboard-configuration)

---

## 📊 Dokümantasyon İstatistikleri

### Dosya Sayıları
- **Markdown Dosyaları:** 8 adet
- **C# Script'ler:** 47 adet
- **Toplam Satır:** ~2500+ satır dokümantasyon

### Kapsam
- ✅ Kurulum rehberleri
- ✅ Kod dokümantasyonu
- ✅ Sistem kullanım kılavuzları
- ✅ Sorun giderme
- ✅ Test senaryoları
- ✅ Kontrol listeleri

---

## 🎯 Önerilen Okuma Sırası

### Senaryo 1: Sıfırdan Başlıyorum
```
1. README.md (5 dk)
2. QUICK_START_GUIDE.md (15 dk)
3. SCENE_SETUP_GUIDE.md (30 dk)
4. UNITY_SERVICES_SETUP.md (20 dk)
5. Assets/_Project/Scripts/README.md (10 dk)
```

### Senaryo 2: Sadece Kod Yazacağım
```
1. Assets/_Project/Scripts/README.md (10 dk)
2. QUICK_START_GUIDE.md - Sistemler bölümü (20 dk)
3. IMPLEMENTATION_SUMMARY.md (5 dk)
```

### Senaryo 3: Sadece Scene Kuracağım
```
1. SCENE_SETUP_GUIDE.md (30 dk)
2. QUICK_START_GUIDE.md - Test Etme bölümü (5 dk)
```

### Senaryo 4: Sadece Unity Services Kuracağım
```
1. UNITY_SERVICES_SETUP.md (20 dk)
2. QUICK_START_GUIDE.md - Unity Dashboard Ayarları (5 dk)
```

---

## 🔄 Dokümantasyon Güncellemeleri

### Son Güncellemeler
- ✅ **GameDataEditor.cs** - Karakter ve Item editörü eklendi
- ✅ **GAME_DATA_EDITOR_GUIDE.md** - Editör kullanım kılavuzu oluşturuldu
- ✅ **GameManager.cs** - `UnityServices.InitializeAsync()` eklendi
- ✅ **BootSceneController.cs** - Yeni scene controller oluşturuldu
- ✅ **LobbyUI.cs** - Lobby UI controller eklendi
- ✅ **MainMenuUI.cs** - LobbyScene geçişi eklendi
- ✅ **SCENE_SETUP_GUIDE.md** - Detaylı scene kurulum rehberi
- ✅ **QUICK_START_GUIDE.md** - Scene setup referansı eklendi
- ✅ **README.md** - Ana proje README'si oluşturuldu

### Versiyon
**v1.1** - Game Data Editor eklendi (9 Kasım 2025)
**v1.0** - İlk tam dokümantasyon seti (7 Kasım 2024)

---

## 💡 İpuçları

### Dokümantasyonu Kullanırken
1. **Kontrol listelerini takip et** - Her rehberde kontrol listeleri var
2. **Console loglarını kontrol et** - Tüm sistemler detaylı log üretir
3. **Kod örneklerini kopyala** - QUICK_START_GUIDE'da kullanıma hazır örnekler var
4. **Sorun giderme bölümlerine bak** - Çoğu sorun zaten çözülmüş

### Yeni Özellik Eklerken
1. İlgili script'i bul (Assets/_Project/Scripts/README.md)
2. Benzer bir sisteme bak (IMPLEMENTATION_SUMMARY.md)
3. Kod örneklerini incele (QUICK_START_GUIDE.md)
4. Test et (QUICK_START_GUIDE.md - Test Etme)

---

## 🎉 Başarılar!

Tüm dokümantasyon hazır. Artık projeyi rahatça geliştirebilirsin! 🚀

**Sorun mu var?** Her dokümanda "Sorun Giderme" bölümü var.

**Yeni bir şey mi öğrenmek istiyorsun?** QUICK_START_GUIDE.md'deki kod örneklerine bak.

**Scene mi kuruyorsun?** SCENE_SETUP_GUIDE.md'yi adım adım takip et.

---

## 📞 Yardım

Eğer dokümantasyonda bulamadığın bir şey varsa:
1. Console loglarını kontrol et
2. Unity Dashboard'u kontrol et
3. Script'lerdeki comment'leri oku
4. GitHub Issues'a yaz

**Kolay gelsin!** 💪

