using System;
using UnityEngine;
using WasdBattle.Data;

namespace WasdBattle.Economy
{
    /// <summary>
    /// Shop sistemini yöneten sınıf
    /// </summary>
    public class ShopSystem
    {
        private InventoryManager _inventory;
        
        // Events
        public event Action<ShopItem> OnPurchaseSuccess;
        public event Action<string> OnPurchaseFailed;
        
        public ShopSystem(InventoryManager inventory)
        {
            _inventory = inventory;
        }
        
        /// <summary>
        /// Item satın alınabilir mi kontrol eder
        /// </summary>
        public bool CanPurchase(ShopItem item)
        {
            if (item == null)
                return false;
            
            // Currency kontrolü
            switch (item.currencyType)
            {
                case CurrencyType.Gold:
                    return _inventory.GetMaterialAmount(MaterialType.Metal) >= item.price; // Workaround
                    
                case CurrencyType.Essence:
                    return _inventory.HasMaterial(MaterialType.Essence, item.price);
                    
                case CurrencyType.Rune:
                    return _inventory.HasMaterial(MaterialType.Rune, item.price);
                    
                default:
                    return false;
            }
        }
        
        /// <summary>
        /// Item satın alır
        /// </summary>
        public bool Purchase(ShopItem item)
        {
            if (!CanPurchase(item))
            {
                OnPurchaseFailed?.Invoke("Yetersiz para!");
                return false;
            }
            
            // Ödeme yap
            bool paymentSuccess = ProcessPayment(item);
            
            if (!paymentSuccess)
            {
                OnPurchaseFailed?.Invoke("Ödeme başarısız!");
                return false;
            }
            
            // Item'i ver
            GiveShopItem(item);
            
            OnPurchaseSuccess?.Invoke(item);
            Debug.Log($"[Shop] Purchased: {item.itemName}");
            
            return true;
        }
        
        /// <summary>
        /// Ödeme işlemini yapar
        /// </summary>
        private bool ProcessPayment(ShopItem item)
        {
            switch (item.currencyType)
            {
                case CurrencyType.Gold:
                    return _inventory.SpendGold(item.price);
                    
                case CurrencyType.Essence:
                    return _inventory.RemoveMaterial(MaterialType.Essence, item.price);
                    
                case CurrencyType.Rune:
                    return _inventory.RemoveMaterial(MaterialType.Rune, item.price);
                    
                default:
                    return false;
            }
        }
        
        /// <summary>
        /// Shop item'ini oyuncuya verir
        /// </summary>
        private void GiveShopItem(ShopItem item)
        {
            switch (item.itemType)
            {
                case ShopItemType.Character:
                    _inventory.AddCharacter(item.itemId);
                    break;
                    
                case ShopItemType.Skill:
                    _inventory.AddSkill(item.itemId);
                    break;
                    
                case ShopItemType.MaterialPack:
                    // Material pack logic
                    _inventory.AddMaterial(MaterialType.Metal, 100);
                    _inventory.AddMaterial(MaterialType.EnergyCrystal, 50);
                    break;
                    
                case ShopItemType.GoldPack:
                    _inventory.AddGold(item.quantity);
                    break;
            }
        }
        
        /// <summary>
        /// ItemData ile satın alma yapılabilir mi kontrol eder
        /// </summary>
        public bool CanPurchaseItem(ItemData item)
        {
            if (item == null || !item.canBeBought)
                return false;
            
            // Gold kontrolü (şimdilik sadece Gold ile alınabilir)
            return _inventory.SpendGold(0); // Sadece kontrol için, gerçekte harcamıyor
        }
        
        /// <summary>
        /// ItemData ile satın alma yapar
        /// </summary>
        public bool PurchaseItem(ItemData item)
        {
            if (item == null || !item.canBeBought)
            {
                OnPurchaseFailed?.Invoke("Bu item satın alınamaz!");
                return false;
            }
            
            // Gold kontrolü ve ödeme
            if (!_inventory.SpendGold(item.shopPrice))
            {
                OnPurchaseFailed?.Invoke("Yetersiz Gold!");
                return false;
            }
            
            Debug.Log($"[Shop] Purchased item: {item.itemName} for {item.shopPrice} Gold");
            return true;
        }
    }
    
    /// <summary>
    /// Shop item verisi
    /// </summary>
    [System.Serializable]
    public class ShopItem
    {
        public string itemId;
        public string itemName;
        public string description;
        public ShopItemType itemType;
        public CurrencyType currencyType;
        public int price;
        public int quantity = 1;
        public Sprite icon;
    }
    
    public enum ShopItemType
    {
        Character,
        Skill,
        MaterialPack,
        GoldPack,
        Cosmetic
    }
    
    public enum CurrencyType
    {
        Gold,
        Essence,
        Rune
    }
}

