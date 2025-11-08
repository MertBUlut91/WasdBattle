using UnityEngine;
using UnityEngine.UI;
using TMPro;
using WasdBattle.Economy;

namespace WasdBattle.UI
{
    /// <summary>
    /// Shop UI
    /// </summary>
    public class ShopUI : MonoBehaviour
    {
        [Header("Shop Items")]
        [SerializeField] private Transform _shopItemsContainer;
        [SerializeField] private GameObject _shopItemPrefab;
        
        [Header("Selected Item Display")]
        [SerializeField] private TextMeshProUGUI _itemNameText;
        [SerializeField] private TextMeshProUGUI _itemDescriptionText;
        [SerializeField] private TextMeshProUGUI _itemPriceText;
        [SerializeField] private Image _itemIcon;
        
        [Header("Buttons")]
        [SerializeField] private Button _purchaseButton;
        [SerializeField] private Button _closeButton;
        
        [Header("Available Items")]
        [SerializeField] private ShopItem[] _shopItems;
        
        private ShopItem _selectedItem;
        private ShopSystem _shopSystem;
        
        private void Start()
        {
            SetupButtons();
            PopulateShop();
        }
        
        private void SetupButtons()
        {
            if (_purchaseButton != null)
                _purchaseButton.onClick.AddListener(OnPurchaseClicked);
            
            if (_closeButton != null)
                _closeButton.onClick.AddListener(OnCloseClicked);
        }
        
        private void PopulateShop()
        {
            if (_shopItemsContainer == null || _shopItemPrefab == null)
                return;
            
            // Shop item'ları oluştur
            foreach (var item in _shopItems)
            {
                GameObject itemObj = Instantiate(_shopItemPrefab, _shopItemsContainer);
                Button btn = itemObj.GetComponent<Button>();
                
                if (btn != null)
                {
                    // Item bilgilerini ayarla
                    TextMeshProUGUI nameText = itemObj.GetComponentInChildren<TextMeshProUGUI>();
                    if (nameText != null)
                        nameText.text = $"{item.itemName}\n{item.price} {item.currencyType}";
                    
                    // Click event
                    ShopItem shopItem = item;
                    btn.onClick.AddListener(() => OnItemClicked(shopItem));
                }
            }
        }
        
        private void OnItemClicked(ShopItem item)
        {
            _selectedItem = item;
            UpdateItemDisplay();
        }
        
        private void UpdateItemDisplay()
        {
            if (_selectedItem == null)
                return;
            
            if (_itemNameText != null)
                _itemNameText.text = _selectedItem.itemName;
            
            if (_itemDescriptionText != null)
                _itemDescriptionText.text = _selectedItem.description;
            
            if (_itemPriceText != null)
                _itemPriceText.text = $"{_selectedItem.price} {_selectedItem.currencyType}";
            
            if (_itemIcon != null && _selectedItem.icon != null)
                _itemIcon.sprite = _selectedItem.icon;
        }
        
        private void OnPurchaseClicked()
        {
            if (_selectedItem == null)
            {
                Debug.LogWarning("[Shop] No item selected!");
                return;
            }
            
            // Shop system ile satın alma
            if (_shopSystem == null)
            {
                // Shop system'i oluştur (normalde GameManager'dan alınmalı)
                Debug.LogWarning("[Shop] Shop system not initialized!");
                return;
            }
            
            bool success = _shopSystem.Purchase(_selectedItem);
            
            if (success)
            {
                Debug.Log($"[Shop] Purchased: {_selectedItem.itemName}");
                // Başarı mesajı göster
            }
            else
            {
                Debug.Log($"[Shop] Purchase failed: {_selectedItem.itemName}");
                // Hata mesajı göster
            }
        }
        
        private void OnCloseClicked()
        {
            gameObject.SetActive(false);
        }
        
        public void SetShopSystem(ShopSystem shopSystem)
        {
            _shopSystem = shopSystem;
        }
    }
}

