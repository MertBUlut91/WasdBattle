using System;
using System.Collections.Generic;
using UnityEngine;
using WasdBattle.Data;

namespace WasdBattle.Economy
{
    /// <summary>
    /// Crafting sistemini yöneten sınıf
    /// </summary>
    public class CraftingSystem
    {
        private InventoryManager _inventory;
        
        // Events
        public event Action<CraftRecipe> OnCraftSuccess;
        public event Action<string> OnCraftFailed;
        
        public CraftingSystem(InventoryManager inventory)
        {
            _inventory = inventory;
        }
        
        /// <summary>
        /// Craft yapılabilir mi kontrol eder
        /// </summary>
        public bool CanCraft(CraftRecipe recipe)
        {
            if (recipe == null)
                return false;
            
            // Gold kontrolü
            if (!_inventory.SpendGold(0)) // Sadece kontrol için
            {
                if (_inventory.GetMaterialAmount(MaterialType.Metal) < recipe.goldCost) // Workaround
                    return false;
            }
            
            // Malzeme kontrolü
            foreach (var material in recipe.requiredMaterials)
            {
                if (!_inventory.HasMaterial(material.materialType, material.amount))
                {
                    return false;
                }
            }
            
            return true;
        }
        
        /// <summary>
        /// Craft yapar
        /// </summary>
        public bool Craft(CraftRecipe recipe)
        {
            if (!CanCraft(recipe))
            {
                OnCraftFailed?.Invoke("Yetersiz malzeme!");
                return false;
            }
            
            // Malzemeleri tüket
            foreach (var material in recipe.requiredMaterials)
            {
                _inventory.RemoveMaterial(material.materialType, material.amount);
            }
            
            // Gold harca
            if (recipe.goldCost > 0)
            {
                _inventory.SpendGold(recipe.goldCost);
            }
            
            // Sonucu ver
            GiveCraftResult(recipe);
            
            OnCraftSuccess?.Invoke(recipe);
            Debug.Log($"[Crafting] Crafted: {recipe.recipeName}");
            
            return true;
        }
        
        /// <summary>
        /// Craft sonucunu verir
        /// </summary>
        private void GiveCraftResult(CraftRecipe recipe)
        {
            switch (recipe.resultType)
            {
                case CraftResultType.Skill:
                    _inventory.AddSkill(recipe.resultId);
                    break;
                    
                case CraftResultType.SkillUpgrade:
                    // Skill upgrade logic (ileride implement edilecek)
                    Debug.Log($"[Crafting] Skill upgraded: {recipe.resultId}");
                    break;
                    
                case CraftResultType.Equipment:
                    // Equipment logic (ileride implement edilecek)
                    Debug.Log($"[Crafting] Equipment crafted: {recipe.resultId}");
                    break;
                    
                case CraftResultType.Consumable:
                    // Consumable logic (ileride implement edilecek)
                    Debug.Log($"[Crafting] Consumable crafted: {recipe.resultId}");
                    break;
            }
        }
        
        /// <summary>
        /// Eksik malzemeleri listeler
        /// </summary>
        public List<string> GetMissingMaterials(CraftRecipe recipe)
        {
            List<string> missing = new List<string>();
            
            foreach (var material in recipe.requiredMaterials)
            {
                int have = _inventory.GetMaterialAmount(material.materialType);
                int need = material.amount;
                
                if (have < need)
                {
                    missing.Add($"{material.materialType}: {have}/{need}");
                }
            }
            
            return missing;
        }
    }
}

