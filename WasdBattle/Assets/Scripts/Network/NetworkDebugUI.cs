using UnityEngine;
using TMPro;
using WasdBattle.Core;

namespace WasdBattle.Network
{
    /// <summary>
    /// Network debug bilgilerini gösteren UI
    /// </summary>
    public class NetworkDebugUI : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private TextMeshProUGUI _debugText;
        [SerializeField] private bool _showOnStart = true;
        
        [Header("Update Settings")]
        [SerializeField] private float _updateInterval = 0.5f;
        
        private float _lastUpdateTime;
        
        private void Start()
        {
            if (!_showOnStart)
            {
                gameObject.SetActive(false);
            }
        }
        
        private void Update()
        {
            if (Time.time - _lastUpdateTime < _updateInterval)
                return;
            
            _lastUpdateTime = Time.time;
            UpdateDebugInfo();
        }
        
        private void UpdateDebugInfo()
        {
            if (_debugText == null)
                return;
            
            string info = "=== NETWORK DEBUG ===\n";
            info += NetworkHelper.GetNetworkDebugInfo();
            info += "\n=== GAME STATE ===\n";
            info += $"State: {GameManager.Instance.CurrentState}\n";
            
            var playerData = GameManager.Instance.CurrentPlayerData;
            if (playerData != null)
            {
                info += $"Player: {playerData.username}\n";
                info += $"Level: {playerData.level} | ELO: {playerData.elo}\n";
            }
            
            _debugText.text = info;
        }
        
        /// <summary>
        /// Debug UI'ı göster/gizle
        /// </summary>
        public void Toggle()
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }
    }
}

