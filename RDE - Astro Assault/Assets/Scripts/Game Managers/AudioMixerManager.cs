using UnityEngine;
using UnityEngine.Audio;

namespace RDE
{
    /// <summary>
    /// 
    /// Manages audio settings for the game by interfacing with Unity's AudioMixer
    /// This class provides a centralized way to adjust various audio volumes like master, music and SFX
    /// 
    /// </summary>
    public class AudioMixerManager : MonoBehaviour
    {
        #region Variables

        public static AudioMixerManager instance;

        [Header("Audio Mixer")]
        [SerializeField] private AudioMixer mainMixer;

        [Header("Parameter Names")]
        private const string musicVolumeParam = "MusicVolume";
        private const string sfxVolumeParam = "SFXVolume";

        #endregion

        #region Base Methods

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            if (mainMixer == null)
            {
                Debug.LogError("AudioMixerManager: mainMixer is not assigned in the inspector");
            }
        }

        #endregion

        #region Public Get/Set Functions

        //Public getter/setter methods for Music volume controls
        public float MusicVolume
        {
            get => GetVolume(musicVolumeParam);
            set => SetVolume(musicVolumeParam, value);
        }

        //Public getter/setter methods for SFX volume controls
        public float SFXVolume
        {
            get => GetVolume(sfxVolumeParam);
            set => SetVolume(sfxVolumeParam, value);
        }

        #endregion

        #region Private Get/Set Functions

        //Private getter methods for volume controls
        private float GetVolume(string volumeParameter)
        {
            if (mainMixer.GetFloat(volumeParameter, out float value))
            {
                return value;
            }
            else
            {
                Debug.LogWarning($"AudioMixerManager: Failed to get {volumeParameter}");
                return 0f;
            }
        }

        //Private setter methods for volume controls
        private void SetVolume(string volumeParameter, float value)
        {
            mainMixer.SetFloat(volumeParameter, value);
        }

        #endregion
    }
}
