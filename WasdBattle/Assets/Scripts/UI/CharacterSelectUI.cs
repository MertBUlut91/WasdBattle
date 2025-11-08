using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using WasdBattle.Core;
using WasdBattle.Data;

namespace WasdBattle.UI
{
    /// <summary>
    /// Karakter seçim ekranı UI
    /// </summary>
    public class CharacterSelectUI : MonoBehaviour
    {
        [Header("Character Display")]
        [SerializeField] private TextMeshProUGUI _characterNameText;
        [SerializeField] private TextMeshProUGUI _characterDescriptionText;
        [SerializeField] private TextMeshProUGUI _statsText;
        [SerializeField] private Image _characterIcon;
        
        [Header("Character List")]
        [SerializeField] private Transform _characterListContainer;
        [SerializeField] private GameObject _characterButtonPrefab;
        
        [Header("Buttons")]
        [SerializeField] private Button _selectButton;
        [SerializeField] private Button _closeButton;
        
        [Header("Available Characters")]
        [SerializeField] private CharacterData[] _allCharacters;
        
        private CharacterData _selectedCharacter;
        private List<Button> _characterButtons = new List<Button>();
        
        private void Start()
        {
            SetupButtons();
            PopulateCharacterList();
        }
        
        private void SetupButtons()
        {
            if (_selectButton != null)
                _selectButton.onClick.AddListener(OnSelectClicked);
            
            if (_closeButton != null)
                _closeButton.onClick.AddListener(OnCloseClicked);
        }
        
        private void PopulateCharacterList()
        {
            if (_characterListContainer == null || _characterButtonPrefab == null)
                return;
            
            // Mevcut butonları temizle
            foreach (var btn in _characterButtons)
            {
                if (btn != null)
                    Destroy(btn.gameObject);
            }
            _characterButtons.Clear();
            
            // Her karakter için buton oluştur
            foreach (var character in _allCharacters)
            {
                GameObject btnObj = Instantiate(_characterButtonPrefab, _characterListContainer);
                Button btn = btnObj.GetComponent<Button>();
                
                if (btn != null)
                {
                    // Buton text'ini ayarla
                    TextMeshProUGUI btnText = btn.GetComponentInChildren<TextMeshProUGUI>();
                    if (btnText != null)
                        btnText.text = character.characterName;
                    
                    // Click event'i ekle
                    CharacterData charData = character; // Local copy for closure
                    btn.onClick.AddListener(() => OnCharacterClicked(charData));
                    
                    _characterButtons.Add(btn);
                }
            }
            
            // İlk karakteri seç
            if (_allCharacters.Length > 0)
            {
                OnCharacterClicked(_allCharacters[0]);
            }
        }
        
        private void OnCharacterClicked(CharacterData character)
        {
            _selectedCharacter = character;
            UpdateCharacterDisplay();
        }
        
        private void UpdateCharacterDisplay()
        {
            if (_selectedCharacter == null)
                return;
            
            // Character info
            if (_characterNameText != null)
                _characterNameText.text = _selectedCharacter.characterName;
            
            if (_characterDescriptionText != null)
                _characterDescriptionText.text = _selectedCharacter.description;
            
            if (_characterIcon != null && _selectedCharacter.characterIcon != null)
                _characterIcon.sprite = _selectedCharacter.characterIcon;
            
            // Stats
            if (_statsText != null)
            {
                _statsText.text = $"HP: {_selectedCharacter.baseHealth}\n" +
                                 $"Stamina: {_selectedCharacter.baseStamina}\n" +
                                 $"Regen: {_selectedCharacter.staminaRegenRate}/s\n" +
                                 $"Defense: {_selectedCharacter.baseDefense:P0}";
            }
        }
        
        private void OnSelectClicked()
        {
            if (_selectedCharacter == null)
            {
                Debug.LogWarning("[CharacterSelect] No character selected!");
                return;
            }
            
            // Karakteri seç
            var playerData = GameManager.Instance.CurrentPlayerData;
            if (playerData != null)
            {
                playerData.selectedCharacterId = _selectedCharacter.characterId;
                GameManager.Instance.SavePlayerData();
                
                Debug.Log($"[CharacterSelect] Selected character: {_selectedCharacter.characterName}");
            }
            
            OnCloseClicked();
        }
        
        private void OnCloseClicked()
        {
            gameObject.SetActive(false);
        }
    }
}

