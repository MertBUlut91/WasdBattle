# Matchmaking Sistemi Güncelleme

## 🔄 Değişiklik

Unity Matchmaker paketi **deprecated** (kullanımdan kaldırıldı) olduğu için kendi basit matchmaking sistemimizi oluşturduk.

---

## ✅ Yeni Sistem: SimpleMatchmakingManager

### Özellikler:
- ✅ ELO bazlı eşleşme
- ✅ Level bazlı filtreleme
- ✅ Timeout sistemi (60 saniye)
- ✅ İptal edilebilir
- ✅ Event-driven architecture

### Dosya:
```
Assets/_Project/Scripts/Matchmaking/SimpleMatchmakingManager.cs
```

---

## 🎮 Nasıl Çalışır?

### 1. Matchmaking Başlat:
```csharp
SimpleMatchmakingManager.Instance.StartMatchmaking();
```

### 2. Event'lere Abone Ol:
```csharp
SimpleMatchmakingManager.Instance.OnMatchFound += (result) => {
    Debug.Log($"Match found! ID: {result.MatchId}");
    // Combat scene'e geç
};

SimpleMatchmakingManager.Instance.OnMatchmakingFailed += () => {
    Debug.Log("Matchmaking failed!");
};
```

### 3. İptal Et:
```csharp
SimpleMatchmakingManager.Instance.CancelMatchmaking();
```

---

## ⚙️ Ayarlar

Inspector'da ayarlanabilir:
- **Matchmaking Timeout**: 60 saniye
- **Search Interval**: 2 saniye
- **ELO Tolerance**: ±200
- **Level Tolerance**: ±10

---

## 🔧 Gerçek Implementasyon İçin

Şu an **mock/simülasyon** kullanıyor. Gerçek matchmaking için:

### Seçenek 1: Unity Relay + Lobby (Önerilen)
```bash
# Package ekle:
com.unity.services.relay
com.unity.netcode.gameobjects

# Kullanım:
1. Relay allocation oluştur
2. Join code ile diğer oyuncu bağlan
3. Netcode ile oyun başlat
```

### Seçenek 2: Cloud Save ile Oyuncu Havuzu
```csharp
// Searching players listesi Cloud Save'de
// Her 2 saniyede bir listeyi kontrol et
// Uygun oyuncu bulunca eşleştir
```

### Seçenek 3: Kendi Backend'iniz
```
Node.js / ASP.NET Core backend
WebSocket ile real-time matchmaking
PostgreSQL ile oyuncu havuzu
```

---

## 📊 ELO Eşleşme Algoritması

```csharp
float CalculateMatchScore(int elo1, int level1, int elo2, int level2)
{
    float eloDiff = Mathf.Abs(elo1 - elo2);
    float levelDiff = Mathf.Abs(level1 - level2);
    
    // Level farkı daha ağırlıklı
    return eloDiff + (levelDiff * 50f);
}

// Skor ne kadar düşükse o kadar iyi eşleşme
```

---

## 🎯 Test Etme

### Editor'da:
1. Play'e bas
2. Ana menüde "Play" tıkla
3. Console'da matchmaking logları görünecek
4. ~10 saniye içinde eşleşme bulunacak (simülasyon)

### Gerçek Test (2 Oyuncu):
1. Build al
2. İki kopya çalıştır
3. Her ikisinde de "Play" tıkla
4. **ŞU AN:** Her biri ayrı ayrı mock eşleşme bulacak
5. **GERÇEK:** Relay/Lobby ile birbirlerini bulacaklar

---

## 🚀 Sonraki Adımlar

### Hemen Yapılabilir:
1. ✅ SimpleMatchmakingManager kullan (hazır)
2. ✅ Mock eşleşme ile test et
3. ⏳ Unity Relay ekle (gerçek P2P için)

### İleride:
1. Unity Relay Service entegrasyonu
2. Lobby sistemi (bekleme odası)
3. Reconnection sistemi
4. Matchmaking analytics

---

## 💡 Neden Kendi Sistemimiz?

**Unity Matchmaker Deprecated:**
- ❌ Artık güncelleme almıyor
- ❌ Unity 6'da kaldırılacak
- ❌ Multiplayer Services'e entegre edildi

**Bizim Sistem:**
- ✅ Tam kontrol
- ✅ Özelleştirilebilir
- ✅ Basit ve anlaşılır
- ✅ ELO sistemi entegre
- ✅ İleride Relay/Lobby eklenebilir

---

## 📝 Özet

**Eski:** Unity Matchmaker (deprecated)
**Yeni:** SimpleMatchmakingManager (custom)

**Şu an:** Mock eşleşme (test için)
**İleride:** Unity Relay + Netcode (gerçek multiplayer)

**Kullanım:** Aynı! Event-driven, kolay entegrasyon.

🎮 Hazır!

