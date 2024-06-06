using System.Collections;
using UnityEngine;

namespace RDE
{
    /// <summary>
    /// 
    /// Manages the enemies stats, building upon the functionalities of the CharacterStatManager
    /// This class includes methods for death and revival
    /// 
    /// </summary>
    public class EnemyStatManager : CharacterStatManager
    {
        #region Variables

        private EnemyManager enemyManager;

        #endregion

        #region Base Functions

        protected override void Awake()
        {
            base.Awake();
            enemyManager = GetComponent<EnemyManager>();
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
                duration /= enemyManager.healthRegen;
            }
            else
            {
                duration *= enemyManager.healthRegen;
            }
            float amountPerSecond = amount / duration;

            while (elapsed < duration)
            {
                enemyManager.currentHealth = Mathf.Clamp(enemyManager.currentHealth + amountPerSecond * Time.deltaTime, 0f, enemyManager.maxHealth);
                enemyManager.enemyUIWorldManager.SetHealthValue(enemyManager.currentHealth);

                if (enemyManager.currentHealth <= 0f)
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
                duration /= enemyManager.energyRegen;
            }
            else
            {
                duration *= enemyManager.energyRegen;
            }
            float amountPerSecond = amount / duration;

            while (elapsed < duration)
            {
                enemyManager.currentEnergy = Mathf.Clamp(enemyManager.currentEnergy + amountPerSecond * Time.deltaTime, 0f, enemyManager.maxEnergy);
                enemyManager.enemyUIWorldManager.SetEnergyValue(enemyManager.currentEnergy);

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
                duration *= enemyManager.heatRegen;
            }
            else
            {
                duration /= enemyManager.heatRegen;
            }
            float amountPerSecond = amount / duration;

            while (elapsed < duration)
            {
                enemyManager.currentHeat = Mathf.Clamp(enemyManager.currentHeat + amountPerSecond * Time.deltaTime, 0f, enemyManager.maxHeat);
                enemyManager.enemyUIWorldManager.SetHeatValue(enemyManager.currentHeat);

                if (enemyManager.currentHeat >= enemyManager.maxHeat)
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
            enemyManager.enemyUIWorldManager.SetHealthValue(enemyManager.currentHealth);
        }

        //Override for instant energy
        public override void InstantEnergy(float amount)
        {
            base.InstantEnergy(amount);
            enemyManager.enemyUIWorldManager.SetEnergyValue(enemyManager.currentEnergy);
        }

        //Override for instant heat
        public override void InstantHeat(float amount)
        {
            base.InstantHeat(amount);
            enemyManager.enemyUIWorldManager.SetHeatValue(enemyManager.currentHeat);
        }

        #endregion

        #region Death Events

        public override void ProcessDeathEvent()
        {
            base.ProcessDeathEvent();
        }

        public override void ReviveCharacter()
        {
            base.ReviveCharacter();
        }

        #endregion
    }
}