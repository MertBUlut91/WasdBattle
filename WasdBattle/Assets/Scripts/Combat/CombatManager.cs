using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;
using WasdBattle.Characters;
using WasdBattle.Data;
using WasdBattle.Input;
using WasdBattle.Skills;

namespace WasdBattle.Combat
{
    /// <summary>
    /// Dövüş sistemini yöneten NetworkBehaviour.
    /// Tur bazlı saldırı/savunma akışını kontrol eder.
    /// </summary>
    public class CombatManager : NetworkBehaviour
    {
        [Header("Players")]
        [SerializeField] private PlayerCharacter _player1;
        [SerializeField] private PlayerCharacter _player2;
        
        [Header("Combat State")]
        private NetworkVariable<CombatState> _currentState = new NetworkVariable<CombatState>(CombatState.WaitingToStart);
        private NetworkVariable<ulong> _attackerId = new NetworkVariable<ulong>();
        private NetworkVariable<ulong> _defenderId = new NetworkVariable<ulong>();
        private NetworkVariable<int> _currentRound = new NetworkVariable<int>(0);
        
        [Header("Settings")]
        [SerializeField] private int _maxRounds = 10;
        [SerializeField] private float _turnDuration = 10f;
        
        // Events
        public event Action<CombatState> OnCombatStateChanged;
        public event Action<ulong> OnAttackerChanged;
        public event Action<int> OnRoundChanged;
        public event Action<ulong> OnMatchEnded; // Winner ID
        
        public CombatState CurrentState => _currentState.Value;
        public int CurrentRound => _currentRound.Value;
        
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            
            _currentState.OnValueChanged += (oldValue, newValue) =>
            {
                OnCombatStateChanged?.Invoke(newValue);
            };
            
            _attackerId.OnValueChanged += (oldValue, newValue) =>
            {
                OnAttackerChanged?.Invoke(newValue);
            };
            
            _currentRound.OnValueChanged += (oldValue, newValue) =>
            {
                OnRoundChanged?.Invoke(newValue);
            };
        }
        
        /// <summary>
        /// Maçı başlatır
        /// </summary>
        public void StartMatch()
        {
            if (!IsServer)
                return;
            
            if (_player1 == null || _player2 == null)
            {
                Debug.LogError("[CombatManager] Players not assigned!");
                return;
            }
            
            _currentRound.Value = 1;
            _currentState.Value = CombatState.RoundStart;
            
            // İlk saldıran rastgele seç
            bool player1Attacks = UnityEngine.Random.value > 0.5f;
            _attackerId.Value = player1Attacks ? _player1.OwnerClientId : _player2.OwnerClientId;
            _defenderId.Value = player1Attacks ? _player2.OwnerClientId : _player1.OwnerClientId;
            
            Debug.Log($"[CombatManager] Match started! Round {_currentRound.Value}");
            
            StartCoroutine(RoundRoutine());
        }
        
        /// <summary>
        /// Tur döngüsü
        /// </summary>
        private IEnumerator RoundRoutine()
        {
            // Round başlangıcı
            _currentState.Value = CombatState.RoundStart;
            yield return new WaitForSeconds(2f);
            
            // Skill seçimi
            _currentState.Value = CombatState.SkillSelection;
            yield return new WaitForSeconds(5f);
            
            // Saldırı fazı
            _currentState.Value = CombatState.AttackPhase;
            yield return new WaitForSeconds(_turnDuration);
            
            // Savunma fazı
            _currentState.Value = CombatState.DefensePhase;
            yield return new WaitForSeconds(_turnDuration);
            
            // Hasar hesaplama
            _currentState.Value = CombatState.DamageCalculation;
            yield return new WaitForSeconds(2f);
            
            // Maç bitti mi kontrol et
            if (CheckMatchEnd())
            {
                EndMatch();
                yield break;
            }
            
            // Rolleri değiştir
            SwapRoles();
            
            // Sonraki round
            _currentRound.Value++;
            
            if (_currentRound.Value > _maxRounds)
            {
                EndMatch();
                yield break;
            }
            
            // Yeni round başlat
            StartCoroutine(RoundRoutine());
        }
        
        /// <summary>
        /// Saldırı ve savunma rollerini değiştirir
        /// </summary>
        private void SwapRoles()
        {
            ulong temp = _attackerId.Value;
            _attackerId.Value = _defenderId.Value;
            _defenderId.Value = temp;
            
            Debug.Log($"[CombatManager] Roles swapped. New attacker: {_attackerId.Value}");
        }
        
        /// <summary>
        /// Saldırı gerçekleştirilir
        /// </summary>
        [ServerRpc(RequireOwnership = false)]
        public void ExecuteAttackServerRpc(ulong attackerId, string skillId, ComboResult attackResult)
        {
            if (_currentState.Value != CombatState.AttackPhase)
            {
                Debug.LogWarning("[CombatManager] Attack attempted in wrong phase!");
                return;
            }
            
            Debug.Log($"[CombatManager] Attack executed by {attackerId}, Accuracy: {attackResult.accuracy:P}");
            
            // Saldırı sonucunu sakla (geçici - ileride daha iyi bir sistem)
            _lastAttackResult = attackResult;
            _lastAttackSkillId = skillId;
        }
        
        private ComboResult _lastAttackResult;
        private string _lastAttackSkillId;
        
        /// <summary>
        /// Savunma gerçekleştirilir
        /// </summary>
        [ServerRpc(RequireOwnership = false)]
        public void ExecuteDefenseServerRpc(ulong defenderId, ComboResult defenseResult)
        {
            if (_currentState.Value != CombatState.DefensePhase)
            {
                Debug.LogWarning("[CombatManager] Defense attempted in wrong phase!");
                return;
            }
            
            Debug.Log($"[CombatManager] Defense executed by {defenderId}, Accuracy: {defenseResult.accuracy:P}");
            
            // Hasar hesapla
            CalculateDamage(_lastAttackResult, defenseResult);
        }
        
        /// <summary>
        /// Hasar hesaplaması yapar
        /// </summary>
        private void CalculateDamage(ComboResult attackResult, ComboResult defenseResult)
        {
            if (!IsServer)
                return;
            
            PlayerCharacter attacker = GetPlayerByClientId(_attackerId.Value);
            PlayerCharacter defender = GetPlayerByClientId(_defenderId.Value);
            
            if (attacker == null || defender == null)
            {
                Debug.LogError("[CombatManager] Player not found for damage calculation!");
                return;
            }
            
            // Temel hasar (örnek - skill'den alınmalı)
            float baseDamage = 30f;
            
            // Saldırı accuracy çarpanı
            float attackMultiplier = attackResult.accuracy;
            
            // Savunma reduction
            float defenseReduction = defenseResult.accuracy;
            
            // Final hasar
            float finalDamage = baseDamage * attackMultiplier * (1f - defenseReduction);
            
            // Karakter damage multiplier'ı ekle
            finalDamage *= attacker.GetDamageMultiplier();
            
            int damage = Mathf.RoundToInt(finalDamage);
            
            Debug.Log($"[CombatManager] Damage calculated: {damage} (Base: {baseDamage}, Attack: {attackMultiplier:P}, Defense: {defenseReduction:P})");
            
            // Hasarı uygula
            defender.TakeDamage(damage);
            
            // Client'lara bildir
            NotifyDamageDealtClientRpc(_attackerId.Value, _defenderId.Value, damage);
        }
        
        [ClientRpc]
        private void NotifyDamageDealtClientRpc(ulong attackerId, ulong defenderId, int damage)
        {
            Debug.Log($"[CombatManager] {attackerId} dealt {damage} damage to {defenderId}");
        }
        
        /// <summary>
        /// Maç bitişini kontrol eder
        /// </summary>
        private bool CheckMatchEnd()
        {
            if (_player1 == null || _player2 == null)
                return true;
            
            return !_player1.IsAlive || !_player2.IsAlive;
        }
        
        /// <summary>
        /// Maçı bitirir
        /// </summary>
        private void EndMatch()
        {
            if (!IsServer)
                return;
            
            _currentState.Value = CombatState.MatchEnded;
            
            // Kazananı belirle
            ulong winnerId = 0;
            
            if (_player1 != null && _player1.IsAlive && (_player2 == null || !_player2.IsAlive))
            {
                winnerId = _player1.OwnerClientId;
            }
            else if (_player2 != null && _player2.IsAlive && (_player1 == null || !_player1.IsAlive))
            {
                winnerId = _player2.OwnerClientId;
            }
            else
            {
                // Berabere - HP'ye göre karar ver
                if (_player1 != null && _player2 != null)
                {
                    winnerId = _player1.CurrentHealth > _player2.CurrentHealth ? _player1.OwnerClientId : _player2.OwnerClientId;
                }
            }
            
            Debug.Log($"[CombatManager] Match ended! Winner: {winnerId}");
            
            OnMatchEnded?.Invoke(winnerId);
            AnnounceWinnerClientRpc(winnerId);
        }
        
        [ClientRpc]
        private void AnnounceWinnerClientRpc(ulong winnerId)
        {
            Debug.Log($"[CombatManager] Winner announced: {winnerId}");
        }
        
        /// <summary>
        /// Client ID'ye göre oyuncu döndürür
        /// </summary>
        private PlayerCharacter GetPlayerByClientId(ulong clientId)
        {
            if (_player1 != null && _player1.OwnerClientId == clientId)
                return _player1;
            
            if (_player2 != null && _player2.OwnerClientId == clientId)
                return _player2;
            
            return null;
        }
        
        /// <summary>
        /// Oyuncuları atar
        /// </summary>
        public void SetPlayers(PlayerCharacter player1, PlayerCharacter player2)
        {
            _player1 = player1;
            _player2 = player2;
            
            Debug.Log($"[CombatManager] Players set: {player1?.CharacterName} vs {player2?.CharacterName}");
        }
        
        public PlayerCharacter GetAttacker() => GetPlayerByClientId(_attackerId.Value);
        public PlayerCharacter GetDefender() => GetPlayerByClientId(_defenderId.Value);
    }
    
    public enum CombatState
    {
        WaitingToStart,
        RoundStart,
        SkillSelection,
        AttackPhase,
        DefensePhase,
        DamageCalculation,
        MatchEnded
    }
}

