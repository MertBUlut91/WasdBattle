# ✅ UI Setup Checklist - Hızlı Kontrol Listesi

Bu dosya, UI kurulumunda hangi adımların tamamlandığını takip etmek için kullanılır.

---

## 📋 MainMenuScene - Temel UI

### Canvas Setup
- [ ] Canvas oluşturuldu (Scale With Screen Size: 1920x1080)
- [ ] Background eklendi (#1A1A2E)

### Player Info (Üst Sol)
- [ ] PlayerInfoPanel oluşturuldu
- [ ] UsernameText eklendi
- [ ] LevelText eklendi
- [ ] ELOText eklendi
- [ ] RankText eklendi
- [ ] XP Bar eklendi (Background + Fill)

### Currency Display (Üst Sağ)
- [ ] CurrencyPanel oluşturuldu
- [ ] Gold display (Icon + Text)
- [ ] Gem display (Icon + Text)
- [ ] Diamond display (Icon + Text)

### Find Match Button
- [ ] FindMatchButton oluşturuldu (500x120, ortada)
- [ ] Button Text: "Find Match"

### Matchmaking Panel (Başta Gizli)
- [ ] MatchmakingPanel oluşturuldu (Active: FALSE)
- [ ] MatchmakingTimerText eklendi
- [ ] ELORangeText eklendi
- [ ] CancelMatchButton eklendi

### Bottom Buttons
- [ ] BottomButtonsPanel oluşturuldu
- [ ] CharacterButton eklendi
- [ ] InventoryButton eklendi
- [ ] CraftButton eklendi
- [ ] ShopButton eklendi

### MainMenuUI Script
- [ ] MainMenuUI.cs bağlandı
- [ ] Tüm referanslar Inspector'da bağlandı

---

## 🎮 Character & Equipment Panel

### Character Panel
- [ ] CharacterPanel oluşturuldu (Active: FALSE)
- [ ] ContentPanel eklendi (1600x900)
- [ ] CloseButton eklendi
- [ ] TitleText eklendi

### Character List (Sol)
- [ ] CharacterDisplayArea oluşturuldu
- [ ] CharacterListScrollView eklendi
- [ ] Content → Vertical Layout Group
- [ ] CharacterItemPrefab oluşturuldu

### Equipment Slots (Sağ - Orta)
- [ ] EquipmentArea oluşturuldu
- [ ] EquipmentSlotsPanel eklendi
- [ ] 9 Equipment Slot oluşturuldu:
  - [ ] Helmet Slot
  - [ ] Chest Slot
  - [ ] Gloves Slot
  - [ ] Legs Slot
  - [ ] Weapon Slot
  - [ ] Ring1 Slot
  - [ ] Ring2 Slot
  - [ ] Necklace Slot
  - [ ] Bracelet Slot

### Item List (Sağ - Alt)
- [ ] ItemListPanel oluşturuldu
- [ ] ItemListTitle eklendi
- [ ] FilterDropdown eklendi
- [ ] ItemScrollView eklendi
- [ ] Content → Grid Layout Group
- [ ] ItemCardPrefab oluşturuldu

### EquipmentUI Script
- [ ] EquipmentUI.cs oluşturuldu
- [ ] Script CharacterPanel'e bağlandı
- [ ] Tüm referanslar bağlandı

---

## 🎯 Skill Panel

### Skill Slots
- [ ] SkillPanel oluşturuldu
- [ ] SkillPanelTitle eklendi
- [ ] SkillSlotsPanel eklendi (Horizontal Layout)
- [ ] 5 Skill Slot oluşturuldu:
  - [ ] Skill Slot Q (Active 1)
  - [ ] Skill Slot E (Active 2)
  - [ ] Skill Slot R (Active 3)
  - [ ] Skill Slot P (Passive)
  - [ ] Skill Slot U (Ultimate)

### Available Skills
- [ ] AvailableSkillsScrollView eklendi
- [ ] Content → Horizontal Layout Group
- [ ] SkillCardPrefab oluşturuldu

### SkillUI Script
- [ ] SkillUI.cs oluşturuldu
- [ ] Drag & Drop implementasyonu eklendi
- [ ] Script bağlandı

---

## 👤 Character Selection (Unlock)

### Character Select Panel
- [ ] CharacterSelectPanel oluşturuldu (Active: FALSE)
- [ ] CharacterGridScrollView eklendi
- [ ] Content → Grid Layout Group (3 column)
- [ ] CharacterCardPrefab oluşturuldu
  - [ ] CharacterPortrait
  - [ ] CharacterName
  - [ ] CharacterClass
  - [ ] StatsPanel (HP, Stamina, Defense)
  - [ ] SelectButton (Owned)
  - [ ] LockedPanel (Locked)
  - [ ] UnlockButton

### CharacterCardUI Script
- [ ] CharacterCardUI.cs oluşturuldu
- [ ] Unlock logic implementasyonu
- [ ] Script prefab'a bağlandı

---

## 🎒 Inventory Panel

### Materials Display
- [ ] MaterialsPanel oluşturuldu
- [ ] Grid Layout Group eklendi
- [ ] 10 Material Item oluşturuldu:
  - [ ] Metal
  - [ ] Energy Crystal
  - [ ] Rune
  - [ ] Essence
  - [ ] Leather
  - [ ] Cloth
  - [ ] Wood
  - [ ] Gem Stone
  - [ ] Dark Essence
  - [ ] Light Essence

### InventoryUI Script
- [ ] InventoryUI.cs güncellendi
- [ ] Material display eklendi

---

## 🔨 Craft Panel

### Craft Panel
- [ ] CraftPanel oluşturuldu (Active: FALSE)
- [ ] CraftContentPanel eklendi

### Recipe List (Sol)
- [ ] RecipeListPanel oluşturuldu
- [ ] RecipeScrollView eklendi
- [ ] Content → Vertical Layout Group
- [ ] RecipeItemPrefab oluşturuldu

### Craft Details (Sağ)
- [ ] CraftDetailsPanel oluşturuldu
- [ ] ResultPreview (Image)
- [ ] ResultName (Text)
- [ ] ResultDescription (Text)
- [ ] MaterialRequirementsPanel
- [ ] CraftButton

### CraftUI Script
- [ ] CraftUI.cs oluşturuldu
- [ ] Craft logic implementasyonu
- [ ] Script bağlandı

---

## 🛒 Shop Panel

### Shop Panel
- [ ] ShopPanel oluşturuldu (Active: FALSE)
- [ ] ShopContentPanel eklendi
- [ ] CategoryDropdown eklendi
- [ ] ShopItemScrollView eklendi
- [ ] Content → Grid Layout Group
- [ ] ShopItemCardPrefab oluşturuldu
  - [ ] ItemPreview
  - [ ] ItemName
  - [ ] ItemDescription
  - [ ] PricePanel (Multiple currencies)
  - [ ] BadgesPanel (New, Sale, Featured)
  - [ ] PurchaseButton

### ShopUI Script
- [ ] ShopUI.cs güncellendi
- [ ] Multiple currency support
- [ ] Purchase logic
- [ ] Script bağlandı

---

## 🎨 Prefab'lar

- [ ] CharacterItemPrefab
- [ ] ItemCardPrefab
- [ ] SkillCardPrefab
- [ ] CharacterCardPrefab
- [ ] RecipeItemPrefab
- [ ] ShopItemCardPrefab
- [ ] MaterialItemPrefab

---

## 📜 Script'ler

### Mevcut (Güncellenmeli)
- [ ] MainMenuUI.cs - Matchmaking UI eklendi
- [ ] CharacterSelectUI.cs - Unlock system eklenmeli
- [ ] InventoryUI.cs - Materials display eklenmeli
- [ ] ShopUI.cs - Multiple currencies eklenmeli

### Yeni (Oluşturulacak)
- [ ] EquipmentUI.cs
- [ ] SkillUI.cs
- [ ] CraftUI.cs
- [ ] CharacterCardUI.cs
- [ ] DragDropHandler.cs (Skill drag & drop için)

---

## 🎯 Test Checklist

### MainMenu
- [ ] Player info doğru gösteriliyor
- [ ] Currency'ler doğru gösteriliyor
- [ ] Find Match butonu çalışıyor
- [ ] Matchmaking timer çalışıyor
- [ ] Cancel butonu çalışıyor
- [ ] Bottom butonlar panel açıyor

### Character & Equipment
- [ ] Karakter listesi gösteriliyor
- [ ] Karakter seçimi çalışıyor
- [ ] Equipment slot'lar gösteriliyor
- [ ] Item listesi filtreleniyor
- [ ] Equip/Unequip çalışıyor
- [ ] Cloud Save'e kaydediliyor

### Skills
- [ ] Skill slot'lar gösteriliyor
- [ ] Available skills gösteriliyor
- [ ] Drag & drop çalışıyor
- [ ] Skill değiştirme çalışıyor
- [ ] Cloud Save'e kaydediliyor

### Character Unlock
- [ ] Tüm karakterler gösteriliyor
- [ ] Locked karakterler işaretli
- [ ] Unlock requirements gösteriliyor
- [ ] Unlock butonu çalışıyor
- [ ] Currency harcama çalışıyor
- [ ] Cloud Save'e kaydediliyor

### Inventory
- [ ] Tüm materials gösteriliyor
- [ ] Miktarlar doğru
- [ ] Items gösteriliyor

### Craft
- [ ] Recipe listesi gösteriliyor
- [ ] Material requirements gösteriliyor
- [ ] Craft butonu enable/disable doğru
- [ ] Craft işlemi çalışıyor
- [ ] Materials harcanıyor
- [ ] Item ekleniyor

### Shop
- [ ] Shop items gösteriliyor
- [ ] Category filter çalışıyor
- [ ] Multiple prices gösteriliyor
- [ ] Badges gösteriliyor
- [ ] Purchase butonu çalışıyor
- [ ] Currency harcama çalışıyor

---

## 📊 İlerleme

**Toplam:** 150+ checkbox
**Tamamlanan:** ___ / 150+

---

## 💡 Notlar

- Her panel için CloseButton eklenmeyi unutma
- Tüm button'lara onClick listener ekle
- Prefab'ları Resources klasörüne koy
- Icon'lar için placeholder sprite'lar kullan
- Test için mock data kullan

---

**Başarılar!** 🚀

