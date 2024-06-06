using UnityEngine;

namespace RDE
{
    /// <summary>
    /// 
    /// Manages combat interactions for characters, including players, NPCs, and enemies
    /// This class handles weapon configurations, attack execution, critical hits, and manages combat-related transformations
    /// 
    /// @TODO:
    /// - Add status effects
    /// - Fix networking
    /// 
    /// </summary>
    public class CharacterCombatManager : MonoBehaviour
    {
        #region Variables

        private CharacterManager characterManager;

        [Header("Weapon Configuration")]
        [SerializeField] protected Weapon weaponConfig;

        [Header("Helper Variables")]
        private Transform bulletGroup;
        private Transform attackPoint;
        private Transform dropPoint;
        private float nextFireTime;

        #endregion

        #region Base Methods

        private void Awake()
        {
            characterManager = GetComponent<CharacterManager>();
        }

        private void Start()
        {
            bulletGroup = transform.Find("Bullets");
            attackPoint = transform.Find("Attack Point");
            dropPoint = transform.Find("Drop Point");
        }

        #endregion

        #region Attacking Methods

        //Determines if the character can attack
        protected bool CanAttack()
        {
            if (Time.time >= nextFireTime)
            {
                nextFireTime = Time.time + (weaponConfig.attackSpeed / characterManager.attackSpeed);
                return true;
            }
            return false;
        }

        //Executes the attack
        public virtual void PerformAttack()
        {
            if (weaponConfig == null || characterManager.isDead || !CanAttack() || characterManager.currentEnergy < weaponConfig.energyCost)
            {
                return;
            }

            characterManager.GetComponent<CharacterLocomotionManager>().ApplyRecoil(weaponConfig.recoilForce);

            Transform spawnPoint = weaponConfig.useDropPoint ? dropPoint : attackPoint;

            if (weaponConfig.weaponPrefab)
            {
                Instantiate(weaponConfig.weaponPrefab, spawnPoint.position, characterManager.transform.rotation, bulletGroup);
            }

            if (weaponConfig.attackSound)
            {
                characterManager.characterSoundFXManager.PlaySound(weaponConfig.attackSound, 1f, true);
            }

            characterManager.characterStatManager.InstantEnergy(-weaponConfig.energyCost);
            characterManager.characterStatManager.InstantHeat(weaponConfig.heatCost);
        }

        //Handles special abilities from power ups
        public void PerformSpecial()
        {

        }

        //Attempts a critical hit calculation
        public float TryCriticalHit()
        {
            if (Random.value < weaponConfig.criticalHitChance)
            {
                characterManager.characterSoundFXManager.PlaySound(characterManager.characterSoundFXManager.GetCriticalSFX(), 1f, true);
                return weaponConfig.criticalHitMultiplier;
            }
            else
            {
                return 1f;
            }
        }

        #endregion
    }
}