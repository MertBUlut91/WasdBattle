using System;
using System.Collections.Generic;
using UnityEngine;
using WasdBattle.Data;

namespace WasdBattle.Economy
{
    /// <summary>
    /// Oyuncu envanterini yöneten sınıf
    /// </summary>
    public class InventoryManager
    {
        private PlayerData _playerData;
        
        // Events
        public event Action<string, int> OnItemAdded;
        public event Action<string, int> OnItemRemoved;
        public event Action<int> OnGoldChanged;
        
        public InventoryManager(PlayerData playerData)
        {
            _playerData = playerData;
        }
        
        /// <summary>
        /// Malzeme ekler
        /// </summary>
        public void AddMaterial(MaterialType type, int amount)
        {
            _playerData.ModifyMaterial(type, amount);
            OnItemAdded?.Invoke(type.ToString(), amount);
            Debug.Log($"[Inventory] Added {amount} {type}");
        }
        
        /// <summary>
        /// Malzeme çıkarır
        /// </summary>
        public bool RemoveMaterial(MaterialType type, int amount)
        {
            if (!HasMaterial(type, amount))
            {
                Debug.LogWarning($"[Inventory] Not enough {type}. Required: {amount}");
                return false;
            }
            
            _playerData.ModifyMaterial(type, -amount);
            OnItemRemoved?.Invoke(type.ToString(), amount);
            Debug.Log($"[Inventory] Removed {amount} {type}");
            return true;
        }
        
        /// <summary>
        /// Malzeme var mı kontrol eder
        /// </summary>
        public bool HasMaterial(MaterialType type, int amount)
        {
            return _playerData.HasMaterial(type, amount);
        }
        
        /// <summary>
        /// Malzeme miktarını döndürür
        /// </summary>
        public int GetMaterialAmount(MaterialType type)
        {
            return _playerData.GetMaterialAmount(type);
        }
        
        /// <summary>
        /// Gold ekler
        /// </summary>
        public void AddGold(int amount)
        {
            _playerData.gold += amount;
            OnGoldChanged?.Invoke(_playerData.gold);
            Debug.Log($"[Inventory] Added {amount} gold. Total: {_playerData.gold}");
        }
        
        /// <summary>
        /// Gold harcama
        /// </summary>
        public bool SpendGold(int amount)
        {
            if (_playerData.gold < amount)
            {
                Debug.LogWarning($"[Inventory] Not enough gold. Required: {amount}, Have: {_playerData.gold}");
                return false;
            }
            
            _playerData.gold -= amount;
            OnGoldChanged?.Invoke(_playerData.gold);
            Debug.Log($"[Inventory] Spent {amount} gold. Remaining: {_playerData.gold}");
            return true;
        }
        
        /// <summary>
        /// Skill'e sahip mi kontrol eder
        /// </summary>
        public bool HasSkill(string skillId)
        {
            return _playerData.ownedSkills.Contains(skillId);
        }
        
        /// <summary>
        /// Skill ekler
        /// </summary>
        public void AddSkill(string skillId)
        {
            if (!HasSkill(skillId))
            {
                _playerData.ownedSkills.Add(skillId);
                Debug.Log($"[Inventory] Added skill: {skillId}");
            }
        }
        
        /// <summary>
        /// Karaktere sahip mi kontrol eder
        /// </summary>
        public bool HasCharacter(string characterId)
        {
            return _playerData.ownedCharacters.Contains(characterId);
        }
        
        /// <summary>
        /// Karakter ekler
        /// </summary>
        public void AddCharacter(string characterId)
        {
            if (!HasCharacter(characterId))
            {
                _playerData.ownedCharacters.Add(characterId);
                Debug.Log($"[Inventory] Added character: {characterId}");
            }
        }
    }
}

