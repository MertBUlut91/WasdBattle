using System;
using System.Collections.Generic;
using UnityEngine;
using WasdBattle.Data;

namespace WasdBattle.Input
{
    /// <summary>
    /// WASD combo input'larını yöneten sınıf.
    /// Oyuncunun girdiği tuşları takip eder ve combo'yu doğrular.
    /// </summary>
    public class ComboInputManager : MonoBehaviour
    {
        [Header("Current Combo")]
        private ComboData _currentCombo;
        private List<KeyCode> _inputBuffer = new List<KeyCode>();
        private float _comboStartTime;
        private float _lastInputTime;
        private bool _isActive;
        
        [Header("Settings")]
        [SerializeField] private bool _allowDuplicates = true;
        
        // Events
        public event Action<KeyCode> OnKeyPressed;
        public event Action<ComboResult> OnComboCompleted;
        public event Action OnComboFailed;
        public event Action<float> OnTimeUpdate; // 0-1 arası progress
        
        private void Update()
        {
            if (!_isActive || _currentCombo == null)
                return;
            
            // Zaman aşımı kontrolü
            float elapsed = Time.time - _comboStartTime;
            if (elapsed > _currentCombo.totalDuration)
            {
                FailCombo();
                return;
            }
            
            // Progress event'i gönder
            OnTimeUpdate?.Invoke(elapsed / _currentCombo.totalDuration);
            
            // WASD tuşlarını dinle
            CheckInput();
        }
        
        private void CheckInput()
        {
            KeyCode[] wasdKeys = { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D };
            
            foreach (var key in wasdKeys)
            {
                if (UnityEngine.Input.GetKeyDown(key))
                {
                    ProcessInput(key);
                    break; // Aynı frame'de sadece bir tuş
                }
            }
        }
        
        private void ProcessInput(KeyCode key)
        {
            if (!_isActive || _currentCombo == null)
                return;
            
            // Tuş basma zamanını kontrol et
            float timeSinceLastInput = Time.time - _lastInputTime;
            if (_inputBuffer.Count > 0 && timeSinceLastInput > _currentCombo.timeWindowPerKey)
            {
                // Çok geç basıldı
                FailCombo();
                return;
            }
            
            _lastInputTime = Time.time;
            _inputBuffer.Add(key);
            
            OnKeyPressed?.Invoke(key);
            
            Debug.Log($"[ComboInput] Key pressed: {key}, Buffer: {_inputBuffer.Count}/{_currentCombo.ComboLength}");
            
            // Combo tamamlandı mı?
            if (_inputBuffer.Count >= _currentCombo.ComboLength)
            {
                ValidateCombo();
            }
        }
        
        /// <summary>
        /// Yeni bir combo başlatır
        /// </summary>
        public void StartCombo(ComboData combo)
        {
            if (combo == null)
            {
                Debug.LogError("[ComboInput] Cannot start combo with null data!");
                return;
            }
            
            _currentCombo = combo;
            _inputBuffer.Clear();
            _comboStartTime = Time.time;
            _lastInputTime = Time.time;
            _isActive = true;
            
            Debug.Log($"[ComboInput] Started combo: {combo.GetComboString()}");
        }
        
        /// <summary>
        /// Combo'yu durdurur
        /// </summary>
        public void StopCombo()
        {
            _isActive = false;
            _currentCombo = null;
            _inputBuffer.Clear();
            
            Debug.Log("[ComboInput] Combo stopped");
        }
        
        /// <summary>
        /// Girilen combo'yu doğrular
        /// </summary>
        private void ValidateCombo()
        {
            if (_currentCombo == null)
                return;
            
            ComboValidator validator = new ComboValidator();
            ComboResult result = validator.Validate(_currentCombo.comboSequence, _inputBuffer.ToArray());
            
            _isActive = false;
            
            Debug.Log($"[ComboInput] Combo completed! Accuracy: {result.accuracy:P}, Perfect: {result.isPerfect}");
            
            OnComboCompleted?.Invoke(result);
        }
        
        /// <summary>
        /// Combo başarısız oldu
        /// </summary>
        private void FailCombo()
        {
            _isActive = false;
            
            Debug.Log("[ComboInput] Combo failed!");
            
            // Başarısız combo için result oluştur
            ComboValidator validator = new ComboValidator();
            ComboResult result = validator.Validate(_currentCombo.comboSequence, _inputBuffer.ToArray());
            
            OnComboCompleted?.Invoke(result);
            OnComboFailed?.Invoke();
        }
        
        /// <summary>
        /// Mevcut combo progress'i döndürür
        /// </summary>
        public float GetProgress()
        {
            if (!_isActive || _currentCombo == null)
                return 0f;
            
            return (float)_inputBuffer.Count / _currentCombo.ComboLength;
        }
        
        /// <summary>
        /// Kalan süreyi döndürür
        /// </summary>
        public float GetRemainingTime()
        {
            if (!_isActive || _currentCombo == null)
                return 0f;
            
            float elapsed = Time.time - _comboStartTime;
            return Mathf.Max(0, _currentCombo.totalDuration - elapsed);
        }
        
        public bool IsActive => _isActive;
        public ComboData CurrentCombo => _currentCombo;
        public int CurrentInputCount => _inputBuffer.Count;
    }
}

