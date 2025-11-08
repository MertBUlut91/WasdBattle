using UnityEngine;
using WasdBattle.Input;

namespace WasdBattle.Combat
{
    /// <summary>
    /// Hasar hesaplama utility sınıfı
    /// </summary>
    public static class DamageCalculator
    {
        /// <summary>
        /// Final hasarı hesaplar
        /// </summary>
        /// <param name="baseDamage">Temel hasar</param>
        /// <param name="attackAccuracy">Saldırı accuracy (0-1)</param>
        /// <param name="defenseAccuracy">Savunma accuracy (0-1)</param>
        /// <param name="damageMultiplier">Ekstra hasar çarpanı</param>
        /// <param name="defenseMultiplier">Savunma çarpanı</param>
        /// <returns>Final hasar değeri</returns>
        public static int CalculateFinalDamage(
            float baseDamage,
            float attackAccuracy,
            float defenseAccuracy,
            float damageMultiplier = 1f,
            float defenseMultiplier = 1f)
        {
            // Saldırı accuracy'sine göre hasar
            float damage = baseDamage * attackAccuracy * damageMultiplier;
            
            // Savunma accuracy'sine göre hasar azaltma
            float damageReduction = defenseAccuracy;
            damage *= (1f - damageReduction);
            
            // Savunma multiplier (debuff'lar için)
            damage *= defenseMultiplier;
            
            return Mathf.Max(0, Mathf.RoundToInt(damage));
        }
        
        /// <summary>
        /// Combo grade'e göre bonus hasar hesaplar
        /// </summary>
        public static float GetGradeMultiplier(ComboGrade grade)
        {
            switch (grade)
            {
                case ComboGrade.Perfect:
                    return 1.2f; // %20 bonus
                case ComboGrade.Excellent:
                    return 1.1f; // %10 bonus
                case ComboGrade.Good:
                    return 1.0f;
                case ComboGrade.Partial:
                    return 0.8f; // %20 azalma
                case ComboGrade.Failed:
                    return 0.5f; // %50 azalma
                default:
                    return 1.0f;
            }
        }
        
        /// <summary>
        /// Kritik vuruş kontrolü
        /// </summary>
        public static bool RollCritical(float critChance)
        {
            return Random.value < critChance;
        }
        
        /// <summary>
        /// Kritik vuruş hasarı
        /// </summary>
        public static int ApplyCritical(int baseDamage, float critMultiplier = 1.5f)
        {
            return Mathf.RoundToInt(baseDamage * critMultiplier);
        }
    }
}

