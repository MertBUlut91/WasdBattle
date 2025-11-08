# 🚀 WasdBattle - Hızlı Başlangıç Rehberi

## 📋 İçindekiler
1. [Proje Yapısı](#-proje-yapısı)
2. [İlk Kurulum](#-ilk-kurulum)
3. [Unity Dashboard Ayarları](#-unity-dashboard-ayarları)
4. [Test Etme](#-test-etme)
5. [Sistemler ve Kullanımları](#-sistemler-ve-kullanımları)
6. [Sık Sorulan Sorular](#-sık-sorulan-sorular)

---

## 📁 Proje Yapısı

```
Assets/_Project/
├── Scripts/
│   ├── Core/              → Temel sistemler (GameManager, AudioManager, vb.)
│   ├── Combat/            → Dövüş mekaniği
│   ├── Characters/        → Karakter sistemi
│   ├── Skills/            → Skill sistemi
│   ├── Input/             → Combo input sistemi
│   ├── Matchmaking/       → Eşleşme sistemi
│   ├── Progression/       → Level, ELO, ödüller
│   ├── Economy/           → Envanter, craft, shop
│   ├── UI/                → Tüm UI elementleri
│   ├── Network/           → Network ve veri yönetimi
│   └── Data/              → ScriptableObject'ler
├── Prefabs/               → Hazır GameObject'ler
├── ScriptableObjects/     → Veri asset'leri
└── Scenes/                → Oyun sahneleri
```

---

## 🔧 İlk Kurulum

### Adım 1: Unity Editor'ı Aç
```
Unity Hub → Projects → Open → WasdBattle klasörünü seç
Unity 6.2 ile açılacak
```

### Adım 2: Package'ları Bekle
Unity Editor açıldığında otomatik olarak package'lar indirilecek:
- ✅ Unity Netcode
- ✅ Unity Services (Authentication, Cloud Save)
- ✅ Input System
- ✅ URP

**Süre:** ~2-5 dakika

### Adım 3: Script Compilation
Tüm script'ler compile olacak. Console'da hata olmamalı.

**Eğer hata varsa:**
- Unity Editor'ı kapat ve tekrar aç
- `Assets → Reimport All`

---

## 🌐 Unity Dashboard Ayarları

### 1. Dashboard'a Git
```
https://dashboard.unity3d.com/
Unity hesabınla giriş yap
```

### 2. Proje Oluştur veya Seç
```
Create Project → "WasdBattle" adını ver
veya
Mevcut projeyi seç
```

### 3. Project ID'yi Kopyala
```
Dashboard → Project Settings → Project ID
Kopyala (örn: 1234abcd-5678-90ef-ghij-klmnopqrstuv)
```

### 4. Unity Editor'da Bağla
```
Unity Editor:
Edit → Project Settings → Services
→ "Link your Unity project" tıkla
→ Projenizi seçin
→ Otomatik bağlanacak
```

### 5. Servisleri Aktif Et

#### a) Authentication
```
Dashboard → Authentication
→ Anonymous Authentication: ENABLE
```

#### b) Cloud Save
```
Dashboard → Cloud Save
→ Enable Cloud Save
```

**NOT:** Matchmaker'a gerek yok! Kendi sistemimizi kullanıyoruz.

---

## 🎮 Test Etme

### 1. Editor'da Test

1. **Scene'i Aç**
   ```
   Assets/Scenes/SampleScene.unity
   ```

2. **GameInitializer Ekle**
   
**ÖNEMLI:** Oyunu test etmek için önce scene'leri kurmalısınız!
   
Detaylı scene kurulum rehberi için:
```
SCENE_SETUP_GUIDE.md dosyasını okuyun!
```

**Hızlı Kurulum:**
```
1. BootScene oluştur
2. GameManager, AudioManager, VFXManager, SimpleMatchmakingManager ekle
3. BootSceneController ekle
4. MainMenuScene oluştur (basit bir Canvas yeter)
5. Build Settings'e her iki scene'i ekle
```

3. **Play'e Bas**
   ```
   Unity Editor → BootScene'i aç
   → Play butonu
   ```

4. **Console'u Kontrol Et**
   ```
   Şunları görmeli:
   [GameManager] Initializing services...
   [GameManager] Initializing Unity Services...
   [GameManager] Unity Services initialized
   [GameManager] Signing in anonymously...
   [GameManager] Signed in as: {PlayerId}
   [CloudSave] Initialized
   [CloudSave] Signed in: {PlayerId}
   [CloudSave] Loaded player data
   [GameManager] Player data loaded: Player_xxxxx, Level: 1, ELO: 1000
   [GameManager] Services initialized successfully!
   ```

### 2. Matchmaking Test

1. **MainMenuUI Oluştur**
   ```
   Hierarchy → UI → Canvas
   → Add Component → MainMenuUI
   → Play butonu ekle
   ```

2. **Play'e Bas ve "Play" Butonuna Tıkla**
   ```
   Console'da:
   [Matchmaking] Starting matchmaking...
   [Matchmaking] Searching...
   [Matchmaking] Match found!
   ```

---

## 🎯 Sistemler ve Kullanımları

### 1. GameManager (Oyun Yöneticisi)

**Ne İşe Yarar:**
- Tüm servisleri başlatır
- Oyuncu verisini yönetir
- Oyun durumunu kontrol eder

**Kullanım:**
```csharp
// Oyuncu verisine eriş
var playerData = GameManager.Instance.CurrentPlayerData;
Debug.Log($"Level: {playerData.level}, ELO: {playerData.elo}");

// Veri kaydet
GameManager.Instance.SavePlayerData();

// Oyun durumunu değiştir
GameManager.Instance.SetGameState(GameState.InCombat);
```

**Otomatik Başlar:** Singleton, ilk erişimde oluşur.

---

### 2. UnityCloudSaveService (Veri Kaydetme)

**Ne İşe Yarar:**
- Oyuncu verisini cloud'a kaydeder
- Otomatik authentication
- Lokal cache backup

**Kullanım:**
```csharp
// Otomatik çalışır, manuel kullanıma gerek yok
// GameManager üzerinden erişilir
```

**Veri Yapısı:**
```csharp
PlayerData {
    username, level, elo, experience,
    gold, metal, energyCrystal, rune, essence,
    ownedCharacters[], ownedSkills[],
    totalMatches, wins, losses
}
```

---

### 3. SimpleMatchmakingManager (Eşleşme)

**Ne İşe Yarar:**
- ELO bazlı oyuncu eşleştirme
- Level filtreleme
- Timeout yönetimi

**Kullanım:**
```csharp
// Matchmaking başlat
SimpleMatchmakingManager.Instance.StartMatchmaking();

// Event'lere abone ol
SimpleMatchmakingManager.Instance.OnMatchFound += (result) => {
    Debug.Log($"Match ID: {result.MatchId}");
    // Combat scene'e geç
};

// İptal et
SimpleMatchmakingManager.Instance.CancelMatchmaking();
```

**Inspector Ayarları:**
- ELO Tolerance: ±200
- Level Tolerance: ±10
- Timeout: 60 saniye

---

### 4. ComboInputManager (WASD Combo)

**Ne İşe Yarar:**
- WASD tuşlarını dinler
- Combo doğrulama
- Zamanlama kontrolü

**Kullanım:**
```csharp
// Combo başlat
ComboInputManager comboManager = GetComponent<ComboInputManager>();
comboManager.StartCombo(comboData);

// Event'lere abone ol
comboManager.OnComboCompleted += (result) => {
    Debug.Log($"Accuracy: {result.accuracy:P}");
    Debug.Log($"Grade: {result.grade}");
};

// Durdur
comboManager.StopCombo();
```

---

### 5. SkillManager (Skill Yönetimi)

**Ne İşe Yarar:**
- Skill kullanımı
- Cooldown takibi
- Stamina kontrolü

**Kullanım:**
```csharp
SkillManager skillManager = GetComponent<SkillManager>();

// Skill kullan
if (skillManager.CanUseSkill(skillData))
{
    skillManager.UseSkill(skillData);
}

// Cooldown kontrol
float remaining = skillManager.GetCooldownRemaining(skillData);

// Skill ekle
skillManager.EquipSkill(skillData, slotIndex: 0);
```

---

### 6. PlayerCharacter (Karakter)

**Ne İşe Yarar:**
- HP, Stamina yönetimi
- Buff/debuff sistemi
- Network senkronizasyonu

**Kullanım:**
```csharp
PlayerCharacter character = GetComponent<PlayerCharacter>();

// Hasar ver
character.TakeDamage(50);

// Heal
character.ModifyHealth(30);

// Stamina değiştir
character.ModifyStamina(-20);

// Buff uygula
character.ApplyDamageBoost(0.25f, duration: 5f);
```

---

### 7. CombatManager (Dövüş)

**Ne İşe Yarar:**
- Tur bazlı dövüş akışı
- Saldırı/Savunma yönetimi
- Hasar hesaplama

**Kullanım:**
```csharp
CombatManager combat = GetComponent<CombatManager>();

// Oyuncuları ata
combat.SetPlayers(player1, player2);

// Maçı başlat
combat.StartMatch();

// Event'lere abone ol
combat.OnMatchEnded += (winnerId) => {
    Debug.Log($"Winner: {winnerId}");
};
```

---

### 8. LevelSystem & ELOSystem (İlerleme)

**Ne İşe Yarar:**
- XP ve level yönetimi
- ELO rating hesaplama
- Ödül sistemi

**Kullanım:**
```csharp
// Level System
LevelSystem levelSystem = new LevelSystem();
levelSystem.GainExperience(playerData, xpAmount: 150);

// ELO System
ELOSystem eloSystem = new ELOSystem();
eloSystem.UpdateELO(player, opponent, playerWon: true);

// Rank al
Rank rank = ELOSystem.GetRank(playerData.elo);
Debug.Log($"Rank: {ELOSystem.GetRankDisplayName(rank)}");
```

---

### 9. InventoryManager (Envanter)

**Ne İşe Yarar:**
- Malzeme yönetimi
- Gold sistemi
- Skill/karakter sahipliği

**Kullanım:**
```csharp
InventoryManager inventory = new InventoryManager(playerData);

// Malzeme ekle
inventory.AddMaterial(MaterialType.Metal, 50);

// Gold harca
if (inventory.SpendGold(100))
{
    Debug.Log("Satın alındı!");
}

// Skill ekle
inventory.AddSkill("skill_fast_strike");
```

---

### 10. CraftingSystem (Üretim)

**Ne İşe Yarar:**
- Item crafting
- Malzeme kontrolü
- Skill upgrade

**Kullanım:**
```csharp
CraftingSystem crafting = new CraftingSystem(inventory);

// Craft yapılabilir mi?
if (crafting.CanCraft(recipe))
{
    crafting.Craft(recipe);
}

// Eksik malzemeler
List<string> missing = crafting.GetMissingMaterials(recipe);
```

---

## 🎨 UI Sistemleri

### MainMenuUI
- Oyuncu bilgileri
- Play butonu (matchmaking)
- Karakter seçimi
- Envanter/Shop

### CombatUI
- HP/Stamina barları
- Skill bar
- Combo göstergesi
- Round bilgisi

### CharacterSelectUI
- Karakter listesi
- İstatistikler
- Seçim butonu

---

## 🛠️ Editor Tools

### 1. Karakter Oluştur
```
Unity Editor:
WasdBattle → Create Default Characters
→ 3 karakter oluşturulur (Mage, Warrior, Ninja)
→ Assets/_Project/ScriptableObjects/Characters/
```

### 2. Skill Oluştur
```
Unity Editor:
WasdBattle → Create Default Skills
→ 3 skill oluşturulur (Fast, Heavy, Special)
→ Assets/_Project/ScriptableObjects/Skills/
```

---

## 🐛 Sık Sorulan Sorular

### S: Console'da "CloudSave" hatası alıyorum
**C:** 
1. Unity Dashboard'da Cloud Save aktif mi?
2. Project ID bağlı mı? (Edit → Project Settings → Services)
3. Internet bağlantınız var mı?

### S: Matchmaking çalışmıyor
**C:**
- Şu an mock eşleşme kullanıyor (test için)
- Console'da "[Matchmaking] Starting..." görünüyor mu?
- SimpleMatchmakingManager GameObject var mı?

### S: Network hatası alıyorum
**C:**
1. NetworkManager GameObject scene'de var mı?
2. Netcode package kurulu mu?
3. ComboResult serializable mi? (Zaten düzelttik)

### S: Script compile hatası
**C:**
1. Unity Editor'ı kapat ve tekrar aç
2. `Assets → Reimport All`
3. Package Manager'da eksik package var mı kontrol et

### S: Karakterler/Skill'ler yok
**C:**
- Editor tool'ları çalıştır:
  - `WasdBattle → Create Default Characters`
  - `WasdBattle → Create Default Skills`

---

## 📊 Sistem Gereksinimleri

### Minimum:
- Unity 6.2+
- 8GB RAM
- Internet bağlantısı (Unity Services için)

### Önerilen:
- Unity 6.2+
- 16GB RAM
- SSD
- Güçlü internet

---

## 🚀 Sonraki Adımlar

### 1. Scene'leri Oluştur
```
- MainMenu scene
- Combat scene
- Lobby scene
```

### 2. UI Prefab'ları Yap
```
- Canvas'lar
- Button'lar
- Panel'ler
```

### 3. Karakterleri Oluştur
```
- 3D modeller veya 2D sprite'lar
- Animator controller'lar
- Prefab'lar
```

### 4. VFX & SFX Ekle
```
- Particle effect'ler
- Ses dosyaları
- AudioManager'a kaydet
```

### 5. Test ve Balance
```
- Hasar değerleri
- Stamina maliyetleri
- ELO algoritması
- Combo zorlukları
```

---

## 📖 Detaylı Dokümantasyon

- **Kod Yapısı:** `Assets/_Project/Scripts/README.md`
- **Unity Services:** `UNITY_SERVICES_SETUP.md`
- **Tamamlanan Sistemler:** `IMPLEMENTATION_SUMMARY.md`
- **Matchmaking:** `MATCHMAKING_UPDATE.md`

---

## 💡 İpuçları

### 1. Debug İçin
```csharp
// Network Debug UI ekle
GameObject debugUI = new GameObject("NetworkDebugUI");
debugUI.AddComponent<NetworkDebugUI>();
```

### 2. Test İçin
```csharp
// Mock data ile test
playerData.gold = 9999;
playerData.level = 10;
playerData.elo = 1500;
```

### 3. Performance
```csharp
// VFX pooling kullan
VFXManager.Instance.SpawnVFX(prefab, position);
```

---

## ✅ Kontrol Listesi

Oyunu çalıştırmadan önce:

- [ ] Unity Dashboard'da proje bağlı
- [ ] Authentication aktif
- [ ] Cloud Save aktif
- [ ] Package'lar yüklü
- [ ] Script'ler compile oldu
- [ ] GameInitializer scene'de
- [ ] Karakterler oluşturuldu
- [ ] Skill'ler oluşturuldu
- [ ] NetworkManager var (multiplayer için)

---

## 🎮 Hazırsınız!

Artık oyunu test edebilirsiniz. Play'e basın! 🚀

**Sorun mu var?** Dokümantasyonlara bakın veya Console loglarını kontrol edin.

**Başarılar!** 🎉

