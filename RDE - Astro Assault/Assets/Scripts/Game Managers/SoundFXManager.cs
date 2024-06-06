using System.Collections;
using UnityEngine;

namespace RDE
{
    /// <summary>
    /// 
    /// Manages sound effects and music within the game
    /// Provides functionality for playing sounds effects and music with optional parameters
    /// 
    /// @TODO:
    /// - Fix menu sounds overlapping
    /// 
    /// </summary>
    public class SoundFXManager : MonoBehaviour
    {
        #region Variables

        public static SoundFXManager instance;

        [Header("Audio Sources")]
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource sfxSource;

        [Header("Menu Music")]
        [SerializeField] private AudioClip menuMusic;

        [Header("Menu Sound FX")]
        [SerializeField] private AudioClip openMenuSFX;
        [SerializeField] private AudioClip hoverMenuSFX;
        [SerializeField] private AudioClip closeMenuSFX;

        #endregion

        #region Base Methods

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);

            ValidateAudioSource(musicSource, "Music Source");
            ValidateAudioSource(sfxSource, "SFX Source");

            PlayMusic(menuMusic);
        }

        private void ValidateAudioSource(AudioSource source, string sourceName)
        {
            if (source == null)
            {
                Debug.LogError($"SoundFXManager: {sourceName} is not assigned");
            }
        }

        #endregion

        #region Getter Methods

        public AudioClip GetOpenMenuSFX() => openMenuSFX;
        public AudioClip GetHoverMenuSFX() => hoverMenuSFX;
        public AudioClip GetCloseMenuSFX() => closeMenuSFX;

        #endregion

        #region Public Methods

        //Plays a sound clip with volume and pitch randomization parameters
        public void PlaySound(AudioClip soundFX, float volume = 1f, bool randomizePitch = false, float pitchRandom = 0.1f)
        {
            if (soundFX == null)
            {
                Debug.LogWarning("SoundFXManager: AudioClip is null");
                return;
            }

            float originalPitch = sfxSource.pitch;
            sfxSource.pitch = randomizePitch ? originalPitch + Random.Range(-pitchRandom, pitchRandom) : originalPitch;
            sfxSource.PlayOneShot(soundFX, volume);
            sfxSource.pitch = originalPitch;
        }

        //Plays a music clip from a specified source with volume, loop, and fade paremeters
        public void PlayMusic(AudioClip musicClip, float volume = 1f, bool loop = true, float fadeDuration = 0f)
        {
            StartCoroutine(FadeMusic(musicClip, volume, fadeDuration, loop));
        }

        //Fades music out
        public void StopMusic(float fadeDuration = 1f)
        {
            StartCoroutine(FadeOutMusic(fadeDuration));
        }

        #endregion

        #region Private Methods

        //Fades music out and back in
        private IEnumerator FadeMusic(AudioClip newClip, float targetVolume, float duration, bool loop)
        {
            if (musicSource.isPlaying)
            {
                yield return StartCoroutine(FadeOutMusic(duration / 2));
            }

            musicSource.clip = newClip;
            musicSource.loop = loop;
            musicSource.Play();
            yield return StartCoroutine(FadeInMusic(targetVolume, duration / 2));
        }

        //Fades music out
        private IEnumerator FadeOutMusic(float duration)
        {
            float startVolume = musicSource.volume;

            while (musicSource.volume > 0f)
            {
                musicSource.volume -= startVolume * Time.deltaTime / duration;
                yield return null;
            }

            musicSource.Stop();
        }

        //Fades music in
        private IEnumerator FadeInMusic(float targetVolume, float duration)
        {
            while (musicSource.volume < targetVolume)
            {
                musicSource.volume += targetVolume * Time.deltaTime / duration;
                yield return null;
            }

            musicSource.volume = targetVolume;
        }

        #endregion
    }
}