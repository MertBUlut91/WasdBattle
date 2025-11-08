using System.Collections.Generic;
using UnityEngine;

namespace WasdBattle.Core
{
    /// <summary>
    /// Ses yönetimi için singleton
    /// </summary>
    public class AudioManager : MonoBehaviour
    {
        private static AudioManager _instance;
        public static AudioManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject go = new GameObject("AudioManager");
                    _instance = go.AddComponent<AudioManager>();
                    DontDestroyOnLoad(go);
                }
                return _instance;
            }
        }
        
        [Header("Audio Sources")]
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioSource _sfxSource;
        
        [Header("Volume Settings")]
        [SerializeField] private float _masterVolume = 1f;
        [SerializeField] private float _musicVolume = 0.7f;
        [SerializeField] private float _sfxVolume = 1f;
        
        private Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();
        
        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            _instance = this;
            DontDestroyOnLoad(gameObject);
            
            InitializeAudioSources();
        }
        
        private void InitializeAudioSources()
        {
            if (_musicSource == null)
            {
                _musicSource = gameObject.AddComponent<AudioSource>();
                _musicSource.loop = true;
                _musicSource.playOnAwake = false;
            }
            
            if (_sfxSource == null)
            {
                _sfxSource = gameObject.AddComponent<AudioSource>();
                _sfxSource.loop = false;
                _sfxSource.playOnAwake = false;
            }
            
            UpdateVolumes();
        }
        
        /// <summary>
        /// Müzik çalar
        /// </summary>
        public void PlayMusic(AudioClip clip, bool loop = true)
        {
            if (_musicSource == null || clip == null)
                return;
            
            _musicSource.clip = clip;
            _musicSource.loop = loop;
            _musicSource.Play();
        }
        
        /// <summary>
        /// Müziği durdurur
        /// </summary>
        public void StopMusic()
        {
            if (_musicSource != null)
                _musicSource.Stop();
        }
        
        /// <summary>
        /// SFX çalar
        /// </summary>
        public void PlaySFX(AudioClip clip, float volumeScale = 1f)
        {
            if (_sfxSource == null || clip == null)
                return;
            
            _sfxSource.PlayOneShot(clip, volumeScale);
        }
        
        /// <summary>
        /// SFX çalar (isimle)
        /// </summary>
        public void PlaySFX(string clipName, float volumeScale = 1f)
        {
            if (_audioClips.TryGetValue(clipName, out AudioClip clip))
            {
                PlaySFX(clip, volumeScale);
            }
            else
            {
                Debug.LogWarning($"[AudioManager] Audio clip not found: {clipName}");
            }
        }
        
        /// <summary>
        /// Audio clip ekler
        /// </summary>
        public void RegisterAudioClip(string name, AudioClip clip)
        {
            if (!_audioClips.ContainsKey(name))
            {
                _audioClips.Add(name, clip);
            }
        }
        
        /// <summary>
        /// Master volume ayarlar
        /// </summary>
        public void SetMasterVolume(float volume)
        {
            _masterVolume = Mathf.Clamp01(volume);
            UpdateVolumes();
        }
        
        /// <summary>
        /// Music volume ayarlar
        /// </summary>
        public void SetMusicVolume(float volume)
        {
            _musicVolume = Mathf.Clamp01(volume);
            UpdateVolumes();
        }
        
        /// <summary>
        /// SFX volume ayarlar
        /// </summary>
        public void SetSFXVolume(float volume)
        {
            _sfxVolume = Mathf.Clamp01(volume);
            UpdateVolumes();
        }
        
        /// <summary>
        /// Volume'leri günceller
        /// </summary>
        private void UpdateVolumes()
        {
            if (_musicSource != null)
                _musicSource.volume = _masterVolume * _musicVolume;
            
            if (_sfxSource != null)
                _sfxSource.volume = _masterVolume * _sfxVolume;
        }
        
        public float MasterVolume => _masterVolume;
        public float MusicVolume => _musicVolume;
        public float SFXVolume => _sfxVolume;
    }
}

