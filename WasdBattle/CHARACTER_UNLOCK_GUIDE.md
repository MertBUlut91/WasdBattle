# 🎮 Character Unlock System - Rehber

## 📋 Karakter Listesi

### ✅ Starter Characters (Ücretsiz)
Oyun başında otomatik açık:

1. **Alev Büyücüsü (Mage)**
   - HP: 80, Stamina: 80
   - Defense: 10%
   - Özellik: Yüksek hasar, düşük dayanıklılık
   - Renk: Turuncu-kırmızı

2. **Kalkan Savaşçısı (Warrior)**
   - HP: 150, Stamina: 120
   - Defense: 30%
   - Özellik: Yüksek savunma, yavaş saldırı
   - Renk: Mavi

3. **Ninja**
   - HP: 100, Stamina: 100
   - Defense: 15%
   - Özellik: Hızlı, çevik, kısa combo
   - Renk: Mor

---

### 🔒 Unlockable Characters

#### 4. **Suikastçi (Assassin)**
**Unlock Requirements:**
- Level: 5
- Gold: 500

**Stats:**
- HP: 90, Stamina: 90
- Defense: 12%
- Özellik: Kritik vuruş uzmanı
- Renk: Koyu gri

---

#### 5. **Paladin**
**Unlock Requirements (2 seçenek):**
- Level: 10 + Gold: 1000
- **VEYA**
- Level: 10 + Gem: 200

**Stats:**
- HP: 130, Stamina: 110
- Defense: 25%
- Özellik: Dengeli savaşçı, heal yetenekleri
- Renk: Altın sarısı

---

#### 6. **Okçu (Ranger)**
**Unlock Requirements:**
- Level: 15
- Gold: 1500

**Stats:**
- HP: 95, Stamina: 105
- Defense: 18%
- Özellik: Uzak mesafe, hızlı
- Renk: Yeşil

---

## 🛠️ Editor Tool Kullanımı

### Starter Karakterleri Oluştur
```
Unity Editor:
WasdBattle → Create Default Characters
→ 3 starter karakter oluşturulur (Mage, Warrior, Ninja)
→ Assets/_Project/ScriptableObjects/Characters/
```

### Unlockable Karakterleri Oluştur
```
Unity Editor:
WasdBattle → Create Unlockable Characters
→ 3 unlockable karakter oluşturulur (Assassin, Paladin, Ranger)
→ Assets/_Project/ScriptableObjects/Characters/
```

---

## 💻 Kod Kullanımı

### Karakter Unlock Edilebilir mi?
```csharp
CharacterData character = ...; // Load from ScriptableObject
PlayerData playerData = GameManager.Instance.CurrentPlayerData;

// Unlock edilebilir mi kontrol et
if (character.CanUnlock(playerData))
{
    Debug.Log("Bu karakteri unlock edebilirsin!");
}
else
{
    Debug.Log($"Gereksinimler: Level {character.requiredLevel}");
}
```

### Karakter Unlock Et
```csharp
CharacterData character = ...; // Assassin, Paladin, veya Ranger

// Unlock edilebilir mi?
if (character.CanUnlock(playerData))
{
    // Uygun fiyatı seç (ilk karşılayabildiği)
    ShopPrice selectedPrice = null;
    foreach (var price in character.unlockPrices)
    {
        if (playerData.HasCurrency(price.currencyType, price.amount))
        {
            selectedPrice = price;
            break;
        }
    }
    
    if (selectedPrice != null)
    {
        // Currency'yi harca
        playerData.ModifyCurrency(selectedPrice.currencyType, -selectedPrice.amount);
        
        // Karakteri ekle
        playerData.ownedCharacters.Add(character.characterId);
        
        // Default loadout oluştur
        CharacterLoadout loadout = new CharacterLoadout(character.characterId);
        playerData.characterLoadouts.Add(loadout);
        
        // Cloud Save'e kaydet
        GameManager.Instance.SavePlayerData();
        
        Debug.Log($"[CharacterUnlock] {character.characterName} unlocked!");
    }
}
```

---

## 🎨 UI Örneği

### Character Select UI'de Gösterim
```csharp
// Karakter listesini göster
foreach (var characterId in allCharacterIds)
{
    CharacterData character = LoadCharacterData(characterId);
    
    // Sahip mi?
    bool owned = playerData.ownedCharacters.Contains(characterId);
    
    if (owned)
    {
        // Karakter seçilebilir
        ShowCharacterButton(character, selectable: true);
    }
    else
    {
        // Locked göster
        if (character.CanUnlock(playerData))
        {
            // Unlock edilebilir (yeşil)
            ShowLockedCharacter(character, canUnlock: true);
        }
        else
        {
            // Unlock edilemez (kırmızı)
            ShowLockedCharacter(character, canUnlock: false);
            
            // Gereksinimleri göster
            string requirements = $"Level {character.requiredLevel}\n";
            foreach (var price in character.unlockPrices)
            {
                requirements += $"{price.currencyType}: {price.amount}\n";
            }
            ShowRequirements(requirements);
        }
    }
}
```

---

## 📊 Unlock Stratejisi

### Oyuncu İlerlemesi
```
Level 1-4:  Starter karakterlerle oyna
            → Gold biriktir

Level 5:    Assassin unlock et (500 Gold)
            → Kritik vuruş avantajı

Level 10:   Paladin unlock et
            → Seçenek 1: 1000 Gold
            → Seçenek 2: 200 Gem (premium)

Level 15:   Ranger unlock et (1500 Gold)
            → Uzak mesafe avantajı
```

### Gold Kazanma
- Her maç: 50-100 Gold (kazanırsa 2x)
- ELO bonus: +10 Gold per 100 ELO
- Daily rewards (ileride)
- Achievements (ileride)

### Gem Kazanma
- İlk giriş: 50 Gem
- Level up: 10 Gem
- Daily login: 5 Gem
- Satın alma (ileride)

---

## 🔮 Gelecek Karakterler

Kolayca yeni karakterler eklenebilir:

```csharp
private static void CreateDragonKnight(string folderPath)
{
    CharacterData dragon = ScriptableObject.CreateInstance<CharacterData>();
    dragon.characterId = "char_dragon_knight";
    dragon.characterName = "Ejderha Şövalyesi";
    dragon.characterClass = CharacterClass.Paladin; // Yeni class eklenebilir
    
    // Unlock Requirements
    dragon.isStarterCharacter = false;
    dragon.requiresUnlock = true;
    dragon.requiredLevel = 20;
    dragon.unlockPrices = new ShopPrice[]
    {
        new ShopPrice(CurrencyType.Gold, 3000),
        new ShopPrice(CurrencyType.Diamond, 100) // Özel premium currency
    };
    
    // Stats...
    
    AssetDatabase.CreateAsset(dragon, $"{folderPath}/DragonKnight.asset");
}
```

---

## 💡 İpuçları

### 1. Multiple Currency Options
Paladin gibi birden fazla unlock seçeneği sunabilirsin:
```csharp
character.unlockPrices = new ShopPrice[]
{
    new ShopPrice(CurrencyType.Gold, 1000),  // Seçenek 1
    new ShopPrice(CurrencyType.Gem, 200),    // Seçenek 2
    new ShopPrice(CurrencyType.Diamond, 50)  // Seçenek 3
};
```

### 2. Level Gating
Bazı karakterler sadece level ile kilitlenebilir:
```csharp
character.requiredLevel = 20;
character.unlockPrices = new ShopPrice[0]; // Boş = sadece level
```

### 3. Free Unlock Events
Event'lerde ücretsiz unlock:
```csharp
// Event sırasında
if (isEventActive)
{
    character.requiresUnlock = false; // Geçici olarak ücretsiz
}
```

---

## 🎉 Özet

✅ **3 Starter Karakter** - Ücretsiz (Mage, Warrior, Ninja)
✅ **3 Unlockable Karakter** - Level + Currency (Assassin, Paladin, Ranger)
✅ **Esnek Unlock Sistemi** - Birden fazla currency seçeneği
✅ **Editor Tool** - Kolay karakter oluşturma
✅ **Genişletilebilir** - Yeni karakterler kolayca eklenebilir

---

**Karakterleri oluştur ve test et!** 🚀

```
Unity Editor → WasdBattle → Create Default Characters
Unity Editor → WasdBattle → Create Unlockable Characters
```

