using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using WasdBattle.Core;
using WasdBattle.Data;

namespace WasdBattle.UI
{
    /// <summary>
    /// Karakter seçim ekranı UI (Unlock System ile)
    /// </summary>
    public class CharacterSelectUI : MonoBehaviour
    {
        [Header("Character Grid")]
        [SerializeField] private Transform _characterGridContainer; // Grid Layout Group içeren Content
        [SerializeField] private GameObject _characterCardPrefab; // CharacterCardUI prefab
        
        [Header("Buttons")]
        [SerializeField] private Button _closeButton;
        
        [Header("Available Characters")]
        [SerializeField] private CharacterData[] _allCharacters; // Inspector'dan atanacak
        
        private List<GameObject> _characterCards = new List<GameObject>();
        
        private void Start()
        {
            SetupButtons();
        }
        
        private void OnEnable()
        {
            // Panel her açıldığında listeyi yenile
            PopulateCharacterGrid();
        }
        
        private void SetupButtons()
        {
            if (_closeButton != null)
                _closeButton.onClick.AddListener(ClosePanel);
        }
        
        /// <summary>
        /// Karakter grid'ini doldur (Owned + Locked)
        /// </summary>
        private void PopulateCharacterGrid()
        {
            if (_characterGridContainer == null)
            {
                Debug.LogError("[CharacterSelectUI] Character Grid Container is null!");
                return;
            }
            
            if (_characterCardPrefab == null)
            {
                Debug.LogError("[CharacterSelectUI] Character Card Prefab is null!");
                return;
            }
            
            // Mevcut kartları temizle
            foreach (var card in _characterCards)
            {
                if (card != null)
                    Destroy(card);
            }
            _characterCards.Clear();
            
            var playerData = GameManager.Instance.CurrentPlayerData;
            if (playerData == null)
            {
                Debug.LogError("[CharacterSelectUI] PlayerData is null!");
                return;
            }
            
            // Eğer _allCharacters boşsa, Resources'tan yükle
            if (_allCharacters == null || _allCharacters.Length == 0)
            {
                Debug.LogWarning("[CharacterSelectUI] No characters assigned! Trying to load from Resources...");
                _allCharacters = Resources.LoadAll<CharacterData>("ScriptableObjects/Characters");
                
                if (_allCharacters.Length == 0)
                {
                    Debug.LogError("[CharacterSelectUI] No characters found in Resources!");
                    return;
                }
            }
            
            Debug.Log($"[CharacterSelectUI] Loading {_allCharacters.Length} characters...");
            
            // Her karakter için card oluştur
            foreach (var character in _allCharacters)
            {
                if (character == null)
                {
                    Debug.LogWarning("[CharacterSelectUI] Null character in array!");
                    continue;
                }
                
                // Karaktere sahip mi?
                bool isOwned = playerData.ownedCharacters.Contains(character.characterId);
                
                // Card oluştur
                GameObject cardObj = Instantiate(_characterCardPrefab, _characterGridContainer);
                _characterCards.Add(cardObj);
                
                // CharacterCardUI component'ini al ve setup et
                CharacterCardUI cardUI = cardObj.GetComponent<CharacterCardUI>();
                if (cardUI != null)
                {
                    cardUI.Setup(character, isOwned);
                    Debug.Log($"[CharacterSelectUI] Created card for {character.characterName} (Owned: {isOwned})");
                }
                else
                {
                    Debug.LogError($"[CharacterSelectUI] CharacterCardUI component not found on prefab!");
                }
            }
            
            Debug.Log($"[CharacterSelectUI] Created {_characterCards.Count} character cards");
        }
        
        /// <summary>
        /// Panel'i aç
        /// </summary>
        public void OpenPanel()
        {
            gameObject.SetActive(true);
            PopulateCharacterGrid();
        }
        
        /// <summary>
        /// Panel'i kapat
        /// </summary>
        public void ClosePanel()
        {
            gameObject.SetActive(false);
        }
        
        /// <summary>
        /// Listeyi yenile (unlock sonrası)
        /// </summary>
        public void RefreshList()
        {
            PopulateCharacterGrid();
        }
    }
}

