using UnityEngine;
using UnityEngine.UI;
using TMPro;
using WasdBattle.Core;
using WasdBattle.Data;

namespace WasdBattle.UI
{
    /// <summary>
    /// Debug menu for testing - Add items, currency, etc.
    /// </summary>
    public class DebugMenuUI : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private GameObject _panel;
        [SerializeField] private Button _toggleButton;
        [SerializeField] private Button _closeButton;
        
        [Header("Item Buttons")]
        [SerializeField] private Button _addWarriorItemsButton;
        [SerializeField] private Button _addMageItemsButton;
        [SerializeField] private Button _addRogueItemsButton;
        [SerializeField] private Button _addAllItemsButton;
        [SerializeField] private Button _clearInventoryButton;
        
        [Header("Currency Buttons")]
        [SerializeField] private Button _add1000GoldButton;
        [SerializeField] private Button _add100GemsButton;
        
        [Header("Info")]
        [SerializeField] private TextMeshProUGUI _inventoryCountText;
        
        private void Start()
        {
            if (_panel != null)
                _panel.SetActive(false);
            
            SetupButtons();
        }
        
        private void SetupButtons()
        {
            if (_toggleButton != null)
                _toggleButton.onClick.AddListener(TogglePanel);
            
            if (_closeButton != null)
                _closeButton.onClick.AddListener(() => _panel.SetActive(false));
            
            if (_addWarriorItemsButton != null)
                _addWarriorItemsButton.onClick.AddListener(AddWarriorItems);
            
            if (_addMageItemsButton != null)
                _addMageItemsButton.onClick.AddListener(AddMageItems);
            
            if (_addRogueItemsButton != null)
                _addRogueItemsButton.onClick.AddListener(AddRogueItems);
            
            if (_addAllItemsButton != null)
                _addAllItemsButton.onClick.AddListener(AddAllItems);
            
            if (_clearInventoryButton != null)
                _clearInventoryButton.onClick.AddListener(ClearInventory);
            
            if (_add1000GoldButton != null)
                _add1000GoldButton.onClick.AddListener(() => AddGold(1000));
            
            if (_add100GemsButton != null)
                _add100GemsButton.onClick.AddListener(() => AddGems(100));
        }
        
        private void TogglePanel()
        {
            if (_panel != null)
            {
                _panel.SetActive(!_panel.activeSelf);
                if (_panel.activeSelf)
                    UpdateInfo();
            }
        }
        
        private void AddWarriorItems()
        {
            var playerData = GameManager.Instance.CurrentPlayerData;
            if (playerData == null)
            {
                Debug.LogError("[DebugMenu] PlayerData is null!");
                return;
            }
            
            playerData.AddItem("item_warrior_sword");
            playerData.AddItem("item_warrior_helmet");
            playerData.AddItem("item_warrior_chest");
            playerData.AddItem("item_warrior_gloves");
            playerData.AddItem("item_warrior_legs");
            
            GameManager.Instance.DataManager.SavePlayerDataAsync(playerData);
            UpdateInfo();
            
            Debug.Log("[DebugMenu] Added Warrior items");
        }
        
        private void AddMageItems()
        {
            var playerData = GameManager.Instance.CurrentPlayerData;
            if (playerData == null)
            {
                Debug.LogError("[DebugMenu] PlayerData is null!");
                return;
            }
            
            playerData.AddItem("item_mage_staff");
            playerData.AddItem("item_mage_helmet");
            playerData.AddItem("item_mage_chest");
            
            GameManager.Instance.DataManager.SavePlayerDataAsync(playerData);
            UpdateInfo();
            
            Debug.Log("[DebugMenu] Added Mage items");
        }
        
        private void AddRogueItems()
        {
            var playerData = GameManager.Instance.CurrentPlayerData;
            if (playerData == null)
            {
                Debug.LogError("[DebugMenu] PlayerData is null!");
                return;
            }
            
            playerData.AddItem("item_rogue_dagger");
            playerData.AddItem("item_rogue_helmet");
            playerData.AddItem("item_rogue_chest");
            
            GameManager.Instance.DataManager.SavePlayerDataAsync(playerData);
            UpdateInfo();
            
            Debug.Log("[DebugMenu] Added Rogue items");
        }
        
        private void AddAllItems()
        {
            var playerData = GameManager.Instance.CurrentPlayerData;
            if (playerData == null)
            {
                Debug.LogError("[DebugMenu] PlayerData is null!");
                return;
            }
            
            var allItems = Resources.LoadAll<ItemData>("Items");
            
            foreach (var item in allItems)
            {
                playerData.AddItem(item.itemId);
            }
            
            GameManager.Instance.DataManager.SavePlayerDataAsync(playerData);
            UpdateInfo();
            
            Debug.Log($"[DebugMenu] Added {allItems.Length} items to inventory");
        }
        
        private void ClearInventory()
        {
            var playerData = GameManager.Instance.CurrentPlayerData;
            if (playerData == null)
            {
                Debug.LogError("[DebugMenu] PlayerData is null!");
                return;
            }
            
            playerData.ownedItems.Clear();
            
            GameManager.Instance.DataManager.SavePlayerDataAsync(playerData);
            UpdateInfo();
            
            Debug.Log("[DebugMenu] Inventory cleared");
        }
        
        private void AddGold(int amount)
        {
            var playerData = GameManager.Instance.CurrentPlayerData;
            if (playerData == null)
            {
                Debug.LogError("[DebugMenu] PlayerData is null!");
                return;
            }
            
            playerData.gold += amount;
            
            GameManager.Instance.DataManager.SavePlayerDataAsync(playerData);
            
            Debug.Log($"[DebugMenu] Added {amount} gold");
        }
        
        private void AddGems(int amount)
        {
            var playerData = GameManager.Instance.CurrentPlayerData;
            if (playerData == null)
            {
                Debug.LogError("[DebugMenu] PlayerData is null!");
                return;
            }
            
            playerData.gem += amount;
            
            GameManager.Instance.DataManager.SavePlayerDataAsync(playerData);
            
            Debug.Log($"[DebugMenu] Added {amount} gems");
        }
        
        private void UpdateInfo()
        {
            if (_inventoryCountText != null)
            {
                var playerData = GameManager.Instance.CurrentPlayerData;
                if (playerData != null)
                {
                    _inventoryCountText.text = $"Inventory: {playerData.ownedItems.Count} items\nGold: {playerData.gold}";
                }
                else
                {
                    _inventoryCountText.text = "PlayerData not loaded";
                }
            }
        }
        
        // F12 tuşu yerine UI button kullanın
        // Eğer F12 kullanmak isterseniz, Edit → Project Settings → Player → Active Input Handling → "Both" seçin
    }
}

