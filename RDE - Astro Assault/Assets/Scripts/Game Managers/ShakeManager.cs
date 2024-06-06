using UnityEngine;
using System.Collections.Generic;
using MilkShake;
using System.Collections;

namespace RDE
{
    /// <summary>
    /// 
    /// ShakeManager is responsible for managing camera shakes within the game
    /// It uses the MilkShake library to apply different shake presets to the camera
    /// This class allows for both one-shot and sustained camera shakes, adding a dynamic and immersive element to gameplay
    /// 
    /// @TODO:
    /// - Adjust shake intensity based on in-game context?
    /// 
    /// </summary>
    public class ShakeManager : MonoBehaviour
    {
        #region Variables

        public static ShakeManager instance;

        private Shaker shaker;

        [SerializeField] private List<ShakePreset> presets;

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
            shaker = GetComponentInChildren<Shaker>();
        }

        #endregion

        #region Shake Methods

        //Initiates a camera shake based on the specified preset
        public void ShakeCamera(string presetName, float duration = 0f)
        {
            ShakePreset preset = presets.Find(p => p.name == presetName);
            if (preset == null)
            {
                Debug.LogError($"No shake preset named {presetName} found");
                return;
            }

            if (preset.ShakeType == ShakeType.Sustained && duration == 0f)
            {
                Debug.LogWarning($"Shake preset {presetName} is a sustained shake but has zero duration");
                return;
            }

            if (preset.ShakeType == ShakeType.OneShot)
            {
                shaker.Shake(preset);
            }
            else
            {
                StartCoroutine(DoShake(preset, duration));
            }
        }

        //Coroutine to handle the duration of a sustained shake
        private IEnumerator DoShake(ShakePreset preset, float duration)
        {
            ShakeInstance shakeInstance = shaker.Shake(preset);
            yield return new WaitForSeconds(duration);
            shakeInstance.Stop(preset.FadeOut, true);
        }

        #endregion
    }
}