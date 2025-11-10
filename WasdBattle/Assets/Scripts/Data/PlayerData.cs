using System;
using System.Collections.Generic;
using UnityEngine;

namespace WasdBattle.Data
{
    [Serializable]
    public class PlayerData
    {
        public string userId;
        public string username;
        public int level;
        public int elo;
        public int experience;
        
        // Currencies (genişletilebilir)
        public int gold;
        public int gem;
        public int diamond;
        public int battleToken;
        public int craftToken;
        public int seasonToken;
        public int eventToken;
        
        // Malzemeler (genişletilebilir)
        public int metal;
        public int energyCrystal;
        public int rune;
        public int essence;
        public int leather;
        public int cloth;
        public int wood;
        public int gemStone; // Renamed from gem to avoid conflict with currency
        public int darkEssence;
        public int lightEssence;
        
        // Sahip olunan karakterler ve skill'ler
        public List<string> ownedCharacters = new List<string>();
        public List<string> ownedSkills = new List<string>();
        public List<string> ownedItems = new List<string>(); // Sahip olunan itemler (unique list)
        
        // Item count sistemi (itemId -> count)
        [SerializeField] private Dictionary<string, int> itemCounts = new Dictionary<string, int>();
        
        // Serialization için helper (Dictionary serialize edilemez)
        [SerializeField] private List<string> _itemCountKeys = new List<string>();
        [SerializeField] private List<int> _itemCountValues = new List<int>();
        
        // Seçili karakter
        public string selectedCharacterId;
        
        // Her karakter için loadout (equipment + skills)
        public List<CharacterLoadout> characterLoadouts = new List<CharacterLoadout>();
        
        // İstatistikler
        public int totalMatches;
        public int wins;
        public int losses;
        
        public PlayerData()
        {
            userId = "";
            username = "Player";
            level = 1;
            elo = 1000;
            experience = 0;
            gold = 100;
            gem = 0;
            diamond = 0;
            battleToken = 0;
            craftToken = 0;
            seasonToken = 0;
            eventToken = 0;
            metal = 50;
            energyCrystal = 50;
            rune = 10;
            essence = 5;
            leather = 0;
            cloth = 0;
            wood = 0;
            gemStone = 0;
            darkEssence = 0;
            lightEssence = 0;
            ownedCharacters = new List<string>();
            ownedSkills = new List<string>();
            ownedItems = new List<string>();
            characterLoadouts = new List<CharacterLoadout>();
            totalMatches = 0;
            wins = 0;
            losses = 0;
        }
        
        /// <summary>
        /// Belirtilen karakter için loadout al (yoksa oluştur)
        /// </summary>
        public CharacterLoadout GetLoadoutForCharacter(string characterId)
        {
            var loadout = characterLoadouts.Find(l => l.characterId == characterId);
            if (loadout == null)
            {
                loadout = new CharacterLoadout(characterId);
                characterLoadouts.Add(loadout);
            }
            return loadout;
        }
        
        /// <summary>
        /// Belirtilen material'den yeterli var mı?
        /// </summary>
        public bool HasMaterial(MaterialType materialType, int amount)
        {
            return GetMaterialAmount(materialType) >= amount;
        }
        
        /// <summary>
        /// Belirtilen material miktarını al
        /// </summary>
        public int GetMaterialAmount(MaterialType materialType)
        {
            switch (materialType)
            {
                case MaterialType.Metal: return metal;
                case MaterialType.EnergyCrystal: return energyCrystal;
                case MaterialType.Rune: return rune;
                case MaterialType.Essence: return essence;
                case MaterialType.Leather: return leather;
                case MaterialType.Cloth: return cloth;
                case MaterialType.Wood: return wood;
                case MaterialType.GemStone: return gemStone;
                case MaterialType.DarkEssence: return darkEssence;
                case MaterialType.LightEssence: return lightEssence;
                default: return 0;
            }
        }
        
        /// <summary>
        /// Material ekle/çıkar
        /// </summary>
        public void ModifyMaterial(MaterialType materialType, int amount)
        {
            switch (materialType)
            {
                case MaterialType.Metal: metal += amount; break;
                case MaterialType.EnergyCrystal: energyCrystal += amount; break;
                case MaterialType.Rune: rune += amount; break;
                case MaterialType.Essence: essence += amount; break;
                case MaterialType.Leather: leather += amount; break;
                case MaterialType.Cloth: cloth += amount; break;
                case MaterialType.Wood: wood += amount; break;
                case MaterialType.GemStone: gemStone += amount; break; // Renamed from Gem to GemStone
                case MaterialType.DarkEssence: darkEssence += amount; break;
                case MaterialType.LightEssence: lightEssence += amount; break;
            }
        }
        
        /// <summary>
        /// Belirtilen currency'den yeterli var mı?
        /// </summary>
        public bool HasCurrency(CurrencyType currencyType, int amount)
        {
            return GetCurrencyAmount(currencyType) >= amount;
        }
        
        /// <summary>
        /// Currency miktarını al
        /// </summary>
        public int GetCurrencyAmount(CurrencyType currencyType)
        {
            switch (currencyType)
            {
                case CurrencyType.Gold: return gold;
                case CurrencyType.Gem: return gem;
                case CurrencyType.Diamond: return diamond;
                case CurrencyType.BattleToken: return battleToken;
                case CurrencyType.CraftToken: return craftToken;
                case CurrencyType.SeasonToken: return seasonToken;
                case CurrencyType.EventToken: return eventToken;
                default: return 0;
            }
        }
        
        /// <summary>
        /// Currency ekle/çıkar
        /// </summary>
        public void ModifyCurrency(CurrencyType currencyType, int amount)
        {
            switch (currencyType)
            {
                case CurrencyType.Gold: gold += amount; break;
                case CurrencyType.Gem: gem += amount; break;
                case CurrencyType.Diamond: diamond += amount; break;
                case CurrencyType.BattleToken: battleToken += amount; break;
                case CurrencyType.CraftToken: craftToken += amount; break;
                case CurrencyType.SeasonToken: seasonToken += amount; break;
                case CurrencyType.EventToken: eventToken += amount; break;
            }
        }
        
        public float WinRate => totalMatches > 0 ? (float)wins / totalMatches : 0f;
        
        /// <summary>
        /// Item'e sahip mi kontrol et
        /// </summary>
        public bool HasItem(string itemId)
        {
            return ownedItems.Contains(itemId);
        }
        
        /// <summary>
        /// Item count'u al (yoksa 0)
        /// </summary>
        public int GetItemCount(string itemId)
        {
            InitializeItemCounts();
            return itemCounts.ContainsKey(itemId) ? itemCounts[itemId] : 0;
        }
        
        /// <summary>
        /// Inventory'e item ekle (stackable ise count artar)
        /// </summary>
        public void AddItem(string itemId, int count = 1)
        {
            InitializeItemCounts();
            
            if (!HasItem(itemId))
            {
                ownedItems.Add(itemId);
                itemCounts[itemId] = count;
                Debug.Log($"[PlayerData] Item added to inventory: {itemId} x{count}");
            }
            else
            {
                itemCounts[itemId] += count;
                Debug.Log($"[PlayerData] Item count increased: {itemId} x{itemCounts[itemId]}");
            }
            
            SyncItemCountsForSerialization();
        }
        
        /// <summary>
        /// Inventory'den item çıkar (count azalır, 0 olursa tamamen kaldırılır)
        /// </summary>
        public void RemoveItem(string itemId, int count = 1)
        {
            InitializeItemCounts();
            
            if (!HasItem(itemId))
                return;
            
            if (!itemCounts.ContainsKey(itemId))
                itemCounts[itemId] = 1;
            
            itemCounts[itemId] -= count;
            
            if (itemCounts[itemId] <= 0)
            {
                ownedItems.Remove(itemId);
                itemCounts.Remove(itemId);
                Debug.Log($"[PlayerData] Item removed from inventory: {itemId}");
            }
            else
            {
                Debug.Log($"[PlayerData] Item count decreased: {itemId} x{itemCounts[itemId]}");
            }
            
            SyncItemCountsForSerialization();
        }
        
        /// <summary>
        /// Dictionary'yi serialize edilebilir list'lere sync et
        /// </summary>
        private void SyncItemCountsForSerialization()
        {
            _itemCountKeys.Clear();
            _itemCountValues.Clear();
            
            foreach (var kvp in itemCounts)
            {
                _itemCountKeys.Add(kvp.Key);
                _itemCountValues.Add(kvp.Value);
            }
        }
        
        /// <summary>
        /// List'lerden Dictionary'yi restore et
        /// </summary>
        private void InitializeItemCounts()
        {
            if (itemCounts == null)
                itemCounts = new Dictionary<string, int>();
            
            // Eğer dictionary boş ama list'ler doluysa, restore et
            if (itemCounts.Count == 0 && _itemCountKeys != null && _itemCountKeys.Count > 0)
            {
                for (int i = 0; i < _itemCountKeys.Count; i++)
                {
                    itemCounts[_itemCountKeys[i]] = _itemCountValues[i];
                }
            }
            
            // Eski save'ler için: ownedItems'da olan ama count'u olmayan itemler için count=1 ekle
            foreach (var itemId in ownedItems)
            {
                if (!itemCounts.ContainsKey(itemId))
                {
                    itemCounts[itemId] = 1;
                }
            }
        }
        
        /// <summary>
        /// Karakterin başlangıç itemlerini inventory'e ekle
        /// </summary>
        public void AddStartingItems(CharacterData characterData)
        {
            if (characterData.startingItems == null || characterData.startingItems.Length == 0)
                return;
            
            foreach (var item in characterData.startingItems)
            {
                if (item != null)
                {
                    AddItem(item.itemId);
                }
            }
            
            Debug.Log($"[PlayerData] Added {characterData.startingItems.Length} starting items for {characterData.characterName}");
        }
    }
}

