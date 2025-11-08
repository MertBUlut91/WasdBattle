# WasdBattle - Code Structure

## 📁 Klasör Yapısı

### Core/
Oyunun temel sistemleri:
- `GameManager.cs` - Oyun durumu ve servis yönetimi
- `WasdNetworkManager.cs` - Network yönetimi
- `DataManager.cs` - Veri persistance
- `AudioManager.cs` - Ses yönetimi
- `VFXManager.cs` - Efekt yönetimi
- `GameInitializer.cs` - Başlangıç kurulumu
- `GameConstants.cs` - Oyun sabitleri

### Combat/
Dövüş mekaniği:
- `CombatManager.cs` - Tur bazlı dövüş akışı
- `DamageCalculator.cs` - Hasar hesaplama

### Characters/
Karakter sistemleri:
- `PlayerCharacter.cs` - NetworkBehaviour karakter sınıfı

### Skills/
Skill sistemleri:
- `SkillManager.cs` - Skill kullanımı ve cooldown
- `ISkillEffect.cs` - Skill efekt interface'i
- `SkillEffects.cs` - Concrete skill efektleri

### Input/
Combo input sistemi:
- `ComboInputManager.cs` - WASD input yönetimi
- `ComboValidator.cs` - Combo doğrulama

### Matchmaking/
Eşleşme sistemi:
- `MatchmakingManager.cs` - Unity Lobby entegrasyonu

### Progression/
İlerleme sistemleri:
- `LevelSystem.cs` - XP ve level yönetimi
- `ELOSystem.cs` - ELO rating sistemi
- `RewardSystem.cs` - Ödül hesaplama

### Economy/
Ekonomi sistemleri:
- `InventoryManager.cs` - Envanter yönetimi
- `CraftingSystem.cs` - Craft sistemi
- `ShopSystem.cs` - Mağaza sistemi

### UI/
Kullanıcı arayüzü:
- `MainMenuUI.cs` - Ana menü
- `CharacterSelectUI.cs` - Karakter seçimi
- `InventoryUI.cs` - Envanter ekranı
- `ShopUI.cs` - Mağaza ekranı
- `CombatUI.cs` - Dövüş ekranı
- `HealthBar.cs`, `StaminaBar.cs`, `SkillBar.cs` - Combat UI bileşenleri
- `ComboDisplay.cs` - Combo göstergesi

### Network/
Network sistemleri:
- `IFirebaseService.cs` - Firebase interface
- `MockFirebaseService.cs` - Test için mock servis
- `NetworkHelper.cs` - Network yardımcı fonksiyonlar
- `NetworkDebugUI.cs` - Network debug UI

### Data/
ScriptableObject ve veri yapıları:
- `PlayerData.cs` - Oyuncu verisi
- `CharacterData.cs` - Karakter ScriptableObject
- `SkillData.cs` - Skill ScriptableObject
- `ComboData.cs` - Combo ScriptableObject
- `PassiveAbilityData.cs` - Pasif yetenek
- `CraftRecipe.cs` - Craft tarifi

### Editor/
Unity Editor araçları:
- `CharacterCreator.cs` - Varsayılan karakterleri oluşturur
- `SkillCreator.cs` - Varsayılan skill'leri oluşturur

## 🎮 Oyun Akışı

1. **Başlangıç**
   - GameInitializer → GameManager → NetworkManager
   - Firebase/Mock servis başlatılır
   - Oyuncu verisi yüklenir

2. **Ana Menü**
   - Oyuncu bilgileri gösterilir
   - Play butonu → Matchmaking başlar

3. **Matchmaking**
   - Unity Lobby Service ile eşleşme
   - ELO + Level bazlı algoritma

4. **Dövüş**
   - CombatManager tur bazlı akışı yönetir
   - Saldıran → Combo girer → Savunan → Combo girer
   - Hasar hesaplanır ve uygulanır
   - Maç bitişinde ödüller verilir

5. **Maç Sonu**
   - XP ve ELO güncellenir
   - Ödüller verilir
   - Veri kaydedilir

## 🔧 Kurulum

### Unity Editor'da:
1. `WasdBattle/Create Default Characters` - 3 temel karakter oluşturur
2. `WasdBattle/Create Default Skills` - Temel skill'leri oluşturur

### Firebase Kurulumu:
1. Firebase Unity SDK'sını import edin
2. `MockFirebaseService` yerine gerçek Firebase implementasyonu kullanın
3. `GameManager.InitializeServices()` içinde değiştirin

### Network Kurulumu:
1. Scene'e NetworkManager prefab ekleyin
2. Unity Transport yapılandırın
3. Lobby Service'i Unity Dashboard'dan aktif edin

## 📊 Veri Akışı

```
GameManager
    ├── DataManager → Firebase/Mock Service
    ├── WasdNetworkManager → Unity Netcode
    └── PlayerData (Current)
        ├── InventoryManager
        ├── LevelSystem
        └── ELOSystem
```

## 🎯 Önemli Notlar

- Tüm manager'lar Singleton pattern kullanır
- Network senkronizasyonu server-authoritative
- Combo validation client-side ama server verify eder
- Firebase şu an mock, gerçek SDK kurulmalı
- ScriptableObject'ler runtime'da oluşturulabilir (Editor tools ile)

## 🚀 Sonraki Adımlar

1. Firebase SDK entegrasyonu
2. VFX ve SFX asset'leri ekleme
3. Karakter modelleri ve animasyonlar
4. UI prefab'ları oluşturma
5. Scene'leri kurma (MainMenu, Combat, vb.)
6. Network test ve optimizasyon
7. Balance tweaking

