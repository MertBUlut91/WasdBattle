using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;
using WasdBattle.Core;
using WasdBattle.Data;
using WasdBattle.Managers;

namespace WasdBattle.UI
{
    /// <summary>
    /// Salvage (çöp kutusu) drop zone component
    /// ItemCardUI'dan sürüklenen itemleri salvage eder
    /// </summary>
    public class SalvageDropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Visual Feedback")]
        [SerializeField] private Image _highlightImage;
        [SerializeField] private Color _normalColor = new Color(0.8f, 0.2f, 0.2f, 0.5f); // Kırmızı
        [SerializeField] private Color _validHighlightColor = new Color(1f, 0.3f, 0.3f, 0.8f); // Parlak kırmızı
        [SerializeField] private Color _invalidHighlightColor = new Color(0.5f, 0.5f, 0.5f, 0.5f); // Gri
        
        [Header("Icon Animation")]
        [SerializeField] private Transform _iconTransform;
        [SerializeField] private float _pulseScale = 1.2f;
        [SerializeField] private float _pulseDuration = 0.3f;
        
        [Header("Confirmation (Optional)")]
        [SerializeField] private bool _requireConfirmation = false;
        [SerializeField] private GameObject _confirmationPanel;
        [SerializeField] private TextMeshProUGUI _confirmationText;
        [SerializeField] private Button _confirmButton;
        [SerializeField] private Button _cancelButton;
        
        [Header("Bulk Salvage")]
        [SerializeField] private GameObject _sliderPanel;
        [SerializeField] private UnityEngine.UI.Slider _countSlider;
        [SerializeField] private TextMeshProUGUI _countText;
        [SerializeField] private TextMeshProUGUI _totalPreviewText;
        
        private Color _originalColor;
        private bool _isDragOver;
        private ItemData _pendingItem;
        private int _pendingSalvageCount = 1;
        private EquipmentUI _equipmentUI;
        
        private void Awake()
        {
            _equipmentUI = GetComponentInParent<EquipmentUI>();
            
            if (_highlightImage != null)
            {
                _originalColor = _highlightImage.color;
            }
            
            // Confirmation button setup
            if (_confirmButton != null)
                _confirmButton.onClick.AddListener(OnConfirmSalvage);
            
            if (_cancelButton != null)
                _cancelButton.onClick.AddListener(OnCancelSalvage);
            
            if (_confirmationPanel != null)
                _confirmationPanel.SetActive(false);
            
            // Slider setup
            if (_countSlider != null)
            {
                _countSlider.onValueChanged.AddListener(OnSliderValueChanged);
                _countSlider.wholeNumbers = true;
            }
        }
        
        public void OnDrop(PointerEventData eventData)
        {
            // Bu method IDropHandler interface'i için gerekli
            // Asıl işlem ItemCardUI.OnEndDrag içinde yapılıyor
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            // Drag sırasında çöp kutusu üzerine gelince highlight yap
            if (eventData.pointerDrag != null)
            {
                var itemCard = eventData.pointerDrag.GetComponent<ItemCardUI>();
                if (itemCard != null)
                {
                    var itemData = itemCard.GetItemData();
                    if (itemData != null)
                    {
                        bool canSalvage = SalvageManager.Instance.CanSalvageItem(itemData, 1);
                        ShowHighlight(canSalvage);
                    }
                }
            }
        }
        
        public void OnPointerExit(PointerEventData eventData)
        {
            // Çöp kutusundan çıkınca highlight'ı kaldır
            ResetHighlight();
        }
        
        /// <summary>
        /// ItemCardUI'dan çağrılır - Item drop edildiğinde
        /// </summary>
        public void OnItemDropped(ItemData itemData)
        {
            if (itemData == null)
            {
                Debug.LogWarning("[SalvageDropZone] Dropped item is null");
                return;
            }
            
            // Salvage edilebilir mi kontrol et
            if (!SalvageManager.Instance.CanSalvageItem(itemData, 1))
            {
                Debug.LogWarning($"[SalvageDropZone] {itemData.itemName} cannot be salvaged!");
                ShowInvalidFeedback();
                return;
            }
            
            Debug.Log($"[SalvageDropZone] Item {itemData.itemName} dropped for salvage");
            
            // Confirmation gerekiyorsa
            if (_requireConfirmation)
            {
                ShowConfirmation(itemData);
            }
            else
            {
                // Direkt salvage et
                PerformSalvage(itemData);
            }
            
            // Highlight'ı kaldır
            ResetHighlight();
        }
        
        private void ShowConfirmation(ItemData itemData)
        {
            _pendingItem = itemData;
            
            if (_confirmationPanel != null)
            {
                _confirmationPanel.SetActive(true);
                
                // Item count'u al
                var playerData = GameManager.Instance?.CurrentPlayerData;
                int availableCount = playerData != null ? playerData.GetItemCount(itemData.itemId) : 1;
                
                // Slider setup
                if (_countSlider != null && availableCount > 1)
                {
                    _countSlider.minValue = 1;
                    _countSlider.maxValue = availableCount;
                    _countSlider.value = 1;
                    _pendingSalvageCount = 1;
                    
                    if (_sliderPanel != null)
                        _sliderPanel.SetActive(true);
                }
                else
                {
                    _pendingSalvageCount = 1;
                    if (_sliderPanel != null)
                        _sliderPanel.SetActive(false);
                }
                
                UpdateConfirmationText();
            }
        }
        
        private void OnSliderValueChanged(float value)
        {
            _pendingSalvageCount = Mathf.RoundToInt(value);
            UpdateConfirmationText();
        }
        
        private void UpdateConfirmationText()
        {
            if (_pendingItem == null)
                return;
            
            // Count text
            if (_countText != null)
            {
                _countText.text = $"Amount: {_pendingSalvageCount}";
            }
            
            // Preview text
            string preview = SalvageManager.Instance.GetSalvagePreview(_pendingItem, _pendingSalvageCount);
            
            if (_confirmationText != null)
            {
                _confirmationText.text = $"Are you sure you want to salvage:\n\n<b>{_pendingItem.itemName} x{_pendingSalvageCount}</b>?";
            }
            
            if (_totalPreviewText != null)
            {
                _totalPreviewText.text = preview;
            }
        }
        
        private void OnConfirmSalvage()
        {
            if (_pendingItem != null)
            {
                PerformSalvage(_pendingItem, _pendingSalvageCount);
                _pendingItem = null;
                _pendingSalvageCount = 1;
            }
            
            if (_confirmationPanel != null)
                _confirmationPanel.SetActive(false);
        }
        
        private void OnCancelSalvage()
        {
            _pendingItem = null;
            _pendingSalvageCount = 1;
            
            if (_confirmationPanel != null)
                _confirmationPanel.SetActive(false);
        }
        
        private void PerformSalvage(ItemData itemData, int count = 1)
        {
            bool success = SalvageManager.Instance.SalvageItem(itemData, count);
            
            if (success)
            {
                Debug.Log($"[SalvageDropZone] Successfully salvaged {itemData.itemName} x{count}");
                
                // Visual feedback
                PlaySalvageAnimation();
                
                // EquipmentUI'ı güncelle (inventory listesini yenile)
                if (_equipmentUI != null)
                {
                    _equipmentUI.RefreshInventoryList();
                }
                
                // TODO: Sound effect
                // AudioManager.Instance?.PlaySFX("ItemSalvage");
                
                // TODO: Particle effect
                // PlaySalvageParticles();
            }
            else
            {
                Debug.LogError($"[SalvageDropZone] Failed to salvage {itemData.itemName} x{count}");
                ShowInvalidFeedback();
            }
        }
        
        private void ShowHighlight(bool isValid)
        {
            if (_highlightImage != null && !_isDragOver)
            {
                _highlightImage.color = isValid ? _validHighlightColor : _invalidHighlightColor;
                _isDragOver = true;
            }
            
            // Icon pulse animation
            if (_iconTransform != null && isValid)
            {
                _iconTransform.DOKill();
                _iconTransform.DOScale(Vector3.one * _pulseScale, _pulseDuration)
                    .SetEase(Ease.InOutSine)
                    .SetLoops(-1, LoopType.Yoyo);
            }
        }
        
        private void ResetHighlight()
        {
            if (_highlightImage != null && _isDragOver)
            {
                _highlightImage.color = _normalColor;
                _isDragOver = false;
            }
            
            // Stop icon animation
            if (_iconTransform != null)
            {
                _iconTransform.DOKill();
                _iconTransform.localScale = Vector3.one;
            }
        }
        
        private void ShowInvalidFeedback()
        {
            // Kısa bir "shake" animasyonu
            if (_iconTransform != null)
            {
                _iconTransform.DOKill();
                _iconTransform.DOShakePosition(0.3f, strength: 10f, vibrato: 10, randomness: 90f);
            }
        }
        
        private void PlaySalvageAnimation()
        {
            // Success animation - scale up and fade
            if (_iconTransform != null)
            {
                _iconTransform.DOKill();
                
                Vector3 originalScale = _iconTransform.localScale;
                Sequence sequence = DOTween.Sequence();
                sequence.Append(_iconTransform.DOScale(originalScale * 1.5f, 0.2f).SetEase(Ease.OutQuad));
                sequence.Append(_iconTransform.DOScale(originalScale, 0.2f).SetEase(Ease.InQuad));
            }
        }
    }
}

