# 🎬 WasdBattle - Scene Setup Rehberi

Bu rehber, oyunun tüm sahnelerini nasıl kuracağınızı **adım adım** gösterir.

---

## 📋 Gerekli Scene'ler

```
1. BootScene        → Oyun başlangıcı, servis başlatma
2. MainMenuScene    → Ana menü
3. CombatScene      → 1v1 dövüş
4. LobbyScene       → Matchmaking bekleme (opsiyonel)
```

---

## 🚀 Scene 1: BootScene (İlk Başlatma)

### Amaç
Oyun açıldığında ilk yüklenen scene. Tüm servisleri başlatır.

### Setup Adımları

#### 1. Yeni Scene Oluştur
```
Unity Editor:
File → New Scene → Empty
File → Save As → "BootScene"
Konum: Assets/_Project/Scenes/BootScene.unity
```

#### 2. GameManager Ekle
```
Hierarchy → Sağ tık → Create Empty
→ Adını "GameManager" yap
→ Add Component → Game Manager (script)
```

**Önemli:** `GameManager` singleton olduğu için otomatik `DontDestroyOnLoad` olacak.

#### 3. AudioManager Ekle
```
Hierarchy → Sağ tık → Create Empty
→ Adını "AudioManager" yap
→ Add Component → Audio Manager (script)
```

#### 4. VFXManager Ekle
```
Hierarchy → Sağ tık → Create Empty
→ Adını "VFXManager" yap
→ Add Component → VFX Manager (script)
```

#### 5. SimpleMatchmakingManager Ekle
```
Hierarchy → Sağ tık → Create Empty
→ Adını "SimpleMatchmakingManager" yap
→ Add Component → Simple Matchmaking Manager (script)
```

**Inspector Ayarları:**
- Matchmaking Timeout: `60`
- Search Interval: `2`
- ELO Tolerance: `200`
- Level Tolerance: `10`

#### 6. Loading Screen (Opsiyonel)
```
Hierarchy → UI → Canvas
→ Adını "LoadingCanvas" yap
→ Canvas altına Text (TMP) ekle: "Loading..."
```

#### 7. Scene Geçişi Ekle

Yeni bir script oluştur: `BootSceneController.cs`

```csharp
using UnityEngine;
using UnityEngine.SceneManagement;
using WasdBattle.Core;

public class BootSceneController : MonoBehaviour
{
    [SerializeField] private float _minLoadTime = 2f;
    
    private async void Start()
    {
        float startTime = Time.time;
        
        // GameManager'ın başlamasını bekle
        while (GameManager.Instance == null || GameManager.Instance.CurrentPlayerData == null)
        {
            await System.Threading.Tasks.Task.Yield();
        }
        
        // Minimum yükleme süresi
        float elapsed = Time.time - startTime;
        if (elapsed < _minLoadTime)
        {
            await System.Threading.Tasks.Task.Delay((int)((_minLoadTime - elapsed) * 1000));
        }
        
        // Ana menüye geç
        SceneManager.LoadScene("MainMenuScene");
    }
}
```

```
Hierarchy → Create Empty → "BootController"
→ Add Component → BootSceneController
→ Min Load Time: 2
```

### ✅ BootScene Kontrol Listesi
- [ ] GameManager var
- [ ] AudioManager var
- [ ] VFXManager var
- [ ] SimpleMatchmakingManager var
- [ ] BootSceneController var
- [ ] Loading UI var (opsiyonel)

---

## 🏠 Scene 2: MainMenuScene

### Amaç
Oyuncunun karakter seçimi, matchmaking, envanter, shop gibi işlemleri yaptığı ana menü.

### Setup Adımları

#### 1. Yeni Scene Oluştur
```
File → New Scene → Empty
File → Save As → "MainMenuScene"
Konum: Assets/_Project/Scenes/MainMenuScene.unity
```

#### 2. Canvas Oluştur
```
Hierarchy → UI → Canvas
→ Canvas Scaler ayarları:
  - UI Scale Mode: Scale With Screen Size
  - Reference Resolution: 1920x1080
  - Match: 0.5
```

#### 3. Background Ekle
```
Canvas altına:
→ UI → Image → "Background"
→ Anchor: Stretch/Stretch
→ Color: İstediğin renk (örn: #1A1A2E)
```

#### 4. Main Panel
```
Canvas → UI → Panel → "MainPanel"
→ Width: 1600, Height: 900
```

#### 5. Player Info Panel (Üst Sol)
```
MainPanel → UI → Panel → "PlayerInfoPanel"
→ Anchor: Top-Left
→ Width: 400, Height: 150
→ Pos X: 220, Pos Y: -100
```

**İçindekiler:**
```
PlayerInfoPanel altına:
→ Text (TMP) → "UsernameText"
  - Text: "Player Name"
  - Font Size: 32
  - Alignment: Center
  
→ Text (TMP) → "LevelText"
  - Text: "Level: 1"
  - Font Size: 24
  
→ Text (TMP) → "ELOText"
  - Text: "ELO: 1000"
  - Font Size: 24
```

#### 6. Play Button (Ortada Büyük)
```
MainPanel → UI → Button (TMP) → "PlayButton"
→ Anchor: Center
→ Width: 400, Height: 100
→ Text: "PLAY"
→ Font Size: 48
→ Color: Yeşil (#4CAF50)
```

#### 7. Alt Butonlar
```
MainPanel → UI → Panel → "BottomButtonsPanel"
→ Anchor: Bottom-Center
→ Width: 1200, Height: 100
→ Add Component → Horizontal Layout Group
  - Spacing: 20
  - Child Alignment: Middle Center
```

**Butonlar:**
```
BottomButtonsPanel altına:
→ Button (TMP) → "CharacterButton" (Text: "Characters")
→ Button (TMP) → "InventoryButton" (Text: "Inventory")
→ Button (TMP) → "ShopButton" (Text: "Shop")
→ Button (TMP) → "SettingsButton" (Text: "Settings")
```

#### 8. MainMenuUI Script Ekle
```
Canvas → Add Component → Main Menu UI
```

**Inspector'da Referansları Bağla:**
- Username Text → UsernameText
- Level Text → LevelText
- ELO Text → ELOText
- Play Button → PlayButton
- Character Button → CharacterButton
- Inventory Button → InventoryButton
- Shop Button → ShopButton

#### 9. Character Select Panel (Başta Kapalı)
```
Canvas → UI → Panel → "CharacterSelectPanel"
→ Active: FALSE (başta kapalı)
→ Width: 1400, Height: 800
```

**İçindekiler:**
```
CharacterSelectPanel altına:
→ Text (TMP) → "Title" (Text: "Select Character")
→ Scroll View → "CharacterScrollView"
  - Content'e Grid Layout Group ekle
  - Cell Size: 300x400
  - Spacing: 20
→ Button (TMP) → "CloseButton" (Text: "X")
```

```
CharacterSelectPanel → Add Component → Character Select UI
```

#### 10. Inventory Panel (Başta Kapalı)
```
Canvas → UI → Panel → "InventoryPanel"
→ Active: FALSE
→ Width: 1400, Height: 800
```

**İçindekiler:**
```
InventoryPanel altına:
→ Text (TMP) → "Title" (Text: "Inventory")
→ Text (TMP) → "GoldText" (Text: "Gold: 0")
→ Text (TMP) → "MetalText" (Text: "Metal: 0")
→ Scroll View → "ItemScrollView"
→ Button (TMP) → "CloseButton"
```

```
InventoryPanel → Add Component → Inventory UI
```

#### 11. Shop Panel (Başta Kapalı)
```
Canvas → UI → Panel → "ShopPanel"
→ Active: FALSE
→ Width: 1400, Height: 800
```

**İçindekiler:**
```
ShopPanel altına:
→ Text (TMP) → "Title" (Text: "Shop")
→ Scroll View → "ShopScrollView"
→ Button (TMP) → "CloseButton"
```

```
ShopPanel → Add Component → Shop UI
```

### ✅ MainMenuScene Kontrol Listesi
- [ ] Canvas var
- [ ] Background var
- [ ] Player Info Panel var
- [ ] Play Button var
- [ ] Alt butonlar var
- [ ] MainMenuUI script bağlı
- [ ] Character Select Panel var (kapalı)
- [ ] Inventory Panel var (kapalı)
- [ ] Shop Panel var (kapalı)

---

## ⚔️ Scene 3: CombatScene

### Amaç
1v1 dövüş sahnesi. İki oyuncu karşı karşıya gelir.

### Setup Adımları

#### 1. Yeni Scene Oluştur
```
File → New Scene → Empty
File → Save As → "CombatScene"
Konum: Assets/_Project/Scenes/CombatScene.unity
```

#### 2. Kamera
```
Hierarchy → Camera → Main Camera
→ Position: (0, 5, -10)
→ Rotation: (20, 0, 0)
→ Background: Solid Color (#0A0A0A)
```

#### 3. Işık
```
Hierarchy → Light → Directional Light
→ Rotation: (50, -30, 0)
→ Intensity: 1
```

#### 4. Arena (Zemin)
```
Hierarchy → 3D Object → Plane → "Arena"
→ Scale: (2, 1, 2)
→ Material: Istediğin renk
```

#### 5. Player 1 Spawn Point
```
Hierarchy → Create Empty → "Player1SpawnPoint"
→ Position: (-3, 1, 0)
→ Add Component → Gizmos (opsiyonel, görselleştirme için)
```

#### 6. Player 2 Spawn Point
```
Hierarchy → Create Empty → "Player2SpawnPoint"
→ Position: (3, 1, 0)
→ Rotation: (0, 180, 0)
```

#### 7. Combat UI Canvas
```
Hierarchy → UI → Canvas → "CombatCanvas"
→ Render Mode: Screen Space - Overlay
```

#### 8. Player 1 Health Bar (Üst Sol)
```
CombatCanvas → UI → Panel → "Player1HealthPanel"
→ Anchor: Top-Left
→ Width: 400, Height: 60
→ Pos X: 220, Pos Y: -50
```

**İçindekiler:**
```
Player1HealthPanel altına:
→ Image → "HealthBarBackground" (Color: Kırmızı koyu)
  - Width: 380, Height: 30
→ Image → "HealthBarFill" (Color: Kırmızı)
  - Image Type: Filled
  - Fill Method: Horizontal
  - Fill Amount: 1
→ Text (TMP) → "HealthText" (Text: "100 / 100")
```

```
Player1HealthPanel → Add Component → Health Bar
→ Fill Image: HealthBarFill
→ Health Text: HealthText
```

#### 9. Player 2 Health Bar (Üst Sağ)
```
CombatCanvas → UI → Panel → "Player2HealthPanel"
→ Anchor: Top-Right
→ Width: 400, Height: 60
→ Pos X: -220, Pos Y: -50
```

*Aynı içerik Player 1 gibi*

#### 10. Player 1 Stamina Bar
```
CombatCanvas → UI → Panel → "Player1StaminaPanel"
→ Anchor: Top-Left
→ Width: 400, Height: 40
→ Pos X: 220, Pos Y: -120
```

**İçindekiler:**
```
→ Image → "StaminaBarBackground" (Color: Mavi koyu)
→ Image → "StaminaBarFill" (Color: Mavi)
→ Text (TMP) → "StaminaText"
```

```
Player1StaminaPanel → Add Component → Stamina Bar
```

#### 11. Player 2 Stamina Bar (Aynı şekilde sağda)

#### 12. Combo Display (Ortada)
```
CombatCanvas → UI → Panel → "ComboDisplayPanel"
→ Anchor: Top-Center
→ Width: 600, Height: 150
→ Pos Y: -100
```

**İçindekiler:**
```
ComboDisplayPanel altına:
→ Text (TMP) → "ComboText" (Text: "Press WASD")
  - Font Size: 48
  - Alignment: Center
→ Text (TMP) → "TimerText" (Text: "3.0s")
  - Font Size: 32
```

```
ComboDisplayPanel → Add Component → Combo Display
```

#### 13. Skill Bar (Alt Ortada)
```
CombatCanvas → UI → Panel → "SkillBarPanel"
→ Anchor: Bottom-Center
→ Width: 800, Height: 100
→ Pos Y: 80
→ Add Component → Horizontal Layout Group
```

**İçindekiler:**
```
SkillBarPanel altına 3 skill slot:
→ Button (TMP) → "Skill1Button"
  - Width: 80, Height: 80
  - Text: "Q"
→ Button (TMP) → "Skill2Button"
  - Text: "E"
→ Button (TMP) → "Skill3Button"
  - Text: "R"
```

```
SkillBarPanel → Add Component → Skill Bar
```

#### 14. Round Info (Ortada Üstte)
```
CombatCanvas → UI → Text (TMP) → "RoundText"
→ Anchor: Top-Center
→ Pos Y: -30
→ Text: "Round 1"
→ Font Size: 36
→ Alignment: Center
```

#### 15. Combat Manager
```
Hierarchy → Create Empty → "CombatManager"
→ Add Component → Combat Manager
```

**Inspector Ayarları:**
- Player 1 Spawn: Player1SpawnPoint
- Player 2 Spawn: Player2SpawnPoint
- Round Text: RoundText

#### 16. Network Manager (Multiplayer için)
```
Hierarchy → Create Empty → "NetworkManager"
→ Add Component → Wasd Network Manager
```

**Inspector Ayarları:**
- Transport: Unity Transport
- Player Prefab: (Henüz yok, sonra eklenecek)

### ✅ CombatScene Kontrol Listesi
- [ ] Kamera var
- [ ] Işık var
- [ ] Arena (zemin) var
- [ ] Spawn point'ler var
- [ ] Player 1 Health Bar var
- [ ] Player 2 Health Bar var
- [ ] Player 1 Stamina Bar var
- [ ] Player 2 Stamina Bar var
- [ ] Combo Display var
- [ ] Skill Bar var
- [ ] Round Text var
- [ ] Combat Manager var
- [ ] Network Manager var

---

## 🎮 Scene 4: LobbyScene (Opsiyonel)

### Amaç
Matchmaking sırasında bekleme ekranı.

### Setup Adımları

#### 1. Yeni Scene Oluştur
```
File → New Scene → Empty
File → Save As → "LobbyScene"
```

#### 2. Canvas
```
Hierarchy → UI → Canvas
```

#### 3. Background
```
Canvas → UI → Image → "Background"
→ Color: Koyu bir renk
```

#### 4. Searching Text
```
Canvas → UI → Text (TMP) → "SearchingText"
→ Text: "Searching for opponent..."
→ Font Size: 48
→ Alignment: Center
→ Add Component → Animator (opsiyonel, animasyon için)
```

#### 5. ELO Range Text
```
Canvas → UI → Text (TMP) → "ELORangeText"
→ Text: "ELO Range: 800 - 1200"
→ Font Size: 24
```

#### 6. Timer Text
```
Canvas → UI → Text (TMP) → "TimerText"
→ Text: "00:15"
→ Font Size: 32
```

#### 7. Cancel Button
```
Canvas → UI → Button (TMP) → "CancelButton"
→ Anchor: Bottom-Center
→ Width: 300, Height: 80
→ Text: "Cancel"
```

#### 8. Lobby Controller Script
```csharp
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using WasdBattle.Matchmaking;
using UnityEngine.SceneManagement;

public class LobbyController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _searchingText;
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private Button _cancelButton;
    
    private float _startTime;
    
    private void Start()
    {
        _startTime = Time.time;
        _cancelButton.onClick.AddListener(OnCancelClicked);
        
        // Matchmaking event'lerine abone ol
        SimpleMatchmakingManager.Instance.OnMatchFound += OnMatchFound;
        SimpleMatchmakingManager.Instance.OnMatchmakingFailed += OnMatchFailed;
    }
    
    private void Update()
    {
        float elapsed = Time.time - _startTime;
        int minutes = (int)(elapsed / 60);
        int seconds = (int)(elapsed % 60);
        _timerText.text = $"{minutes:00}:{seconds:00}";
    }
    
    private void OnMatchFound(MatchmakingResult result)
    {
        Debug.Log("[Lobby] Match found! Loading combat scene...");
        SceneManager.LoadScene("CombatScene");
    }
    
    private void OnMatchFailed()
    {
        Debug.Log("[Lobby] Match failed, returning to main menu");
        SceneManager.LoadScene("MainMenuScene");
    }
    
    private void OnCancelClicked()
    {
        SimpleMatchmakingManager.Instance.CancelMatchmaking();
        SceneManager.LoadScene("MainMenuScene");
    }
    
    private void OnDestroy()
    {
        if (SimpleMatchmakingManager.Instance != null)
        {
            SimpleMatchmakingManager.Instance.OnMatchFound -= OnMatchFound;
            SimpleMatchmakingManager.Instance.OnMatchmakingFailed -= OnMatchFailed;
        }
    }
}
```

```
Hierarchy → Create Empty → "LobbyController"
→ Add Component → LobbyController
→ Referansları bağla
```

### ✅ LobbyScene Kontrol Listesi
- [ ] Canvas var
- [ ] Searching Text var
- [ ] Timer Text var
- [ ] Cancel Button var
- [ ] LobbyController var

---

## 🔗 Build Settings

Scene'leri Build Settings'e ekle:

```
File → Build Settings
→ Add Open Scenes (her scene'i aç ve ekle)

Sıralama:
0. BootScene
1. MainMenuScene
2. LobbyScene (opsiyonel)
3. CombatScene
```

---

## 🎯 Test Etme

### 1. BootScene'den Başlat
```
BootScene'i aç
Play'e bas
```

**Beklenen:**
```
Console:
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

→ 2 saniye sonra MainMenuScene'e geçmeli
```

### 2. MainMenu Test
```
MainMenuScene'de:
- Oyuncu adı, level, ELO görünmeli
- Play butonuna tıkla
- LobbyScene'e geçmeli (veya direkt matchmaking başlamalı)
```

### 3. Matchmaking Test
```
LobbyScene'de:
- "Searching..." yazısı görünmeli
- Timer saymalı
- Birkaç saniye sonra "Match found!" mesajı
- CombatScene'e geçmeli
```

### 4. Combat Test
```
CombatScene'de:
- Health/Stamina barları görünmeli
- Combo display görünmeli
- Skill bar görünmeli
```

---

## 🐛 Sık Karşılaşılan Sorunlar

### S: "Scene 'MainMenuScene' couldn't be loaded"
**C:** Build Settings'e scene'i ekle

### S: "NullReferenceException" UI script'lerinde
**C:** Inspector'da tüm referansları bağladığından emin ol

### S: GameManager başlamıyor
**C:** BootScene'den başladığından emin ol, direkt MainMenu'den başlama

### S: Matchmaking çalışmıyor
**C:** SimpleMatchmakingManager BootScene'de mi? DontDestroyOnLoad mu?

---

## ✅ Final Kontrol Listesi

- [ ] BootScene kuruldu ve çalışıyor
- [ ] MainMenuScene kuruldu ve UI bağlı
- [ ] CombatScene kuruldu ve spawn point'ler var
- [ ] LobbyScene kuruldu (opsiyonel)
- [ ] Tüm scene'ler Build Settings'te
- [ ] Scene geçişleri çalışıyor
- [ ] Console'da hata yok
- [ ] Unity Dashboard'da Project ID bağlı
- [ ] Authentication ve Cloud Save aktif

---

## 🚀 Sonraki Adımlar

1. **Player Prefab Oluştur**
   - 3D model veya capsule
   - PlayerCharacter script ekle
   - NetworkObject ekle

2. **Skill Prefab'ları Oluştur**
   - VFX effect'ler
   - Ses efektleri

3. **Character ScriptableObject'leri Oluştur**
   - Editor tool kullan
   - `WasdBattle → Create Default Characters`

4. **UI Animasyonları Ekle**
   - Button hover effect'leri
   - Panel açılma/kapanma animasyonları

5. **Gerçek Multiplayer Test**
   - 2 build yap
   - Aynı anda çalıştır
   - Matchmaking test et

---

Artık tüm scene'ler hazır! 🎉

**Sorun mu var?** Console loglarını kontrol et ve QUICK_START_GUIDE.md'ye bak.

