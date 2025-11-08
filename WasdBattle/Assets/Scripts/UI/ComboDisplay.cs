using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using WasdBattle.Data;
using WasdBattle.Input;

namespace WasdBattle.UI
{
    /// <summary>
    /// Combo dizisini ve progress'i gösteren UI component
    /// </summary>
    public class ComboDisplay : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform _keyContainer;
        [SerializeField] private GameObject _keyIconPrefab;
        [SerializeField] private Image _progressBar;
        [SerializeField] private TextMeshProUGUI _timerText;
        [SerializeField] private TextMeshProUGUI _accuracyText;
        
        [Header("Colors")]
        [SerializeField] private Color _pendingColor = Color.gray;
        [SerializeField] private Color _correctColor = Color.green;
        [SerializeField] private Color _incorrectColor = Color.red;
        [SerializeField] private Color _currentColor = Color.yellow;
        
        [Header("Key Sprites")]
        [SerializeField] private Sprite _wKeySprite;
        [SerializeField] private Sprite _aKeySprite;
        [SerializeField] private Sprite _sKeySprite;
        [SerializeField] private Sprite _dKeySprite;
        
        private List<Image> _keyIcons = new List<Image>();
        private ComboInputManager _comboInputManager;
        private ComboData _currentCombo;
        private int _currentKeyIndex = 0;
        
        private void Awake()
        {
            _comboInputManager = FindObjectOfType<ComboInputManager>();
            
            if (_comboInputManager != null)
            {
                _comboInputManager.OnKeyPressed += OnKeyPressed;
                _comboInputManager.OnComboCompleted += OnComboCompleted;
                _comboInputManager.OnTimeUpdate += OnTimeUpdate;
            }
        }
        
        /// <summary>
        /// Combo'yu gösterir
        /// </summary>
        public void ShowCombo(ComboData combo)
        {
            _currentCombo = combo;
            _currentKeyIndex = 0;
            
            ClearKeyIcons();
            
            if (combo == null || combo.comboSequence == null)
                return;
            
            // Her tuş için icon oluştur
            foreach (var key in combo.comboSequence)
            {
                GameObject iconObj = Instantiate(_keyIconPrefab, _keyContainer);
                Image iconImage = iconObj.GetComponent<Image>();
                
                if (iconImage != null)
                {
                    iconImage.sprite = GetKeySprite(key);
                    iconImage.color = _pendingColor;
                    _keyIcons.Add(iconImage);
                }
            }
            
            // İlk tuşu highlight et
            if (_keyIcons.Count > 0)
            {
                _keyIcons[0].color = _currentColor;
            }
        }
        
        /// <summary>
        /// Tuş basıldığında çağrılır
        /// </summary>
        private void OnKeyPressed(KeyCode key)
        {
            if (_currentCombo == null || _currentKeyIndex >= _keyIcons.Count)
                return;
            
            // Doğru tuş mı kontrol et
            bool isCorrect = _currentCombo.comboSequence[_currentKeyIndex] == key;
            
            // Icon rengini güncelle
            _keyIcons[_currentKeyIndex].color = isCorrect ? _correctColor : _incorrectColor;
            
            _currentKeyIndex++;
            
            // Sonraki tuşu highlight et
            if (_currentKeyIndex < _keyIcons.Count)
            {
                _keyIcons[_currentKeyIndex].color = _currentColor;
            }
        }
        
        /// <summary>
        /// Combo tamamlandığında çağrılır
        /// </summary>
        private void OnComboCompleted(ComboResult result)
        {
            if (_accuracyText != null)
            {
                _accuracyText.text = $"Accuracy: {result.accuracy:P0}\nGrade: {result.grade}";
            }
        }
        
        /// <summary>
        /// Zaman güncellemesi
        /// </summary>
        private void OnTimeUpdate(float progress)
        {
            if (_progressBar != null)
            {
                _progressBar.fillAmount = 1f - progress;
            }
            
            if (_timerText != null && _comboInputManager != null)
            {
                float remainingTime = _comboInputManager.GetRemainingTime();
                _timerText.text = $"{remainingTime:F1}s";
            }
        }
        
        /// <summary>
        /// Icon'ları temizler
        /// </summary>
        private void ClearKeyIcons()
        {
            foreach (var icon in _keyIcons)
            {
                if (icon != null)
                    Destroy(icon.gameObject);
            }
            
            _keyIcons.Clear();
        }
        
        /// <summary>
        /// KeyCode'a göre sprite döndürür
        /// </summary>
        private Sprite GetKeySprite(KeyCode key)
        {
            switch (key)
            {
                case KeyCode.W:
                    return _wKeySprite;
                case KeyCode.A:
                    return _aKeySprite;
                case KeyCode.S:
                    return _sKeySprite;
                case KeyCode.D:
                    return _dKeySprite;
                default:
                    return null;
            }
        }
        
        /// <summary>
        /// Display'i gizler
        /// </summary>
        public void Hide()
        {
            ClearKeyIcons();
            gameObject.SetActive(false);
        }
        
        /// <summary>
        /// Display'i gösterir
        /// </summary>
        public void Show()
        {
            gameObject.SetActive(true);
        }
        
        private void OnDestroy()
        {
            if (_comboInputManager != null)
            {
                _comboInputManager.OnKeyPressed -= OnKeyPressed;
                _comboInputManager.OnComboCompleted -= OnComboCompleted;
                _comboInputManager.OnTimeUpdate -= OnTimeUpdate;
            }
        }
    }
}

