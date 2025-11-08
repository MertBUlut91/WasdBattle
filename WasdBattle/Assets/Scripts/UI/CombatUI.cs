using UnityEngine;
using TMPro;
using WasdBattle.Characters;
using WasdBattle.Combat;

namespace WasdBattle.UI
{
    /// <summary>
    /// Dövüş ekranının ana UI controller'ı
    /// </summary>
    public class CombatUI : MonoBehaviour
    {
        [Header("Player 1 UI")]
        [SerializeField] private HealthBar _player1HealthBar;
        [SerializeField] private StaminaBar _player1StaminaBar;
        [SerializeField] private SkillBar _player1SkillBar;
        [SerializeField] private TextMeshProUGUI _player1NameText;
        
        [Header("Player 2 UI")]
        [SerializeField] private HealthBar _player2HealthBar;
        [SerializeField] private StaminaBar _player2StaminaBar;
        [SerializeField] private SkillBar _player2SkillBar;
        [SerializeField] private TextMeshProUGUI _player2NameText;
        
        [Header("Combo Display")]
        [SerializeField] private ComboDisplay _comboDisplay;
        
        [Header("Combat Info")]
        [SerializeField] private TextMeshProUGUI _roundText;
        [SerializeField] private TextMeshProUGUI _phaseText;
        [SerializeField] private TextMeshProUGUI _roleText;
        
        [Header("References")]
        [SerializeField] private CombatManager _combatManager;
        
        private void Start()
        {
            if (_combatManager != null)
            {
                _combatManager.OnCombatStateChanged += OnCombatStateChanged;
                _combatManager.OnRoundChanged += OnRoundChanged;
                _combatManager.OnAttackerChanged += OnAttackerChanged;
            }
        }
        
        /// <summary>
        /// Oyuncuları UI'a bağlar
        /// </summary>
        public void SetupPlayers(PlayerCharacter player1, PlayerCharacter player2)
        {
            // Player 1
            if (_player1HealthBar != null)
                _player1HealthBar.SetTarget(player1);
            
            if (_player1StaminaBar != null)
                _player1StaminaBar.SetTarget(player1);
            
            if (_player1SkillBar != null && player1 != null)
                _player1SkillBar.SetSkillManager(player1.GetSkillManager());
            
            if (_player1NameText != null && player1 != null)
                _player1NameText.text = player1.CharacterName;
            
            // Player 2
            if (_player2HealthBar != null)
                _player2HealthBar.SetTarget(player2);
            
            if (_player2StaminaBar != null)
                _player2StaminaBar.SetTarget(player2);
            
            if (_player2SkillBar != null && player2 != null)
                _player2SkillBar.SetSkillManager(player2.GetSkillManager());
            
            if (_player2NameText != null && player2 != null)
                _player2NameText.text = player2.CharacterName;
        }
        
        /// <summary>
        /// Combat state değiştiğinde çağrılır
        /// </summary>
        private void OnCombatStateChanged(CombatState newState)
        {
            if (_phaseText != null)
            {
                _phaseText.text = GetStateDisplayText(newState);
            }
        }
        
        /// <summary>
        /// Round değiştiğinde çağrılır
        /// </summary>
        private void OnRoundChanged(int round)
        {
            if (_roundText != null)
            {
                _roundText.text = $"Round {round}";
            }
        }
        
        /// <summary>
        /// Saldıran değiştiğinde çağrılır
        /// </summary>
        private void OnAttackerChanged(ulong attackerId)
        {
            if (_roleText != null)
            {
                // Bu client saldırıyor mu?
                // (Bu kısım network client ID'ye göre ayarlanmalı)
                _roleText.text = "Role: Waiting...";
            }
        }
        
        /// <summary>
        /// State'in display text'ini döndürür
        /// </summary>
        private string GetStateDisplayText(CombatState state)
        {
            switch (state)
            {
                case CombatState.WaitingToStart:
                    return "Waiting to Start";
                case CombatState.RoundStart:
                    return "Round Start!";
                case CombatState.SkillSelection:
                    return "Select Your Skill";
                case CombatState.AttackPhase:
                    return "ATTACK!";
                case CombatState.DefensePhase:
                    return "DEFEND!";
                case CombatState.DamageCalculation:
                    return "Calculating Damage...";
                case CombatState.MatchEnded:
                    return "Match Ended";
                default:
                    return "";
            }
        }
        
        public ComboDisplay GetComboDisplay() => _comboDisplay;
        
        private void OnDestroy()
        {
            if (_combatManager != null)
            {
                _combatManager.OnCombatStateChanged -= OnCombatStateChanged;
                _combatManager.OnRoundChanged -= OnRoundChanged;
                _combatManager.OnAttackerChanged -= OnAttackerChanged;
            }
        }
    }
}

