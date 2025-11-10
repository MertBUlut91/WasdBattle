using UnityEngine;
using UnityEditor;
using WasdBattle.Data;
using System.IO;

namespace WasdBattle.Editor
{
    /// <summary>
    /// Editor tool to create test items
    /// </summary>
    public class ItemCreator : EditorWindow
    {
        [MenuItem("WasdBattle/Create Test Items")]
        public static void CreateTestItems()
        {
            string itemsPath = "Assets/Resources/Items";
            
            // Klasör yoksa oluştur
            if (!Directory.Exists(itemsPath))
            {
                Directory.CreateDirectory(itemsPath);
            }
            
            // Test itemleri oluştur
            CreateWeapons(itemsPath);
            CreateArmor(itemsPath);
            CreateAccessories(itemsPath);
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            Debug.Log("[ItemCreator] Test items created successfully!");
        }
        
        private static void CreateWeapons(string path)
        {
            // Warrior Sword
            CreateItem(path, "item_warrior_sword", "Warrior's Blade", EquipmentSlot.Weapon, 
                ItemClass.Warrior, ItemRarity.Common, 
                hpBonus: 0, staminaBonus: 0, armorBonus: 5, magicResistBonus: 0);
            
            // Mage Staff
            CreateItem(path, "item_mage_staff", "Mage's Staff", EquipmentSlot.Weapon, 
                ItemClass.Mage, ItemRarity.Common, 
                hpBonus: 0, staminaBonus: 10, armorBonus: 0, magicResistBonus: 5);
            
            // Ninja Dagger
            CreateItem(path, "item_ninja_dagger", "Ninja's Dagger", EquipmentSlot.Weapon, 
                ItemClass.Ninja, ItemRarity.Common, 
                hpBonus: 0, staminaBonus: 15, armorBonus: 0, magicResistBonus: 0);
            
            // Legendary Sword
            CreateItem(path, "item_legendary_sword", "Legendary Excalibur", EquipmentSlot.Weapon, 
                ItemClass.Warrior, ItemRarity.Legendary, 
                hpBonus: 50, staminaBonus: 30, armorBonus: 20, magicResistBonus: 10);
        }
        
        private static void CreateArmor(string path)
        {
            // Warrior Helmet
            CreateItem(path, "item_warrior_helmet", "Iron Helmet", EquipmentSlot.Helmet, 
                ItemClass.Warrior, ItemRarity.Common, 
                hpBonus: 20, staminaBonus: 0, armorBonus: 10, magicResistBonus: 0);
            
            // Warrior Chest
            CreateItem(path, "item_warrior_chest", "Iron Chestplate", EquipmentSlot.Chest, 
                ItemClass.Warrior, ItemRarity.Common, 
                hpBonus: 30, staminaBonus: 0, armorBonus: 15, magicResistBonus: 0);
            
            // Warrior Gloves
            CreateItem(path, "item_warrior_gloves", "Iron Gauntlets", EquipmentSlot.Gloves, 
                ItemClass.Warrior, ItemRarity.Common, 
                hpBonus: 10, staminaBonus: 0, armorBonus: 8, magicResistBonus: 0);
            
            // Warrior Legs
            CreateItem(path, "item_warrior_legs", "Iron Greaves", EquipmentSlot.Legs, 
                ItemClass.Warrior, ItemRarity.Common, 
                hpBonus: 20, staminaBonus: 0, armorBonus: 12, magicResistBonus: 0);
            
            // Mage Helmet
            CreateItem(path, "item_mage_helmet", "Mystic Hood", EquipmentSlot.Helmet, 
                ItemClass.Mage, ItemRarity.Common, 
                hpBonus: 10, staminaBonus: 10, armorBonus: 0, magicResistBonus: 10);
            
            // Mage Chest
            CreateItem(path, "item_mage_chest", "Mystic Robe", EquipmentSlot.Chest, 
                ItemClass.Mage, ItemRarity.Common, 
                hpBonus: 15, staminaBonus: 15, armorBonus: 0, magicResistBonus: 15);
            
            // Ninja Helmet
            CreateItem(path, "item_ninja_helmet", "Leather Cap", EquipmentSlot.Helmet, 
                ItemClass.Ninja, ItemRarity.Common, 
                hpBonus: 15, staminaBonus: 10, armorBonus: 5, magicResistBonus: 5);
            
            // Ninja Chest
            CreateItem(path, "item_ninja_chest", "Leather Armor", EquipmentSlot.Chest, 
                ItemClass.Ninja, ItemRarity.Common, 
                hpBonus: 20, staminaBonus: 15, armorBonus: 8, magicResistBonus: 8);
        }
        
        private static void CreateAccessories(string path)
        {
            // Common Ring
            CreateItem(path, "item_ring_common", "Simple Ring", EquipmentSlot.Ring1, 
                ItemClass.All, ItemRarity.Common, 
                hpBonus: 5, staminaBonus: 5, armorBonus: 2, magicResistBonus: 2);
            
            // Rare Ring
            CreateItem(path, "item_ring_rare", "Enchanted Ring", EquipmentSlot.Ring1, 
                ItemClass.All, ItemRarity.Rare, 
                hpBonus: 15, staminaBonus: 15, armorBonus: 5, magicResistBonus: 5);
            
            // Common Necklace
            CreateItem(path, "item_necklace_common", "Simple Necklace", EquipmentSlot.Necklace, 
                ItemClass.All, ItemRarity.Common, 
                hpBonus: 10, staminaBonus: 5, armorBonus: 3, magicResistBonus: 3);
            
            // Epic Necklace
            CreateItem(path, "item_necklace_epic", "Dragon's Pendant", EquipmentSlot.Necklace, 
                ItemClass.All, ItemRarity.Epic, 
                hpBonus: 30, staminaBonus: 20, armorBonus: 10, magicResistBonus: 10);
            
            // Common Bracelet
            CreateItem(path, "item_bracelet_common", "Simple Bracelet", EquipmentSlot.Bracelet, 
                ItemClass.All, ItemRarity.Common, 
                hpBonus: 8, staminaBonus: 8, armorBonus: 4, magicResistBonus: 4);
        }
        
        private static void CreateItem(string path, string id, string itemName, EquipmentSlot slot, 
            ItemClass requiredClass, ItemRarity rarity, 
            int hpBonus, int staminaBonus, int armorBonus, int magicResistBonus)
        {
            ItemData item = ScriptableObject.CreateInstance<ItemData>();
            item.itemId = id;
            item.itemName = itemName;
            item.slot = slot;
            item.requiredClass = requiredClass;
            item.rarity = rarity;
            item.healthBonus = hpBonus;
            item.staminaBonus = staminaBonus;
            item.armorBonus = armorBonus;
            item.magicResistanceBonus = magicResistBonus;
            item.description = $"A {rarity} {slot} for {requiredClass}";
            
            string assetPath = $"{path}/{id}.asset";
            AssetDatabase.CreateAsset(item, assetPath);
            Debug.Log($"[ItemCreator] Created: {itemName} at {assetPath}");
        }
    }
}
