using UnityEngine;
using UnityEditor;
using WasdBattle.Data;
using System.IO;

namespace WasdBattle.Editor
{
    /// <summary>
    /// Editor tool - Starter itemleri otomatik oluşturur
    /// </summary>
    public class ItemCreator : EditorWindow
    {
        [MenuItem("WasdBattle/Create Default Items")]
        public static void CreateDefaultItems()
        {
            string itemPath = "Assets/ScriptableObjects/Items";
            
            // Klasör yoksa oluştur
            if (!Directory.Exists(itemPath))
            {
                Directory.CreateDirectory(itemPath);
                AssetDatabase.Refresh();
            }
            
            Debug.Log("[ItemCreator] Creating default starter items...");
            
            // Mage Starter Items
            CreateMageStarterItems(itemPath);
            
            // Warrior Starter Items
            CreateWarriorStarterItems(itemPath);
            
            // Ninja Starter Items
            CreateNinjaStarterItems(itemPath);
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            Debug.Log("[ItemCreator] Default items created successfully!");
            EditorUtility.DisplayDialog("Success", "Default starter items created in:\n" + itemPath, "OK");
        }
        
        private static void CreateMageStarterItems(string basePath)
        {
            // Mage Starter Robe
            ItemData mageRobe = ScriptableObject.CreateInstance<ItemData>();
            mageRobe.itemId = "item_mage_starter_robe";
            mageRobe.itemName = "Apprentice Robe";
            mageRobe.description = "A simple robe for beginner mages.";
            mageRobe.slot = EquipmentSlot.Chest;
            mageRobe.requiredClass = ItemClass.Mage;
            mageRobe.rarity = ItemRarity.Common;
            mageRobe.level = 1;
            mageRobe.healthBonus = 10;
            mageRobe.staminaBonus = 5;
            mageRobe.magicResistanceBonus = 5;
            mageRobe.canBeCrafted = false;
            mageRobe.canBeBought = false;
            
            AssetDatabase.CreateAsset(mageRobe, $"{basePath}/Mage_Starter_Robe.asset");
            
            // Mage Starter Staff
            ItemData mageStaff = ScriptableObject.CreateInstance<ItemData>();
            mageStaff.itemId = "item_mage_starter_staff";
            mageStaff.itemName = "Wooden Staff";
            mageStaff.description = "A basic staff for casting spells.";
            mageStaff.slot = EquipmentSlot.Weapon;
            mageStaff.requiredClass = ItemClass.Mage;
            mageStaff.rarity = ItemRarity.Common;
            mageStaff.level = 1;
            mageStaff.damageBonus = 5;
            mageStaff.staminaBonus = 10;
            mageStaff.canBeCrafted = false;
            mageStaff.canBeBought = false;
            
            AssetDatabase.CreateAsset(mageStaff, $"{basePath}/Mage_Starter_Staff.asset");
            
            Debug.Log("[ItemCreator] Created Mage starter items");
        }
        
        private static void CreateWarriorStarterItems(string basePath)
        {
            // Warrior Starter Armor
            ItemData warriorArmor = ScriptableObject.CreateInstance<ItemData>();
            warriorArmor.itemId = "item_warrior_starter_armor";
            warriorArmor.itemName = "Iron Armor";
            warriorArmor.description = "Basic iron armor for warriors.";
            warriorArmor.slot = EquipmentSlot.Chest;
            warriorArmor.requiredClass = ItemClass.Warrior;
            warriorArmor.rarity = ItemRarity.Common;
            warriorArmor.level = 1;
            warriorArmor.healthBonus = 20;
            warriorArmor.armorBonus = 10;
            warriorArmor.canBeCrafted = false;
            warriorArmor.canBeBought = false;
            
            AssetDatabase.CreateAsset(warriorArmor, $"{basePath}/Warrior_Starter_Armor.asset");
            
            // Warrior Starter Sword
            ItemData warriorSword = ScriptableObject.CreateInstance<ItemData>();
            warriorSword.itemId = "item_warrior_starter_sword";
            warriorSword.itemName = "Iron Sword";
            warriorSword.description = "A reliable iron sword.";
            warriorSword.slot = EquipmentSlot.Weapon;
            warriorSword.requiredClass = ItemClass.Warrior;
            warriorSword.rarity = ItemRarity.Common;
            warriorSword.level = 1;
            warriorSword.damageBonus = 8;
            warriorSword.healthBonus = 5;
            warriorSword.canBeCrafted = false;
            warriorSword.canBeBought = false;
            
            AssetDatabase.CreateAsset(warriorSword, $"{basePath}/Warrior_Starter_Sword.asset");
            
            Debug.Log("[ItemCreator] Created Warrior starter items");
        }
        
        private static void CreateNinjaStarterItems(string basePath)
        {
            // Ninja Starter Outfit
            ItemData ninjaOutfit = ScriptableObject.CreateInstance<ItemData>();
            ninjaOutfit.itemId = "item_ninja_starter_outfit";
            ninjaOutfit.itemName = "Shadow Garb";
            ninjaOutfit.description = "Light armor for agile ninjas.";
            ninjaOutfit.slot = EquipmentSlot.Chest;
            ninjaOutfit.requiredClass = ItemClass.Ninja;
            ninjaOutfit.rarity = ItemRarity.Common;
            ninjaOutfit.level = 1;
            ninjaOutfit.healthBonus = 15;
            ninjaOutfit.staminaBonus = 10;
            ninjaOutfit.armorBonus = 3;
            ninjaOutfit.canBeCrafted = false;
            ninjaOutfit.canBeBought = false;
            
            AssetDatabase.CreateAsset(ninjaOutfit, $"{basePath}/Ninja_Starter_Outfit.asset");
            
            // Ninja Starter Daggers
            ItemData ninjaDaggers = ScriptableObject.CreateInstance<ItemData>();
            ninjaDaggers.itemId = "item_ninja_starter_daggers";
            ninjaDaggers.itemName = "Steel Daggers";
            ninjaDaggers.description = "Quick and deadly daggers.";
            ninjaDaggers.slot = EquipmentSlot.Weapon;
            ninjaDaggers.requiredClass = ItemClass.Ninja;
            ninjaDaggers.rarity = ItemRarity.Common;
            ninjaDaggers.level = 1;
            ninjaDaggers.damageBonus = 6;
            ninjaDaggers.staminaBonus = 5;
            ninjaDaggers.critChanceBonus = 0.05f; // %5 crit
            ninjaDaggers.canBeCrafted = false;
            ninjaDaggers.canBeBought = false;
            
            AssetDatabase.CreateAsset(ninjaDaggers, $"{basePath}/Ninja_Starter_Daggers.asset");
            
            Debug.Log("[ItemCreator] Created Ninja starter items");
        }
        
        [MenuItem("WasdBattle/Create Example Craftable Items")]
        public static void CreateExampleCraftableItems()
        {
            string itemPath = "Assets/ScriptableObjects/Items/Craftable";
            
            if (!Directory.Exists(itemPath))
            {
                Directory.CreateDirectory(itemPath);
                AssetDatabase.Refresh();
            }
            
            Debug.Log("[ItemCreator] Creating example craftable items...");
            
            // Example: Epic Mage Helmet
            ItemData epicHelmet = ScriptableObject.CreateInstance<ItemData>();
            epicHelmet.itemId = "item_epic_mage_helmet";
            epicHelmet.itemName = "Arcane Crown";
            epicHelmet.description = "A powerful helmet imbued with arcane energy.";
            epicHelmet.slot = EquipmentSlot.Helmet;
            epicHelmet.requiredClass = ItemClass.Mage;
            epicHelmet.rarity = ItemRarity.Epic;
            epicHelmet.level = 10;
            epicHelmet.healthBonus = 30;
            epicHelmet.staminaBonus = 20;
            epicHelmet.magicResistanceBonus = 15;
            epicHelmet.damageBonus = 10;
            epicHelmet.canBeCrafted = true;
            epicHelmet.craftingMaterials = new CraftingMaterial[]
            {
                new CraftingMaterial { materialType = MaterialType.Metal, amount = 50 },
                new CraftingMaterial { materialType = MaterialType.EnergyCrystal, amount = 30 },
                new CraftingMaterial { materialType = MaterialType.Rune, amount = 10 },
                new CraftingMaterial { materialType = MaterialType.GemStone, amount = 5 }
            };
            epicHelmet.canBeBought = false;
            
            AssetDatabase.CreateAsset(epicHelmet, $"{itemPath}/Epic_Mage_Helmet.asset");
            
            // Example: Legendary Weapon
            ItemData legendaryWeapon = ScriptableObject.CreateInstance<ItemData>();
            legendaryWeapon.itemId = "item_legendary_sword";
            legendaryWeapon.itemName = "Dragonslayer";
            legendaryWeapon.description = "A legendary blade forged from dragon scales.";
            legendaryWeapon.slot = EquipmentSlot.Weapon;
            legendaryWeapon.requiredClass = ItemClass.All; // Herkes kullanabilir
            legendaryWeapon.rarity = ItemRarity.Legendary;
            legendaryWeapon.level = 20;
            legendaryWeapon.damageBonus = 50;
            legendaryWeapon.healthBonus = 20;
            legendaryWeapon.critChanceBonus = 0.15f; // %15 crit
            legendaryWeapon.critDamageBonus = 0.50f; // %50 crit damage
            legendaryWeapon.canBeCrafted = true;
            legendaryWeapon.craftingMaterials = new CraftingMaterial[]
            {
                new CraftingMaterial { materialType = MaterialType.Metal, amount = 200 },
                new CraftingMaterial { materialType = MaterialType.GemStone, amount = 50 },
                new CraftingMaterial { materialType = MaterialType.DarkEssence, amount = 20 },
                new CraftingMaterial { materialType = MaterialType.LightEssence, amount = 20 }
            };
            legendaryWeapon.canBeBought = false;
            
            AssetDatabase.CreateAsset(legendaryWeapon, $"{itemPath}/Legendary_Dragonslayer.asset");
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            Debug.Log("[ItemCreator] Example craftable items created!");
            EditorUtility.DisplayDialog("Success", "Example craftable items created in:\n" + itemPath, "OK");
        }
    }
}

