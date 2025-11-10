# 🔥 Salvage (Item Eritme) Sistemi

## 📋 Genel Bakış

Salvage sistemi, oyuncuların istedikleri itemleri eritip crafting materyallerine dönüştürmelerini sağlar. Sistem **tamamen otomatik** çalışır - sadece crafting materyallerini ve salvage oranını ayarlamanız yeterli!

---

## 🎯 Özellikler

### ✅ Otomatik Hesaplama
- Crafting materyallerinden otomatik salvage materyalleri hesaplanır
- Manuel olarak salvage materyalleri girmenize gerek yok
- Sadece salvage oranını (%) ayarlarsınız

### ✅ Inspector Preview
- ItemData Inspector'da salvage preview otomatik gösterilir
- Crafting vs Salvage karşılaştırması görürsünüz
- Hangi materyallerin ne kadar geri döneceğini görebilirsiniz

### ✅ Güvenli İşlem
- Equipped itemler salvage edilemez
- Inventory'de yeterli item kontrolü yapılır
- Tüm işlemler otomatik kaydedilir

---

## 🛠️ ItemData Kurulumu

### 1. Crafting Materials Ekle

ItemData'nızı açın ve Crafting bölümüne materyalleri ekleyin:

```
[Header("Crafting")]
☑ Can Be Crafted
Crafting Materials:
  - Material Type: Metal, Amount: 100
  - Material Type: Energy Crystal, Amount: 50
  - Material Type: Rune, Amount: 10
```

### 2. Salvage Ayarları

Salvage bölümünde sadece oranı ayarlayın:

```
[Header("Salvage (Item Eritme)")]
☑ Can Be Salvaged
Salvage Return Rate: 0.5  (slider: 0-1)
```

**Salvage Return Rate Örnekleri:**
- `0.25` = %25 geri dönüş (düşük)
- `0.50` = %50 geri dönüş (orta - varsayılan)
- `0.75` = %75 geri dönüş (yüksek)
- `1.00` = %100 geri dönüş (tam geri dönüş)

### 3. Otomatik Preview

Inspector'da otomatik olarak şu bilgileri göreceksiniz:

```
┌─────────────────────────────────────────┐
│ Salvage Preview                         │
│ Bu item eritildiğinde şu materyaller    │
│ geri dönecek: (Crafting maliyetinin %50)│
│                                         │
│ • Metal: 50                             │
│ • EnergyCrystal: 25                     │
│ • Rune: 5                               │
│                                         │
│ Crafting Cost vs Salvage Return:        │
│ Metal: 100 → 50                         │
│ EnergyCrystal: 50 → 25                  │
│ Rune: 10 → 5                            │
└─────────────────────────────────────────┘
```

---

## 💻 Kod Kullanımı

### SalvageManager Singleton

```csharp
using WasdBattle.Managers;

// Tek item salvage
bool success = SalvageManager.Instance.SalvageItem(itemData, 1);

// Birden fazla item salvage
bool success = SalvageManager.Instance.SalvageItem(itemData, 5);
```

### Salvage Kontrolü

```csharp
// Item salvage edilebilir mi?
bool canSalvage = SalvageManager.Instance.CanSalvageItem(itemData, 1);

if (canSalvage)
{
    // Salvage işlemini yap
    SalvageManager.Instance.SalvageItem(itemData, 1);
}
```

### Preview Alma

```csharp
// Salvage preview string al (UI için)
string preview = SalvageManager.Instance.GetSalvagePreview(itemData, 3);
Debug.Log(preview);

// Çıktı:
// Salvaging Iron Sword x3 will give:
// • Metal: 150
// • EnergyCrystal: 75
// • Rune: 15
```

### ItemData'dan Direkt Erişim

```csharp
// Salvage materyallerini al
CraftingMaterial[] salvageMats = itemData.GetSalvageMaterials();

foreach (var mat in salvageMats)
{
    Debug.Log($"{mat.materialType}: {mat.amount}");
}

// Salvage özeti al
string summary = itemData.GetSalvageRewardSummary();
Debug.Log(summary);
```

---

## 🎮 Oyun İçi Kullanım

### Salvage İşlemi Akışı

1. **Oyuncu item seçer** (Inventory'den)
2. **Salvage butonuna basar**
3. **Confirmation popup açılır:**
   ```
   Are you sure you want to salvage:
   Iron Sword x3
   
   You will receive:
   • Metal: 150
   • Energy Crystal: 75
   • Rune: 15
   
   [Cancel] [Confirm]
   ```
4. **Confirm ederse:**
   - Item inventory'den kaldırılır
   - Materyaller PlayerData'ya eklenir
   - UI güncellenir
   - Veriler kaydedilir

### UI Button Örneği

```csharp
public void OnSalvageButtonClicked()
{
    if (selectedItem == null) return;
    
    // Preview göster
    string preview = SalvageManager.Instance.GetSalvagePreview(selectedItem, 1);
    
    // Confirmation popup aç
    ConfirmationPopup.Show(
        $"Salvage {selectedItem.itemName}?",
        preview,
        onConfirm: () => {
            bool success = SalvageManager.Instance.SalvageItem(selectedItem, 1);
            if (success)
            {
                RefreshInventoryUI();
                ShowSuccessMessage("Item salvaged successfully!");
            }
        }
    );
}
```

---

## 🔒 Salvage Kuralları

### ✅ İzin Verilen Durumlar
- Item `canBeSalvaged = true` olmalı
- Item inventory'de olmalı (equipped değil)
- Crafting materials tanımlanmış olmalı
- Yeterli sayıda item olmalı

### ❌ İzin Verilmeyen Durumlar
- Equipped itemler salvage edilemez
- `canBeSalvaged = false` itemler
- Crafting materials olmayan itemler
- Inventory'de olmayan itemler

### Equipped Item Salvage Denemesi

```csharp
// Equipped item salvage edilemez
bool success = SalvageManager.Instance.SalvageItem(equippedItem, 1);
// success = false
// Console: "Not enough [ItemName] in inventory!"
```

**Çözüm:** Önce item'ı unequip edin, sonra salvage edin.

---

## 📊 Örnek Senaryolar

### Senaryo 1: Tek Item Salvage

**Item:** Iron Sword
- **Crafting Cost:** Metal 100, Energy Crystal 50
- **Salvage Rate:** 0.5 (50%)
- **Salvage Return:** Metal 50, Energy Crystal 25

```csharp
SalvageManager.Instance.SalvageItem(ironSword, 1);
// Oyuncu kazanır: Metal +50, Energy Crystal +25
```

### Senaryo 2: Toplu Salvage

**Item:** Simple Ring x10
- **Crafting Cost:** Metal 20, Rune 5
- **Salvage Rate:** 0.75 (75%)
- **Salvage Return (tek):** Metal 15, Rune 3

```csharp
SalvageManager.Instance.SalvageItem(simpleRing, 10);
// Oyuncu kazanır: Metal +150, Rune +30
```

### Senaryo 3: Yüksek Rarity Item

**Item:** Legendary Sword
- **Crafting Cost:** Metal 500, Energy Crystal 200, Dark Essence 50
- **Salvage Rate:** 0.25 (25% - legendary itemler düşük oran)
- **Salvage Return:** Metal 125, Energy Crystal 50, Dark Essence 12

```csharp
SalvageManager.Instance.SalvageItem(legendarySword, 1);
// Oyuncu kazanır: Metal +125, Energy Crystal +50, Dark Essence +12
```

---

## 🎨 Rarity'ye Göre Salvage Oranları (Öneri)

Farklı rarity'lere farklı salvage oranları atayabilirsiniz:

| Rarity    | Salvage Rate | Açıklama                          |
|-----------|--------------|-----------------------------------|
| Common    | 0.50 (50%)   | Standart geri dönüş               |
| Uncommon  | 0.45 (45%)   | Biraz daha düşük                  |
| Rare      | 0.40 (40%)   | Daha değerli, daha az geri dönüş  |
| Epic      | 0.30 (30%)   | Çok değerli                       |
| Legendary | 0.25 (25%)   | En değerli, en düşük geri dönüş   |

**Mantık:** Daha değerli itemler daha düşük salvage oranına sahip olmalı ki oyuncular onları eritmeyi düşünsün.

---

## 🧪 Test Etme

### Unity Editor'da Test

1. **ItemData Oluştur:**
   - `Assets/Resources/Items/test_salvage_item.asset`
   - Crafting materials ekle
   - Salvage rate ayarla

2. **Inspector'da Kontrol:**
   - Salvage preview görünüyor mu?
   - Hesaplamalar doğru mu?

3. **Play Mode'da Test:**
   ```csharp
   // Debug menüden item ekle
   playerData.AddItem("test_salvage_item", 5);
   
   // Salvage et
   SalvageManager.Instance.SalvageItem(testItem, 3);
   
   // Materyalleri kontrol et
   Debug.Log($"Metal: {playerData.metal}");
   ```

### Debug Komutları

```csharp
// Tüm salvage bilgilerini logla
[MenuItem("Debug/Log Salvage Info")]
static void LogSalvageInfo()
{
    var items = Resources.LoadAll<ItemData>("Items");
    foreach (var item in items)
    {
        if (item.canBeSalvaged)
        {
            Debug.Log($"{item.itemName}:");
            Debug.Log(item.GetSalvageRewardSummary());
        }
    }
}
```

---

## 🚀 Gelecek Geliştirmeler

### Potansiyel Özellikler

1. **Salvage Bonus:**
   - Oyuncu seviyesine göre bonus materyal
   - Özel event'lerde 2x salvage
   - Skill tree'de salvage bonus

2. **Salvage Animation:**
   - Item eritme animasyonu
   - Materyal kazanma particle effect'i
   - Sound effect'ler

3. **Bulk Salvage:**
   - Tüm common itemleri salvage et
   - Rarity'ye göre toplu salvage
   - Filter'a göre toplu salvage

4. **Salvage History:**
   - Son salvage edilen itemler
   - Toplam kazanılan materyaller
   - İstatistikler

---

## 📝 Notlar

- **Salvage Rate** her item için ayrı ayrı ayarlanabilir
- **Crafting Materials** olmayan itemler salvage edilemez
- **Equipped Items** otomatik olarak korunur (salvage edilemez)
- **Tüm işlemler** otomatik olarak kaydedilir
- **Preview** her zaman güncel ve doğrudur

---

## ✅ Checklist

Salvage sistemi kurulumu için:

- [ ] `SalvageManager` prefab'ı scene'e ekle
- [ ] ItemData'lara crafting materials ekle
- [ ] Salvage rate'leri ayarla (rarity'ye göre)
- [ ] Inspector'da preview'ları kontrol et
- [ ] UI'da salvage butonu ekle
- [ ] Confirmation popup oluştur
- [ ] Test et (play mode)
- [ ] Debug menüden test et
- [ ] Equipped item kontrolünü test et
- [ ] Save/Load sistemini test et

---

## 🎉 Tamamlandı!

Salvage sistemi artık hazır! Oyuncular itemlerini eritip crafting materyallerine dönüştürebilirler. Sistem tamamen otomatik çalışır ve güvenlidir.

**Önemli:** Salvage UI'ı henüz eklenmedi. Inventory paneline salvage butonu ve confirmation popup eklemeniz gerekecek.

