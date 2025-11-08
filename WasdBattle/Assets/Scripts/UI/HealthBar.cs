using UnityEngine;
using UnityEngine.UI;
using TMPro;
using WasdBattle.Characters;

namespace WasdBattle.UI
{
    /// <summary>
    /// HP barını gösteren UI component
    /// </summary>
    public class HealthBar : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Image _fillImage;
        [SerializeField] private TextMeshProUGUI _healthText;
        [SerializeField] private PlayerCharacter _targetCharacter;
        
        [Header("Colors")]
        [SerializeField] private Color _highHealthColor = Color.green;
        [SerializeField] private Color _mediumHealthColor = Color.yellow;
        [SerializeField] private Color _lowHealthColor = Color.red;
        
        [Header("Animation")]
        [SerializeField] private float _smoothSpeed = 5f;
        
        private float _targetFillAmount;
        
        private void Update()
        {
            if (_targetCharacter == null || _fillImage == null)
                return;
            
            UpdateHealthBar();
        }
        
        private void UpdateHealthBar()
        {
            float healthPercentage = _targetCharacter.HealthPercentage;
            _targetFillAmount = healthPercentage;
            
            // Smooth transition
            _fillImage.fillAmount = Mathf.Lerp(_fillImage.fillAmount, _targetFillAmount, Time.deltaTime * _smoothSpeed);
            
            // Color based on health
            _fillImage.color = GetHealthColor(healthPercentage);
            
            // Update text
            if (_healthText != null)
            {
                _healthText.text = $"{_targetCharacter.CurrentHealth} / {_targetCharacter.MaxHealth}";
            }
        }
        
        private Color GetHealthColor(float percentage)
        {
            if (percentage > 0.6f)
                return _highHealthColor;
            else if (percentage > 0.3f)
                return _mediumHealthColor;
            else
                return _lowHealthColor;
        }
        
        public void SetTarget(PlayerCharacter character)
        {
            _targetCharacter = character;
        }
    }
}

