using UnityEngine;
using UnityEngine.UI;
using TMPro;
using WasdBattle.Data;
using WasdBattle.Core;
using System.Collections.Generic;

namespace WasdBattle.UI
{
    public class EquipmentUI : MonoBehaviour
    {
        [Header("Character List")]
        [SerializeField] private ScrollRect _characterScrollView;
        [SerializeField] private Transform _characterListContent;
        [SerializeField] private GameObject _characterItemPrefab;
        
        [Header("Equipment Slots")]
        [SerializeField] private EquipmentSlotUI[] _equipmentSlots; // 9 slot
        
        [Header("Item List")]
        [SerializeField] private ScrollRect _itemScrollView;
        [SerializeField] private Transform _itemListContent;
        [SerializeField] private GameObject _itemCardPrefab;
        [SerializeField] private TMP_Dropdown _filterDropdown;
        
        [Header("Panels")]
        [SerializeField] private GameObject _characterPanel;
        [SerializeField] private Button _closeButton;
        
        private string _selectedCharacterId;
        private CharacterLoadout _currentLoadout;
        
        private void Start()
        {
            _closeButton.onClick.AddListener(ClosePanel);
            _filterDropdown.onValueChanged.AddListener(OnFilterChanged);
            
            LoadCharacterList();
        }
        
        public void OpenPanel()
        {
            _characterPanel.SetActive(true);
            LoadCharacterList();
        }
        
        public void ClosePanel()
        {
            _characterPanel.SetActive(false);
        }
        
        private void LoadCharacterList()
        {
            // Clear existing
            foreach (Transform child in _characterListContent)
            {
                Destroy(child.gameObject);
            }
            
            var playerData = GameManager.Instance.CurrentPlayerData;
            
            // Load all characters (owned + locked)
            // TODO: Load from Resources or AssetDatabase
            // For now, just show owned characters
            foreach (var characterId in playerData.ownedCharacters)
            {
                CreateCharacterItem(characterId);
            }
        }
        
        private void CreateCharacterItem(string characterId)
        {
            GameObject item = Instantiate(_characterItemPrefab, _characterListContent);
            
            // TODO: Set character info
            // item.GetComponent<CharacterItemUI>().Setup(characterData, OnCharacterSelected);
        }
        
        private void OnCharacterSelected(string characterId)
        {
            _selectedCharacterId = characterId;
            
            // Load character's loadout
            var playerData = GameManager.Instance.CurrentPlayerData;
            _currentLoadout = playerData.GetLoadoutForCharacter(characterId);
            
            // Update equipment slots
            UpdateEquipmentSlots();
            
            // Update item list
            UpdateItemList();
        }
        
        private void UpdateEquipmentSlots()
        {
            // Update each slot with equipped item
            for (int i = 0; i < _equipmentSlots.Length; i++)
            {
                EquipmentSlot slot = (EquipmentSlot)i;
                string itemId = _currentLoadout.GetEquippedItem(slot);
                
                if (!string.IsNullOrEmpty(itemId))
                {
                    // TODO: Load ItemData and display
                    // _equipmentSlots[i].SetItem(itemData);
                }
                else
                {
                    _equipmentSlots[i].Clear();
                }
            }
        }
        
        private void UpdateItemList()
        {
            // Clear existing
            foreach (Transform child in _itemListContent)
            {
                Destroy(child.gameObject);
            }
            
            var playerData = GameManager.Instance.CurrentPlayerData;
            
            // Filter items by selected character's class
            // TODO: Load ItemData for each ownedItem
            // Filter by class and current filter dropdown
            
            foreach (var itemId in playerData.ownedItems)
            {
                // TODO: Load ItemData
                // if (itemData.CanBeEquippedBy(characterClass))
                // {
                //     CreateItemCard(itemData);
                // }
            }
        }
        
        private void OnFilterChanged(int filterIndex)
        {
            UpdateItemList();
        }
        
        public void OnEquipItem(ItemData item, EquipmentSlot slot)
        {
            _currentLoadout.EquipItem(slot, item.itemId);
            UpdateEquipmentSlots();
            
            // Save to cloud
            GameManager.Instance.SavePlayerData();
        }
        
        public void OnUnequipItem(EquipmentSlot slot)
        {
            _currentLoadout.UnequipItem(slot);
            UpdateEquipmentSlots();
            
            // Save to cloud
            GameManager.Instance.SavePlayerData();
        }
    }
    
    [System.Serializable]
    public class EquipmentSlotUI
    {
        public EquipmentSlot slotType;
        public Image itemIcon;
        public TextMeshProUGUI slotName;
        public Button unequipButton;
        
        public void SetItem(ItemData item)
        {
            itemIcon.sprite = item.icon;
            itemIcon.enabled = true;
            unequipButton.gameObject.SetActive(true);
        }
        
        public void Clear()
        {
            itemIcon.sprite = null;
            itemIcon.enabled = false;
            unequipButton.gameObject.SetActive(false);
        }
    }
}