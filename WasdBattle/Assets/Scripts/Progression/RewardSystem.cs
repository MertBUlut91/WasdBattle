using UnityEngine;
using WasdBattle.Data;

namespace WasdBattle.Progression
{
    /// <summary>
    /// Ödül sistemini yöneten sınıf
    /// </summary>
    public class RewardSystem
    {
        /// <summary>
        /// Maç sonu ödüllerini hesaplar ve verir
        /// </summary>
        public static MatchRewards CalculateMatchRewards(bool won, int roundsPlayed, float averageAccuracy, int opponentELO)
        {
            MatchRewards rewards = new MatchRewards();
            
            // Base rewards
            rewards.gold = won ? 50 : 25;
            rewards.metal = won ? 10 : 5;
            rewards.energyCrystal = won ? 5 : 2;
            
            // Round bonus
            rewards.gold += roundsPlayed * 5;
            rewards.metal += roundsPlayed;
            
            // Accuracy bonus
            if (averageAccuracy >= 0.9f)
            {
                rewards.gold += 20;
                rewards.energyCrystal += 3;
            }
            else if (averageAccuracy >= 0.75f)
            {
                rewards.gold += 10;
                rewards.energyCrystal += 1;
            }
            
            // Win streak bonus (bu özellik ileride eklenebilir)
            // if (winStreak >= 3)
            // {
            //     rewards.gold *= 1.5f;
            // }
            
            // Opponent ELO bonus (güçlü rakibi yenmek daha fazla ödül verir)
            if (won && opponentELO > 1500)
            {
                rewards.rune += 1;
                rewards.gold += 25;
            }
            
            // XP hesapla
            rewards.experience = LevelSystem.CalculateMatchXP(won, roundsPlayed, averageAccuracy);
            
            return rewards;
        }
        
        /// <summary>
        /// Ödülleri oyuncuya verir
        /// </summary>
        public static void GiveRewards(PlayerData playerData, MatchRewards rewards)
        {
            playerData.gold += rewards.gold;
            playerData.metal += rewards.metal;
            playerData.energyCrystal += rewards.energyCrystal;
            playerData.rune += rewards.rune;
            playerData.essence += rewards.essence;
            
            Debug.Log($"[RewardSystem] Rewards given: {rewards.gold} gold, {rewards.experience} XP");
        }
        
        /// <summary>
        /// Günlük görev ödülü
        /// </summary>
        public static MatchRewards GetDailyQuestReward(DailyQuestType questType)
        {
            MatchRewards rewards = new MatchRewards();
            
            switch (questType)
            {
                case DailyQuestType.PlayMatches:
                    rewards.gold = 100;
                    rewards.experience = 50;
                    break;
                case DailyQuestType.WinMatches:
                    rewards.gold = 150;
                    rewards.energyCrystal = 10;
                    break;
                case DailyQuestType.PerfectCombos:
                    rewards.rune = 5;
                    rewards.gold = 75;
                    break;
            }
            
            return rewards;
        }
    }
    
    /// <summary>
    /// Maç ödülleri yapısı
    /// </summary>
    [System.Serializable]
    public struct MatchRewards
    {
        public int gold;
        public int metal;
        public int energyCrystal;
        public int rune;
        public int essence;
        public int experience;
        
        public override string ToString()
        {
            return $"Gold: {gold}, Metal: {metal}, Crystal: {energyCrystal}, Rune: {rune}, Essence: {essence}, XP: {experience}";
        }
    }
    
    public enum DailyQuestType
    {
        PlayMatches,
        WinMatches,
        PerfectCombos
    }
}

