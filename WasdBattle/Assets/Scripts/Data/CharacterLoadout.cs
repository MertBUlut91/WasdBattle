using System;
using System.Collections.Generic;
using UnityEngine;

namespace WasdBattle.Data
{
    /// <summary>
    /// Bir karakterin equipment ve skill loadout'u
    /// Her karakter için ayrı loadout kaydedilir
    /// </summary>
    [Serializable]
    public class CharacterLoadout
    {
        public string characterId;
        
        // Equipment (8 slot)
        public string equippedHelmet;      // Kask
        public string equippedChest;       // Gövdelik
        public string equippedGloves;      // Ellik
        public string equippedLegs;        // Bacaklık
        public string equippedWeapon;      // Silah
        public string equippedRing1;       // Yüzük 1
        public string equippedRing2;       // Yüzük 2
        public string equippedNecklace;    // Kolye
        public string equippedBracelet;    // Bileklik
        
        // Skills (5 slot: 3 active, 1 passive, 1 ultimate)
        public string activeSkill1;        // Q
        public string activeSkill2;        // E
        public string activeSkill3;        // R
        public string passiveSkill;        // Passive
        public string ultimateSkill;       // Ultimate
        
        public CharacterLoadout()
        {
            characterId = "";
            
            // Equipment başlangıçta boş
            equippedHelmet = "";
            equippedChest = "";
            equippedGloves = "";
            equippedLegs = "";
            equippedWeapon = "";
            equippedRing1 = "";
            equippedRing2 = "";
            equippedNecklace = "";
            equippedBracelet = "";
            
            // Skills başlangıçta boş
            activeSkill1 = "";
            activeSkill2 = "";
            activeSkill3 = "";
            passiveSkill = "";
            ultimateSkill = "";
        }
        
        public CharacterLoadout(string charId)
        {
            characterId = charId;
            
            // Equipment başlangıçta boş
            equippedHelmet = "";
            equippedChest = "";
            equippedGloves = "";
            equippedLegs = "";
            equippedWeapon = "";
            equippedRing1 = "";
            equippedRing2 = "";
            equippedNecklace = "";
            equippedBracelet = "";
            
            // Skills başlangıçta boş
            activeSkill1 = "";
            activeSkill2 = "";
            activeSkill3 = "";
            passiveSkill = "";
            ultimateSkill = "";
        }
        
        /// <summary>
        /// Belirtilen slot'a item ekle
        /// </summary>
        public void EquipItem(EquipmentSlot slot, string itemId)
        {
            switch (slot)
            {
                case EquipmentSlot.Helmet:
                    equippedHelmet = itemId;
                    break;
                case EquipmentSlot.Chest:
                    equippedChest = itemId;
                    break;
                case EquipmentSlot.Gloves:
                    equippedGloves = itemId;
                    break;
                case EquipmentSlot.Legs:
                    equippedLegs = itemId;
                    break;
                case EquipmentSlot.Weapon:
                    equippedWeapon = itemId;
                    break;
                case EquipmentSlot.Ring1:
                    equippedRing1 = itemId;
                    break;
                case EquipmentSlot.Ring2:
                    equippedRing2 = itemId;
                    break;
                case EquipmentSlot.Necklace:
                    equippedNecklace = itemId;
                    break;
                case EquipmentSlot.Bracelet:
                    equippedBracelet = itemId;
                    break;
            }
        }
        
        /// <summary>
        /// Belirtilen slot'taki item'i al
        /// </summary>
        public string GetEquippedItem(EquipmentSlot slot)
        {
            switch (slot)
            {
                case EquipmentSlot.Helmet: return equippedHelmet;
                case EquipmentSlot.Chest: return equippedChest;
                case EquipmentSlot.Gloves: return equippedGloves;
                case EquipmentSlot.Legs: return equippedLegs;
                case EquipmentSlot.Weapon: return equippedWeapon;
                case EquipmentSlot.Ring1: return equippedRing1;
                case EquipmentSlot.Ring2: return equippedRing2;
                case EquipmentSlot.Necklace: return equippedNecklace;
                case EquipmentSlot.Bracelet: return equippedBracelet;
                default: return "";
            }
        }
        
        /// <summary>
        /// Belirtilen slot'taki item'i çıkar
        /// </summary>
        public void UnequipItem(EquipmentSlot slot)
        {
            EquipItem(slot, "");
        }
        
        /// <summary>
        /// Skill slot'a skill ekle
        /// </summary>
        public void EquipSkill(int slotIndex, string skillId)
        {
            switch (slotIndex)
            {
                case 0: activeSkill1 = skillId; break;
                case 1: activeSkill2 = skillId; break;
                case 2: activeSkill3 = skillId; break;
                case 3: passiveSkill = skillId; break;
                case 4: ultimateSkill = skillId; break;
            }
        }
        
        /// <summary>
        /// Tüm equipped item ID'lerini liste olarak al
        /// </summary>
        public List<string> GetAllEquippedItems()
        {
            var items = new List<string>();
            
            if (!string.IsNullOrEmpty(equippedHelmet)) items.Add(equippedHelmet);
            if (!string.IsNullOrEmpty(equippedChest)) items.Add(equippedChest);
            if (!string.IsNullOrEmpty(equippedGloves)) items.Add(equippedGloves);
            if (!string.IsNullOrEmpty(equippedLegs)) items.Add(equippedLegs);
            if (!string.IsNullOrEmpty(equippedWeapon)) items.Add(equippedWeapon);
            if (!string.IsNullOrEmpty(equippedRing1)) items.Add(equippedRing1);
            if (!string.IsNullOrEmpty(equippedRing2)) items.Add(equippedRing2);
            if (!string.IsNullOrEmpty(equippedNecklace)) items.Add(equippedNecklace);
            if (!string.IsNullOrEmpty(equippedBracelet)) items.Add(equippedBracelet);
            
            return items;
        }
        
        /// <summary>
        /// Tüm equipped skill ID'lerini liste olarak al
        /// </summary>
        public List<string> GetAllEquippedSkills()
        {
            var skills = new List<string>();
            
            if (!string.IsNullOrEmpty(activeSkill1)) skills.Add(activeSkill1);
            if (!string.IsNullOrEmpty(activeSkill2)) skills.Add(activeSkill2);
            if (!string.IsNullOrEmpty(activeSkill3)) skills.Add(activeSkill3);
            if (!string.IsNullOrEmpty(passiveSkill)) skills.Add(passiveSkill);
            if (!string.IsNullOrEmpty(ultimateSkill)) skills.Add(ultimateSkill);
            
            return skills;
        }
    }
}

