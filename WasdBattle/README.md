# 🎮 WasdBattle

**WASD Battle** - Combo-based 1v1 PvP oyunu. Unity 6.2 ile geliştirilmiştir.

---

## 📖 Dokümantasyon

### 🚀 Başlangıç
- **[QUICK_START_GUIDE.md](QUICK_START_GUIDE.md)** - Hızlı başlangıç rehberi
- **[SCENE_SETUP_GUIDE.md](SCENE_SETUP_GUIDE.md)** - Detaylı scene kurulum rehberi

### 🔧 Teknik Dokümantasyon
- **[UNITY_SERVICES_SETUP.md](UNITY_SERVICES_SETUP.md)** - Unity Services kurulumu
- **[IMPLEMENTATION_SUMMARY.md](IMPLEMENTATION_SUMMARY.md)** - Tamamlanan sistemler
- **[MATCHMAKING_UPDATE.md](MATCHMAKING_UPDATE.md)** - Matchmaking sistemi detayları
- **[Assets/_Project/Scripts/README.md](Assets/_Project/Scripts/README.md)** - Kod yapısı

### 🛠️ Geliştirme Araçları
- **[GAME_DATA_EDITOR_GUIDE.md](GAME_DATA_EDITOR_GUIDE.md)** - Karakter ve Item editörü kullanım kılavuzu

---

## 🎯 Oyun Özellikleri

### ⚔️ Combat System
- **WASD Combo Sistemi** - Zamanlama ve doğruluk bazlı combo girişi
- **Turn-Based Combat** - Saldırı/Savunma fazları
- **Skill System** - 3 aktif skill, 1 ultimate, pasif yetenekler
- **Stamina Management** - Stratejik kaynak yönetimi

### 🏆 Progression
- **Level System** - XP kazanma ve seviye atlama
- **ELO Rating** - Rekabetçi sıralama sistemi (Bronze → Grandmaster)
- **Rewards** - Maç sonrası gold ve XP ödülleri

### 🛠️ Crafting & Economy
- **Inventory** - Malzeme ve item yönetimi
- **Crafting** - Skill ve item üretimi
- **Shop** - Karakter ve skill satın alma

### 🌐 Multiplayer
- **Unity Netcode** - Multiplayer networking
- **Custom Matchmaking** - ELO ve level bazlı eşleşme
- **Unity Cloud Save** - Oyuncu verisi kaydetme

---

## 🔧 Teknolojiler

- **Unity 6.2** - Oyun motoru
- **Unity Netcode for GameObjects** - Multiplayer
- **Unity Gaming Services:**
  - Authentication (Anonymous)
  - Cloud Save
- **C#** - Programlama dili
- **ScriptableObjects** - Veri yönetimi

---

## 📦 Kurulum

### Gereksinimler
- Unity 6.2 veya üzeri
- Unity Hub
- Internet bağlantısı (Unity Services için)

### Adımlar

1. **Projeyi Aç**
   ```
   Unity Hub → Open → WasdBattle klasörünü seç
   ```

2. **Package'ları Bekle**
   - Unity otomatik olarak gerekli package'ları indirecek
   - ~2-5 dakika sürer

3. **Unity Dashboard Ayarları**
   - [UNITY_SERVICES_SETUP.md](UNITY_SERVICES_SETUP.md) dosyasını takip edin
   - Authentication ve Cloud Save'i aktif edin

4. **Scene'leri Kur**
   - [SCENE_SETUP_GUIDE.md](SCENE_SETUP_GUIDE.md) dosyasını takip edin
   - BootScene, MainMenuScene, CombatScene oluşturun

5. **Test Et**
   - BootScene'i açın
   - Play'e basın
   - Console'da başarılı başlatma loglarını görün

---

## 🎮 Nasıl Oynanır?

### Combat
1. **Matchmaking** - "Play" butonuna tıklayın
2. **Character Select** - Karakterinizi seçin
3. **Combat** - Rakibinizle 1v1 dövüşün
   - **Attack Phase:** WASD combo girin
   - **Defense Phase:** Rakibin combosu görünür, WASD ile blokla
   - **Skills:** Q, E, R tuşları ile skill kullan
4. **Victory** - 3 round kazanan maçı kazanır

### Progression
- Maç kazanın → XP ve Gold kazanın
- Level atlayın → Yeni karakterler açın
- ELO yükseltin → Daha güçlü rakiplerle eşleşin
- Craft yapın → Yeni skill'ler oluşturun

---

## 📁 Proje Yapısı

```
WasdBattle/
├── Assets/
│   └── _Project/
│       ├── Scenes/           → Oyun sahneleri
│       ├── Scripts/          → Tüm C# script'ler
│       ├── Prefabs/          → GameObject prefab'ları
│       └── ScriptableObjects/ → Veri asset'leri
├── Packages/                 → Unity package'ları
└── ProjectSettings/          → Unity ayarları
```

**Detaylı kod yapısı için:** [Assets/_Project/Scripts/README.md](Assets/_Project/Scripts/README.md)

---

## 🚧 Geliştirme Durumu

### ✅ Tamamlanan Sistemler
- [x] Core Game Manager
- [x] Unity Cloud Save Integration
- [x] Custom Matchmaking System
- [x] Combo Input System
- [x] Skill System
- [x] Character System
- [x] Combat Manager
- [x] Progression (Level, ELO)
- [x] Economy (Inventory, Crafting, Shop)
- [x] UI Framework

### 🔨 Yapılacaklar
- [ ] 3D Modeller ve Animasyonlar
- [ ] VFX ve SFX
- [ ] Gerçek Multiplayer Test
- [ ] Balance Ayarları
- [ ] Tutorial System
- [ ] Achievement System
- [ ] Leaderboard

**Detaylı liste için:** [IMPLEMENTATION_SUMMARY.md](IMPLEMENTATION_SUMMARY.md)

---

## 🐛 Sorun Giderme

### "CloudSave Sign in failed: Singleton is not initialized"
**Çözüm:** `GameManager` içinde `UnityServices.InitializeAsync()` çağrıldığından emin olun. ✅ (Düzeltildi)

### "Scene 'MainMenuScene' couldn't be loaded"
**Çözüm:** Build Settings'e scene'i ekleyin (`File → Build Settings`)

### Script compile hatası
**Çözüm:** 
1. Unity Editor'ı kapat ve tekrar aç
2. `Assets → Reimport All`

**Daha fazla sorun için:** [QUICK_START_GUIDE.md - Sık Sorulan Sorular](QUICK_START_GUIDE.md#-sık-sorulan-sorular)

---

## 📞 İletişim

Sorularınız için:
- GitHub Issues
- Unity Forums
- Discord (yakında)

---

## 📄 Lisans

Bu proje eğitim amaçlıdır.

---

## 🙏 Teşekkürler

- Unity Technologies - Unity Engine ve Services
- Community - Feedback ve destek

---

## 🎉 Başlayın!

1. [QUICK_START_GUIDE.md](QUICK_START_GUIDE.md) dosyasını okuyun
2. [SCENE_SETUP_GUIDE.md](SCENE_SETUP_GUIDE.md) ile scene'leri kurun
3. Play'e basın ve eğlenin! 🎮

**Başarılar!** ⚔️

