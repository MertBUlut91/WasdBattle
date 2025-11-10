using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using WasdBattle.Data;

namespace WasdBattle.Editor
{
    /// <summary>
    /// Karakter ve Item oluşturma, düzenleme ve yönetme editörü
    /// </summary>
    public class GameDataEditor : EditorWindow
    {
        // Tab selection
        private int selectedTab = 0;
        private readonly string[] tabs = { "Karakterler", "Itemler", "Oluştur" };
        
        // Character list
        private List<CharacterData> allCharacters = new List<CharacterData>();
        private CharacterData selectedCharacter;
        private Vector2 characterScrollPos;
        private Vector2 characterDetailScrollPos;
        
        // Item list
        private List<ItemData> allItems = new List<ItemData>();
        private ItemData selectedItem;
        private Vector2 itemScrollPos;
        private Vector2 itemDetailScrollPos;
        
        // Create tab
        private int createTab = 0;
        private readonly string[] createTabs = { "Yeni Karakter", "Yeni Item" };
        private Vector2 createScrollPos;
        
        // New character data
        private string newCharName = "";
        private string newCharId = "";
        private CharacterClass newCharClass = CharacterClass.Mage;
        private string newCharDescription = "";
        private int newCharHealth = 100;
        private int newCharStamina = 100;
        private float newCharStaminaRegen = 10f;
        private float newCharDefense = 0f;
        private Sprite newCharIcon;
        private GameObject newCharPrefab;
        private Color newCharColor = Color.white;
        private bool newCharIsStarter = false;
        
        // New item data
        private string newItemName = "";
        private string newItemId = "";
        private EquipmentSlot newItemSlot = EquipmentSlot.Weapon;
        private ItemClass newItemClass = ItemClass.All;
        private ItemRarity newItemRarity = ItemRarity.Common;
        private string newItemDescription = "";
        private int newItemLevel = 1;
        private int newItemHealth = 0;
        private int newItemStamina = 0;
        private int newItemDamage = 0;
        private int newItemArmor = 0;
        private int newItemMagicRes = 0;
        private float newItemCritChance = 0f;
        private float newItemCritDamage = 0f;
        private Sprite newItemIcon;
        private GameObject newItemPrefab;
        private bool newItemCanCraft = true;
        private bool newItemCanBuy = true;
        private int newItemShopPrice = 100;
        
        // Search & Filter
        private string characterSearchText = "";
        private string itemSearchText = "";
        private CharacterClass characterClassFilter = CharacterClass.Mage;
        private bool useCharacterClassFilter = false;
        private ItemClass itemClassFilter = ItemClass.All;
        private bool useItemClassFilter = false;
        private EquipmentSlot itemSlotFilter = EquipmentSlot.Weapon;
        private bool useItemSlotFilter = false;
        
        // Styles
        private GUIStyle headerStyle;
        private GUIStyle boxStyle;
        private GUIStyle selectedStyle;
        private bool stylesInitialized = false;
        
        [MenuItem("WasdBattle/Game Data Editor")]
        public static void ShowWindow()
        {
            GameDataEditor window = GetWindow<GameDataEditor>("Game Data Editor");
            window.minSize = new Vector2(900, 600);
            window.Show();
        }
        
        private void OnEnable()
        {
            LoadAllData();
        }
        
        private void OnGUI()
        {
            InitializeStyles();
            
            EditorGUILayout.BeginVertical();
            
            // Header
            DrawHeader();
            
            // Tabs
            selectedTab = GUILayout.Toolbar(selectedTab, tabs, GUILayout.Height(30));
            
            EditorGUILayout.Space(10);
            
            // Tab content
            switch (selectedTab)
            {
                case 0:
                    DrawCharactersTab();
                    break;
                case 1:
                    DrawItemsTab();
                    break;
                case 2:
                    DrawCreateTab();
                    break;
            }
            
            EditorGUILayout.EndVertical();
        }
        
        private void InitializeStyles()
        {
            if (stylesInitialized) return;
            
            headerStyle = new GUIStyle(EditorStyles.boldLabel)
            {
                fontSize = 18,
                alignment = TextAnchor.MiddleCenter,
                normal = { textColor = Color.white }
            };
            
            boxStyle = new GUIStyle(GUI.skin.box)
            {
                padding = new RectOffset(10, 10, 10, 10),
                margin = new RectOffset(5, 5, 5, 5)
            };
            
            selectedStyle = new GUIStyle(GUI.skin.box)
            {
                normal = { background = MakeTexture(2, 2, new Color(0.3f, 0.5f, 0.8f, 0.3f)) },
                padding = new RectOffset(5, 5, 5, 5)
            };
            
            stylesInitialized = true;
        }
        
        private Texture2D MakeTexture(int width, int height, Color col)
        {
            Color[] pix = new Color[width * height];
            for (int i = 0; i < pix.Length; i++)
                pix[i] = col;
            
            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();
            return result;
        }
        
        private void DrawHeader()
        {
            EditorGUILayout.BeginVertical(boxStyle);
            GUILayout.Label("🎮 WasdBattle - Game Data Editor", headerStyle);
            GUILayout.Label("Karakter ve Item Oluşturma & Düzenleme Aracı", EditorStyles.centeredGreyMiniLabel);
            EditorGUILayout.EndVertical();
        }
        
        #region Characters Tab
        
        private void DrawCharactersTab()
        {
            EditorGUILayout.BeginHorizontal();
            
            // Left panel - Character list
            EditorGUILayout.BeginVertical(GUILayout.Width(300));
            DrawCharacterListPanel();
            EditorGUILayout.EndVertical();
            
            // Right panel - Character details
            EditorGUILayout.BeginVertical();
            DrawCharacterDetailsPanel();
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.EndHorizontal();
        }
        
        private void DrawCharacterListPanel()
        {
            EditorGUILayout.BeginVertical(boxStyle);
            
            GUILayout.Label("📋 Karakter Listesi", EditorStyles.boldLabel);
            
            // Refresh button
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("🔄 Yenile", GUILayout.Height(25)))
            {
                LoadAllData();
            }
            if (GUILayout.Button("➕ Yeni Karakter", GUILayout.Height(25)))
            {
                selectedTab = 2;
                createTab = 0;
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.Space(5);
            
            // Search
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("🔍", GUILayout.Width(20));
            characterSearchText = EditorGUILayout.TextField(characterSearchText);
            if (GUILayout.Button("X", GUILayout.Width(25)))
            {
                characterSearchText = "";
            }
            EditorGUILayout.EndHorizontal();
            
            // Class filter
            EditorGUILayout.BeginHorizontal();
            useCharacterClassFilter = EditorGUILayout.Toggle(useCharacterClassFilter, GUILayout.Width(20));
            GUI.enabled = useCharacterClassFilter;
            characterClassFilter = (CharacterClass)EditorGUILayout.EnumPopup("Class Filter:", characterClassFilter);
            GUI.enabled = true;
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.Space(5);
            
            // Character count
            var filteredChars = GetFilteredCharacters();
            GUILayout.Label($"Toplam: {filteredChars.Count} karakter", EditorStyles.miniLabel);
            
            EditorGUILayout.Space(5);
            
            // Character list
            characterScrollPos = EditorGUILayout.BeginScrollView(characterScrollPos, GUILayout.ExpandHeight(true));
            
            foreach (var character in filteredChars)
            {
                if (character == null) continue;
                
                bool isSelected = selectedCharacter == character;
                GUIStyle style = isSelected ? selectedStyle : boxStyle;
                
                EditorGUILayout.BeginVertical(style);
                
                EditorGUILayout.BeginHorizontal();
                
                // Icon
                if (character.characterIcon != null)
                {
                    Texture2D iconTexture = AssetPreview.GetAssetPreview(character.characterIcon);
                    if (iconTexture != null)
                    {
                        GUILayout.Label(iconTexture, GUILayout.Width(40), GUILayout.Height(40));
                    }
                }
                else
                {
                    GUILayout.Box("No Icon", GUILayout.Width(40), GUILayout.Height(40));
                }
                
                // Info
                EditorGUILayout.BeginVertical();
                GUILayout.Label(character.characterName, EditorStyles.boldLabel);
                GUILayout.Label($"Class: {character.characterClass}", EditorStyles.miniLabel);
                GUILayout.Label($"ID: {character.characterId}", EditorStyles.miniLabel);
                EditorGUILayout.EndVertical();
                
                EditorGUILayout.EndHorizontal();
                
                // Select button
                if (GUILayout.Button(isSelected ? "✓ Seçili" : "Seç"))
                {
                    selectedCharacter = character;
                }
                
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space(3);
            }
            
            EditorGUILayout.EndScrollView();
            
            EditorGUILayout.EndVertical();
        }
        
        private void DrawCharacterDetailsPanel()
        {
            EditorGUILayout.BeginVertical(boxStyle);
            
            if (selectedCharacter == null)
            {
                GUILayout.Label("Bir karakter seçin", EditorStyles.centeredGreyMiniLabel);
                EditorGUILayout.EndVertical();
                return;
            }
            
            GUILayout.Label("✏️ Karakter Detayları", EditorStyles.boldLabel);
            
            EditorGUILayout.Space(5);
            
            characterDetailScrollPos = EditorGUILayout.BeginScrollView(characterDetailScrollPos);
            
            // Basic Info
            EditorGUILayout.LabelField("Temel Bilgiler", EditorStyles.boldLabel);
            selectedCharacter.characterName = EditorGUILayout.TextField("İsim:", selectedCharacter.characterName);
            selectedCharacter.characterId = EditorGUILayout.TextField("ID:", selectedCharacter.characterId);
            selectedCharacter.characterClass = (CharacterClass)EditorGUILayout.EnumPopup("Class:", selectedCharacter.characterClass);
            selectedCharacter.description = EditorGUILayout.TextArea(selectedCharacter.description, GUILayout.Height(60));
            
            EditorGUILayout.Space(10);
            
            // Base Stats
            EditorGUILayout.LabelField("Temel İstatistikler", EditorStyles.boldLabel);
            selectedCharacter.baseHealth = EditorGUILayout.IntSlider("Health:", selectedCharacter.baseHealth, 50, 500);
            selectedCharacter.baseStamina = EditorGUILayout.IntSlider("Stamina:", selectedCharacter.baseStamina, 50, 300);
            selectedCharacter.staminaRegenRate = EditorGUILayout.Slider("Stamina Regen:", selectedCharacter.staminaRegenRate, 1f, 50f);
            selectedCharacter.baseDefense = EditorGUILayout.Slider("Defense:", selectedCharacter.baseDefense, 0f, 1f);
            
            EditorGUILayout.Space(10);
            
            // Visual
            EditorGUILayout.LabelField("Görsel", EditorStyles.boldLabel);
            selectedCharacter.characterIcon = (Sprite)EditorGUILayout.ObjectField("Icon:", selectedCharacter.characterIcon, typeof(Sprite), false);
            selectedCharacter.characterPrefab = (GameObject)EditorGUILayout.ObjectField("Prefab:", selectedCharacter.characterPrefab, typeof(GameObject), false);
            selectedCharacter.characterColor = EditorGUILayout.ColorField("Renk:", selectedCharacter.characterColor);
            
            EditorGUILayout.Space(10);
            
            // Unlock Requirements
            EditorGUILayout.LabelField("Unlock Ayarları", EditorStyles.boldLabel);
            selectedCharacter.isStarterCharacter = EditorGUILayout.Toggle("Başlangıç Karakteri:", selectedCharacter.isStarterCharacter);
            selectedCharacter.requiresUnlock = EditorGUILayout.Toggle("Unlock Gerekiyor:", selectedCharacter.requiresUnlock);
            selectedCharacter.requiredLevel = EditorGUILayout.IntField("Gerekli Level:", selectedCharacter.requiredLevel);
            
            EditorGUILayout.Space(10);
            
            // Starting Equipment
            EditorGUILayout.LabelField("Başlangıç Ekipmanı", EditorStyles.boldLabel);
            SerializedObject so = new SerializedObject(selectedCharacter);
            SerializedProperty startingItems = so.FindProperty("startingItems");
            EditorGUILayout.PropertyField(startingItems, true);
            
            EditorGUILayout.Space(10);
            
            // Starting Skills
            EditorGUILayout.LabelField("Başlangıç Skillleri", EditorStyles.boldLabel);
            SerializedProperty startingSkills = so.FindProperty("startingSkills");
            EditorGUILayout.PropertyField(startingSkills, true);
            
            so.ApplyModifiedProperties();
            
            EditorGUILayout.EndScrollView();
            
            EditorGUILayout.Space(10);
            
            // Action buttons
            EditorGUILayout.BeginHorizontal();
            
            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("💾 Kaydet", GUILayout.Height(35)))
            {
                SaveCharacter(selectedCharacter);
            }
            
            GUI.backgroundColor = Color.yellow;
            if (GUILayout.Button("📋 Kopyala", GUILayout.Height(35)))
            {
                DuplicateCharacter(selectedCharacter);
            }
            
            GUI.backgroundColor = Color.red;
            if (GUILayout.Button("🗑️ Sil", GUILayout.Height(35)))
            {
                if (EditorUtility.DisplayDialog("Karakteri Sil", 
                    $"{selectedCharacter.characterName} karakterini silmek istediğinizden emin misiniz?", 
                    "Sil", "İptal"))
                {
                    DeleteCharacter(selectedCharacter);
                }
            }
            
            GUI.backgroundColor = Color.white;
            
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.EndVertical();
        }
        
        #endregion
        
        #region Items Tab
        
        private void DrawItemsTab()
        {
            EditorGUILayout.BeginHorizontal();
            
            // Left panel - Item list
            EditorGUILayout.BeginVertical(GUILayout.Width(300));
            DrawItemListPanel();
            EditorGUILayout.EndVertical();
            
            // Right panel - Item details
            EditorGUILayout.BeginVertical();
            DrawItemDetailsPanel();
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.EndHorizontal();
        }
        
        private void DrawItemListPanel()
        {
            EditorGUILayout.BeginVertical(boxStyle);
            
            GUILayout.Label("📦 Item Listesi", EditorStyles.boldLabel);
            
            // Refresh button
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("🔄 Yenile", GUILayout.Height(25)))
            {
                LoadAllData();
            }
            if (GUILayout.Button("➕ Yeni Item", GUILayout.Height(25)))
            {
                selectedTab = 2;
                createTab = 1;
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.Space(5);
            
            // Search
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("🔍", GUILayout.Width(20));
            itemSearchText = EditorGUILayout.TextField(itemSearchText);
            if (GUILayout.Button("X", GUILayout.Width(25)))
            {
                itemSearchText = "";
            }
            EditorGUILayout.EndHorizontal();
            
            // Class filter
            EditorGUILayout.BeginHorizontal();
            useItemClassFilter = EditorGUILayout.Toggle(useItemClassFilter, GUILayout.Width(20));
            GUI.enabled = useItemClassFilter;
            itemClassFilter = (ItemClass)EditorGUILayout.EnumPopup("Class Filter:", itemClassFilter);
            GUI.enabled = true;
            EditorGUILayout.EndHorizontal();
            
            // Slot filter
            EditorGUILayout.BeginHorizontal();
            useItemSlotFilter = EditorGUILayout.Toggle(useItemSlotFilter, GUILayout.Width(20));
            GUI.enabled = useItemSlotFilter;
            itemSlotFilter = (EquipmentSlot)EditorGUILayout.EnumPopup("Slot Filter:", itemSlotFilter);
            GUI.enabled = true;
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.Space(5);
            
            // Item count
            var filteredItems = GetFilteredItems();
            GUILayout.Label($"Toplam: {filteredItems.Count} item", EditorStyles.miniLabel);
            
            EditorGUILayout.Space(5);
            
            // Item list
            itemScrollPos = EditorGUILayout.BeginScrollView(itemScrollPos, GUILayout.ExpandHeight(true));
            
            foreach (var item in filteredItems)
            {
                if (item == null) continue;
                
                bool isSelected = selectedItem == item;
                GUIStyle style = isSelected ? selectedStyle : boxStyle;
                
                EditorGUILayout.BeginVertical(style);
                
                EditorGUILayout.BeginHorizontal();
                
                // Icon
                if (item.icon != null)
                {
                    Texture2D iconTexture = AssetPreview.GetAssetPreview(item.icon);
                    if (iconTexture != null)
                    {
                        GUILayout.Label(iconTexture, GUILayout.Width(40), GUILayout.Height(40));
                    }
                }
                else
                {
                    GUILayout.Box("No Icon", GUILayout.Width(40), GUILayout.Height(40));
                }
                
                // Info
                EditorGUILayout.BeginVertical();
                GUILayout.Label(item.itemName, EditorStyles.boldLabel);
                GUILayout.Label($"{GetRarityColor(item.rarity)} {item.rarity}", EditorStyles.miniLabel);
                GUILayout.Label($"Slot: {item.slot} | Class: {item.requiredClass}", EditorStyles.miniLabel);
                EditorGUILayout.EndVertical();
                
                EditorGUILayout.EndHorizontal();
                
                // Select button
                if (GUILayout.Button(isSelected ? "✓ Seçili" : "Seç"))
                {
                    selectedItem = item;
                }
                
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space(3);
            }
            
            EditorGUILayout.EndScrollView();
            
            EditorGUILayout.EndVertical();
        }
        
        private void DrawItemDetailsPanel()
        {
            EditorGUILayout.BeginVertical(boxStyle);
            
            if (selectedItem == null)
            {
                GUILayout.Label("Bir item seçin", EditorStyles.centeredGreyMiniLabel);
                EditorGUILayout.EndVertical();
                return;
            }
            
            GUILayout.Label("✏️ Item Detayları", EditorStyles.boldLabel);
            
            EditorGUILayout.Space(5);
            
            itemDetailScrollPos = EditorGUILayout.BeginScrollView(itemDetailScrollPos);
            
            // Basic Info
            EditorGUILayout.LabelField("Temel Bilgiler", EditorStyles.boldLabel);
            selectedItem.itemName = EditorGUILayout.TextField("İsim:", selectedItem.itemName);
            selectedItem.itemId = EditorGUILayout.TextField("ID:", selectedItem.itemId);
            selectedItem.slot = (EquipmentSlot)EditorGUILayout.EnumPopup("Slot:", selectedItem.slot);
            selectedItem.requiredClass = (ItemClass)EditorGUILayout.EnumPopup("Class:", selectedItem.requiredClass);
            selectedItem.rarity = (ItemRarity)EditorGUILayout.EnumPopup("Rarity:", selectedItem.rarity);
            selectedItem.level = EditorGUILayout.IntField("Level:", selectedItem.level);
            selectedItem.description = EditorGUILayout.TextArea(selectedItem.description, GUILayout.Height(60));
            
            EditorGUILayout.Space(10);
            
            // Stats
            EditorGUILayout.LabelField("İstatistikler", EditorStyles.boldLabel);
            selectedItem.healthBonus = EditorGUILayout.IntSlider("Health Bonus:", selectedItem.healthBonus, 0, 200);
            selectedItem.staminaBonus = EditorGUILayout.IntSlider("Stamina Bonus:", selectedItem.staminaBonus, 0, 100);
            selectedItem.damageBonus = EditorGUILayout.IntSlider("Damage Bonus:", selectedItem.damageBonus, 0, 100);
            selectedItem.armorBonus = EditorGUILayout.IntSlider("Armor Bonus:", selectedItem.armorBonus, 0, 100);
            selectedItem.magicResistanceBonus = EditorGUILayout.IntSlider("Magic Res Bonus:", selectedItem.magicResistanceBonus, 0, 100);
            selectedItem.critChanceBonus = EditorGUILayout.Slider("Crit Chance:", selectedItem.critChanceBonus, 0f, 1f);
            selectedItem.critDamageBonus = EditorGUILayout.Slider("Crit Damage:", selectedItem.critDamageBonus, 0f, 2f);
            
            EditorGUILayout.Space(5);
            GUILayout.Label($"Toplam Stat Bonusu: {selectedItem.TotalStatBonus}", EditorStyles.helpBox);
            
            EditorGUILayout.Space(10);
            
            // Visual
            EditorGUILayout.LabelField("Görsel", EditorStyles.boldLabel);
            selectedItem.icon = (Sprite)EditorGUILayout.ObjectField("Icon:", selectedItem.icon, typeof(Sprite), false);
            selectedItem.prefab = (GameObject)EditorGUILayout.ObjectField("Prefab:", selectedItem.prefab, typeof(GameObject), false);
            
            EditorGUILayout.Space(10);
            
            // Crafting
            EditorGUILayout.LabelField("Crafting", EditorStyles.boldLabel);
            selectedItem.canBeCrafted = EditorGUILayout.Toggle("Craft Edilebilir:", selectedItem.canBeCrafted);
            
            if (selectedItem.canBeCrafted)
            {
                SerializedObject so = new SerializedObject(selectedItem);
                SerializedProperty craftingMats = so.FindProperty("craftingMaterials");
                EditorGUILayout.PropertyField(craftingMats, true);
                so.ApplyModifiedProperties();
            }
            
            EditorGUILayout.Space(10);
            
            // Shop
            EditorGUILayout.LabelField("Mağaza", EditorStyles.boldLabel);
            selectedItem.canBeBought = EditorGUILayout.Toggle("Satın Alınabilir:", selectedItem.canBeBought);
            if (selectedItem.canBeBought)
            {
                selectedItem.shopPrice = EditorGUILayout.IntField("Fiyat:", selectedItem.shopPrice);
            }
            
            EditorGUILayout.Space(10);
            
            // Salvage
            EditorGUILayout.LabelField("Salvage (Eritme)", EditorStyles.boldLabel);
            selectedItem.canBeSalvaged = EditorGUILayout.Toggle("Eritilebilir:", selectedItem.canBeSalvaged);
            if (selectedItem.canBeSalvaged)
            {
                selectedItem.salvageReturnRate = EditorGUILayout.Slider("Geri Dönüş Oranı:", selectedItem.salvageReturnRate, 0f, 1f);
                
                if (selectedItem.craftingMaterials != null && selectedItem.craftingMaterials.Length > 0)
                {
                    EditorGUILayout.HelpBox($"Salvage Ödülleri:\n{selectedItem.GetSalvageRewardSummary()}", MessageType.Info);
                }
            }
            
            EditorGUILayout.EndScrollView();
            
            EditorGUILayout.Space(10);
            
            // Action buttons
            EditorGUILayout.BeginHorizontal();
            
            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("💾 Kaydet", GUILayout.Height(35)))
            {
                SaveItem(selectedItem);
            }
            
            GUI.backgroundColor = Color.yellow;
            if (GUILayout.Button("📋 Kopyala", GUILayout.Height(35)))
            {
                DuplicateItem(selectedItem);
            }
            
            GUI.backgroundColor = Color.red;
            if (GUILayout.Button("🗑️ Sil", GUILayout.Height(35)))
            {
                if (EditorUtility.DisplayDialog("Item'i Sil", 
                    $"{selectedItem.itemName} item'ini silmek istediğinizden emin misiniz?", 
                    "Sil", "İptal"))
                {
                    DeleteItem(selectedItem);
                }
            }
            
            GUI.backgroundColor = Color.white;
            
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.EndVertical();
        }
        
        #endregion
        
        #region Create Tab
        
        private void DrawCreateTab()
        {
            EditorGUILayout.BeginVertical(boxStyle);
            
            createTab = GUILayout.Toolbar(createTab, createTabs, GUILayout.Height(30));
            
            EditorGUILayout.Space(10);
            
            createScrollPos = EditorGUILayout.BeginScrollView(createScrollPos);
            
            if (createTab == 0)
            {
                DrawCreateCharacterPanel();
            }
            else
            {
                DrawCreateItemPanel();
            }
            
            EditorGUILayout.EndScrollView();
            
            EditorGUILayout.EndVertical();
        }
        
        private void DrawCreateCharacterPanel()
        {
            GUILayout.Label("➕ Yeni Karakter Oluştur", EditorStyles.boldLabel);
            
            EditorGUILayout.Space(10);
            
            // Basic Info
            EditorGUILayout.LabelField("Temel Bilgiler", EditorStyles.boldLabel);
            newCharName = EditorGUILayout.TextField("İsim:", newCharName);
            newCharId = EditorGUILayout.TextField("ID:", newCharId);
            
            if (GUILayout.Button("ID'yi Otomatik Oluştur"))
            {
                newCharId = "char_" + newCharName.ToLower().Replace(" ", "_");
            }
            
            newCharClass = (CharacterClass)EditorGUILayout.EnumPopup("Class:", newCharClass);
            newCharDescription = EditorGUILayout.TextArea(newCharDescription, GUILayout.Height(60));
            
            EditorGUILayout.Space(10);
            
            // Base Stats
            EditorGUILayout.LabelField("Temel İstatistikler", EditorStyles.boldLabel);
            newCharHealth = EditorGUILayout.IntSlider("Health:", newCharHealth, 50, 500);
            newCharStamina = EditorGUILayout.IntSlider("Stamina:", newCharStamina, 50, 300);
            newCharStaminaRegen = EditorGUILayout.Slider("Stamina Regen:", newCharStaminaRegen, 1f, 50f);
            newCharDefense = EditorGUILayout.Slider("Defense:", newCharDefense, 0f, 1f);
            
            EditorGUILayout.Space(10);
            
            // Visual
            EditorGUILayout.LabelField("Görsel", EditorStyles.boldLabel);
            newCharIcon = (Sprite)EditorGUILayout.ObjectField("Icon:", newCharIcon, typeof(Sprite), false);
            newCharPrefab = (GameObject)EditorGUILayout.ObjectField("Prefab:", newCharPrefab, typeof(GameObject), false);
            newCharColor = EditorGUILayout.ColorField("Renk:", newCharColor);
            
            EditorGUILayout.Space(10);
            
            // Unlock
            EditorGUILayout.LabelField("Unlock Ayarları", EditorStyles.boldLabel);
            newCharIsStarter = EditorGUILayout.Toggle("Başlangıç Karakteri:", newCharIsStarter);
            
            EditorGUILayout.Space(20);
            
            // Create button
            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("✨ Karakteri Oluştur", GUILayout.Height(40)))
            {
                CreateNewCharacter();
            }
            GUI.backgroundColor = Color.white;
            
            EditorGUILayout.Space(10);
            
            // Reset button
            if (GUILayout.Button("🔄 Formu Temizle"))
            {
                ResetCharacterForm();
            }
        }
        
        private void DrawCreateItemPanel()
        {
            GUILayout.Label("➕ Yeni Item Oluştur", EditorStyles.boldLabel);
            
            EditorGUILayout.Space(10);
            
            // Basic Info
            EditorGUILayout.LabelField("Temel Bilgiler", EditorStyles.boldLabel);
            newItemName = EditorGUILayout.TextField("İsim:", newItemName);
            newItemId = EditorGUILayout.TextField("ID:", newItemId);
            
            if (GUILayout.Button("ID'yi Otomatik Oluştur"))
            {
                newItemId = "item_" + newItemName.ToLower().Replace(" ", "_");
            }
            
            newItemSlot = (EquipmentSlot)EditorGUILayout.EnumPopup("Slot:", newItemSlot);
            newItemClass = (ItemClass)EditorGUILayout.EnumPopup("Class:", newItemClass);
            newItemRarity = (ItemRarity)EditorGUILayout.EnumPopup("Rarity:", newItemRarity);
            newItemLevel = EditorGUILayout.IntField("Level:", newItemLevel);
            newItemDescription = EditorGUILayout.TextArea(newItemDescription, GUILayout.Height(60));
            
            EditorGUILayout.Space(10);
            
            // Stats
            EditorGUILayout.LabelField("İstatistikler", EditorStyles.boldLabel);
            newItemHealth = EditorGUILayout.IntSlider("Health Bonus:", newItemHealth, 0, 200);
            newItemStamina = EditorGUILayout.IntSlider("Stamina Bonus:", newItemStamina, 0, 100);
            newItemDamage = EditorGUILayout.IntSlider("Damage Bonus:", newItemDamage, 0, 100);
            newItemArmor = EditorGUILayout.IntSlider("Armor Bonus:", newItemArmor, 0, 100);
            newItemMagicRes = EditorGUILayout.IntSlider("Magic Res Bonus:", newItemMagicRes, 0, 100);
            newItemCritChance = EditorGUILayout.Slider("Crit Chance:", newItemCritChance, 0f, 1f);
            newItemCritDamage = EditorGUILayout.Slider("Crit Damage:", newItemCritDamage, 0f, 2f);
            
            EditorGUILayout.Space(10);
            
            // Visual
            EditorGUILayout.LabelField("Görsel", EditorStyles.boldLabel);
            newItemIcon = (Sprite)EditorGUILayout.ObjectField("Icon:", newItemIcon, typeof(Sprite), false);
            newItemPrefab = (GameObject)EditorGUILayout.ObjectField("Prefab:", newItemPrefab, typeof(GameObject), false);
            
            EditorGUILayout.Space(10);
            
            // Shop & Craft
            EditorGUILayout.LabelField("Mağaza & Crafting", EditorStyles.boldLabel);
            newItemCanBuy = EditorGUILayout.Toggle("Satın Alınabilir:", newItemCanBuy);
            if (newItemCanBuy)
            {
                newItemShopPrice = EditorGUILayout.IntField("Fiyat:", newItemShopPrice);
            }
            newItemCanCraft = EditorGUILayout.Toggle("Craft Edilebilir:", newItemCanCraft);
            
            EditorGUILayout.Space(20);
            
            // Create button
            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("✨ Item'i Oluştur", GUILayout.Height(40)))
            {
                CreateNewItem();
            }
            GUI.backgroundColor = Color.white;
            
            EditorGUILayout.Space(10);
            
            // Reset button
            if (GUILayout.Button("🔄 Formu Temizle"))
            {
                ResetItemForm();
            }
        }
        
        #endregion
        
        #region Data Management
        
        private void LoadAllData()
        {
            // Load all characters
            string[] characterGuids = AssetDatabase.FindAssets("t:CharacterData");
            allCharacters.Clear();
            
            foreach (string guid in characterGuids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                CharacterData character = AssetDatabase.LoadAssetAtPath<CharacterData>(path);
                if (character != null)
                {
                    allCharacters.Add(character);
                }
            }
            
            allCharacters = allCharacters.OrderBy(c => c.characterName).ToList();
            
            // Load all items
            string[] itemGuids = AssetDatabase.FindAssets("t:ItemData");
            allItems.Clear();
            
            foreach (string guid in itemGuids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                ItemData item = AssetDatabase.LoadAssetAtPath<ItemData>(path);
                if (item != null)
                {
                    allItems.Add(item);
                }
            }
            
            allItems = allItems.OrderBy(i => i.itemName).ToList();
            
            Debug.Log($"[GameDataEditor] Loaded {allCharacters.Count} characters and {allItems.Count} items");
        }
        
        private List<CharacterData> GetFilteredCharacters()
        {
            var filtered = allCharacters.Where(c => c != null);
            
            // Search filter
            if (!string.IsNullOrEmpty(characterSearchText))
            {
                filtered = filtered.Where(c => 
                    c.characterName.ToLower().Contains(characterSearchText.ToLower()) ||
                    c.characterId.ToLower().Contains(characterSearchText.ToLower()));
            }
            
            // Class filter
            if (useCharacterClassFilter)
            {
                filtered = filtered.Where(c => c.characterClass == characterClassFilter);
            }
            
            return filtered.ToList();
        }
        
        private List<ItemData> GetFilteredItems()
        {
            var filtered = allItems.Where(i => i != null);
            
            // Search filter
            if (!string.IsNullOrEmpty(itemSearchText))
            {
                filtered = filtered.Where(i => 
                    i.itemName.ToLower().Contains(itemSearchText.ToLower()) ||
                    i.itemId.ToLower().Contains(itemSearchText.ToLower()));
            }
            
            // Class filter
            if (useItemClassFilter)
            {
                filtered = filtered.Where(i => i.requiredClass == itemClassFilter || i.requiredClass == ItemClass.All);
            }
            
            // Slot filter
            if (useItemSlotFilter)
            {
                filtered = filtered.Where(i => i.slot == itemSlotFilter);
            }
            
            return filtered.ToList();
        }
        
        private void CreateNewCharacter()
        {
            if (string.IsNullOrEmpty(newCharName))
            {
                EditorUtility.DisplayDialog("Hata", "Karakter ismi boş olamaz!", "Tamam");
                return;
            }
            
            if (string.IsNullOrEmpty(newCharId))
            {
                EditorUtility.DisplayDialog("Hata", "Karakter ID'si boş olamaz!", "Tamam");
                return;
            }
            
            // Create folder if not exists
            string folderPath = "Assets/ScriptableObjects/Characters";
            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                AssetDatabase.CreateFolder("Assets/ScriptableObjects", "Characters");
            }
            
            // Create character
            CharacterData newChar = ScriptableObject.CreateInstance<CharacterData>();
            newChar.characterName = newCharName;
            newChar.characterId = newCharId;
            newChar.characterClass = newCharClass;
            newChar.description = newCharDescription;
            newChar.baseHealth = newCharHealth;
            newChar.baseStamina = newCharStamina;
            newChar.staminaRegenRate = newCharStaminaRegen;
            newChar.baseDefense = newCharDefense;
            newChar.characterIcon = newCharIcon;
            newChar.characterPrefab = newCharPrefab;
            newChar.characterColor = newCharColor;
            newChar.isStarterCharacter = newCharIsStarter;
            
            string assetPath = $"{folderPath}/{newCharName}.asset";
            AssetDatabase.CreateAsset(newChar, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            Debug.Log($"[GameDataEditor] Created character: {newCharName} at {assetPath}");
            
            LoadAllData();
            selectedCharacter = newChar;
            selectedTab = 0; // Switch to characters tab
            
            EditorUtility.DisplayDialog("Başarılı", $"{newCharName} karakteri oluşturuldu!", "Tamam");
            
            ResetCharacterForm();
        }
        
        private void CreateNewItem()
        {
            if (string.IsNullOrEmpty(newItemName))
            {
                EditorUtility.DisplayDialog("Hata", "Item ismi boş olamaz!", "Tamam");
                return;
            }
            
            if (string.IsNullOrEmpty(newItemId))
            {
                EditorUtility.DisplayDialog("Hata", "Item ID'si boş olamaz!", "Tamam");
                return;
            }
            
            // Create folder if not exists
            string folderPath = "Assets/ScriptableObjects/Items";
            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                AssetDatabase.CreateFolder("Assets/ScriptableObjects", "Items");
            }
            
            // Create item
            ItemData newItem = ScriptableObject.CreateInstance<ItemData>();
            newItem.itemName = newItemName;
            newItem.itemId = newItemId;
            newItem.slot = newItemSlot;
            newItem.requiredClass = newItemClass;
            newItem.rarity = newItemRarity;
            newItem.level = newItemLevel;
            newItem.description = newItemDescription;
            newItem.healthBonus = newItemHealth;
            newItem.staminaBonus = newItemStamina;
            newItem.damageBonus = newItemDamage;
            newItem.armorBonus = newItemArmor;
            newItem.magicResistanceBonus = newItemMagicRes;
            newItem.critChanceBonus = newItemCritChance;
            newItem.critDamageBonus = newItemCritDamage;
            newItem.icon = newItemIcon;
            newItem.prefab = newItemPrefab;
            newItem.canBeBought = newItemCanBuy;
            newItem.shopPrice = newItemShopPrice;
            newItem.canBeCrafted = newItemCanCraft;
            
            string assetPath = $"{folderPath}/{newItemName}.asset";
            AssetDatabase.CreateAsset(newItem, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            Debug.Log($"[GameDataEditor] Created item: {newItemName} at {assetPath}");
            
            LoadAllData();
            selectedItem = newItem;
            selectedTab = 1; // Switch to items tab
            
            EditorUtility.DisplayDialog("Başarılı", $"{newItemName} item'i oluşturuldu!", "Tamam");
            
            ResetItemForm();
        }
        
        private void SaveCharacter(CharacterData character)
        {
            EditorUtility.SetDirty(character);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            Debug.Log($"[GameDataEditor] Saved character: {character.characterName}");
            EditorUtility.DisplayDialog("Başarılı", $"{character.characterName} kaydedildi!", "Tamam");
        }
        
        private void SaveItem(ItemData item)
        {
            EditorUtility.SetDirty(item);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            Debug.Log($"[GameDataEditor] Saved item: {item.itemName}");
            EditorUtility.DisplayDialog("Başarılı", $"{item.itemName} kaydedildi!", "Tamam");
        }
        
        private void DuplicateCharacter(CharacterData character)
        {
            string path = AssetDatabase.GetAssetPath(character);
            string newPath = AssetDatabase.GenerateUniqueAssetPath(path);
            
            AssetDatabase.CopyAsset(path, newPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            CharacterData duplicate = AssetDatabase.LoadAssetAtPath<CharacterData>(newPath);
            duplicate.characterId = character.characterId + "_copy";
            duplicate.characterName = character.characterName + " (Copy)";
            
            EditorUtility.SetDirty(duplicate);
            AssetDatabase.SaveAssets();
            
            LoadAllData();
            selectedCharacter = duplicate;
            
            Debug.Log($"[GameDataEditor] Duplicated character: {character.characterName}");
            EditorUtility.DisplayDialog("Başarılı", $"{character.characterName} kopyalandı!", "Tamam");
        }
        
        private void DuplicateItem(ItemData item)
        {
            string path = AssetDatabase.GetAssetPath(item);
            string newPath = AssetDatabase.GenerateUniqueAssetPath(path);
            
            AssetDatabase.CopyAsset(path, newPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            ItemData duplicate = AssetDatabase.LoadAssetAtPath<ItemData>(newPath);
            duplicate.itemId = item.itemId + "_copy";
            duplicate.itemName = item.itemName + " (Copy)";
            
            EditorUtility.SetDirty(duplicate);
            AssetDatabase.SaveAssets();
            
            LoadAllData();
            selectedItem = duplicate;
            
            Debug.Log($"[GameDataEditor] Duplicated item: {item.itemName}");
            EditorUtility.DisplayDialog("Başarılı", $"{item.itemName} kopyalandı!", "Tamam");
        }
        
        private void DeleteCharacter(CharacterData character)
        {
            string path = AssetDatabase.GetAssetPath(character);
            AssetDatabase.DeleteAsset(path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            LoadAllData();
            selectedCharacter = null;
            
            Debug.Log($"[GameDataEditor] Deleted character: {character.characterName}");
        }
        
        private void DeleteItem(ItemData item)
        {
            string path = AssetDatabase.GetAssetPath(item);
            AssetDatabase.DeleteAsset(path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            LoadAllData();
            selectedItem = null;
            
            Debug.Log($"[GameDataEditor] Deleted item: {item.itemName}");
        }
        
        private void ResetCharacterForm()
        {
            newCharName = "";
            newCharId = "";
            newCharClass = CharacterClass.Mage;
            newCharDescription = "";
            newCharHealth = 100;
            newCharStamina = 100;
            newCharStaminaRegen = 10f;
            newCharDefense = 0f;
            newCharIcon = null;
            newCharPrefab = null;
            newCharColor = Color.white;
            newCharIsStarter = false;
        }
        
        private void ResetItemForm()
        {
            newItemName = "";
            newItemId = "";
            newItemSlot = EquipmentSlot.Weapon;
            newItemClass = ItemClass.All;
            newItemRarity = ItemRarity.Common;
            newItemDescription = "";
            newItemLevel = 1;
            newItemHealth = 0;
            newItemStamina = 0;
            newItemDamage = 0;
            newItemArmor = 0;
            newItemMagicRes = 0;
            newItemCritChance = 0f;
            newItemCritDamage = 0f;
            newItemIcon = null;
            newItemPrefab = null;
            newItemCanCraft = true;
            newItemCanBuy = true;
            newItemShopPrice = 100;
        }
        
        private string GetRarityColor(ItemRarity rarity)
        {
            switch (rarity)
            {
                case ItemRarity.Common: return "⚪";
                case ItemRarity.Uncommon: return "🟢";
                case ItemRarity.Rare: return "🔵";
                case ItemRarity.Epic: return "🟣";
                case ItemRarity.Legendary: return "🟠";
                default: return "⚪";
            }
        }
        
        #endregion
    }
}

