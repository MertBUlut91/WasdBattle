using UnityEngine;
using UnityEditor;
using WasdBattle.Data;

namespace WasdBattle.Editor
{
    /// <summary>
    /// Temel karakterleri oluşturmak için editor tool
    /// </summary>
    public class CharacterCreator : EditorWindow
    {
        [MenuItem("WasdBattle/Create Default Characters")]
        public static void CreateDefaultCharacters()
        {
            // Klasörleri oluştur (hiyerarşik)
            string folderPath = "Assets/_Project/ScriptableObjects/Characters";
            
            // Assets/_Project klasörü
            if (!AssetDatabase.IsValidFolder("Assets/_Project"))
            {
                AssetDatabase.CreateFolder("Assets", "_Project");
            }
            
            // Assets/_Project/ScriptableObjects klasörü
            if (!AssetDatabase.IsValidFolder("Assets/_Project/ScriptableObjects"))
            {
                AssetDatabase.CreateFolder("Assets/_Project", "ScriptableObjects");
            }
            
            // Assets/_Project/ScriptableObjects/Characters klasörü
            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                AssetDatabase.CreateFolder("Assets/_Project/ScriptableObjects", "Characters");
            }
            
            AssetDatabase.Refresh();
            
            // Starter Characters (Ücretsiz)
            CreateMage(folderPath);
            CreateWarrior(folderPath);
            CreateNinja(folderPath);
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            Debug.Log("[CharacterCreator] Created 3 starter characters!");
        }
        
        [MenuItem("WasdBattle/Create Unlockable Characters")]
        public static void CreateUnlockableCharacters()
        {
            // Klasörleri oluştur (hiyerarşik)
            string folderPath = "Assets/_Project/ScriptableObjects/Characters";
            
            // Assets/_Project klasörü
            if (!AssetDatabase.IsValidFolder("Assets/_Project"))
            {
                AssetDatabase.CreateFolder("Assets", "_Project");
            }
            
            // Assets/_Project/ScriptableObjects klasörü
            if (!AssetDatabase.IsValidFolder("Assets/_Project/ScriptableObjects"))
            {
                AssetDatabase.CreateFolder("Assets/_Project", "ScriptableObjects");
            }
            
            // Assets/_Project/ScriptableObjects/Characters klasörü
            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                AssetDatabase.CreateFolder("Assets/_Project/ScriptableObjects", "Characters");
            }
            
            AssetDatabase.Refresh();
            
            // Unlockable Characters
            CreateAssassin(folderPath);  // Level 5 + Gold
            CreatePaladin(folderPath);   // Level 10 + Gold VEYA Gem
            CreateRanger(folderPath);    // Level 15 + Gold
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            Debug.Log("[CharacterCreator] Created 3 unlockable characters!");
            EditorUtility.DisplayDialog("Success", "Created 3 unlockable characters:\n- Assassin (Level 5 + 500 Gold)\n- Paladin (Level 10 + 1000 Gold OR 200 Gem)\n- Ranger (Level 15 + 1500 Gold)", "OK");
        }
        
        private static void CreateMage(string folderPath)
        {
            CharacterData mage = ScriptableObject.CreateInstance<CharacterData>();
            mage.characterId = "char_mage";
            mage.characterName = "Alev Büyücüsü";
            mage.characterClass = CharacterClass.Mage;
            mage.description = "Yüksek hasar veren büyüler kullanır ancak dayanıklılığı düşüktür. Hızlı tükenir ama büyük burst hasar verir.";
            mage.baseHealth = 80;
            mage.baseStamina = 80;
            mage.staminaRegenRate = 8f;
            mage.baseDefense = 0.1f;
            mage.isStarterCharacter = true;
            mage.requiresUnlock = false;
            mage.requiredLevel = 1;
            mage.characterColor = new Color(1f, 0.3f, 0f); // Turuncu-kırmızı
            
            AssetDatabase.CreateAsset(mage, $"{folderPath}/Mage.asset");
        }
        
        private static void CreateWarrior(string folderPath)
        {
            CharacterData warrior = ScriptableObject.CreateInstance<CharacterData>();
            warrior.characterId = "char_warrior";
            warrior.characterName = "Kalkan Savaşçısı";
            warrior.characterClass = CharacterClass.Warrior;
            warrior.description = "Yüksek savunma ve dayanıklılığa sahiptir. Uzun süre dayanabilir ancak saldırıları yavaştır.";
            warrior.baseHealth = 150;
            warrior.baseStamina = 120;
            warrior.staminaRegenRate = 12f;
            warrior.baseDefense = 0.3f;
            warrior.isStarterCharacter = true;
            warrior.requiresUnlock = false;
            warrior.requiredLevel = 1;
            warrior.characterColor = new Color(0.2f, 0.5f, 1f); // Mavi
            
            AssetDatabase.CreateAsset(warrior, $"{folderPath}/Warrior.asset");
        }
        
        private static void CreateNinja(string folderPath)
        {
            CharacterData ninja = ScriptableObject.CreateInstance<CharacterData>();
            ninja.characterId = "char_ninja";
            ninja.characterName = "Ninja";
            ninja.characterClass = CharacterClass.Ninja;
            ninja.description = "Hızlı ve çevik. Kısa combo'lar ile seri saldırı yapar ancak savunması düşüktür.";
            ninja.baseHealth = 100;
            ninja.baseStamina = 100;
            ninja.staminaRegenRate = 15f;
            ninja.baseDefense = 0.15f;
            ninja.isStarterCharacter = true;
            ninja.requiresUnlock = false;
            ninja.requiredLevel = 1;
            ninja.characterColor = new Color(0.5f, 0f, 0.8f); // Mor
            
            AssetDatabase.CreateAsset(ninja, $"{folderPath}/Ninja.asset");
        }
        
        private static void CreateAssassin(string folderPath)
        {
            CharacterData assassin = ScriptableObject.CreateInstance<CharacterData>();
            assassin.characterId = "char_assassin";
            assassin.characterName = "Suikastçi";
            assassin.characterClass = CharacterClass.Assassin;
            assassin.description = "Gölgelerden saldıran ölümcül bir savaşçı. Kritik vuruş şansı yüksektir.";
            assassin.baseHealth = 90;
            assassin.baseStamina = 90;
            assassin.staminaRegenRate = 12f;
            assassin.baseDefense = 0.12f;
            
            // Unlock Requirements
            assassin.isStarterCharacter = false;
            assassin.requiresUnlock = true;
            assassin.requiredLevel = 5;
            assassin.unlockPrices = new ShopPrice[]
            {
                new ShopPrice(CurrencyType.Gold, 500)
            };
            
            assassin.characterColor = new Color(0.2f, 0.2f, 0.2f); // Koyu gri
            
            AssetDatabase.CreateAsset(assassin, $"{folderPath}/Assassin.asset");
        }
        
        private static void CreatePaladin(string folderPath)
        {
            CharacterData paladin = ScriptableObject.CreateInstance<CharacterData>();
            paladin.characterId = "char_paladin";
            paladin.characterName = "Paladin";
            paladin.characterClass = CharacterClass.Paladin;
            paladin.description = "Kutsal bir savaşçı. Hem saldırı hem de savunmada dengelidir. Heal yetenekleri vardır.";
            paladin.baseHealth = 130;
            paladin.baseStamina = 110;
            paladin.staminaRegenRate = 10f;
            paladin.baseDefense = 0.25f;
            
            // Unlock Requirements (2 seçenek: Gold VEYA Gem)
            paladin.isStarterCharacter = false;
            paladin.requiresUnlock = true;
            paladin.requiredLevel = 10;
            paladin.unlockPrices = new ShopPrice[]
            {
                new ShopPrice(CurrencyType.Gold, 1000),
                new ShopPrice(CurrencyType.Gem, 200)
            };
            
            paladin.characterColor = new Color(1f, 0.84f, 0f); // Altın sarısı
            
            AssetDatabase.CreateAsset(paladin, $"{folderPath}/Paladin.asset");
        }
        
        private static void CreateRanger(string folderPath)
        {
            CharacterData ranger = ScriptableObject.CreateInstance<CharacterData>();
            ranger.characterId = "char_ranger";
            ranger.characterName = "Okçu";
            ranger.characterClass = CharacterClass.Ranger;
            ranger.description = "Uzak mesafeden saldırı yapan usta bir okçu. Hızlı ve çevik.";
            ranger.baseHealth = 95;
            ranger.baseStamina = 105;
            ranger.staminaRegenRate = 13f;
            ranger.baseDefense = 0.18f;
            
            // Unlock Requirements
            ranger.isStarterCharacter = false;
            ranger.requiresUnlock = true;
            ranger.requiredLevel = 15;
            ranger.unlockPrices = new ShopPrice[]
            {
                new ShopPrice(CurrencyType.Gold, 1500)
            };
            
            ranger.characterColor = new Color(0f, 0.8f, 0.2f); // Yeşil
            
            AssetDatabase.CreateAsset(ranger, $"{folderPath}/Ranger.asset");
        }
    }
}

