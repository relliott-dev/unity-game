using UnityEngine;

namespace RDE
{
    /// <summary>
    /// 
    /// Manages character sound effects in the game
    /// This class handles the playing of sound effects for characters with options for volume adjustment and pitch randomization
    /// 
    /// </summary>
    public class CharacterSoundFXManager : MonoBehaviour
    {
        #region Variables

        [Header("Audio Sources")]
        private AudioSource sfxSource;

        [Header("Character Sound FX")]
        [SerializeField] private AudioClip idleSFX;
        [SerializeField] private AudioClip boostSFX;
        [SerializeField] private AudioClip impactSFX;
        [SerializeField] private AudioClip damageSFX;
        [SerializeField] private AudioClip criticalSFX;
        [SerializeField] private AudioClip deathSFX;
        [SerializeField] private AudioClip healSFX;
        [SerializeField] private AudioClip reviveSFX;

        #endregion

        #region Base Methods

        protected virtual void Awake()
        {
            sfxSource = GetComponent<AudioSource>();

            if (sfxSource == null)
            {
                Debug.LogWarning("CharacterSoundFXManager: Missing AudioSource components");
            }
        }

        #endregion

        #region Getter Methods

        public AudioClip GetIdleSFX() => idleSFX;
        public AudioClip GetBoostSFX() => boostSFX;
        public AudioClip GetImpactSFX() => impactSFX;
        public AudioClip GetDamageSFX() => damageSFX;
        public AudioClip GetCriticalSFX() => criticalSFX;
        public AudioClip GetDeathSFX() => deathSFX;
        public AudioClip GetHealSFX() => deathSFX;
        public AudioClip GetReviveSFX() => reviveSFX;

        #endregion

        #region Public Methods

        //Plays a sound clip with volume and pitch randomization parameters
        public void PlaySound(AudioClip soundFX,  float volume = 1f, bool randomizePitch = false, float pitchRandom = 0.1f)
        {
            if (soundFX == null)
            {
                Debug.LogWarning("CharacterSoundFXManager: AudioClip is null");
                return;
            }

            float originalPitch = sfxSource.pitch;
            sfxSource.pitch = randomizePitch ? originalPitch + Random.Range(-pitchRandom, pitchRandom) : originalPitch;
            sfxSource.PlayOneShot(soundFX, volume);
            sfxSource.pitch = originalPitch;
        }

        #endregion
    }
}