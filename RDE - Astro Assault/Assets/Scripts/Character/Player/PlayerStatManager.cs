using System.Collections;
using UnityEngine;

namespace RDE
{
    /// <summary>
    /// 
    /// Manages the player's stats, building upon the functionalities of the CharacterStatManager
    /// This class includes methods for death and revival and manages stat alert events
    /// 
    /// </summary>
    public class PlayerStatManager : CharacterStatManager
    {
        #region Variables

        private PlayerManager playerManager;

        [Header("Alert Thresholds")]
        [SerializeField] private float lowHealthThreshold = 0.25f;
        [SerializeField] private float highHeatThreshold = 0.75f;

        [Header("Helper Variables")]
        private bool isLowHealthAlerted;
        private bool isHighHeatAlerted;

        #endregion

        #region Base Functions

        protected override void Awake()
        {
            base.Awake();
            playerManager = GetComponent<PlayerManager>();
        }

        #endregion

        #region Damage Over Time

        //Override for health regeneration
        public override void RegenerateHealth(float amount, float duration)
        {
            StartCoroutine(RegenerateHealthOverTime(amount, duration));
        }

        // Coroutine for health regeneration
        private IEnumerator RegenerateHealthOverTime(float amount, float duration)
        {
            float elapsed = 0f;
            if (amount > 0f)
            {
                duration /= playerManager.healthRegen;
            }
            else
            {
                duration *= playerManager.healthRegen;
                ShakeManager.instance.ShakeCamera("Damage Over Time", duration);
            }
            float amountPerSecond = amount / duration;

            while (elapsed < duration)
            {
                playerManager.currentHealth = Mathf.Clamp(playerManager.currentHealth + amountPerSecond * Time.deltaTime, 0f, playerManager.maxHealth);
                PlayerUIManager.instance.playerUIHUDManager.SetHealthValue(playerManager.currentHealth);
                playerManager.playerUIWorldManager.SetHealthValue(playerManager.currentHealth);

                HandleHealthAlert();

                if (playerManager.currentHealth <= 0f)
                {
                    ProcessDeathEvent();
                    yield break;
                }

                elapsed += Time.deltaTime;
                yield return null;
            }
        }

        //Override for energy regeneration
        public override void RegenerateEnergy(float amount, float duration)
        {
            StartCoroutine(RegenerateEnergyOverTime(amount, duration));
        }

        // Coroutine for energy regeneration
        private IEnumerator RegenerateEnergyOverTime(float amount, float duration)
        {
            float elapsed = 0f;
            if (amount > 0f)
            {
                duration /= playerManager.energyRegen;
            }
            else
            {
                duration *= playerManager.energyRegen;
            }
            float amountPerSecond = amount / duration;

            while (elapsed < duration)
            {
                playerManager.currentEnergy = Mathf.Clamp(playerManager.currentEnergy + amountPerSecond * Time.deltaTime, 0f, playerManager.maxEnergy);
                PlayerUIManager.instance.playerUIHUDManager.SetEnergyValue(playerManager.currentEnergy);
                playerManager.playerUIWorldManager.SetEnergyValue(playerManager.currentEnergy);

                elapsed += Time.deltaTime;
                yield return null;
            }
        }

        //Override for heat regeneration
        public override void RegenerateHeat(float amount, float duration)
        {
            StartCoroutine(RegenerateHeatOverTime(amount, duration));
        }

        //Coroutine for heat degeneration
        private IEnumerator RegenerateHeatOverTime(float amount, float duration)
        {
            float elapsed = 0f;
            if (amount > 0f)
            {
                duration *= playerManager.heatRegen;
            }
            else
            {
                duration /= playerManager.heatRegen;
            }
            float amountPerSecond = amount / duration;

            while (elapsed < duration)
            {
                playerManager.currentHeat = Mathf.Clamp(playerManager.currentHeat + amountPerSecond * Time.deltaTime, 0f, playerManager.maxHeat);
                PlayerUIManager.instance.playerUIHUDManager.SetHeatValue(playerManager.currentHeat);
                playerManager.playerUIWorldManager.SetHeatValue(playerManager.currentHeat);

                HandleHeatAlert();

                if (playerManager.currentHeat >= playerManager.maxHeat)
                {
                    ProcessDeathEvent();
                    yield break;
                }

                elapsed += Time.deltaTime;
                yield return null;
            }
        }

        #endregion

        #region Instant Damage

        //Override for instant health
        public override void InstantHealth(float amount)
        {
            base.InstantHealth(amount);
            PlayerUIManager.instance.playerUIHUDManager.SetHealthValue(playerManager.currentHealth);
            playerManager.playerUIWorldManager.SetHealthValue(playerManager.currentHealth);
            if (amount < 0f)
            {
                HandleHealthAlert();
                ShakeManager.instance.ShakeCamera("Damage");
            }
        }

        //Override for instant energy
        public override void InstantEnergy(float amount)
        {
            base.InstantEnergy(amount);
            PlayerUIManager.instance.playerUIHUDManager.SetEnergyValue(playerManager.currentEnergy);
            playerManager.playerUIWorldManager.SetEnergyValue(playerManager.currentEnergy);
        }

        //Override for instant heat
        public override void InstantHeat(float amount)
        {
            base.InstantHeat(amount);
            PlayerUIManager.instance.playerUIHUDManager.SetHeatValue(playerManager.currentHeat);
            playerManager.playerUIWorldManager.SetHeatValue(playerManager.currentHeat);
            HandleHeatAlert();
        }

        #endregion

        #region Alerts

        //Handles health alert
        private void HandleHealthAlert()
        {
            bool isLowHealth = playerManager.currentHealth <= lowHealthThreshold * playerManager.maxHealth;
            if (!isLowHealthAlerted && isLowHealth)
            {
                playerManager.playerSoundFXManager.PlaySound(playerManager.playerSoundFXManager.GetLowHealthSFX());
                isLowHealthAlerted = true;
            }
            else if (isLowHealthAlerted && !isLowHealth)
            {
                isLowHealthAlerted = false;
            }
        }

        //Handles heat alert
        private void HandleHeatAlert()
        {
            bool isHighHeat = playerManager.currentHeat >= highHeatThreshold * playerManager.maxHeat;
            if (!isHighHeatAlerted && isHighHeat)
            {
                playerManager.playerSoundFXManager.PlaySound(playerManager.playerSoundFXManager.GetHighHeatSFX());
                isHighHeatAlerted = true;
            }
            else if (isHighHeatAlerted && !isHighHeat)
            {
                isHighHeatAlerted = false;
            }
        }

        #endregion

        #region Death Events

        public override void ProcessDeathEvent()
        {
            base.ProcessDeathEvent();

            if (playerManager.playerNetworkManager != null && playerManager.playerNetworkManager.IsOwner)
            {
                //PlayerUIManager.instance.deathUI.SetActive(true);
            }
        }

        public override void ReviveCharacter()
        {
            base.ReviveCharacter();

        }

        #endregion
    }
}