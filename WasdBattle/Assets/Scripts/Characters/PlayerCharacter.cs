using Unity.Netcode;
using UnityEngine;
using WasdBattle.Data;
using WasdBattle.Skills;

namespace WasdBattle.Characters
{
    /// <summary>
    /// Oyuncu karakterini temsil eden NetworkBehaviour sınıfı
    /// </summary>
    public class PlayerCharacter : NetworkBehaviour
    {
        [Header("Character Data")]
        [SerializeField] private CharacterData _characterData;
        
        [Header("Current Stats")]
        private NetworkVariable<int> _currentHealth = new NetworkVariable<int>();
        private NetworkVariable<int> _currentStamina = new NetworkVariable<int>();
        private NetworkVariable<int> _maxHealth = new NetworkVariable<int>();
        private NetworkVariable<int> _maxStamina = new NetworkVariable<int>();
        
        [Header("Buffs & Debuffs")]
        private float _defenseMultiplier = 1f;
        private float _damageMultiplier = 1f;
        private bool _isComboScrambled = false;
        
        private float _defenseDebuffEndTime;
        private float _damageBoostEndTime;
        private float _comboScrambleEndTime;
        
        [Header("Components")]
        private SkillManager _skillManager;
        
        // Properties
        public CharacterData CharacterData => _characterData;
        public string CharacterName => _characterData != null ? _characterData.characterName : "Unknown";
        public int CurrentHealth => _currentHealth.Value;
        public int CurrentStamina => _currentStamina.Value;
        public int MaxHealth => _maxHealth.Value;
        public int MaxStamina => _maxStamina.Value;
        public float HealthPercentage => _maxHealth.Value > 0 ? (float)_currentHealth.Value / _maxHealth.Value : 0f;
        public float StaminaPercentage => _maxStamina.Value > 0 ? (float)_currentStamina.Value / _maxStamina.Value : 0f;
        public bool IsAlive => _currentHealth.Value > 0;
        public bool IsComboScrambled => _isComboScrambled;
        
        private void Awake()
        {
            _skillManager = GetComponent<SkillManager>();
            if (_skillManager == null)
            {
                _skillManager = gameObject.AddComponent<SkillManager>();
            }
        }
        
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            
            if (IsServer)
            {
                InitializeStats();
            }
        }
        
        private void Update()
        {
            if (!IsServer)
                return;
            
            // Stamina regeneration
            RegenerateStamina();
            
            // Update buffs/debuffs
            UpdateBuffs();
        }
        
        /// <summary>
        /// Karakteri başlangıç değerleriyle başlatır
        /// </summary>
        private void InitializeStats()
        {
            if (_characterData == null)
            {
                Debug.LogError("[PlayerCharacter] Character data is null!");
                return;
            }
            
            _maxHealth.Value = _characterData.baseHealth;
            _maxStamina.Value = _characterData.baseStamina;
            _currentHealth.Value = _maxHealth.Value;
            _currentStamina.Value = _maxStamina.Value;
            
            Debug.Log($"[PlayerCharacter] {CharacterName} initialized - HP: {_currentHealth.Value}/{_maxHealth.Value}, Stamina: {_currentStamina.Value}/{_maxStamina.Value}");
        }
        
        /// <summary>
        /// Karaktere hasar verir
        /// </summary>
        public void TakeDamage(int damage)
        {
            if (!IsServer)
                return;
            
            // Savunma hesapla
            float actualDamage = damage * _defenseMultiplier;
            if (_characterData != null)
            {
                actualDamage *= (1f - _characterData.baseDefense);
            }
            
            int finalDamage = Mathf.RoundToInt(actualDamage);
            _currentHealth.Value = Mathf.Max(0, _currentHealth.Value - finalDamage);
            
            Debug.Log($"[PlayerCharacter] {CharacterName} took {finalDamage} damage. HP: {_currentHealth.Value}/{_maxHealth.Value}");
            
            if (_currentHealth.Value <= 0)
            {
                OnDeath();
            }
        }
        
        /// <summary>
        /// HP'yi değiştirir (pozitif = heal, negatif = damage)
        /// </summary>
        public void ModifyHealth(int amount)
        {
            if (!IsServer)
                return;
            
            _currentHealth.Value = Mathf.Clamp(_currentHealth.Value + amount, 0, _maxHealth.Value);
            
            if (amount > 0)
                Debug.Log($"[PlayerCharacter] {CharacterName} healed {amount} HP");
        }
        
        /// <summary>
        /// Stamina'yı değiştirir
        /// </summary>
        public void ModifyStamina(int amount)
        {
            if (!IsServer)
                return;
            
            _currentStamina.Value = Mathf.Clamp(_currentStamina.Value + amount, 0, _maxStamina.Value);
        }
        
        /// <summary>
        /// Stamina regeneration
        /// </summary>
        private void RegenerateStamina()
        {
            if (_characterData == null || _currentStamina.Value >= _maxStamina.Value)
                return;
            
            float regenAmount = _characterData.staminaRegenRate * Time.deltaTime;
            _currentStamina.Value = Mathf.Min(_maxStamina.Value, _currentStamina.Value + Mathf.RoundToInt(regenAmount));
        }
        
        /// <summary>
        /// Savunma debuff uygular
        /// </summary>
        public void ApplyDefenseDebuff(float multiplier, float duration)
        {
            _defenseMultiplier = multiplier;
            _defenseDebuffEndTime = Time.time + duration;
        }
        
        /// <summary>
        /// Hasar boost uygular
        /// </summary>
        public void ApplyDamageBoost(float boost, float duration)
        {
            _damageMultiplier = 1f + boost;
            _damageBoostEndTime = Time.time + duration;
        }
        
        /// <summary>
        /// Combo scramble uygular
        /// </summary>
        public void ApplyComboScramble(float duration)
        {
            _isComboScrambled = true;
            _comboScrambleEndTime = Time.time + duration;
        }
        
        /// <summary>
        /// Buff/debuff'ları günceller
        /// </summary>
        private void UpdateBuffs()
        {
            // Defense debuff
            if (_defenseMultiplier != 1f && Time.time >= _defenseDebuffEndTime)
            {
                _defenseMultiplier = 1f;
            }
            
            // Damage boost
            if (_damageMultiplier != 1f && Time.time >= _damageBoostEndTime)
            {
                _damageMultiplier = 1f;
            }
            
            // Combo scramble
            if (_isComboScrambled && Time.time >= _comboScrambleEndTime)
            {
                _isComboScrambled = false;
            }
        }
        
        /// <summary>
        /// Hasar çarpanını döndürür
        /// </summary>
        public float GetDamageMultiplier()
        {
            return _damageMultiplier;
        }
        
        /// <summary>
        /// Karakter öldüğünde çağrılır
        /// </summary>
        private void OnDeath()
        {
            Debug.Log($"[PlayerCharacter] {CharacterName} died!");
            // Death event'i tetiklenebilir
        }
        
        /// <summary>
        /// Karakteri belirli data ile ayarlar
        /// </summary>
        public void SetCharacterData(CharacterData data)
        {
            _characterData = data;
            
            if (IsServer)
            {
                InitializeStats();
            }
        }
        
        /// <summary>
        /// Skill manager'ı döndürür
        /// </summary>
        public SkillManager GetSkillManager()
        {
            return _skillManager;
        }
    }
}

