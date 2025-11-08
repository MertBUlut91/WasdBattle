using UnityEngine;

namespace WasdBattle.Data
{
    /// <summary>
    /// Pasif yetenek verilerini tutan ScriptableObject
    /// </summary>
    [CreateAssetMenu(fileName = "NewPassive", menuName = "WasdBattle/Passive Ability")]
    public class PassiveAbilityData : ScriptableObject
    {
        [Header("Basic Info")]
        public string passiveName;
        
        [TextArea(2, 4)]
        public string description;
        
        [Header("Effect Type")]
        public PassiveType passiveType;
        
        [Header("Values")]
        public float value1;
        public float value2;
        
        [Header("Visual")]
        public Sprite icon;
    }
    
    public enum PassiveType
    {
        HealthBoost,        // Max HP artışı
        StaminaBoost,       // Max Stamina artışı
        StaminaRegen,       // Stamina regen hızı artışı
        DamageBoost,        // Hasar artışı
        DefenseBoost,       // Savunma artışı
        ComboTimeBoost,     // Combo süresi artışı
        CooldownReduction,  // Cooldown azaltma
        CriticalChance      // Kritik vuruş şansı
    }
}

