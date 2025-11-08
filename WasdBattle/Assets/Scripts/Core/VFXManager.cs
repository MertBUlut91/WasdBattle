using System.Collections.Generic;
using UnityEngine;

namespace WasdBattle.Core
{
    /// <summary>
    /// VFX yönetimi için singleton
    /// </summary>
    public class VFXManager : MonoBehaviour
    {
        private static VFXManager _instance;
        public static VFXManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject go = new GameObject("VFXManager");
                    _instance = go.AddComponent<VFXManager>();
                    DontDestroyOnLoad(go);
                }
                return _instance;
            }
        }
        
        [Header("VFX Prefabs")]
        [SerializeField] private GameObject _hitEffectPrefab;
        [SerializeField] private GameObject _comboSuccessPrefab;
        [SerializeField] private GameObject _comboFailPrefab;
        [SerializeField] private GameObject _levelUpPrefab;
        
        private Dictionary<string, GameObject> _vfxPrefabs = new Dictionary<string, GameObject>();
        private Queue<GameObject> _vfxPool = new Queue<GameObject>();
        
        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        
        /// <summary>
        /// VFX spawn eder
        /// </summary>
        public GameObject SpawnVFX(GameObject prefab, Vector3 position, Quaternion rotation = default)
        {
            if (prefab == null)
                return null;
            
            if (rotation == default)
                rotation = Quaternion.identity;
            
            GameObject vfx = Instantiate(prefab, position, rotation);
            
            // Otomatik yok etme
            ParticleSystem ps = vfx.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                Destroy(vfx, ps.main.duration + ps.main.startLifetime.constantMax);
            }
            else
            {
                Destroy(vfx, 3f); // Default 3 saniye
            }
            
            return vfx;
        }
        
        /// <summary>
        /// VFX spawn eder (isimle)
        /// </summary>
        public GameObject SpawnVFX(string vfxName, Vector3 position, Quaternion rotation = default)
        {
            if (_vfxPrefabs.TryGetValue(vfxName, out GameObject prefab))
            {
                return SpawnVFX(prefab, position, rotation);
            }
            
            Debug.LogWarning($"[VFXManager] VFX prefab not found: {vfxName}");
            return null;
        }
        
        /// <summary>
        /// Hit effect spawn eder
        /// </summary>
        public void PlayHitEffect(Vector3 position)
        {
            if (_hitEffectPrefab != null)
            {
                SpawnVFX(_hitEffectPrefab, position);
            }
        }
        
        /// <summary>
        /// Combo success effect
        /// </summary>
        public void PlayComboSuccessEffect(Vector3 position)
        {
            if (_comboSuccessPrefab != null)
            {
                SpawnVFX(_comboSuccessPrefab, position);
            }
        }
        
        /// <summary>
        /// Combo fail effect
        /// </summary>
        public void PlayComboFailEffect(Vector3 position)
        {
            if (_comboFailPrefab != null)
            {
                SpawnVFX(_comboFailPrefab, position);
            }
        }
        
        /// <summary>
        /// Level up effect
        /// </summary>
        public void PlayLevelUpEffect(Vector3 position)
        {
            if (_levelUpPrefab != null)
            {
                SpawnVFX(_levelUpPrefab, position);
            }
        }
        
        /// <summary>
        /// VFX prefab kaydeder
        /// </summary>
        public void RegisterVFX(string name, GameObject prefab)
        {
            if (!_vfxPrefabs.ContainsKey(name))
            {
                _vfxPrefabs.Add(name, prefab);
            }
        }
    }
}

