using UnityEngine;
using UnityEngine.EventSystems;
using WasdBattle.Core;
using WasdBattle.Data;

namespace WasdBattle.UI
{
    /// <summary>
    /// 3D karakter gösterimi ve rotasyon kontrolü
    /// RenderTexture kullanarak UI'da 3D karakteri gösterir
    /// </summary>
    public class CharacterDisplayController : MonoBehaviour
    {
        [Header("Camera & Render")]
        [SerializeField] private Camera _displayCamera;
        [SerializeField] private RenderTexture _renderTexture;
        [SerializeField] private Transform _characterRoot; // Karakterin instantiate edileceği yer
        
        [Header("Rotation Settings")]
        [SerializeField] private float _autoRotationSpeed = 20f; // Otomatik dönüş hızı
        [SerializeField] private float _manualRotationSpeed = 200f; // Mouse ile dönüş hızı
        [SerializeField] private bool _enableAutoRotation = true;
        
        [Header("Camera Positions")]
        [SerializeField] private Vector3 _mainMenuCameraPosition = new Vector3(0, 1.5f, 3f);
        [SerializeField] private Vector3 _characterPanelCameraPosition = new Vector3(-1.5f, 1.5f, 3f);
        [SerializeField] private Vector3 _inventoryPanelCameraPosition = new Vector3(1.5f, 1.5f, 3f);
        [SerializeField] private float _cameraTransitionSpeed = 5f;
        
        private GameObject _currentCharacterInstance;
        private string _currentCharacterId;
        private bool _isDragging = false;
        private Vector3 _lastMousePosition;
        private Vector3 _targetCameraPosition;
        
        private void Start()
        {
            // RenderTexture oluştur (eğer yoksa)
            if (_renderTexture == null)
            {
                _renderTexture = new RenderTexture(1024, 1024, 24);
                _renderTexture.antiAliasing = 4;
                _displayCamera.targetTexture = _renderTexture;
            }
            
            _targetCameraPosition = _mainMenuCameraPosition;
            _displayCamera.transform.localPosition = _mainMenuCameraPosition;
            
            // Seçili karakteri göster
            LoadSelectedCharacter();
        }
        
        private void Update()
        {
            // Otomatik rotasyon
            if (_enableAutoRotation && !_isDragging && _characterRoot != null)
            {
                _characterRoot.Rotate(Vector3.up, _autoRotationSpeed * Time.deltaTime);
            }
            
            // Kamera pozisyon geçişi
            if (_displayCamera != null)
            {
                _displayCamera.transform.localPosition = Vector3.Lerp(
                    _displayCamera.transform.localPosition,
                    _targetCameraPosition,
                    Time.deltaTime * _cameraTransitionSpeed
                );
            }
        }
        
        /// <summary>
        /// Mouse drag ile manuel rotasyon
        /// </summary>
        public void OnMouseDrag()
        {
            if (_characterRoot == null) return;
            
            if (!_isDragging)
            {
                _isDragging = true;
                _lastMousePosition = UnityEngine.Input.mousePosition;
            }
            
            Vector3 delta = UnityEngine.Input.mousePosition - _lastMousePosition;
            _characterRoot.Rotate(Vector3.up, -delta.x * _manualRotationSpeed * Time.deltaTime, Space.World);
            
            _lastMousePosition = UnityEngine.Input.mousePosition;
        }
        
        public void OnMouseUp()
        {
            _isDragging = false;
        }
        
        /// <summary>
        /// Seçili karakteri yükle (PlayerData'dan)
        /// </summary>
        public void LoadSelectedCharacter()
        {
            var playerData = GameManager.Instance?.CurrentPlayerData;
            if (playerData == null)
            {
                Debug.LogWarning("[CharacterDisplay] PlayerData is null!");
                return;
            }
            
            string selectedCharacterId = playerData.selectedCharacterId;
            
            // Eğer seçili karakter yoksa, ilk owned karakteri seç
            if (string.IsNullOrEmpty(selectedCharacterId) && playerData.ownedCharacters.Count > 0)
            {
                selectedCharacterId = playerData.ownedCharacters[0];
                playerData.selectedCharacterId = selectedCharacterId;
            }
            
            if (!string.IsNullOrEmpty(selectedCharacterId))
            {
                LoadCharacter(selectedCharacterId);
            }
        }
        
        /// <summary>
        /// Belirtilen karakteri yükle
        /// </summary>
        public void LoadCharacter(string characterId)
        {
            if (_currentCharacterId == characterId && _currentCharacterInstance != null)
            {
                // Zaten yüklü
                return;
            }
            
            // Eski karakteri temizle
            if (_currentCharacterInstance != null)
            {
                Destroy(_currentCharacterInstance);
                _currentCharacterInstance = null;
            }
            
            // Karakter verisini yükle
            CharacterData characterData = LoadCharacterData(characterId);
            if (characterData == null)
            {
                Debug.LogError($"[CharacterDisplay] Character data not found: {characterId}");
                return;
            }
            
            // Karakter prefab'ını instantiate et
            if (characterData.characterPrefab != null)
            {
                _currentCharacterInstance = Instantiate(characterData.characterPrefab, _characterRoot);
                _currentCharacterInstance.transform.localPosition = Vector3.zero;
                _currentCharacterInstance.transform.localRotation = Quaternion.identity;
                _currentCharacterInstance.transform.localScale = Vector3.one;
                
                _currentCharacterId = characterId;
                
                Debug.Log($"[CharacterDisplay] Loaded character: {characterData.characterName}");
            }
            else
            {
                Debug.LogWarning($"[CharacterDisplay] Character prefab is null: {characterData.characterName}");
            }
        }
        
        /// <summary>
        /// Karakter verisini Resources'tan yükle
        /// </summary>
        private CharacterData LoadCharacterData(string characterId)
        {
            // Resources/Characters klasöründen yükle
            CharacterData[] allCharacters = Resources.LoadAll<CharacterData>("Characters");
            
            foreach (var character in allCharacters)
            {
                if (character.characterId == characterId)
                {
                    return character;
                }
            }
            
            return null;
        }
        
        /// <summary>
        /// Kamera pozisyonunu değiştir (panel değişiminde)
        /// </summary>
        public void SetCameraPosition(CameraPosition position)
        {
            switch (position)
            {
                case CameraPosition.MainMenu:
                    _targetCameraPosition = _mainMenuCameraPosition;
                    break;
                case CameraPosition.CharacterPanel:
                    _targetCameraPosition = _characterPanelCameraPosition;
                    break;
                case CameraPosition.InventoryPanel:
                    _targetCameraPosition = _inventoryPanelCameraPosition;
                    break;
            }
        }
        
        /// <summary>
        /// Otomatik rotasyonu aç/kapat
        /// </summary>
        public void SetAutoRotation(bool enabled)
        {
            _enableAutoRotation = enabled;
        }
        
        /// <summary>
        /// RenderTexture'ı al (UI'da göstermek için)
        /// </summary>
        public RenderTexture GetRenderTexture()
        {
            return _renderTexture;
        }
        
        private void OnDestroy()
        {
            if (_currentCharacterInstance != null)
            {
                Destroy(_currentCharacterInstance);
            }
        }
    }
    
    public enum CameraPosition
    {
        MainMenu,
        CharacterPanel,
        InventoryPanel
    }
}

