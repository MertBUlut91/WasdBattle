using UnityEngine;
using UnityEngine.UI;
using TMPro;
using WasdBattle.Data;
using WasdBattle.Skills;

namespace WasdBattle.UI
{
    /// <summary>
    /// Skill bar UI component - equipped skill'leri gösterir
    /// </summary>
    public class SkillBar : MonoBehaviour
    {
        [Header("Skill Slots")]
        [SerializeField] private SkillSlotUI[] _skillSlots = new SkillSlotUI[3];
        
        [Header("References")]
        [SerializeField] private SkillManager _skillManager;
        
        private void Start()
        {
            UpdateSkillBar();
        }
        
        private void Update()
        {
            UpdateCooldowns();
        }
        
        /// <summary>
        /// Skill bar'ı günceller
        /// </summary>
        public void UpdateSkillBar()
        {
            if (_skillManager == null)
                return;
            
            SkillData[] equippedSkills = _skillManager.EquippedSkills;
            
            for (int i = 0; i < _skillSlots.Length; i++)
            {
                if (i < equippedSkills.Length && equippedSkills[i] != null)
                {
                    _skillSlots[i].SetSkill(equippedSkills[i]);
                }
                else
                {
                    _skillSlots[i].Clear();
                }
            }
        }
        
        /// <summary>
        /// Cooldown'ları günceller
        /// </summary>
        private void UpdateCooldowns()
        {
            if (_skillManager == null)
                return;
            
            SkillData[] equippedSkills = _skillManager.EquippedSkills;
            
            for (int i = 0; i < _skillSlots.Length; i++)
            {
                if (i < equippedSkills.Length && equippedSkills[i] != null)
                {
                    float cooldownRemaining = _skillManager.GetCooldownRemaining(equippedSkills[i]);
                    _skillSlots[i].UpdateCooldown(cooldownRemaining, equippedSkills[i].cooldown);
                }
            }
        }
        
        public void SetSkillManager(SkillManager manager)
        {
            _skillManager = manager;
            UpdateSkillBar();
        }
    }
    
    /// <summary>
    /// Tek bir skill slot'u temsil eder
    /// </summary>
    [System.Serializable]
    public class SkillSlotUI
    {
        public Image iconImage;
        public Image cooldownOverlay;
        public TextMeshProUGUI cooldownText;
        public TextMeshProUGUI staminaCostText;
        
        private SkillData _currentSkill;
        
        public void SetSkill(SkillData skill)
        {
            _currentSkill = skill;
            
            if (iconImage != null)
            {
                iconImage.sprite = skill.icon;
                iconImage.enabled = true;
            }
            
            if (staminaCostText != null)
            {
                staminaCostText.text = skill.staminaCost.ToString();
            }
            
            if (cooldownOverlay != null)
            {
                cooldownOverlay.fillAmount = 0f;
            }
        }
        
        public void Clear()
        {
            _currentSkill = null;
            
            if (iconImage != null)
            {
                iconImage.enabled = false;
            }
            
            if (staminaCostText != null)
            {
                staminaCostText.text = "";
            }
            
            if (cooldownText != null)
            {
                cooldownText.text = "";
            }
        }
        
        public void UpdateCooldown(float remaining, float total)
        {
            if (cooldownOverlay != null)
            {
                cooldownOverlay.fillAmount = total > 0 ? remaining / total : 0f;
            }
            
            if (cooldownText != null)
            {
                cooldownText.text = remaining > 0 ? $"{remaining:F1}s" : "";
            }
        }
    }
}

