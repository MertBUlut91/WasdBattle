using UnityEngine;
using WasdBattle.Economy;

namespace WasdBattle.Data
{
    /// <summary>
    /// Craft tarifi ScriptableObject
    /// </summary>
    [CreateAssetMenu(fileName = "NewRecipe", menuName = "WasdBattle/Craft Recipe")]
    public class CraftRecipe : ScriptableObject
    {
        [Header("Recipe Info")]
        public string recipeId;
        public string recipeName;
        
        [TextArea(2, 4)]
        public string description;
        
        [Header("Requirements")]
        public CraftMaterial[] requiredMaterials;
        public int goldCost;
        
        [Header("Result")]
        public CraftResultType resultType;
        public string resultId; // Skill ID veya Item ID
        public int resultAmount = 1;
        
        [Header("Visual")]
        public Sprite icon;
        
        /// <summary>
        /// Tarif maliyetini string olarak döndürür
        /// </summary>
        public string GetCostString()
        {
            string cost = "";
            
            foreach (var mat in requiredMaterials)
            {
                cost += $"{mat.amount} {mat.materialType}, ";
            }
            
            if (goldCost > 0)
            {
                cost += $"{goldCost} Gold";
            }
            
            return cost.TrimEnd(',', ' ');
        }
    }
    
    [System.Serializable]
    public struct CraftMaterial
    {
        public MaterialType materialType;
        public int amount;
    }
    
    public enum CraftResultType
    {
        Skill,
        SkillUpgrade,
        Equipment,
        Consumable
    }
}

