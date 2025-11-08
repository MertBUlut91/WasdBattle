using System.Collections.Generic;
using UnityEngine;
using WasdBattle.Characters;
using WasdBattle.Data;

namespace WasdBattle.Skills
{
    /// <summary>
    /// Skill kullanımını ve cooldown'ları yöneten sınıf
    /// </summary>
    public class SkillManager : MonoBehaviour
    {
        [Header("Equipped Skills")]
        [SerializeField] private SkillData[] _equippedSkills = new SkillData[3];
        
        [Header("Cooldowns")]
        private Dictionary<string, float> _cooldowns = new Dictionary<string, float>();
        
        private PlayerCharacter _owner;
        
        public SkillData[] EquippedSkills => _equippedSkills;
        
        private void Awake()
        {
            _owner = GetComponent<PlayerCharacter>();
        }
        
        private void Update()
        {
            UpdateCooldowns();
        }
        
        /// <summary>
        /// Skill kullanılabilir mi kontrol eder
        /// </summary>
        public bool CanUseSkill(SkillData skill)
        {
            if (skill == null)
                return false;
            
            // Stamina kontrolü
            if (_owner != null && _owner.CurrentStamina < skill.staminaCost)
                return false;
            
            // Cooldown kontrolü
            if (IsOnCooldown(skill))
                return false;
            
            return true;
        }
        
        /// <summary>
        /// Skill'i kullanır (stamina tüketir ve cooldown başlatır)
        /// </summary>
        public bool UseSkill(SkillData skill)
        {
            if (!CanUseSkill(skill))
                return false;
            
            // Stamina tüket
            if (_owner != null)
            {
                _owner.ModifyStamina(-skill.staminaCost);
            }
            
            // Cooldown başlat
            if (skill.cooldown > 0)
            {
                _cooldowns[skill.skillId] = skill.cooldown;
            }
            
            Debug.Log($"[SkillManager] Used skill: {skill.skillName}");
            return true;
        }
        
        /// <summary>
        /// Skill cooldown'da mı kontrol eder
        /// </summary>
        public bool IsOnCooldown(SkillData skill)
        {
            if (skill == null || skill.cooldown <= 0)
                return false;
            
            return _cooldowns.ContainsKey(skill.skillId) && _cooldowns[skill.skillId] > 0;
        }
        
        /// <summary>
        /// Skill'in kalan cooldown süresini döndürür
        /// </summary>
        public float GetCooldownRemaining(SkillData skill)
        {
            if (skill == null || !_cooldowns.ContainsKey(skill.skillId))
                return 0f;
            
            return Mathf.Max(0, _cooldowns[skill.skillId]);
        }
        
        /// <summary>
        /// Cooldown'ları günceller
        /// </summary>
        private void UpdateCooldowns()
        {
            List<string> keys = new List<string>(_cooldowns.Keys);
            foreach (string key in keys)
            {
                _cooldowns[key] -= Time.deltaTime;
                if (_cooldowns[key] <= 0)
                {
                    _cooldowns.Remove(key);
                }
            }
        }
        
        /// <summary>
        /// Skill slot'una skill ekler
        /// </summary>
        public void EquipSkill(SkillData skill, int slotIndex)
        {
            if (slotIndex < 0 || slotIndex >= _equippedSkills.Length)
            {
                Debug.LogError($"[SkillManager] Invalid slot index: {slotIndex}");
                return;
            }
            
            _equippedSkills[slotIndex] = skill;
            Debug.Log($"[SkillManager] Equipped {skill.skillName} to slot {slotIndex}");
        }
        
        /// <summary>
        /// Slot'taki skill'i döndürür
        /// </summary>
        public SkillData GetSkillInSlot(int slotIndex)
        {
            if (slotIndex < 0 || slotIndex >= _equippedSkills.Length)
                return null;
            
            return _equippedSkills[slotIndex];
        }
        
        /// <summary>
        /// Tüm cooldown'ları temizler
        /// </summary>
        public void ClearAllCooldowns()
        {
            _cooldowns.Clear();
        }
    }
}

