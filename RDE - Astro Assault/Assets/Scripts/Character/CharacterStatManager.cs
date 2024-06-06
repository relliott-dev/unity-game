using UnityEngine;
using System.Collections;

namespace RDE
{
    /// <summary>
    /// 
    /// Manages the automatic and manual regeneration of health, energy and heat for characters
    /// This includes coroutines for both types of regeneration, influenced by character regeneration stats
    /// 
    /// @TODO:
    /// - Animations for death/revive
    /// - Particles for death/revive
    /// - Visual feedback of character status ie low health, high heat, etc
    /// 
    /// </summary>
    public class CharacterStatManager : MonoBehaviour
    {
        #region Variables

        private CharacterManager characterManager;

        [Header("Settings")]
        [SerializeField] private float regenDelay = 2f;
        [SerializeField] private float regenRate = 5f;

        #endregion

        #region Base Methods

        protected virtual void Awake()
        {
            characterManager = GetComponent<CharacterManager>();
        }

        private void Start()
        {
            StartCoroutine(AutoRegenerateEnergy());
            StartCoroutine(AutoRegenerateHeat());
        }

        #endregion

        #region Auto Regenerate Methods

        //Coroutine for auto stamina regeneration
        private IEnumerator AutoRegenerateEnergy()
        {
            while (!characterManager.isDead)
            {
                yield return new WaitWhile(() => CanRegenerateEnergy());
                yield return new WaitForSeconds(regenDelay);
                while (CanRegenerateEnergy())
                {
                    characterManager.currentEnergy += regenRate * characterManager.energyRegen * Time.deltaTime;
                    characterManager.currentEnergy = Mathf.Clamp(characterManager.currentEnergy, 0f, characterManager.maxEnergy);
                    yield return null;
                }
            }
        }

        //Coroutine for auto mana regeneration
        private IEnumerator AutoRegenerateHeat()
        {
            while (!characterManager.isDead)
            {
                yield return new WaitWhile(() => CanRegenerateHeat());
                yield return new WaitForSeconds(regenDelay);
                while (CanRegenerateHeat())
                {
                    characterManager.currentHeat -= regenRate * characterManager.heatRegen * Time.deltaTime;
                    characterManager.currentHeat = Mathf.Clamp(characterManager.currentHeat, 0f, characterManager.maxHeat);
                    yield return null;
                }
            }
        }

        #endregion

        #region Public Regen Methods

        //Method for health regeneration
        public virtual void RegenerateHealth(float amount, float duration)
        {
            StartCoroutine(RegenerateHealthOverTime(amount, duration));
        }

        //Method for energy regeneration
        public virtual void RegenerateEnergy(float amount, float duration)
        {
            StartCoroutine(RegenerateEnergyOverTime(amount, duration));
        }

        //Method for heat degeneration
        public virtual void RegenerateHeat(float amount, float duration)
        {
            StartCoroutine(RegenerateHeatOverTime(amount, duration));
        }

        #endregion

        #region Private Regen Methods

        //Coroutine for health regeneration
        private IEnumerator RegenerateHealthOverTime(float amount, float duration)
        {
            float elapsed = 0f;
            if(amount > 0f)
            {
                duration /= characterManager.healthRegen;
            }
            else
            {
                duration *= characterManager.healthRegen;
            }
            float amountPerSecond = amount / duration;

            while (elapsed < duration)
            {
                characterManager.currentHealth = Mathf.Clamp(characterManager.currentHealth + amountPerSecond * Time.deltaTime, 0f, characterManager.maxHealth);

                if (characterManager.currentHealth <= 0f)
                {
                    ProcessDeathEvent();
                    yield break;
                }

                elapsed += Time.deltaTime;
                yield return null;
            }
        }

        //Coroutine for energy regeneration
        private IEnumerator RegenerateEnergyOverTime(float amount, float duration)
        {
            float elapsed = 0f;
            if (amount > 0f)
            {
                duration /= characterManager.energyRegen;
            }
            else
            {
                duration *= characterManager.energyRegen;
            }
            float amountPerSecond = amount / duration;

            while (elapsed < duration)
            {
                characterManager.currentEnergy = Mathf.Clamp(characterManager.currentEnergy + amountPerSecond * Time.deltaTime, 0f, characterManager.maxEnergy);

                elapsed += Time.deltaTime;
                yield return null;
            }
        }

        //Coroutine for heat degeneration
        private IEnumerator RegenerateHeatOverTime(float amount, float duration)
        {
            float elapsed = 0f;
            if (amount > 0f)
            {
                duration *= characterManager.heatRegen;
            }
            else
            {
                duration /= characterManager.heatRegen;
            }
            float amountPerSecond = amount / duration;

            while (elapsed < duration)
            {
                characterManager.currentHeat = Mathf.Clamp(characterManager.currentHeat + amountPerSecond * Time.deltaTime, 0f, characterManager.maxHeat);

                if(characterManager.currentHeat >= characterManager.maxHeat)
                {
                    ProcessDeathEvent();
                    yield break;
                }

                elapsed += Time.deltaTime;
                yield return null;
            }
        }

        #endregion

        #region Immediate Regen Methods

        //Method for instant health
        public virtual void InstantHealth(float amount)
        {
            characterManager.currentHealth = Mathf.Clamp(characterManager.currentHealth + amount, 0f, characterManager.maxHealth);

            if(amount < 0f)
            {
                characterManager.characterSoundFXManager.PlaySound(characterManager.characterSoundFXManager.GetDamageSFX(), 1f, true);
            }
            else
            {
                characterManager.characterSoundFXManager.PlaySound(characterManager.characterSoundFXManager.GetHealSFX(), 1f, true);
            }

            if (characterManager.currentHealth <= 0)
            {
                ProcessDeathEvent();
            }
        }

        //Method for instant energy
        public virtual void InstantEnergy(float amount)
        {
            characterManager.currentEnergy = Mathf.Clamp(characterManager.currentEnergy + amount, 0f, characterManager.maxEnergy);
        }

        //Method for instant heat
        public virtual void InstantHeat(float amount)
        {
            characterManager.currentHeat = Mathf.Clamp(characterManager.currentHeat + amount, 0f, characterManager.maxHeat);

            if (characterManager.currentHeat >= characterManager.maxHeat)
            {
                ProcessDeathEvent();
            }
        }

        #endregion

        #region Death Events

        //Handle death event
        public virtual void ProcessDeathEvent()
        {
            characterManager.currentHealth = 0;
            characterManager.isDead = true;
            characterManager.canMove = false;
            characterManager.canRotate = false;
            characterManager.isBoosting = false;
            characterManager.isInterruptible = false;
            characterManager.isStunned = false;
            characterManager.isRooted = false;
            characterManager.isInvincible = false;

            characterManager.characterSoundFXManager.PlaySound(characterManager.characterSoundFXManager.GetDeathSFX());
        }

        //Handle revive event
        public virtual void ReviveCharacter()
        {
            characterManager.currentHealth = characterManager.maxHealth;
            characterManager.currentEnergy = characterManager.maxEnergy;
            characterManager.currentHeat = 0f;
            characterManager.isDead = false;
            characterManager.canMove = true;
            characterManager.canRotate = true;
            characterManager.isBoosting = false;
            characterManager.isInterruptible = false;
            characterManager.isStunned = false;
            characterManager.isRooted = false;
            characterManager.isInvincible = false;

            characterManager.characterSoundFXManager.PlaySound(characterManager.characterSoundFXManager.GetReviveSFX());
        }

        #endregion

        #region Bool Functions

        //Check for if stamina can be regenerated
        private bool CanRegenerateEnergy()
        {
            return !characterManager.isDead &&
                !characterManager.isBoosting &&
                !characterManager.isShooting &&
                characterManager.currentEnergy < characterManager.maxEnergy;
        }

        //Check for if mana can be regenerated
        private bool CanRegenerateHeat()
        {
            return !characterManager.isDead &&
                !characterManager.isBoosting &&
                !characterManager.isShooting &&
                characterManager.currentHeat > 0f;
        }

        #endregion
    }
}