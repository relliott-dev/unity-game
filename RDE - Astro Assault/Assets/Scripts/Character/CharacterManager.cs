using UnityEngine;

namespace RDE
{
    /// <summary>
    /// 
    /// Manages various aspects of a character and acts as main controller
    /// This class coordinates with multiple manager components to handle specific functionalities
    /// These include animation, combat, effects, locomotion, networking, sound effects, and stats
    /// It also maintains state information such as movement, health, mana, stamina, resistances, and status effects
    /// 
    /// @TODO:
    /// - Remove server updates
    /// 
    /// </summary>
    public class CharacterManager : MonoBehaviour
    {
        #region Variables 

        [HideInInspector] public CharacterCombatManager characterCombatManager;
        [HideInInspector] public CharacterLocomotionManager characterLocomotionManager;
        [HideInInspector] public CharacterNetworkManager characterNetworkManager;
        [HideInInspector] public CharacterSoundFXManager characterSoundFXManager;
        [HideInInspector] public CharacterStatManager characterStatManager;

        [Header("Flags")]
        [HideInInspector] public bool canMove = true;
        [HideInInspector] public bool canRotate = true;
        [HideInInspector] public bool isBoosting = false;
        [HideInInspector] public bool isShooting = false;
        [HideInInspector] public bool isDead = false;

        [Header("Names")]
        public string characterName;

        [Header("Stats")]
        [HideInInspector] public float currentHealth;
        [HideInInspector] public float maxHealth;
        [HideInInspector] public float currentEnergy;
        [HideInInspector] public float maxEnergy;
        [HideInInspector] public float currentHeat;
        [HideInInspector] public float maxHeat;

        [Header("Stat Regens")]
        [HideInInspector] public float healthRegen;
        [HideInInspector] public float energyRegen;
        [HideInInspector] public float heatRegen;

        [Header("Speeds")]
        [HideInInspector] public float movementSpeed;
        [HideInInspector] public float attackSpeed;

        [Header("Status")]
        [HideInInspector] public bool isInterruptible = false;
        [HideInInspector] public bool isStunned = false;
        [HideInInspector] public bool isRooted = false;
        [HideInInspector] public bool isInvincible = false;

        #endregion

        #region Base Methods

        protected virtual void Awake()
        {
            characterCombatManager = GetComponent<CharacterCombatManager>();
            characterLocomotionManager = GetComponent<CharacterLocomotionManager>();
            characterNetworkManager = GetComponent<CharacterNetworkManager>();
            characterSoundFXManager = GetComponent<CharacterSoundFXManager>();
            characterStatManager = GetComponent<CharacterStatManager>();
        }

        protected virtual void Start()
        {
            currentHealth = maxHealth;
            currentEnergy = maxEnergy;
            currentHeat = 0f;
        }    

        protected virtual void Update()
        {
            if (characterNetworkManager != null)
            {
                if(characterNetworkManager.IsOwner)
                {
                    UpdateNetworkVariablesFromLocal();
                }
                else
                {
                    UpdateLocalVariablesFromNetwork();
                }
            }
        }

        #endregion

        #region Local/Server Updates

        //Updates network variables with the local state for the owner of this character
        private void UpdateNetworkVariablesFromLocal()
        {
            //Stats
            characterNetworkManager.currentHealth = currentHealth;
            characterNetworkManager.currentEnergy = currentEnergy;
            characterNetworkManager.currentHeat = currentHeat;

            //Status
            characterNetworkManager.isInterruptible = isInterruptible;
            characterNetworkManager.isStunned = isStunned;
            characterNetworkManager.isRooted = isRooted;
            characterNetworkManager.isInvincible = isInvincible;
        }

        //Updates local variables with the network state for non-owner clients
        private void UpdateLocalVariablesFromNetwork()
        {
            //Stats
            currentHealth = characterNetworkManager.currentHealth;
            currentEnergy = characterNetworkManager.currentEnergy;
            currentHeat = characterNetworkManager.currentHeat;

            //Status
            isInterruptible = characterNetworkManager.isInterruptible;
            isStunned = characterNetworkManager.isStunned;
            isRooted = characterNetworkManager.isRooted;
            isInvincible = characterNetworkManager.isInvincible;
        }

        #endregion
    }
}