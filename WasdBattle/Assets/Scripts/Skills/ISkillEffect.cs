using WasdBattle.Characters;

namespace WasdBattle.Skills
{
    /// <summary>
    /// Skill efektleri için interface
    /// </summary>
    public interface ISkillEffect
    {
        string EffectName { get; }
        void Apply(PlayerCharacter caster, PlayerCharacter target, float effectStrength);
    }
}

