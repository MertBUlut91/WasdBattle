# Unity Services Kurulum Rehberi

## 🎮 WasdBattle için Unity Gaming Services

Steam PC oyunu için **Firebase yerine Unity Gaming Services** kullanıyoruz.

### Kullanılan Servisler:
- ✅ **Unity Authentication** - Oyuncu kimlik doğrulama
- ✅ **Unity Cloud Save** - Oyuncu verisi kaydetme
- ✅ **Custom Matchmaking** - ELO bazlı eşleşme (kendi sistemimiz)
- ✅ **Unity Netcode** - Multiplayer network

---

## 📦 1. Package Kurulumu

Tüm gerekli package'lar zaten `manifest.json`'a eklendi:

```json
{
  "com.unity.services.authentication": "3.5.2",
  "com.unity.services.cloudsave": "3.2.2",
  "com.unity.services.core": "1.13.0",
  "com.unity.netcode.gameobjects": "2.7.0"
}
```

**Not:** Unity Matchmaker deprecated olduğu için kendi matchmaking sistemimizi kullanıyoruz.

Unity Editor açıldığında otomatik olarak indirilecek.

---

## 🔧 2. Unity Dashboard Kurulumu

### Adım 1: Unity Dashboard'a Git
1. https://dashboard.unity3d.com/ adresine git
2. Unity hesabınla giriş yap
3. "Create Project" veya mevcut projeyi seç

### Adım 2: Project ID'yi Al
1. Dashboard'da projenizi seçin
2. **Project Settings** → **Project ID**'yi kopyalayın

### Adım 3: Unity Editor'da Project ID'yi Ayarla
```
Unity Editor:
Edit → Project Settings → Services
→ "Link your Unity project" tıkla
→ Projenizi seçin veya yeni oluşturun
```

### Adım 4: Servisleri Aktif Et

#### a) Authentication
```
Dashboard → Authentication → Get Started
→ Anonymous Authentication: ENABLE
```

#### b) Cloud Save
```
Dashboard → Cloud Save → Get Started
→ Enable Cloud Save
```

#### c) Matchmaking (Custom)
```
Kendi matchmaking sistemimizi kullanıyoruz!
Unity Matchmaker deprecated olduğu için.

Dosya: Assets/_Project/Scripts/Matchmaking/SimpleMatchmakingManager.cs
```

**Matchmaking Ayarları (Inspector'da):**
```yaml
Matchmaking Timeout: 60 seconds
Search Interval: 2 seconds
ELO Tolerance: ±200
Level Tolerance: ±10
```

---

## 🎯 3. Kod Yapısı

### Kullanılan Servisler:

#### UnityCloudSaveService
```csharp
// Assets/_Project/Scripts/Network/UnityCloudSaveService.cs
public class UnityCloudSaveService : IFirebaseService
{
    // PlayerData'yı Cloud Save'e kaydeder
    // Unity Authentication ile entegre
}
```

#### SimpleMatchmakingManager
```csharp
// Assets/_Project/Scripts/Matchmaking/SimpleMatchmakingManager.cs
public class SimpleMatchmakingManager : MonoBehaviour
{
    // Custom ELO bazlı eşleşme
    // Basit ve özelleştirilebilir
    // Mock eşleşme (test için)
}
```

#### GameManager
```csharp
// Assets/_Project/Scripts/Core/GameManager.cs
private async void InitializeServices()
{
    // Unity Cloud Save başlatır
    _firebaseService = new UnityCloudSaveService();
    
    // Anonim giriş yapar
    await _firebaseService.SignInAnonymouslyAsync();
    
    // Player data yükler
    _currentPlayerData = await _dataManager.LoadPlayerDataAsync();
}
```

---

## 🚀 4. Test Etme

### Editor'da Test:
1. Unity Editor'da Play'e bas
2. Console'da şu logları göreceksin:
   ```
   [GameManager] Initializing services...
   [CloudSave] Initialized
   [CloudSave] Signed in: {PlayerId}
   [CloudSave] Loaded player data for {PlayerId}
   ```

### Matchmaking Test:
1. Ana menüde "Play" butonuna tıkla
2. Console'da:
   ```
   [Matchmaking] Starting matchmaking...
   [Matchmaking] Searching... (ELO: 1000, Level: 1)
   [Matchmaking] Match found! Opponent ELO: 980, Level: 1
   ```

### 2 Oyuncu Test:
1. Build al (PC Standalone)
2. İki kopya çalıştır
3. Her ikisinde de "Play" tıkla
4. **ŞU AN:** Mock eşleşme (simülasyon)
5. **İLERİDE:** Unity Relay ile gerçek P2P bağlantı

---

## 📊 5. Veri Yapısı

### Cloud Save'de Saklanan Veri:
```json
{
  "playerData": {
    "userId": "abc123",
    "username": "Player_abc123",
    "level": 1,
    "elo": 1000,
    "experience": 0,
    "gold": 100,
    "metal": 50,
    "energyCrystal": 50,
    "rune": 10,
    "essence": 5,
    "ownedCharacters": ["char_mage", "char_warrior", "char_ninja"],
    "ownedSkills": [],
    "selectedCharacterId": "char_mage",
    "totalMatches": 0,
    "wins": 0,
    "losses": 0
  }
}
```

### Matchmaking Parametreleri:
```csharp
// SimpleMatchmakingManager ayarları
ELO Tolerance: ±200
Level Tolerance: ±10
Timeout: 60 seconds
Search Interval: 2 seconds
```

---

## 🔍 6. Dashboard'da Veri Görüntüleme

### Cloud Save Verileri:
```
Dashboard → Cloud Save → Player Data
→ Player ID gir → View Data
```

### Matchmaking Logları:
```
Unity Console'da:
→ Matchmaking başlangıç
→ Arama durumu
→ Eşleşme bulundu/başarısız
→ Timeout durumu
```

---

## ⚙️ 7. ELO Sistemi Ayarları

### SimpleMatchmakingManager Ayarları:

**Inspector'da:**
```yaml
ELO Tolerance: 200 (başlangıç için geniş)
Level Tolerance: 10
Matchmaking Timeout: 60
Search Interval: 2
```

**Kod ile Güncelleme:**
```csharp
var matchmaking = SimpleMatchmakingManager.Instance;
// Inspector'dan ayarlanabilir, kod değişikliği gerekmez
```

**Oyuncu Sayısı Arttıkça:**
- ELO Tolerance: 100'e düşür
- Level Tolerance: 5'e düşür
- Search Interval: 1'e düşür (daha hızlı)

---

## 🐛 8. Troubleshooting

### "Project not linked" Hatası:
```
Edit → Project Settings → Services
→ Link your Unity project
```

### "Authentication failed" Hatası:
```
Dashboard → Authentication → Enable Anonymous
Unity Editor'ı yeniden başlat
```

### Matchmaking çalışmıyor:
```
Console loglarını kontrol et:
- [Matchmaking] Starting matchmaking... görünüyor mu?
- SimpleMatchmakingManager GameObject var mı?
- GameManager düzgün başlatıldı mı?
```

### Cloud Save çalışmıyor:
```
Dashboard → Cloud Save → Enable
Window → Package Manager → Cloud Save → Reinstall
```

---

## 💰 9. Maliyet (Ücretsiz Tier)

### Unity Gaming Services - Free Tier:
- **Authentication**: 100K MAU (Monthly Active Users)
- **Cloud Save**: 1GB storage, 50K requests/month
- **Custom Matchmaking**: Sınırsız (kendi sistemimiz)
- **Netcode**: Sınırsız (player-hosted)

**Sonuç:** Başlangıç için tamamen ücretsiz! 🎉

---

## 🔄 10. Firebase'den Geçiş

### Eski Kod (Firebase):
```csharp
_firebaseService = new MockFirebaseService();
```

### Yeni Kod (Unity Cloud Save):
```csharp
_firebaseService = new UnityCloudSaveService();
```

**Not:** Interface aynı (`IFirebaseService`), sadece implementasyon değişti!

---

## 📝 11. Sonraki Adımlar

### Hemen Yapılacaklar:
1. ✅ Unity Dashboard'da proje oluştur
2. ✅ Project ID'yi Unity Editor'a bağla
3. ✅ Authentication ve Cloud Save aktif et
4. ✅ Test et! (Matchmaking otomatik çalışıyor)

### İleride (Steam Entegrasyonu):
1. Steamworks.NET ekle
2. Steam ID ile authentication
3. Steam achievements
4. Steam leaderboards

---

## 🎮 12. Kullanım Örnekleri

### Matchmaking Başlat:
```csharp
SimpleMatchmakingManager.Instance.StartMatchmaking();
```

### Player Data Kaydet:
```csharp
GameManager.Instance.SavePlayerData();
```

### Player Data Güncelle:
```csharp
var playerData = GameManager.Instance.CurrentPlayerData;
playerData.gold += 100;
GameManager.Instance.SavePlayerData();
```

### Matchmaking İptal:
```csharp
SimpleMatchmakingManager.Instance.CancelMatchmaking();
```

---

## ✅ Özet

**Artık kullanıyoruz:**
- ❌ Firebase (mobil için)
- ✅ Unity Cloud Save (PC için)
- ✅ Custom Matchmaking (kendi sistemimiz)
- ✅ Unity Authentication (anonim)

**Avantajlar:**
- Steam ile uyumlu
- Ücretsiz tier cömert
- Unity'ye native
- Kolay kurulum
- Tam kontrol (custom matchmaking)

**Hazır!** 🚀

