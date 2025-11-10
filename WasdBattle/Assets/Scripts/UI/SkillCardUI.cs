using UnityEngine;
using UnityEngine.UI;

namespace WasdBattle.UI
{
    /// <summary>
    /// Skill card UI component (sadece icon gösterir)
    /// SkillCard prefab için
    /// </summary>
    public class SkillCardUI : MonoBehaviour
    {
        [Header("UI Elements")]
        public Image iconImage;
        public GameObject selectedIndicator;
        
        private Button _button;
        
        private void Awake()
        {
            _button = GetComponent<Button>();
            if (_button == null)
            {
                Debug.LogError("[SkillCardUI] Button component not found on this GameObject!");
            }
            else
            {
                Debug.Log("[SkillCardUI] Button component found and cached");
            }
        }
        
        public void Setup(Sprite icon, bool isSelected, System.Action onClick)
        {
            Debug.Log($"[SkillCardUI] Setup called - Icon: {icon != null}, Selected: {isSelected}, OnClick: {onClick != null}");
            
            if (iconImage != null)
                iconImage.sprite = icon;
            else
                Debug.LogWarning("[SkillCardUI] iconImage is null!");
            
            if (selectedIndicator != null)
                selectedIndicator.SetActive(isSelected);
            else
                Debug.LogWarning("[SkillCardUI] selectedIndicator is null!");
            
            if (_button != null && onClick != null)
            {
                _button.onClick.RemoveAllListeners();
                _button.onClick.AddListener(() => {
                    Debug.Log("[SkillCardUI] Button clicked!");
                    onClick();
                });
                Debug.Log("[SkillCardUI] Click listener added successfully");
            }
            else
            {
                Debug.LogError($"[SkillCardUI] Cannot add listener - Button: {_button != null}, OnClick: {onClick != null}");
            }
        }
    }
}

