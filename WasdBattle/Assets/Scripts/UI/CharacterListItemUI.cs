using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace WasdBattle.UI
{
    /// <summary>
    /// Karakter listesi item UI component
    /// CharacterListItem prefab için
    /// </summary>
    public class CharacterListItemUI : MonoBehaviour
    {
        [Header("UI Elements")]
        public Image iconImage;
        public TextMeshProUGUI nameText;
        public TextMeshProUGUI levelText;
        public GameObject selectedIndicator;
        
        private Button _button;
        
        private void Awake()
        {
            _button = GetComponent<Button>();
        }
        
        public void Setup(Sprite icon, string characterName, int level, bool isSelected, System.Action onClick)
        {
            if (iconImage != null)
                iconImage.sprite = icon;
            
            if (nameText != null)
                nameText.text = characterName;
            
            if (levelText != null)
                levelText.text = $"Lv. {level}";
            
            if (selectedIndicator != null)
                selectedIndicator.SetActive(isSelected);
            
            if (_button != null && onClick != null)
            {
                _button.onClick.RemoveAllListeners();
                _button.onClick.AddListener(() => onClick());
            }
        }
    }
}

