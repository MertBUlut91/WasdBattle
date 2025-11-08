using UnityEngine;
using UnityEngine.UI;
using TMPro;
using WasdBattle.Core;
using WasdBattle.Economy;

namespace WasdBattle.UI
{
    /// <summary>
    /// Envanter UI
    /// </summary>
    public class InventoryUI : MonoBehaviour
    {
        [Header("Material Display")]
        [SerializeField] private TextMeshProUGUI _metalText;
        [SerializeField] private TextMeshProUGUI _crystalText;
        [SerializeField] private TextMeshProUGUI _runeText;
        [SerializeField] private TextMeshProUGUI _essenceText;
        [SerializeField] private TextMeshProUGUI _goldText;
        
        [Header("Tabs")]
        [SerializeField] private Button _materialsTabButton;
        [SerializeField] private Button _skillsTabButton;
        [SerializeField] private Button _charactersTabButton;
        
        [Header("Tab Panels")]
        [SerializeField] private GameObject _materialsPanel;
        [SerializeField] private GameObject _skillsPanel;
        [SerializeField] private GameObject _charactersPanel;
        
        [Header("Buttons")]
        [SerializeField] private Button _closeButton;
        
        private void Start()
        {
            SetupButtons();
            UpdateInventoryDisplay();
            ShowMaterialsTab();
        }
        
        private void OnEnable()
        {
            UpdateInventoryDisplay();
        }
        
        private void SetupButtons()
        {
            if (_materialsTabButton != null)
                _materialsTabButton.onClick.AddListener(ShowMaterialsTab);
            
            if (_skillsTabButton != null)
                _skillsTabButton.onClick.AddListener(ShowSkillsTab);
            
            if (_charactersTabButton != null)
                _charactersTabButton.onClick.AddListener(ShowCharactersTab);
            
            if (_closeButton != null)
                _closeButton.onClick.AddListener(OnCloseClicked);
        }
        
        private void UpdateInventoryDisplay()
        {
            var playerData = GameManager.Instance.CurrentPlayerData;
            
            if (playerData == null)
                return;
            
            // Materials
            if (_metalText != null)
                _metalText.text = $"Metal: {playerData.metal}";
            
            if (_crystalText != null)
                _crystalText.text = $"Crystal: {playerData.energyCrystal}";
            
            if (_runeText != null)
                _runeText.text = $"Rune: {playerData.rune}";
            
            if (_essenceText != null)
                _essenceText.text = $"Essence: {playerData.essence}";
            
            if (_goldText != null)
                _goldText.text = $"Gold: {playerData.gold}";
        }
        
        private void ShowMaterialsTab()
        {
            SetActivePanel(_materialsPanel);
        }
        
        private void ShowSkillsTab()
        {
            SetActivePanel(_skillsPanel);
            // Skill listesini güncelle
            UpdateSkillsList();
        }
        
        private void ShowCharactersTab()
        {
            SetActivePanel(_charactersPanel);
            // Karakter listesini güncelle
            UpdateCharactersList();
        }
        
        private void SetActivePanel(GameObject activePanel)
        {
            if (_materialsPanel != null)
                _materialsPanel.SetActive(_materialsPanel == activePanel);
            
            if (_skillsPanel != null)
                _skillsPanel.SetActive(_skillsPanel == activePanel);
            
            if (_charactersPanel != null)
                _charactersPanel.SetActive(_charactersPanel == activePanel);
        }
        
        private void UpdateSkillsList()
        {
            var playerData = GameManager.Instance.CurrentPlayerData;
            
            if (playerData == null)
                return;
            
            Debug.Log($"[Inventory] Player has {playerData.ownedSkills.Count} skills");
            // Skill listesi UI'ı oluşturulacak
        }
        
        private void UpdateCharactersList()
        {
            var playerData = GameManager.Instance.CurrentPlayerData;
            
            if (playerData == null)
                return;
            
            Debug.Log($"[Inventory] Player has {playerData.ownedCharacters.Count} characters");
            // Karakter listesi UI'ı oluşturulacak
        }
        
        private void OnCloseClicked()
        {
            gameObject.SetActive(false);
        }
    }
}

