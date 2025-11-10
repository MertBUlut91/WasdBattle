using UnityEngine;
using UnityEngine.UI;
using TMPro;
using WasdBattle.Data;
using WasdBattle.Core;

namespace WasdBattle.UI
{
    public class CharacterCardUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Image _characterPortrait;
        [SerializeField] private TextMeshProUGUI _characterName;
        [SerializeField] private TextMeshProUGUI _characterClass;
        [SerializeField] private TextMeshProUGUI _hpText;
        [SerializeField] private TextMeshProUGUI _staminaText;
        [SerializeField] private TextMeshProUGUI _defenseText;
        
        [Header("Buttons")]
        [SerializeField] private Button _selectButton;
        [SerializeField] private GameObject _lockedPanel;
        [SerializeField] private TextMeshProUGUI _requiredLevelText;
        [SerializeField] private TextMeshProUGUI _priceText;
        [SerializeField] private Button _unlockButton;
        
        private CharacterData _characterData;
        private bool _isOwned;
        
        public void Setup(CharacterData character, bool isOwned)
        {
            _characterData = character;
            _isOwned = isOwned;
            
            // Basic info
            _characterPortrait.sprite = character.characterIcon;
            _characterName.text = character.characterName;
            _characterClass.text = character.characterClass.ToString();
            
            // Stats
            _hpText.text = $"HP: {character.baseHealth}";
            _staminaText.text = $"Stamina: {character.baseStamina}";
            _defenseText.text = $"Defense: {character.baseDefense * 100}%";
            
            // Owned or Locked
            if (_isOwned)
            {
                _selectButton.gameObject.SetActive(true);
                _lockedPanel.SetActive(false);
                _selectButton.onClick.AddListener(OnSelectClicked);
            }
            else
            {
                _selectButton.gameObject.SetActive(false);
                _lockedPanel.SetActive(true);
                
                // Unlock requirements
                _requiredLevelText.text = $"Level {character.requiredLevel}";
                
                if (character.unlockPrices != null && character.unlockPrices.Length > 0)
                {
                    // Show first price option
                    var price = character.unlockPrices[0];
                    _priceText.text = $"{price.amount} {price.currencyType}";
                }
                
                _unlockButton.onClick.AddListener(OnUnlockClicked);
                
                // Can unlock?
                var playerData = GameManager.Instance.CurrentPlayerData;
                _unlockButton.interactable = character.CanUnlock(playerData);
            }
        }
        
        private void OnSelectClicked()
        {
            var playerData = GameManager.Instance.CurrentPlayerData;
            playerData.selectedCharacterId = _characterData.characterId;
            GameManager.Instance.SavePlayerData();
            
            Debug.Log($"[CharacterCard] Selected: {_characterData.characterName}");
        }
        
        private void OnUnlockClicked()
        {
            var playerData = GameManager.Instance.CurrentPlayerData;
            
            // Find affordable price
            ShopPrice selectedPrice = null;
            foreach (var price in _characterData.unlockPrices)
            {
                if (playerData.HasCurrency(price.currencyType, price.amount))
                {
                    selectedPrice = price;
                    break;
                }
            }
            
            if (selectedPrice != null)
            {
                // Spend currency
                playerData.ModifyCurrency(selectedPrice.currencyType, -selectedPrice.amount);
                
                // Add character
                playerData.ownedCharacters.Add(_characterData.characterId);
                
                // Create default loadout
                var loadout = new CharacterLoadout(_characterData.characterId);
                playerData.characterLoadouts.Add(loadout);
                
                // Save
                GameManager.Instance.SavePlayerData();
                
                Debug.Log($"[CharacterCard] Unlocked: {_characterData.characterName}");
                
                // Refresh UI
                Setup(_characterData, true);
                
                // Parent CharacterSelectUI'yi yenile
                CharacterSelectUI parentUI = GetComponentInParent<CharacterSelectUI>();
                if (parentUI != null)
                {
                    parentUI.RefreshList();
                }
            }
        }
    }
}