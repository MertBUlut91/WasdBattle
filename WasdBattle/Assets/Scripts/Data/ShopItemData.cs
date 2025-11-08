using UnityEngine;
using System;

namespace WasdBattle.Data
{
    /// <summary>
    /// Shop Item ScriptableObject - Shop'ta satılacak itemler
    /// Her item hangi currency ile alınacağını belirler
    /// </summary>
    [CreateAssetMenu(fileName = "New Shop Item", menuName = "WasdBattle/Shop Item Data")]
    public class ShopItemData : ScriptableObject
    {
        [Header("Item Reference")]
        public ShopItemType itemType;
        public string itemId; // ItemData, CharacterData veya SkillData ID'si
        
        [Header("Price")]
        public ShopPrice[] prices; // Birden fazla currency ile alınabilir
        
        [Header("Availability")]
        public bool isAvailable = true;
        public bool isLimited; // Sınırlı sayıda mı?
        public int limitedStock = 1; // Sınırlı ise kaç adet
        public bool requiresLevel; // Level requirement var mı?
        public int requiredLevel = 1;
        
        [Header("Special")]
        public bool isFeatured; // Öne çıkan item mi?
        public bool isNew; // Yeni item mi? (NEW badge)
        public bool isOnSale; // İndirimde mi?
        [Range(0f, 1f)]
        public float saleDiscount = 0f; // İndirim oranı (0.2 = %20 off)
        
        /// <summary>
        /// Bu item'i satın alabilir mi? (Level, stock kontrolü)
        /// </summary>
        public bool CanPurchase(PlayerData playerData, int purchasedCount)
        {
            if (!isAvailable)
                return false;
            
            if (requiresLevel && playerData.level < requiredLevel)
                return false;
            
            if (isLimited && purchasedCount >= limitedStock)
                return false;
            
            return true;
        }
        
        /// <summary>
        /// İndirimli fiyatı hesapla
        /// </summary>
        public int GetDiscountedPrice(int originalPrice)
        {
            if (isOnSale && saleDiscount > 0)
            {
                return Mathf.RoundToInt(originalPrice * (1f - saleDiscount));
            }
            return originalPrice;
        }
    }
    
    /// <summary>
    /// Shop item price entry (birden fazla currency seçeneği)
    /// </summary>
    [Serializable]
    public class ShopPrice
    {
        public CurrencyType currencyType;
        public int amount;
        
        public ShopPrice(CurrencyType currency, int price)
        {
            currencyType = currency;
            amount = price;
        }
    }
    
    /// <summary>
    /// Shop'ta satılabilecek item tipleri
    /// </summary>
    public enum ShopItemType
    {
        Equipment,  // ItemData
        Character,  // CharacterData
        Skill,      // SkillData
        Material,   // Crafting material
        Currency    // Currency paketi (örn: 100 Gem)
    }
}

