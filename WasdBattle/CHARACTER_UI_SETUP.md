# 🎮 Character UI Setup - Adım Adım Rehber

## ⚠️ Sorun: Karakterler Gözükmüyor

Eğer karakterler gözükmüyorsa, bu rehberi takip et!

---

## 📋 Gerekli Adımlar

### 1. CharacterData'ları Oluştur

```
Unity Editor:
WasdBattle → Create Default Characters
→ 3 starter karakter oluşturulur

WasdBattle → Create Unlockable Characters
→ 3 unlockable karakter oluşturulur

Konum: Assets/_Project/ScriptableObjects/Characters/
```

**Oluşturulan Karakterler:**
- Mage.asset
- Warrior.asset
- Ninja.asset
- Assassin.asset
- Paladin.asset
- Ranger.asset

---

### 2. Resources Klasörüne Taşı (Opsiyonel ama Önerilen)

```
1. Assets/Resources klasörü oluştur (yoksa)
2. Assets/Resources/ScriptableObjects klasörü oluştur
3. Assets/Resources/ScriptableObjects/Characters klasörü oluştur
4. Tüm .asset dosyalarını buraya taşı
```

**VEYA**

Inspector'dan manuel olarak ata (Adım 5'te)

---

### 3. CharacterCardPrefab Oluştur

```
Hierarchy → UI → Panel → "CharacterCardPrefab"

Inspector:
- Width: 300, Height: 400
```

**İçindekiler:**

```
CharacterCardPrefab altına:

1. Image → "CharacterPortrait"
   - Anchor: Top-Center
   - Width: 280, Height: 280
   - Pos Y: -10

2. Text (TMP) → "CharacterName"
   - Anchor: Top-Center
   - Pos Y: -300
   - Font Size: 24
   - Alignment: Center

3. Text (TMP) → "CharacterClass"
   - Anchor: Top-Center
   - Pos Y: -330
   - Font Size: 18
   - Alignment: Center

4. Panel → "StatsPanel"
   - Anchor: Top-Center
   - Width: 260, Height: 80
   - Pos Y: -360
   
   StatsPanel altına:
   - Text (TMP) → "HPText" (Text: "HP: 100")
   - Text (TMP) → "StaminaText" (Text: "Stamina: 100")
   - Text (TMP) → "DefenseText" (Text: "Defense: 10%")

5. Button (TMP) → "SelectButton"
   - Anchor: Bottom-Center
   - Width: 260, Height: 50
   - Pos Y: 10
   - Text: "SELECT"
   - Color: Green

6. Panel → "LockedPanel" (Active: FALSE başta)
   - Anchor: Stretch/Stretch
   - Color: #00000099 (Yarı saydam)
   
   LockedPanel altına:
   - Image → "LockIcon" (Center, 64x64)
   - Text (TMP) → "RequiredLevelText" (Text: "Level 5")
   - Text (TMP) → "PriceText" (Text: "500 Gold")
   - Button (TMP) → "UnlockButton" (Text: "UNLOCK", Color: Yellow)
```

**Script Ekle:**

```
CharacterCardPrefab → Add Component → Character Card UI

Inspector'da referansları bağla:
- Character Portrait: CharacterPortrait
- Character Name: CharacterName
- Character Class: CharacterClass
- HP Text: HPText
- Stamina Text: StaminaText
- Defense Text: DefenseText
- Select Button: SelectButton
- Locked Panel: LockedPanel
- Required Level Text: RequiredLevelText
- Price Text: PriceText
- Unlock Button: UnlockButton
```

**Prefab Yap:**

```
CharacterCardPrefab'ı sürükle → Assets/Prefabs/ klasörüne
Hierarchy'den sil
```

---

### 4. CharacterSelectPanel Setup

```
MainCanvas → UI → Panel → "CharacterSelectPanel"

Inspector:
- Anchor: Stretch/Stretch
- Active: FALSE (başta kapalı)
- Color: #000000CC (Yarı saydam siyah)
```

**İçindekiler:**

```
CharacterSelectPanel altına:

1. Panel → "ContentPanel"
   - Anchor: Center
   - Width: 1600, Height: 900
   - Color: #2D2D2D

2. Button (TMP) → "CloseButton"
   - Anchor: Top-Right
   - Width: 60, Height: 60
   - Pos X: -10, Pos Y: -10
   - Text: "X"
   - Color: Red

3. Text (TMP) → "TitleText"
   - Anchor: Top-Center
   - Pos Y: -30
   - Text: "Character Selection"
   - Font Size: 48

4. Scroll View → "CharacterGridScrollView"
   - Anchor: Stretch/Stretch
   - Offset: Left 20, Right -20, Top -100, Bottom 20
   
   Content → Add Component: Grid Layout Group
   - Cell Size: 300x400
   - Spacing: 20
   - Constraint: Fixed Column Count (3)
   
   Content → Add Component: Content Size Fitter
   - Vertical Fit: Preferred Size
```

---

### 5. CharacterSelectUI Script Bağla

```
CharacterSelectPanel → Add Component → Character Select UI

Inspector'da referansları bağla:

- Character Grid Container: CharacterGridScrollView → Content
- Character Card Prefab: CharacterCardPrefab (Assets/Prefabs/)
- Close Button: CloseButton
- All Characters: (Boş bırakabilirsin, Resources'tan yüklenecek)
```

**VEYA All Characters'ı Manuel Ata:**

```
Inspector'da:
All Characters → Size: 6
Element 0: Mage
Element 1: Warrior
Element 2: Ninja
Element 3: Assassin
Element 4: Paladin
Element 5: Ranger

(Assets/_Project/ScriptableObjects/Characters/ klasöründen sürükle)
```

---

### 6. MainMenuUI'ye Bağla

```
MainCanvas → Main Menu UI (script)

Inspector'da:
- Character Button: CharacterButton
- Character Select Panel: CharacterSelectPanel (yeni eklendi)
```

**MainMenuUI.cs'e Ekle:**

```csharp
[Header("Panels")]
[SerializeField] private GameObject _characterSelectPanel;

private void OnCharacterSelectClicked()
{
    Debug.Log("[MainMenu] Character Select clicked");
    
    if (_characterSelectPanel != null)
    {
        _characterSelectPanel.SetActive(true);
    }
}
```

---

## 🔍 Sorun Giderme

### Sorun 1: "Karakterler gözükmüyor"

**Kontrol Et:**
1. Console'da hata var mı?
2. CharacterData'lar oluşturuldu mu? (Assets/_Project/ScriptableObjects/Characters/)
3. CharacterCardPrefab'da CharacterCardUI script'i var mı?
4. CharacterSelectUI'de referanslar bağlı mı?

**Debug:**
```
Console'da şunları aramalısın:
[CharacterSelectUI] Loading 6 characters...
[CharacterSelectUI] Created card for Mage (Owned: True)
[CharacterSelectUI] Created card for Warrior (Owned: True)
...
```

---

### Sorun 2: "Locked karakterler locked gözükmüyor"

**Kontrol Et:**
1. CharacterCardUI.cs'de `Setup()` metodu çağrılıyor mu?
2. `isOwned` parametresi doğru mu?
3. LockedPanel başta `Active: FALSE` mi?

**Debug:**
```csharp
// CharacterCardUI.cs Setup() metodunda:
Debug.Log($"Setting up {character.characterName}, Owned: {isOwned}");
```

---

### Sorun 3: "Stat'lar, isimler gözükmüyor"

**Kontrol Et:**
1. CharacterData'larda değerler dolu mu?
   - characterName: "Alev Büyücüsü"
   - characterClass: Mage
   - baseHealth: 80
   - baseStamina: 80
   - baseDefense: 0.1

2. CharacterCardUI'de referanslar bağlı mı?
   - _characterName
   - _characterClass
   - _hpText
   - _staminaText
   - _defenseText

**Debug:**
```csharp
// CharacterCardUI.cs Setup() metodunda:
Debug.Log($"Character: {character.characterName}");
Debug.Log($"HP: {character.baseHealth}");
Debug.Log($"Stamina: {character.baseStamina}");
```

---

### Sorun 4: "Resimler gözükmüyor"

**Kontrol Et:**
1. CharacterData'da `characterIcon` atanmış mı?
2. CharacterCardUI'de `_characterPortrait` referansı bağlı mı?

**Geçici Çözüm:**
```
CharacterData → Character Icon: Unity Default Sprite (Knob)
```

---

## ✅ Test Checklist

- [ ] CharacterData'lar oluşturuldu (6 adet)
- [ ] CharacterCardPrefab oluşturuldu
- [ ] CharacterCardUI script bağlandı
- [ ] CharacterSelectPanel oluşturuldu
- [ ] CharacterSelectUI script bağlandı
- [ ] Tüm referanslar bağlandı
- [ ] MainMenuUI'de Character butonu bağlandı
- [ ] Play → Character butonuna tıkla
- [ ] 6 karakter kartı göründü
- [ ] 3 starter karakter "SELECT" butonu var
- [ ] 3 unlockable karakter "LOCKED" göründü
- [ ] Stat'lar doğru göründü
- [ ] İsimler doğru göründü
- [ ] Class'lar doğru göründü

---

## 🎯 Beklenen Sonuç

```
Character Button'a tıklayınca:
┌──────────────────────────────────────────┐
│  Character Selection             [X]     │
├──────────────────────────────────────────┤
│  ┌────────┐  ┌────────┐  ┌────────┐     │
│  │  Mage  │  │Warrior │  │ Ninja  │     │
│  │  [IMG] │  │ [IMG]  │  │ [IMG]  │     │
│  │ HP: 80 │  │HP: 150 │  │HP: 100 │     │
│  │[SELECT]│  │[SELECT]│  │[SELECT]│     │
│  └────────┘  └────────┘  └────────┘     │
│                                          │
│  ┌────────┐  ┌────────┐  ┌────────┐     │
│  │Assassin│  │Paladin │  │ Ranger │     │
│  │  [🔒]  │  │  [🔒]  │  │  [🔒]  │     │
│  │ Lvl 5  │  │ Lvl 10 │  │ Lvl 15 │     │
│  │500 Gold│  │1000 G  │  │1500 G  │     │
│  │[UNLOCK]│  │[UNLOCK]│  │[UNLOCK]│     │
│  └────────┘  └────────┘  └────────┘     │
└──────────────────────────────────────────┘
```

---

**Başarılar!** 🎉

