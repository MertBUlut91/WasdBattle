using UnityEngine;
using System;

namespace WasdBattle.Data
{
    /// <summary>
    /// Equipment item data (ScriptableObject)
    /// Kask, Gövde, Ellik, Bacak, Silah, Yüzük, Kolye, Bileklik
    /// </summary>
    [CreateAssetMenu(fileName = "New Item", menuName = "WasdBattle/Item Data")]
    public class ItemData : ScriptableObject
    {
        [Header("Basic Info")]
        public string itemId;
        public string itemName;
        [TextArea(3, 5)]
        public string description;
        public Sprite icon;
        public GameObject prefab; // 3D model for character
        
        [Header("Item Properties")]
        public EquipmentSlot slot;
        public ItemClass requiredClass; // Hangi class giyebilir
        public ItemRarity rarity;
        public int level; // Minimum level requirement
        
        [Header("Stats")]
        public int healthBonus;
        public int staminaBonus;
        public int damageBonus;
        public int armorBonus;              // Zırh (fiziksel savunma)
        public int magicResistanceBonus;    // Büyü direnci
        public float critChanceBonus;       // 0.0 - 1.0
        public float critDamageBonus;       // 0.0 - 1.0
        
        [Header("Crafting")]
        public bool canBeCrafted;
        public CraftingMaterial[] craftingMaterials; // Genişletilebilir material listesi
        
        [Header("Shop")]
        public bool canBeBought;
        public int shopPrice;
        
        /// <summary>
        /// Bu item'i belirtilen class giyebilir mi?
        /// </summary>
        public bool CanBeEquippedBy(ItemClass characterClass)
        {
            return requiredClass == ItemClass.All || requiredClass == characterClass;
        }
        
        /// <summary>
        /// Toplam stat bonusu (UI'da göstermek için)
        /// </summary>
        public int TotalStatBonus => healthBonus + staminaBonus + damageBonus + armorBonus + magicResistanceBonus;
        
        /// <summary>
        /// Bu item'i craft etmek için gereken toplam maliyet (UI için)
        /// </summary>
        public string GetCraftingCostSummary()
        {
            if (!canBeCrafted || craftingMaterials == null || craftingMaterials.Length == 0)
                return "Cannot be crafted";
            
            string summary = "";
            foreach (var material in craftingMaterials)
            {
                if (material.amount > 0)
                {
                    summary += $"{material.materialType}: {material.amount}\n";
                }
            }
            return summary.TrimEnd('\n');
        }
    }
    
    /// <summary>
    /// Crafting material entry (genişletilebilir)
    /// </summary>
    [System.Serializable]
    public class CraftingMaterial
    {
        public MaterialType materialType;
        public int amount;
    }
    
    /// <summary>
    /// Equipment slot types
    /// </summary>
    public enum EquipmentSlot
    {
        Helmet,      // Kask
        Chest,       // Gövdelik
        Gloves,      // Ellik
        Legs,        // Bacaklık
        Weapon,      // Silah
        Ring1,       // Yüzük 1
        Ring2,       // Yüzük 2
        Necklace,    // Kolye
        Bracelet     // Bileklik
    }
    
    /// <summary>
    /// Item class restrictions
    /// </summary>
    public enum ItemClass
    {
        All,      // Herkes giyebilir
        Mage,     // Sadece Mage
        Warrior,  // Sadece Warrior
        Ninja     // Sadece Ninja
    }
    
    /// <summary>
    /// Item rarity
    /// </summary>
    public enum ItemRarity
    {
        Common,    // Gri
        Uncommon,  // Yeşil
        Rare,      // Mavi
        Epic,      // Mor
        Legendary  // Turuncu
    }
    
    /// <summary>
    /// Crafting material types (genişletilebilir)
    /// </summary>
    public enum MaterialType
    {
        Metal,          // Metal
        EnergyCrystal,  // Enerji Kristali
        Rune,           // Rün
        Essence,        // Öz
        Leather,        // Deri
        Cloth,          // Kumaş
        Wood,           // Tahta
        GemStone,       // Mücevher (renamed from Gem to avoid conflict with currency)
        DarkEssence,    // Karanlık Öz
        LightEssence    // Işık Öz
    }
}

