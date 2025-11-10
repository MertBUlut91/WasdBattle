using UnityEngine;
using WasdBattle.Core;

namespace WasdBattle.UI
{
    /// <summary>
    /// NPC gösterimi ve render kontrolü
    /// İki NPC'yi yan yana gösterir: Craft NPC (sol) ve Shop NPC (sağ)
    /// </summary>
    public class NPCDisplayController : MonoBehaviour
    {
        [Header("Camera & Render")]
        [SerializeField] private Camera _displayCamera;
        [SerializeField] private RenderTexture _renderTexture;
        [SerializeField] private Transform _npcRoot; // NPC'lerin instantiate edileceği yer
        
        [Header("NPC Prefabs")]
        [SerializeField] private GameObject _craftNPCPrefab;
        [SerializeField] private GameObject _shopNPCPrefab;
        
        [Header("NPC Positions")]
        [SerializeField] private Vector3 _craftNPCPosition = new Vector3(-1.5f, 0, 0);
        [SerializeField] private Vector3 _shopNPCPosition = new Vector3(1.5f, 0, 0);
        
        [Header("Rotation Settings")]
        [SerializeField] private float _autoRotationSpeed = 20f;
        [SerializeField] private bool _enableAutoRotation = true;
        
        [Header("Highlight Settings")]
        [SerializeField] private Color _highlightColor = Color.yellow;
        [SerializeField] private Color _normalColor = Color.white;
        
        private GameObject _craftNPCInstance;
        private GameObject _shopNPCInstance;
        private NPCType _highlightedNPC = NPCType.None;
        
        private void Start()
        {
            // RenderTexture oluştur (eğer yoksa)
            if (_renderTexture == null)
            {
                _renderTexture = new RenderTexture(1024, 1024, 24);
                _renderTexture.antiAliasing = 4;
                _displayCamera.targetTexture = _renderTexture;
            }
            
            // NPC'leri yükle
            LoadNPCs();
        }
        
        private void Update()
        {
            // Otomatik rotasyon
            if (_enableAutoRotation && _npcRoot != null)
            {
                _npcRoot.Rotate(Vector3.up, _autoRotationSpeed * Time.deltaTime);
            }
        }
        
        /// <summary>
        /// NPC'leri yükle
        /// </summary>
        private void LoadNPCs()
        {
            // Craft NPC
            if (_craftNPCPrefab != null)
            {
                _craftNPCInstance = Instantiate(_craftNPCPrefab, _npcRoot);
                _craftNPCInstance.transform.localPosition = _craftNPCPosition;
                _craftNPCInstance.transform.localRotation = Quaternion.Euler(0, 30, 0); // Hafif sağa bak
                _craftNPCInstance.transform.localScale = Vector3.one;
                
                Debug.Log("[NPCDisplay] Loaded Craft NPC");
            }
            
            // Shop NPC
            if (_shopNPCPrefab != null)
            {
                _shopNPCInstance = Instantiate(_shopNPCPrefab, _npcRoot);
                _shopNPCInstance.transform.localPosition = _shopNPCPosition;
                _shopNPCInstance.transform.localRotation = Quaternion.Euler(0, -30, 0); // Hafif sola bak
                _shopNPCInstance.transform.localScale = Vector3.one;
                
                Debug.Log("[NPCDisplay] Loaded Shop NPC");
            }
        }
        
        /// <summary>
        /// NPC'yi highlight et
        /// </summary>
        public void HighlightNPC(NPCType npcType)
        {
            _highlightedNPC = npcType;
            
            // Craft NPC highlight
            if (_craftNPCInstance != null)
            {
                Renderer[] renderers = _craftNPCInstance.GetComponentsInChildren<Renderer>();
                Color color = (npcType == NPCType.Craft) ? _highlightColor : _normalColor;
                foreach (var renderer in renderers)
                {
                    renderer.material.color = color;
                }
            }
            
            // Shop NPC highlight
            if (_shopNPCInstance != null)
            {
                Renderer[] renderers = _shopNPCInstance.GetComponentsInChildren<Renderer>();
                Color color = (npcType == NPCType.Shop) ? _highlightColor : _normalColor;
                foreach (var renderer in renderers)
                {
                    renderer.material.color = color;
                }
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
            if (_craftNPCInstance != null)
                Destroy(_craftNPCInstance);
            
            if (_shopNPCInstance != null)
                Destroy(_shopNPCInstance);
        }
    }
    
    public enum NPCType
    {
        None,
        Craft,
        Shop
    }
}

