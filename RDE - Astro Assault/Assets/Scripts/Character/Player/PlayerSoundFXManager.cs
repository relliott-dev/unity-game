using UnityEngine;

namespace RDE
{
    /// <summary>
    /// 
    /// Manages sound effects specifically for the player character
    /// 
    /// </summary>
    public class PlayerSoundFXManager : CharacterSoundFXManager
    {
        #region Variables

        [Header("Player Sound FX")]
        [SerializeField] private AudioClip lowHealthSFX;
        [SerializeField] private AudioClip highHeatSFX;

        #endregion

        #region Getter Functions

        public AudioClip GetLowHealthSFX() => lowHealthSFX;
        public AudioClip GetHighHeatSFX() => highHeatSFX;

        #endregion
    }
}