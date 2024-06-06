using TMPro;
using UnityEngine;

namespace RDE
{
    /// <summary>
    /// 
    /// Manages all aspects of player functionality within the game
    /// This class orchestrates interactions between combat, locomotion, network synchronization, sound effects, and player stats
    /// 
    /// @TODO:
    /// - Fix Saving/Loading
    /// 
    /// </summary>
    public class PlayerManager : CharacterManager
    {
        #region Variables

        [HideInInspector] public PlayerCombatManager playerCombatManager;
        [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;
        [HideInInspector] public PlayerNetworkManager playerNetworkManager;
        [HideInInspector] public PlayerSoundFXManager playerSoundFXManager;
        [HideInInspector] public PlayerStatManager playerStatManager;
        [HideInInspector] public PlayerUIWorldManager playerUIWorldManager;

        #endregion

        #region Base Functions

        protected override void Awake()
        {
            base.Awake();

            playerCombatManager = GetComponent<PlayerCombatManager>();
            playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
            playerNetworkManager = GetComponent<PlayerNetworkManager>();
            playerSoundFXManager = GetComponent<PlayerSoundFXManager>();
            playerStatManager = GetComponent<PlayerStatManager>();
            playerUIWorldManager = GetComponent<PlayerUIWorldManager>();
        }

        protected override void Start()
        {
            base.Start();

            PlayerCameraManager.instance.playerManager = this;
            PlayerInputManager.instance.playerManager = this;
            PlayerInputManager.instance.enabled = true;
            PlayerUIManager.instance.playerManager = this;
            SaveGameManager.instance.playerManager = this;

            //LoadGame(ref SaveGameManager.instance.currentCharacterData);

            PlayerUIManager.instance.playerUIHUDManager.GetComponent<CanvasGroup>().alpha = 1f;
            PlayerUIManager.instance.playerUIHUDManager.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }

        #endregion

        #region Save/Load Functions

        //Saves the current player character data
        public void SaveGame(ref PlayerSaveData currentCharacterData)
        {
            currentCharacterData.playerName = characterName.ToString();
        }

        //Loads the current player character data
        public void LoadGame(ref PlayerSaveData currentCharacterData)
        {
            characterName = currentCharacterData.playerName;

            currentHealth = maxHealth;
            currentEnergy = maxEnergy;
            currentHeat = maxHeat;

            PlayerUIManager.instance.playerUIHUDManager.SetMaxHealthValue(maxHealth);
            PlayerUIManager.instance.playerUIHUDManager.SetMaxEnergyValue(maxEnergy);
            PlayerUIManager.instance.playerUIHUDManager.SetMaxHeatValue(maxHeat);
            playerUIWorldManager.SetName(characterName);
            playerUIWorldManager.SetMaxHealthValue(maxHealth);
            playerUIWorldManager.SetMaxEnergyValue(maxEnergy);
            playerUIWorldManager.SetMaxHeatValue(maxHeat);
        }

        #endregion
    }
}