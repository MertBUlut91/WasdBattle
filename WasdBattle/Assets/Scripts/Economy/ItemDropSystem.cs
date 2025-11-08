using UnityEngine;
using System.Collections.Generic;
using WasdBattle.Data;

namespace WasdBattle.Economy
{
    /// <summary>
    /// Match sonrası random item drop sistemi
    /// Rarity'ye göre drop rate'ler
    /// </summary>
    public class ItemDropSystem
    {
        // Drop rate'ler (rarity'ye göre)
        private static readonly Dictionary<ItemRarity, float> DropRates = new Dictionary<ItemRarity, float>
        {
            { ItemRarity.Common, 0.50f },      // %50
            { ItemRarity.Uncommon, 0.30f },    // %30
            { ItemRarity.Rare, 0.15f },        // %15
            { ItemRarity.Epic, 0.04f },        // %4
            { ItemRarity.Legendary, 0.01f }    // %1
        };
        
        // Match sonrası drop sayısı (ELO'ya göre artabilir)
        private const int BaseDropCount = 1;
        private const int MaxDropCount = 3;
        
        /// <summary>
        /// Match sonrası item drop'u
        /// </summary>
        public static List<ItemData> RollDrops(PlayerData playerData, bool won, List<ItemData> availableItems)
        {
            List<ItemData> droppedItems = new List<ItemData>();
            
            // Drop sayısını belirle (kazanırsa daha fazla)
            int dropCount = CalculateDropCount(playerData, won);
            
            for (int i = 0; i < dropCount; i++)
            {
                ItemData item = RollSingleDrop(availableItems);
                if (item != null)
                {
                    droppedItems.Add(item);
                }
            }
            
            Debug.Log($"[ItemDrop] Dropped {droppedItems.Count} items. Won: {won}, ELO: {playerData.elo}");
            return droppedItems;
        }
        
        /// <summary>
        /// Tek bir item drop'u (rarity'ye göre weighted random)
        /// </summary>
        private static ItemData RollSingleDrop(List<ItemData> availableItems)
        {
            if (availableItems == null || availableItems.Count == 0)
                return null;
            
            // Önce rarity belirle
            ItemRarity rolledRarity = RollRarity();
            
            // Bu rarity'deki itemleri filtrele
            List<ItemData> itemsOfRarity = availableItems.FindAll(item => item.rarity == rolledRarity);
            
            // Eğer bu rarity'de item yoksa, bir alt rarity'ye düş
            if (itemsOfRarity.Count == 0)
            {
                itemsOfRarity = availableItems.FindAll(item => item.rarity < rolledRarity);
                if (itemsOfRarity.Count == 0)
                {
                    // Hiç item yok, herhangi birini ver
                    return availableItems[Random.Range(0, availableItems.Count)];
                }
            }
            
            // Random bir item seç
            return itemsOfRarity[Random.Range(0, itemsOfRarity.Count)];
        }
        
        /// <summary>
        /// Rarity roll (weighted random)
        /// </summary>
        private static ItemRarity RollRarity()
        {
            float roll = Random.value; // 0.0 - 1.0
            float cumulative = 0f;
            
            // Legendary'den başla (en düşük şans)
            foreach (var kvp in DropRates)
            {
                cumulative += kvp.Value;
                if (roll <= cumulative)
                {
                    return kvp.Key;
                }
            }
            
            // Fallback (olmaması gereken durum)
            return ItemRarity.Common;
        }
        
        /// <summary>
        /// Drop sayısını hesapla (ELO ve kazanma durumuna göre)
        /// </summary>
        private static int CalculateDropCount(PlayerData playerData, bool won)
        {
            int dropCount = BaseDropCount;
            
            // Kazanırsa +1
            if (won)
                dropCount++;
            
            // Yüksek ELO'da bonus drop şansı
            if (playerData.elo >= 2000) // Diamond+
            {
                if (Random.value < 0.3f) // %30 şans
                    dropCount++;
            }
            else if (playerData.elo >= 1500) // Platinum+
            {
                if (Random.value < 0.15f) // %15 şans
                    dropCount++;
            }
            
            return Mathf.Min(dropCount, MaxDropCount);
        }
        
        /// <summary>
        /// Material drop (match sonrası her zaman verilir)
        /// </summary>
        public static Dictionary<MaterialType, int> RollMaterialDrops(PlayerData playerData, bool won)
        {
            Dictionary<MaterialType, int> materials = new Dictionary<MaterialType, int>();
            
            // Base material drops
            materials[MaterialType.Metal] = Random.Range(5, 15);
            materials[MaterialType.EnergyCrystal] = Random.Range(3, 10);
            materials[MaterialType.Rune] = Random.Range(1, 5);
            
            // Kazanırsa bonus
            if (won)
            {
                materials[MaterialType.Essence] = Random.Range(1, 3);
                
                // Yüksek ELO'da rare material şansı
                if (playerData.elo >= 1500)
                {
                    if (Random.value < 0.2f)
                        materials[MaterialType.GemStone] = Random.Range(1, 2);
                }
            }
            
            Debug.Log($"[MaterialDrop] Dropped materials. Won: {won}, ELO: {playerData.elo}");
            return materials;
        }
        
        /// <summary>
        /// Gold drop (match sonrası)
        /// </summary>
        public static int RollGoldDrop(PlayerData playerData, bool won)
        {
            int baseGold = 50;
            
            // Kazanırsa 2x
            if (won)
                baseGold *= 2;
            
            // ELO'ya göre bonus
            int eloBonus = Mathf.RoundToInt(playerData.elo / 100f);
            
            int totalGold = baseGold + eloBonus + Random.Range(-10, 20);
            
            Debug.Log($"[GoldDrop] Dropped {totalGold} gold. Won: {won}, ELO: {playerData.elo}");
            return Mathf.Max(totalGold, 10); // Minimum 10 gold
        }
    }
}

