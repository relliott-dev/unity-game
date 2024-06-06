using UnityEngine;

namespace RDE
{
    public class EnemyManager : CharacterManager
    {
        #region Variables

        public EnemyCharacter enemyCharacter;

        [HideInInspector] public EnemyCombatManager enemyCombatManager;
        [HideInInspector] public EnemyLocomotionManager enemyLocomotionManager;
        [HideInInspector] public EnemyNetworkManager enemyNetworkManager;
        [HideInInspector] public EnemySoundFXManager enemySoundFXManager;
        [HideInInspector] public EnemyStatManager enemyStatManager;
        [HideInInspector] public EnemyUIWorldManager enemyUIWorldManager;

        #endregion

        #region Base Functions

        protected override void Awake()
        {
            base.Awake();

            enemyCombatManager = GetComponent<EnemyCombatManager>();
            enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
            enemyNetworkManager = GetComponent<EnemyNetworkManager>();
            enemySoundFXManager = GetComponent<EnemySoundFXManager>();
            enemyStatManager = GetComponent<EnemyStatManager>();
            enemyUIWorldManager = GetComponent<EnemyUIWorldManager>();
        }

        protected override void Start()
        {
            base.Start();

            transform.Find("Model").GetComponent<SpriteRenderer>().sprite = enemyCharacter.characterIcon;

            maxHealth = enemyCharacter.maxHealth;
            currentHealth = maxHealth;
            healthRegen = enemyCharacter.healthRegen;
            maxEnergy = enemyCharacter.maxEnergy;
            currentEnergy = maxEnergy;
            energyRegen = enemyCharacter.energyRegen;
            maxHeat = enemyCharacter.maxHeat;
            heatRegen = enemyCharacter.heatRegen;

            enemyUIWorldManager.SetMaxHealthValue(maxHealth);
            enemyUIWorldManager.SetHealthValue(currentHealth);
            enemyUIWorldManager.SetMaxEnergyValue(maxEnergy);
            enemyUIWorldManager.SetEnergyValue(currentEnergy);
            enemyUIWorldManager.SetMaxHeatValue(maxHeat);

            movementSpeed = enemyCharacter.movementSpeed;
            attackSpeed = enemyCharacter.attackSpeed;
        }

        #endregion

    }
}