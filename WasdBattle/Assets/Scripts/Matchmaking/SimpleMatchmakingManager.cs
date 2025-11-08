using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using WasdBattle.Core;
using WasdBattle.Data;

namespace WasdBattle.Matchmaking
{
    /// <summary>
    /// Basit matchmaking sistemi - Cloud Save ile oyuncu havuzu
    /// ELO ve Level bazlı eşleşme
    /// </summary>
    public class SimpleMatchmakingManager : MonoBehaviour
    {
        private static SimpleMatchmakingManager _instance;
        public static SimpleMatchmakingManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject go = new GameObject("SimpleMatchmakingManager");
                    _instance = go.AddComponent<SimpleMatchmakingManager>();
                    DontDestroyOnLoad(go);
                }
                return _instance;
            }
        }
        
        [Header("Settings")]
        [SerializeField] private float _matchmakingTimeout = 60f;
        [SerializeField] private float _searchInterval = 2f;
        [SerializeField] private int _eloTolerance = 200;
        [SerializeField] private int _levelTolerance = 10;
        
        private bool _isSearching = false;
        private float _searchStartTime;
        private string _currentSearchId;
        
        // Events
        public event Action OnMatchmakingStarted;
        public event Action<MatchmakingResult> OnMatchFound;
        public event Action OnMatchmakingFailed;
        public event Action OnMatchmakingCancelled;
        
        public bool IsSearching => _isSearching;
        
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
        /// Matchmaking başlatır
        /// </summary>
        public void StartMatchmaking()
        {
            if (_isSearching)
            {
                Debug.LogWarning("[Matchmaking] Already searching!");
                return;
            }
            
            _isSearching = true;
            _searchStartTime = Time.time;
            _currentSearchId = Guid.NewGuid().ToString();
            
            OnMatchmakingStarted?.Invoke();
            
            Debug.Log("[Matchmaking] Starting matchmaking...");
            
            // Matchmaking coroutine başlat
            StartCoroutine(MatchmakingCoroutine());
        }
        
        /// <summary>
        /// Matchmaking döngüsü
        /// </summary>
        private IEnumerator MatchmakingCoroutine()
        {
            PlayerData myData = GameManager.Instance.CurrentPlayerData;
            
            while (_isSearching)
            {
                // Timeout kontrolü
                if (Time.time - _searchStartTime > _matchmakingTimeout)
                {
                    Debug.LogWarning("[Matchmaking] Timeout!");
                    _isSearching = false;
                    OnMatchmakingFailed?.Invoke();
                    yield break;
                }
                
                // Eşleşme ara
                Debug.Log($"[Matchmaking] Searching... (ELO: {myData.elo}, Level: {myData.level})");
                
                // Burada gerçek bir matchmaking havuzu olmalı
                // Şimdilik simüle ediyoruz - gerçek implementasyon için:
                // 1. Cloud Save'e "searching players" listesi
                // 2. Relay Service ile connection
                // 3. Veya kendi backend'iniz
                
                // Simülasyon: %20 şans ile eşleşme bulundu
                if (UnityEngine.Random.value < 0.2f)
                {
                    // Eşleşme bulundu!
                    var matchResult = CreateMockMatch(myData);
                    _isSearching = false;
                    OnMatchFound?.Invoke(matchResult);
                    yield break;
                }
                
                // Tekrar dene
                yield return new WaitForSeconds(_searchInterval);
            }
        }
        
        /// <summary>
        /// Mock eşleşme oluşturur (test için)
        /// </summary>
        private MatchmakingResult CreateMockMatch(PlayerData myData)
        {
            var result = new MatchmakingResult
            {
                MatchId = Guid.NewGuid().ToString(),
                Players = new List<MatchPlayer>
                {
                    new MatchPlayer
                    {
                        PlayerId = AuthenticationService.Instance.PlayerId,
                        Username = myData.username,
                        Elo = myData.elo,
                        Level = myData.level
                    },
                    new MatchPlayer
                    {
                        PlayerId = "opponent_" + Guid.NewGuid().ToString().Substring(0, 8),
                        Username = "Opponent",
                        Elo = myData.elo + UnityEngine.Random.Range(-_eloTolerance, _eloTolerance),
                        Level = myData.level + UnityEngine.Random.Range(-_levelTolerance, _levelTolerance)
                    }
                }
            };
            
            Debug.Log($"[Matchmaking] Match found! Opponent ELO: {result.Players[1].Elo}, Level: {result.Players[1].Level}");
            
            return result;
        }
        
        /// <summary>
        /// Matchmaking'i iptal eder
        /// </summary>
        public void CancelMatchmaking()
        {
            if (!_isSearching)
                return;
            
            _isSearching = false;
            StopAllCoroutines();
            
            OnMatchmakingCancelled?.Invoke();
            Debug.Log("[Matchmaking] Matchmaking cancelled");
        }
        
        /// <summary>
        /// ELO farkını hesaplar
        /// </summary>
        public float CalculateMatchScore(int elo1, int level1, int elo2, int level2)
        {
            float eloDiff = Mathf.Abs(elo1 - elo2);
            float levelDiff = Mathf.Abs(level1 - level2);
            
            return eloDiff + (levelDiff * 50f);
        }
    }
    
    /// <summary>
    /// Matchmaking sonucu
    /// </summary>
    [Serializable]
    public class MatchmakingResult
    {
        public string MatchId;
        public List<MatchPlayer> Players = new List<MatchPlayer>();
    }
    
    /// <summary>
    /// Eşleşen oyuncu bilgisi
    /// </summary>
    [Serializable]
    public class MatchPlayer
    {
        public string PlayerId;
        public string Username;
        public int Elo;
        public int Level;
    }
}

