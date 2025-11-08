using UnityEngine;

namespace WasdBattle.Data
{
    /// <summary>
    /// Currency ScriptableObject - Esnek currency sistemi
    /// Kolayca yeni currency'ler eklenebilir
    /// </summary>
    [CreateAssetMenu(fileName = "New Currency", menuName = "WasdBattle/Currency Data")]
    public class CurrencyData : ScriptableObject
    {
        [Header("Basic Info")]
        public string currencyId;
        public string currencyName;
        [TextArea(2, 3)]
        public string description;
        public Sprite icon;
        
        [Header("Properties")]
        public CurrencyType currencyType;
        public bool isPremium; // Premium currency mi? (ödeme ile alınır)
        public int maxAmount = 999999; // Maksimum tutulabilir miktar
        
        [Header("Display")]
        public Color displayColor = Color.yellow;
        public string displayFormat = "{0}"; // Örn: "{0} Gold", "{0}💎"
        
        /// <summary>
        /// Formatlanmış currency miktarı
        /// </summary>
        public string FormatAmount(int amount)
        {
            return string.Format(displayFormat, amount.ToString("N0"));
        }
    }
    
    /// <summary>
    /// Currency types (genişletilebilir)
    /// </summary>
    public enum CurrencyType
    {
        Gold,           // Ana para birimi
        Gem,            // Premium currency
        Diamond,        // Özel premium currency
        BattleToken,    // PvP currency
        CraftToken,     // Craft currency
        SeasonToken,    // Sezon özel currency
        EventToken      // Event özel currency
    }
}

