using UnityEngine;
using UnityEngine.EventSystems;
using WasdBattle.Data;

namespace WasdBattle.UI
{
    /// <summary>
    /// Equipment slot için drop zone component
    /// ItemCardUI'dan sürüklenen itemleri kabul eder
    /// </summary>
    public class EquipmentSlotDropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Settings")]
        [SerializeField] private EquipmentSlot _slotType;
        [SerializeField] private UnityEngine.UI.Image _highlightImage;
        [SerializeField] private Color _highlightColor = new Color(1f, 1f, 0f, 0.3f);
        
        private EquipmentUI _equipmentUI;
        private Color _originalColor;
        private bool _isDragOver;
        
        private void Awake()
        {
            _equipmentUI = GetComponentInParent<EquipmentUI>();
            
            if (_highlightImage != null)
            {
                _originalColor = _highlightImage.color;
            }
        }
        
        public void SetSlotType(EquipmentSlot slotType)
        {
            _slotType = slotType;
        }
        
        public void OnItemDropped(ItemData itemData)
        {
            if (itemData == null)
            {
                Debug.LogWarning("[EquipmentSlotDropZone] Dropped item is null");
                return;
            }
            
            // Slot type kontrolü
            if (!IsValidSlot(itemData))
            {
                Debug.LogWarning($"[EquipmentSlotDropZone] Item {itemData.itemName} cannot be equipped in {_slotType} slot");
                return;
            }
            
            Debug.Log($"[EquipmentSlotDropZone] Item {itemData.itemName} dropped on {_slotType} slot");
            
            // EquipmentUI'a equip isteği gönder
            if (_equipmentUI != null)
            {
                _equipmentUI.EquipItemFromDrag(itemData, _slotType);
            }
            
            // Highlight'ı kaldır
            ResetHighlight();
        }
        
        public void OnDrop(PointerEventData eventData)
        {
            // Bu method IDropHandler interface'i için gerekli
            // Asıl işlem ItemCardUI.OnEndDrag içinde yapılıyor
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            // Drag sırasında slot üzerine gelince highlight yap
            if (eventData.pointerDrag != null)
            {
                var itemCard = eventData.pointerDrag.GetComponent<ItemCardUI>();
                if (itemCard != null)
                {
                    var itemData = itemCard.GetItemData();
                    if (itemData != null && IsValidSlot(itemData))
                    {
                        ShowHighlight();
                    }
                }
            }
        }
        
        public void OnPointerExit(PointerEventData eventData)
        {
            // Slot'tan çıkınca highlight'ı kaldır
            ResetHighlight();
        }
        
        private bool IsValidSlot(ItemData itemData)
        {
            // Ring slotları için özel kontrol
            if (_slotType == EquipmentSlot.Ring1 || _slotType == EquipmentSlot.Ring2)
            {
                return itemData.slot == EquipmentSlot.Ring1 || itemData.slot == EquipmentSlot.Ring2;
            }
            
            // Diğer slotlar için direkt karşılaştırma
            return itemData.slot == _slotType;
        }
        
        private void ShowHighlight()
        {
            if (_highlightImage != null && !_isDragOver)
            {
                _highlightImage.color = _highlightColor;
                _isDragOver = true;
            }
        }
        
        private void ResetHighlight()
        {
            if (_highlightImage != null && _isDragOver)
            {
                _highlightImage.color = _originalColor;
                _isDragOver = false;
            }
        }
    }
}

