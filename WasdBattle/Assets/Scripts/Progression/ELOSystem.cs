using System;
using UnityEngine;
using WasdBattle.Data;

namespace WasdBattle.Progression
{
    /// <summary>
    /// ELO rating sistemini yöneten sınıf
    /// </summary>
    public class ELOSystem
    {
        // K-factor: ELO değişim hızı
        private const float K_FACTOR = 32f;
        
        // Events
        public event Action<int> OnELOChanged;
        public event Action<Rank> OnRankChanged;
        
        /// <summary>
        /// Maç sonucuna göre ELO günceller
        /// </summary>
        public void UpdateELO(PlayerData player, PlayerData opponent, bool playerWon)
        {
            int oldELO = player.elo;
            
            // Beklenen skor hesapla
            float expectedScore = CalculateExpectedScore(player.elo, opponent.elo);
            
            // Gerçek skor (1 = kazandı, 0 = kaybetti)
            float actualScore = playerWon ? 1f : 0f;
            
            // ELO değişimi
            int eloChange = Mathf.RoundToInt(K_FACTOR * (actualScore - expectedScore));
            
            player.elo += eloChange;
            
            // Minimum ELO 0
            player.elo = Mathf.Max(0, player.elo);
            
            Debug.Log($"[ELOSystem] ELO changed: {oldELO} -> {player.elo} ({(eloChange >= 0 ? "+" : "")}{eloChange})");
            
            OnELOChanged?.Invoke(player.elo);
            
            // Rank değişimi kontrol et
            Rank oldRank = GetRank(oldELO);
            Rank newRank = GetRank(player.elo);
            
            if (oldRank != newRank)
            {
                OnRankChanged?.Invoke(newRank);
                Debug.Log($"[ELOSystem] Rank changed: {oldRank} -> {newRank}");
            }
        }
        
        /// <summary>
        /// Beklenen skoru hesaplar (0-1 arası)
        /// </summary>
        private float CalculateExpectedScore(int playerELO, int opponentELO)
        {
            return 1f / (1f + Mathf.Pow(10f, (opponentELO - playerELO) / 400f));
        }
        
        /// <summary>
        /// ELO'ya göre rank döndürür
        /// </summary>
        public static Rank GetRank(int elo)
        {
            if (elo < 800)
                return Rank.Bronze;
            else if (elo < 1200)
                return Rank.Silver;
            else if (elo < 1600)
                return Rank.Gold;
            else if (elo < 2000)
                return Rank.Platinum;
            else if (elo < 2400)
                return Rank.Diamond;
            else
                return Rank.Master;
        }
        
        /// <summary>
        /// Rank'in renk kodunu döndürür
        /// </summary>
        public static Color GetRankColor(Rank rank)
        {
            switch (rank)
            {
                case Rank.Bronze:
                    return new Color(0.8f, 0.5f, 0.2f);
                case Rank.Silver:
                    return new Color(0.75f, 0.75f, 0.75f);
                case Rank.Gold:
                    return new Color(1f, 0.84f, 0f);
                case Rank.Platinum:
                    return new Color(0.5f, 0.9f, 0.9f);
                case Rank.Diamond:
                    return new Color(0.7f, 0.4f, 1f);
                case Rank.Master:
                    return new Color(1f, 0.2f, 0.2f);
                default:
                    return Color.white;
            }
        }
        
        /// <summary>
        /// Rank'in string temsilini döndürür
        /// </summary>
        public static string GetRankDisplayName(Rank rank)
        {
            switch (rank)
            {
                case Rank.Bronze:
                    return "Bronz";
                case Rank.Silver:
                    return "Gümüş";
                case Rank.Gold:
                    return "Altın";
                case Rank.Platinum:
                    return "Platin";
                case Rank.Diamond:
                    return "Elmas";
                case Rank.Master:
                    return "Usta";
                default:
                    return "Bilinmeyen";
            }
        }
        
        /// <summary>
        /// Bir sonraki rank için gereken ELO'yu döndürür
        /// </summary>
        public static int GetELOForNextRank(Rank currentRank)
        {
            switch (currentRank)
            {
                case Rank.Bronze:
                    return 800;
                case Rank.Silver:
                    return 1200;
                case Rank.Gold:
                    return 1600;
                case Rank.Platinum:
                    return 2000;
                case Rank.Diamond:
                    return 2400;
                case Rank.Master:
                    return int.MaxValue;
                default:
                    return 0;
            }
        }
    }
    
    public enum Rank
    {
        Bronze,
        Silver,
        Gold,
        Platinum,
        Diamond,
        Master
    }
}

