using UnityEngine;
using UnityEngine.UI;
using TMPro;
using WasdBattle.Characters;

namespace WasdBattle.UI
{
    /// <summary>
    /// Stamina barını gösteren UI component
    /// </summary>
    public class StaminaBar : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Image _fillImage;
        [SerializeField] private TextMeshProUGUI _staminaText;
        [SerializeField] private PlayerCharacter _targetCharacter;
        
        [Header("Colors")]
        [SerializeField] private Color _fullStaminaColor = new Color(0.2f, 0.8f, 1f);
        [SerializeField] private Color _lowStaminaColor = new Color(1f, 0.5f, 0f);
        
        [Header("Animation")]
        [SerializeField] private float _smoothSpeed = 8f;
        
        private float _targetFillAmount;
        
        private void Update()
        {
            if (_targetCharacter == null || _fillImage == null)
                return;
            
            UpdateStaminaBar();
        }
        
        private void UpdateStaminaBar()
        {
            float staminaPercentage = _targetCharacter.StaminaPercentage;
            _targetFillAmount = staminaPercentage;
            
            // Smooth transition
            _fillImage.fillAmount = Mathf.Lerp(_fillImage.fillAmount, _targetFillAmount, Time.deltaTime * _smoothSpeed);
            
            // Color based on stamina
            _fillImage.color = Color.Lerp(_lowStaminaColor, _fullStaminaColor, staminaPercentage);
            
            // Update text
            if (_staminaText != null)
            {
                _staminaText.text = $"{_targetCharacter.CurrentStamina} / {_targetCharacter.MaxStamina}";
            }
        }
        
        public void SetTarget(PlayerCharacter character)
        {
            _targetCharacter = character;
        }
    }
}

