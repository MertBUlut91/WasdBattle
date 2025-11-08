using UnityEngine;
using WasdBattle.Skills;

namespace WasdBattle.Data
{
    /// <summary>
    /// Skill verilerini tutan ScriptableObject
    /// </summary>
    [CreateAssetMenu(fileName = "NewSkill", menuName = "WasdBattle/Skill Data")]
    public class SkillData : ScriptableObject
    {
        [Header("Basic Info")]
        public string skillId;
        public string skillName;
        public int skillLevel = 1;
        
        [TextArea(2, 4)]
        public string description;
        
        [Header("Type & Stats")]
        public SkillType skillType;
        public int staminaCost = 20;
        public int baseDamage = 30;
        public float cooldown = 0f; // 0 = cooldown yok
        
        [Header("Combo")]
        public ComboData comboData;
        
        [Header("Visual")]
        public Sprite icon;
        public Color skillColor = Color.white;
        
        [Header("Special Effects")]
        public SkillEffectType[] specialEffects;
        public float effectStrength = 1f;
        
        [Header("Rarity")]
        public SkillRarity rarity = SkillRarity.Common;
        
        /// <summary>
        /// Skill'in efektlerini uygular
        /// </summary>
        public ISkillEffect[] GetEffects()
        {
            if (specialEffects == null || specialEffects.Length == 0)
                return new ISkillEffect[0];
            
            ISkillEffect[] effects = new ISkillEffect[specialEffects.Length];
            for (int i = 0; i < specialEffects.Length; i++)
            {
                effects[i] = CreateEffect(specialEffects[i]);
            }
            
            return effects;
        }
        
        private ISkillEffect CreateEffect(SkillEffectType type)
        {
            switch (type)
            {
                case SkillEffectType.StaminaDrain:
                    return new StaminaDrainEffect();
                case SkillEffectType.DefenseBreak:
                    return new DefenseBreakEffect();
                case SkillEffectType.ComboScramble:
                    return new ComboScrambleEffect();
                case SkillEffectType.DamageBoost:
                    return new DamageBoostEffect();
                case SkillEffectType.Heal:
                    return new HealEffect();
                default:
                    return null;
            }
        }
    }
    
    public enum SkillType
    {
        Fast,      // Hızlı, düşük hasar, kısa combo
        Heavy,     // Yavaş, yüksek hasar, uzun combo
        Special,   // Özel efektli
        Ultimate   // Ultimate skill
    }
    
    public enum SkillEffectType
    {
        None,
        StaminaDrain,
        DefenseBreak,
        ComboScramble,
        DamageBoost,
        Heal
    }
    
    public enum SkillRarity
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary
    }
}

