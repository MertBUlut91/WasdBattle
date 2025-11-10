using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using WasdBattle.Core;
using WasdBattle.Data;

namespace WasdBattle.UI
{
    /// <summary>
    /// Karakter seçim ve skill yönetim paneli
    /// Sol: Karakter listesi, Orta: 3D karakter, Sağ: Info + Stats + Skills
    /// </summary>
    public class CharacterPanelUI : MonoBehaviour
    {
        [Header("Panel")]
        [SerializeField] private GameObject _panel;
        [SerializeField] private Button _closeButton;
        
        [Header("Character List (Sol Panel)")]
        [SerializeField] private Transform _characterListContent;
        [SerializeField] private GameObject _characterListItemPrefab;
        
        [Header("Character Info (Sağ Panel - Üst)")]
        [SerializeField] private TextMeshProUGUI _characterNameText;
        [SerializeField] private TextMeshProUGUI _characterLevelText;
        [SerializeField] private TextMeshProUGUI _characterDescriptionText;
        
        [Header("Character Stats (Sağ Panel - Orta)")]
        [SerializeField] private TextMeshProUGUI _hpText;
        [SerializeField] private TextMeshProUGUI _staminaText;
        [SerializeField] private TextMeshProUGUI _armorText;
        [SerializeField] private TextMeshProUGUI _magicResistText;
        
        [Header("Skill Categories (Sağ Panel - Alt)")]
        [SerializeField] private Button _lightSkillButton;
        [SerializeField] private Button _normalSkillButton;
        [SerializeField] private Button _heavySkillButton;
        [SerializeField] private Button _ultimateSkillButton;
        [SerializeField] private Button _passiveSkillButton;
        [SerializeField] private Button _comboSkillButton;
        
        [Header("Skill List (Sağ Panel - Scroll)")]
        [SerializeField] private Transform _skillListContent;
        [SerializeField] private GameObject _skillCardPrefab;
        
        [Header("Skill Details (Sağ Panel - En Alt)")]
        [SerializeField] private GameObject _skillDetailsPanel;
        [SerializeField] private Image _skillDetailIcon;
        [SerializeField] private TextMeshProUGUI _skillDetailNameText;
        [SerializeField] private TextMeshProUGUI _skillDetailDamageText;
        [SerializeField] private TextMeshProUGUI _skillDetailEffectText;
        
        [Header("References")]
        [SerializeField] private CharacterDisplayController _characterDisplayController;
        [SerializeField] private MainMenuUI _mainMenuUI;
        
        private string _selectedCharacterId;
        private SkillType _selectedSkillCategory = SkillType.Light; // Light
        private SkillData _selectedSkill;
        private List<GameObject> _characterListItems = new List<GameObject>();
        private List<GameObject> _skillCards = new List<GameObject>();
        
        private void Start()
        {
            SetupButtons();
            
            if (_skillDetailsPanel != null)
                _skillDetailsPanel.SetActive(false);
        }
        
        private void SetupButtons()
        {
            if (_closeButton != null)
                _closeButton.onClick.AddListener(ClosePanel);
            
            if (_lightSkillButton != null)
                _lightSkillButton.onClick.AddListener(() => OnSkillCategorySelected(SkillType.Light));
            
            if (_normalSkillButton != null)
                _normalSkillButton.onClick.AddListener(() => OnSkillCategorySelected(SkillType.Normal));
            
            if (_heavySkillButton != null)
                _heavySkillButton.onClick.AddListener(() => OnSkillCategorySelected(SkillType.Heavy));
            
            if (_ultimateSkillButton != null)
                _ultimateSkillButton.onClick.AddListener(() => OnSkillCategorySelected(SkillType.Ultimate));
            
            if (_passiveSkillButton != null)
                _passiveSkillButton.onClick.AddListener(() => OnSkillCategorySelected(SkillType.Passive));
            
            if (_comboSkillButton != null)
                _comboSkillButton.onClick.AddListener(() => OnSkillCategorySelected(SkillType.Combo));
        }
        
        public void OpenPanel()
        {
            if (_panel != null)
                _panel.SetActive(true);
            
            LoadCharacterList();
            
            // Seçili karakteri göster
            var playerData = GameManager.Instance?.CurrentPlayerData;
            if (playerData != null && !string.IsNullOrEmpty(playerData.selectedCharacterId))
            {
                SelectCharacter(playerData.selectedCharacterId);
            }
        }
        
        public void ClosePanel()
        {
            if (_panel != null)
                _panel.SetActive(false);
            
            // Main menu'ye kamerayı döndür
            if (_mainMenuUI != null)
                _mainMenuUI.OnPanelClosed();
        }
        
        private void LoadCharacterList()
        {
            // Mevcut listeyi temizle
            foreach (var item in _characterListItems)
            {
                if (item != null)
                    Destroy(item);
            }
            _characterListItems.Clear();
            
            var playerData = GameManager.Instance?.CurrentPlayerData;
            if (playerData == null) return;
            
            // Owned karakterleri yükle
            foreach (var characterId in playerData.ownedCharacters)
            {
                CharacterData characterData = LoadCharacterData(characterId);
                if (characterData != null)
                {
                    CreateCharacterListItem(characterData);
                }
            }
        }
        
        private void CreateCharacterListItem(CharacterData characterData)
        {
            if (_characterListItemPrefab == null || _characterListContent == null)
                return;
            
            GameObject item = Instantiate(_characterListItemPrefab, _characterListContent);
            _characterListItems.Add(item);
            
            // CharacterListItemUI script'ini kullan
            var itemUI = item.GetComponent<CharacterListItemUI>();
            if (itemUI != null)
            {
                var playerData = GameManager.Instance?.CurrentPlayerData;
                bool isSelected = playerData != null && playerData.selectedCharacterId == characterData.characterId;
                
                itemUI.Setup(
                    characterData.characterIcon,
                    characterData.characterName,
                    playerData?.level ?? 1,
                    isSelected,
                    () => SelectCharacter(characterData.characterId)
                );
            }
            else
            {
                // Fallback: Manuel setup
                var iconImage = item.transform.Find("Icon")?.GetComponent<Image>();
                if (iconImage != null && characterData.characterIcon != null)
                    iconImage.sprite = characterData.characterIcon;
                
                var nameText = item.transform.Find("Name")?.GetComponent<TextMeshProUGUI>();
                if (nameText != null)
                    nameText.text = characterData.characterName;
                
                // Button click event
                var button = item.GetComponent<Button>();
                if (button != null)
                {
                    string charId = characterData.characterId;
                    button.onClick.AddListener(() => SelectCharacter(charId));
                }
            }
        }
        
        private void SelectCharacter(string characterId)
        {
            _selectedCharacterId = characterId;
            
            // 3D modeli güncelle
            if (_characterDisplayController != null)
                _characterDisplayController.LoadCharacter(characterId);
            
            // Karakter bilgilerini güncelle
            CharacterData characterData = LoadCharacterData(characterId);
            if (characterData != null)
            {
                UpdateCharacterInfo(characterData);
                UpdateCharacterStats(characterData);
                
                // Skill listesini güncelle
                LoadSkillList(characterData);
            }
            
            // PlayerData'da seçili karakteri güncelle
            var playerData = GameManager.Instance?.CurrentPlayerData;
            if (playerData != null)
            {
                playerData.selectedCharacterId = characterId;
                GameManager.Instance.SavePlayerData();
            }
            
            // Karakter listesini yenile (selected indicator için)
            LoadCharacterList();
        }
        
        private void UpdateCharacterInfo(CharacterData characterData)
        {
            if (_characterNameText != null)
                _characterNameText.text = characterData.characterName;
            
            if (_characterLevelText != null)
            {
                var playerData = GameManager.Instance?.CurrentPlayerData;
                _characterLevelText.text = $"Level {playerData?.level ?? 1}";
            }
            
            if (_characterDescriptionText != null)
                _characterDescriptionText.text = characterData.description;
        }
        
        private void UpdateCharacterStats(CharacterData characterData)
        {
            // Base stats + equipped items
            var playerData = GameManager.Instance?.CurrentPlayerData;
            var loadout = playerData?.GetLoadoutForCharacter(characterData.characterId);
            
            int totalHP = characterData.baseHealth;
            int totalStamina = characterData.baseStamina;
            int totalArmor = 0;
            int totalMagicResist = 0;
            
            // Equipped items'dan bonus ekle
            if (loadout != null)
            {
                var equippedItems = loadout.GetAllEquippedItems();
                foreach (var itemId in equippedItems)
                {
                    ItemData itemData = LoadItemData(itemId);
                    if (itemData != null)
                    {
                        totalHP += itemData.healthBonus;
                        totalStamina += itemData.staminaBonus;
                        totalArmor += itemData.armorBonus;
                        totalMagicResist += itemData.magicResistanceBonus;
                    }
                }
            }
            
            if (_hpText != null)
                _hpText.text = $"HP: {totalHP}";
            
            if (_staminaText != null)
                _staminaText.text = $"Stamina: {totalStamina}";
            
            if (_armorText != null)
                _armorText.text = $"Armor: {totalArmor}";
            
            if (_magicResistText != null)
                _magicResistText.text = $"Magic Resist: {totalMagicResist}";
        }
        
        private void LoadSkillList(CharacterData characterData)
        {
            // Mevcut skill listesini temizle
            foreach (var card in _skillCards)
            {
                if (card != null)
                    Destroy(card);
            }
            _skillCards.Clear();
            
            // Seçili kategoriye göre skillleri filtrele
            List<SkillData> skills = GetSkillsForCategory(characterData, _selectedSkillCategory);
            
            Debug.Log($"[CharacterPanel] Loading {skills.Count} skills for category {_selectedSkillCategory}");
            
            foreach (var skill in skills)
            {
                CreateSkillCard(skill);
            }
            
            if (skills.Count == 0)
            {
                Debug.LogWarning($"[CharacterPanel] No skills found for category {_selectedSkillCategory} on character {characterData.characterName}");
            }
        }
        
        private List<SkillData> GetSkillsForCategory(CharacterData characterData, SkillType category)
        {
            List<SkillData> skills = new List<SkillData>();
            
            // Kategoriye göre tüm available skill'leri ekle
            switch (category)
            {
                case SkillType.Light:
                    if (characterData.allLightSkills != null)
                    {
                        foreach (var skill in characterData.allLightSkills)
                        {
                            if (skill != null)
                                skills.Add(skill);
                        }
                    }
                    break;
                
                case SkillType.Normal:
                    if (characterData.allNormalSkills != null)
                    {
                        foreach (var skill in characterData.allNormalSkills)
                        {
                            if (skill != null)
                                skills.Add(skill);
                        }
                    }
                    break;
                
                case SkillType.Heavy:
                    if (characterData.allHeavySkills != null)
                    {
                        foreach (var skill in characterData.allHeavySkills)
                        {
                            if (skill != null)
                                skills.Add(skill);
                        }
                    }
                    break;
                
                case SkillType.Ultimate:
                    if (characterData.allUltimateSkills != null)
                    {
                        foreach (var skill in characterData.allUltimateSkills)
                        {
                            if (skill != null)
                                skills.Add(skill);
                        }
                    }
                    // Ultimate'i de ekle
                    if (characterData.ultimate != null)
                        skills.Add(characterData.ultimate);
                    break;
                
                case SkillType.Passive:
                    if (characterData.allPassiveSkills != null)
                    {
                        foreach (var skill in characterData.allPassiveSkills)
                        {
                            if (skill != null)
                                skills.Add(skill);
                        }
                    }
                    // Passive ability'yi de ekle
                    if (characterData.passive != null)
                    {
                        // PassiveAbilityData'yı SkillData'ya dönüştür veya ayrı göster
                        // TODO: Passive ability gösterimi
                    }
                    break;
                
                case SkillType.Combo:
                    if (characterData.allComboSkills != null)
                    {
                        foreach (var skill in characterData.allComboSkills)
                        {
                            if (skill != null)
                                skills.Add(skill);
                        }
                    }
                    break;
            }
            
            return skills;
        }
        
        private void CreateSkillCard(SkillData skillData)
        {
            if (_skillCardPrefab == null || _skillListContent == null)
                return;
            
            GameObject card = Instantiate(_skillCardPrefab, _skillListContent);
            _skillCards.Add(card);
            
            Debug.Log($"[CharacterPanel] Created skill card for: {skillData.skillName}");
            
            // SkillCardUI script'ini kullan
            var cardUI = card.GetComponent<SkillCardUI>();
            if (cardUI != null)
            {
                Debug.Log($"[CharacterPanel] Using SkillCardUI component");
                bool isSelected = _selectedSkill == skillData;
                cardUI.Setup(skillData.icon, isSelected, () => SelectSkill(skillData));
            }
            else
            {
                Debug.LogWarning($"[CharacterPanel] SkillCardUI component not found, using fallback");
                // Fallback: Manuel setup
                var iconImage = card.transform.Find("Icon")?.GetComponent<Image>();
                if (iconImage != null && skillData.icon != null)
                    iconImage.sprite = skillData.icon;
                
                // Button click event
                var button = card.GetComponent<Button>();
                if (button != null)
                {
                    Debug.Log($"[CharacterPanel] Button found, adding listener");
                    button.onClick.AddListener(() => SelectSkill(skillData));
                }
                else
                {
                    Debug.LogError($"[CharacterPanel] Button component not found on skill card!");
                }
            }
        }
        
        private void SelectSkill(SkillData skillData)
        {
            Debug.Log($"[CharacterPanel] Skill selected: {skillData.skillName}");
            _selectedSkill = skillData;
            
            if (_skillDetailsPanel != null)
                _skillDetailsPanel.SetActive(true);
            
            if (_skillDetailIcon != null && skillData.icon != null)
                _skillDetailIcon.sprite = skillData.icon;
            
            if (_skillDetailNameText != null)
                _skillDetailNameText.text = skillData.skillName;
            
            if (_skillDetailDamageText != null)
                _skillDetailDamageText.text = $"Damage: {skillData.baseDamage} / Effect: {skillData.description}";
            
            if (_skillDetailEffectText != null)
                _skillDetailEffectText.text = skillData.description;
            
            // Skill listesini yenile (selected indicator için)
            if (!string.IsNullOrEmpty(_selectedCharacterId))
            {
                CharacterData characterData = LoadCharacterData(_selectedCharacterId);
                if (characterData != null)
                {
                    LoadSkillList(characterData);
                }
            }
        }
        
        private void OnSkillCategorySelected(SkillType category)
        {
            Debug.Log($"[CharacterPanel] Skill category selected: {category}");
            _selectedSkillCategory = category;
            
            // Skill listesini yeniden yükle
            if (!string.IsNullOrEmpty(_selectedCharacterId))
            {
                CharacterData characterData = LoadCharacterData(_selectedCharacterId);
                if (characterData != null)
                {
                    LoadSkillList(characterData);
                }
                else
                {
                    Debug.LogWarning($"[CharacterPanel] Character data not found: {_selectedCharacterId}");
                }
            }
            else
            {
                Debug.LogWarning("[CharacterPanel] No character selected!");
            }
        }
        
        private CharacterData LoadCharacterData(string characterId)
        {
            CharacterData[] allCharacters = Resources.LoadAll<CharacterData>("Characters");
            foreach (var character in allCharacters)
            {
                if (character.characterId == characterId)
                    return character;
            }
            return null;
        }
        
        private ItemData LoadItemData(string itemId)
        {
            ItemData[] allItems = Resources.LoadAll<ItemData>("Items");
            foreach (var item in allItems)
            {
                if (item.itemId == itemId)
                    return item;
            }
            return null;
        }
    }
}

