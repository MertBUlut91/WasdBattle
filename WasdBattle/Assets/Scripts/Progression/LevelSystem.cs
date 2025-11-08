using System;
using UnityEngine;
using WasdBattle.Data;

namespace WasdBattle.Progression
{
    /// <summary>
    /// Level ve XP sistemini yöneten sınıf
    /// </summary>
    public class LevelSystem
    {
        // Events
        public event Action<int> OnLevelUp;
        public event Action<int> OnExperienceGained;
        
        /// <summary>
        /// Belirli bir level için gereken XP'yi hesaplar
        /// </summary>
        public static int GetXPRequiredForLevel(int level)
        {
            // Formül: 100 * level^1.5
            return Mathf.RoundToInt(100f * Mathf.Pow(level, 1.5f));
        }
        
        /// <summary>
        /// Toplam XP'den level hesaplar
        /// </summary>
        public static int CalculateLevelFromXP(int totalXP)
        {
            int level = 1;
            int xpForNextLevel = GetXPRequiredForLevel(level);
            int accumulatedXP = 0;
            
            while (accumulatedXP + xpForNextLevel <= totalXP)
            {
                accumulatedXP += xpForNextLevel;
                level++;
                xpForNextLevel = GetXPRequiredForLevel(level);
            }
            
            return level;
        }
        
        /// <summary>
        /// Mevcut level'daki progress'i hesaplar (0-1 arası)
        /// </summary>
        public static float GetLevelProgress(int totalXP, int currentLevel)
        {
            int xpForCurrentLevel = GetTotalXPForLevel(currentLevel - 1);
            int xpForNextLevel = GetTotalXPForLevel(currentLevel);
            
            int currentLevelXP = totalXP - xpForCurrentLevel;
            int requiredXP = xpForNextLevel - xpForCurrentLevel;
            
            return requiredXP > 0 ? (float)currentLevelXP / requiredXP : 0f;
        }
        
        /// <summary>
        /// Belirli bir level'a ulaşmak için gereken toplam XP
        /// </summary>
        public static int GetTotalXPForLevel(int level)
        {
            int totalXP = 0;
            for (int i = 1; i <= level; i++)
            {
                totalXP += GetXPRequiredForLevel(i);
            }
            return totalXP;
        }
        
        /// <summary>
        /// XP kazandırır ve level atlatır
        /// </summary>
        public void GainExperience(PlayerData playerData, int xpAmount)
        {
            int oldLevel = playerData.level;
            playerData.experience += xpAmount;
            
            OnExperienceGained?.Invoke(xpAmount);
            
            // Yeni level'i hesapla
            int newLevel = CalculateLevelFromXP(playerData.experience);
            
            if (newLevel > oldLevel)
            {
                playerData.level = newLevel;
                OnLevelUp?.Invoke(newLevel);
                
                Debug.Log($"[LevelSystem] Level Up! {oldLevel} -> {newLevel}");
                
                // Level up ödülleri
                GiveLevelUpRewards(playerData, newLevel);
            }
        }
        
        /// <summary>
        /// Level atlama ödüllerini verir
        /// </summary>
        private void GiveLevelUpRewards(PlayerData playerData, int level)
        {
            // Her level'da gold ver
            int goldReward = 50 + (level * 10);
            playerData.gold += goldReward;
            
            // Her 5 level'da ekstra ödül
            if (level % 5 == 0)
            {
                playerData.essence += 5;
                playerData.energyCrystal += 50;
                Debug.Log($"[LevelSystem] Milestone reward! Level {level}");
            }
            
            Debug.Log($"[LevelSystem] Rewards: {goldReward} gold");
        }
        
        /// <summary>
        /// Maç sonucu bazlı XP hesaplar
        /// </summary>
        public static int CalculateMatchXP(bool won, int roundsPlayed, float averageAccuracy)
        {
            int baseXP = won ? 100 : 50;
            int roundBonus = roundsPlayed * 10;
            int accuracyBonus = Mathf.RoundToInt(averageAccuracy * 50);
            
            return baseXP + roundBonus + accuracyBonus;
        }
    }
}

