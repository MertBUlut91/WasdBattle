using UnityEngine;

namespace WasdBattle.Data
{
    /// <summary>
    /// Karakter verilerini tutan ScriptableObject
    /// </summary>
    [CreateAssetMenu(fileName = "NewCharacter", menuName = "WasdBattle/Character Data")]
    public class CharacterData : ScriptableObject
    {
        [Header("Basic Info")]
        public string characterId;
        public string characterName;
        public CharacterClass characterClass;
        
        [TextArea(3, 5)]
        public string description;
        
        [Header("Base Stats")]
        public int baseHealth = 100;
        public int baseStamina = 100;
        public float staminaRegenRate = 10f; // per second
        public float baseDefense = 0f; // 0-1 arası hasar azaltma
        
        [Header("Starting Equipment")]
        [Tooltip("Karakterin başlangıçta sahip olduğu itemler")]
        public ItemData[] startingItems;
        
        [Header("Starting Skills")]
        public SkillData[] startingSkills = new SkillData[3];
        
        [Header("Passive & Ultimate")]
        public PassiveAbilityData passive;
        public SkillData ultimate;
        
        [Header("All Available Skills (Unlock edilebilir)")]
        [Tooltip("Bu karakterin açabileceği tüm skill'ler (kategori bazlı)")]
        public SkillData[] allLightSkills;    // Light - Hızlı, düşük hasar
        public SkillData[] allNormalSkills;   // Normal - Orta hız, orta hasar
        public SkillData[] allHeavySkills;    // Heavy - Yavaş, yüksek hasar
        public SkillData[] allUltimateSkills; // Ultimate - En güçlü skill
        public SkillData[] allPassiveSkills;  // Passive - Pasif yetenekler
        public SkillData[] allComboSkills;    // Combo - Combo skill'ler
        
        [Header("Visual")]
        public Sprite characterIcon;
        public GameObject characterPrefab;
        public Color characterColor = Color.white;
        
        [Header("Unlock Requirements")]
        public bool isStarterCharacter = true; // Başlangıçta açık mı?
        public bool requiresUnlock = false; // Unlock gerekiyor mu?
        public int requiredLevel = 1; // Minimum level
        public ShopPrice[] unlockPrices; // Unlock için gerekli currency'ler (birden fazla seçenek)
        
        /// <summary>
        /// Bu karakteri unlock edebilir mi?
        /// </summary>
        public bool CanUnlock(PlayerData playerData)
        {
            if (isStarterCharacter || !requiresUnlock)
                return true;
            
            if (playerData.level < requiredLevel)
                return false;
            
            // En az bir unlock price'ı karşılayabilmeli
            if (unlockPrices == null || unlockPrices.Length == 0)
                return true;
            
            foreach (var price in unlockPrices)
            {
                if (playerData.HasCurrency(price.currencyType, price.amount))
                    return true;
            }
            
            return false;
        }
    }
    
    public enum CharacterClass
    {
        Mage,      // Alev Büyücüsü - Yüksek hasar, düşük stamina
        Warrior,   // Kalkan Savaşçısı - Düşük hasar, yüksek savunma
        Ninja,     // Ninja - Kısa combo, yüksek hız
        Assassin,  // Suikastçi
        Paladin,   // Paladin
        Ranger     // Okçu
    }
}

