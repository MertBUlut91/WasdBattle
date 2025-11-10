using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using WasdBattle.Data;

namespace WasdBattle.UI
{
    /// <summary>
    /// Item card UI component
    /// ItemListCard prefab için
    /// Double-click ve drag-and-drop desteği ile
    /// </summary>
    public class ItemCardUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
    {
        [Header("UI Elements")]
        public Image iconImage;
        public Image rarityBorder;
        public TextMeshProUGUI nameText;
        public TextMeshProUGUI countText;  // "x2" gibi count gösterimi
        public GameObject equippedIndicator;
        
        [Header("Drag Settings")]
        [SerializeField] private Canvas _canvas;
        [SerializeField] private float _dragAlpha = 0.6f;
        
        private Button _button;
        private ItemData _itemData;
        private System.Action _onDoubleClick;
        private CanvasGroup _canvasGroup;
        private RectTransform _rectTransform;
        private Vector2 _originalPosition;
        private Transform _originalParent;
        
        // Double-click detection
        private float _lastClickTime;
        private const float DOUBLE_CLICK_TIME = 0.3f;
        
        private void Awake()
        {
            _button = GetComponent<Button>();
            _canvasGroup = GetComponent<CanvasGroup>();
            if (_canvasGroup == null)
                _canvasGroup = gameObject.AddComponent<CanvasGroup>();
            
            _rectTransform = GetComponent<RectTransform>();
            
            // Canvas'ı bul (drag için gerekli)
            if (_canvas == null)
                _canvas = GetComponentInParent<Canvas>();
        }
        
        public void Setup(Sprite icon, string itemName, Color rarityColor, bool isEquipped, int count, System.Action onDoubleClick)
        {
            if (iconImage != null)
                iconImage.sprite = icon;
            
            if (rarityBorder != null)
                rarityBorder.color = rarityColor;
            
            if (nameText != null)
                nameText.text = itemName;
            
            // Count gösterimi (1'den fazlaysa)
            if (countText != null)
            {
                if (count > 1)
                {
                    countText.text = $"x{count}";
                    countText.gameObject.SetActive(true);
                }
                else
                {
                    countText.gameObject.SetActive(false);
                }
            }
            
            if (equippedIndicator != null)
                equippedIndicator.SetActive(isEquipped);
            
            _onDoubleClick = onDoubleClick;
            
            // Button'u devre dışı bırak (çünkü artık IPointerClickHandler kullanıyoruz)
            if (_button != null)
            {
                _button.onClick.RemoveAllListeners();
                _button.interactable = false;
            }
        }
        
        public void SetItemData(ItemData itemData)
        {
            _itemData = itemData;
        }
        
        public ItemData GetItemData()
        {
            return _itemData;
        }
        
        // Double-click detection
        public void OnPointerClick(PointerEventData eventData)
        {
            float timeSinceLastClick = Time.time - _lastClickTime;
            
            if (timeSinceLastClick <= DOUBLE_CLICK_TIME)
            {
                // Double-click detected
                Debug.Log($"[ItemCardUI] Double-click detected on {_itemData?.itemName}");
                _onDoubleClick?.Invoke();
                _lastClickTime = 0f; // Reset
            }
            else
            {
                // Single click - sadece zamanı kaydet
                _lastClickTime = Time.time;
            }
        }
        
        // Drag-and-drop implementation
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (_itemData == null)
                return;
            
            Debug.Log($"[ItemCardUI] Begin drag: {_itemData.itemName}");
            
            // Orijinal pozisyon ve parent'ı kaydet
            _originalPosition = _rectTransform.anchoredPosition;
            _originalParent = transform.parent;
            
            // Canvas'ın en üstüne taşı (diğer UI elementlerinin üstünde görünsün)
            transform.SetParent(_canvas.transform, true);
            transform.SetAsLastSibling();
            
            // Yarı saydam yap
            _canvasGroup.alpha = _dragAlpha;
            _canvasGroup.blocksRaycasts = false;
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            if (_itemData == null)
                return;
            
            // Mouse pozisyonunu takip et
            _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
        }
        
        public void OnEndDrag(PointerEventData eventData)
        {
            if (_itemData == null)
                return;
            
            Debug.Log($"[ItemCardUI] End drag: {_itemData.itemName}");
            
            // Raycast ile drop target'ı bul
            var results = new System.Collections.Generic.List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);
            
            bool droppedOnTarget = false;
            foreach (var result in results)
            {
                // Equipment slot'a drop edildi mi?
                var equipmentDropZone = result.gameObject.GetComponent<EquipmentSlotDropZone>();
                if (equipmentDropZone != null)
                {
                    equipmentDropZone.OnItemDropped(_itemData);
                    droppedOnTarget = true;
                    break;
                }
                
                // Salvage (çöp kutusu) drop zone'a drop edildi mi?
                var salvageDropZone = result.gameObject.GetComponent<SalvageDropZone>();
                if (salvageDropZone != null)
                {
                    salvageDropZone.OnItemDropped(_itemData);
                    droppedOnTarget = true;
                    break;
                }
            }
            
            if (!droppedOnTarget)
            {
                Debug.Log("[ItemCardUI] Dropped outside valid target");
            }
            
            // Orijinal duruma döndür
            _canvasGroup.alpha = 1f;
            _canvasGroup.blocksRaycasts = true;
            transform.SetParent(_originalParent, true);
            _rectTransform.anchoredPosition = _originalPosition;
        }
    }
}

