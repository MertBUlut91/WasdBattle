using UnityEngine;
using WasdBattle.Characters;

namespace WasdBattle.Skills
{
    /// <summary>
    /// Stamina çalma efekti
    /// </summary>
    public class StaminaDrainEffect : ISkillEffect
    {
        public string EffectName => "Stamina Drain";
        
        public void Apply(PlayerCharacter caster, PlayerCharacter target, float effectStrength)
        {
            int drainAmount = Mathf.RoundToInt(20 * effectStrength);
            target.ModifyStamina(-drainAmount);
            caster.ModifyStamina(drainAmount / 2); // Yarısını geri al
            
            Debug.Log($"[StaminaDrain] {caster.CharacterName} drained {drainAmount} stamina from {target.CharacterName}");
        }
    }
    
    /// <summary>
    /// Savunma kırma efekti
    /// </summary>
    public class DefenseBreakEffect : ISkillEffect
    {
        public string EffectName => "Defense Break";
        
        public void Apply(PlayerCharacter caster, PlayerCharacter target, float effectStrength)
        {
            // Bir sonraki tura kadar savunma azaltılır
            float duration = 1f + effectStrength;
            target.ApplyDefenseDebuff(0.5f, duration);
            
            Debug.Log($"[DefenseBreak] {target.CharacterName}'s defense broken for {duration}s");
        }
    }
    
    /// <summary>
    /// Combo karıştırma efekti (rakibin combo'sunu zorlaştırır)
    /// </summary>
    public class ComboScrambleEffect : ISkillEffect
    {
        public string EffectName => "Combo Scramble";
        
        public void Apply(PlayerCharacter caster, PlayerCharacter target, float effectStrength)
        {
            // Bu efekt UI tarafında combo gösterimini zorlaştırır
            float duration = 2f + effectStrength;
            target.ApplyComboScramble(duration);
            
            Debug.Log($"[ComboScramble] {target.CharacterName}'s combo vision scrambled for {duration}s");
        }
    }
    
    /// <summary>
    /// Hasar artırma efekti
    /// </summary>
    public class DamageBoostEffect : ISkillEffect
    {
        public string EffectName => "Damage Boost";
        
        public void Apply(PlayerCharacter caster, PlayerCharacter target, float effectStrength)
        {
            float duration = 3f;
            float boostAmount = 0.25f + (effectStrength * 0.25f);
            caster.ApplyDamageBoost(boostAmount, duration);
            
            Debug.Log($"[DamageBoost] {caster.CharacterName} gained {boostAmount:P} damage boost for {duration}s");
        }
    }
    
    /// <summary>
    /// Şifa efekti
    /// </summary>
    public class HealEffect : ISkillEffect
    {
        public string EffectName => "Heal";
        
        public void Apply(PlayerCharacter caster, PlayerCharacter target, float effectStrength)
        {
            int healAmount = Mathf.RoundToInt(30 * effectStrength);
            caster.ModifyHealth(healAmount);
            
            Debug.Log($"[Heal] {caster.CharacterName} healed {healAmount} HP");
        }
    }
}

