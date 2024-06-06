using UnityEngine;

namespace RDE
{
    /// <summary>
    /// 
    /// Manages all player inputs for movement, actions, camera control, and menu interactions within the game
    /// Facilitates the handling of direct player commands, including interaction, combat, and UI navigation
    /// 
    /// @TODO:
    /// - Add tab input for scoreboard
    /// - Adding support for additional input devices
    /// - Rebinding options
    /// 
    /// </summary>
    public class PlayerInputManager : MonoBehaviour
    {
        #region Variables

        public static PlayerInputManager instance;

        [HideInInspector] public PlayerManager playerManager;
        private PlayerControls playerControls;

        [Header("Camera Input")]
        private Vector2 zoomInput;

        [Header("Movement Input")]
        private Vector2 movementInput;
        private bool strafeInput = false;
        private bool boostInput = false;

        [Header("Attack Input")]
        private bool attackInput = false;
        private bool specialInput = false;

        [Header("Menu Input")]
        private bool escapeInput = false;
        private bool chatInput = false;

        [Header("Helper Variables")]
        [HideInInspector] public float verticalInput;
        [HideInInspector] public float horizontalInput;

        #endregion

        #region Base Functions

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

            instance.enabled = false;

            if (playerControls != null)
            {
                playerControls.Disable();
            }
        }

        private void OnEnable()
        {
            if (playerControls == null)
            {
                playerControls = new PlayerControls();

                playerControls.PlayerCamera.Zoom.performed += i => zoomInput = i.ReadValue<Vector2>();

                playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
                playerControls.PlayerMovement.Strafe.performed += _ => strafeInput = true;
                playerControls.PlayerMovement.Strafe.canceled += _ => strafeInput = false;
                playerControls.PlayerMovement.Boost.performed += i => boostInput = true;
                playerControls.PlayerMovement.Boost.canceled += i => boostInput = false;

                playerControls.PlayerCombat.LightAttack.performed += i => attackInput = true;
                playerControls.PlayerCombat.LightAttack.canceled += i => attackInput = false;
                playerControls.PlayerCombat.HeavyAttack.performed += i => specialInput = true;

                playerControls.UI.EscapeMenu.performed += i => escapeInput = true;
                playerControls.UI.ChatMenu.performed += i => chatInput = true;
            }

            playerControls.Enable();
        }

        private void OnApplicationFocus(bool focus)
        {
            if (enabled)
            {
                if (focus)
                {
                    playerControls.Enable();
                }
                else
                {
                    playerControls.Disable();
                }
            }
        }

        private void Update()
        {
            HandleAllInput();
        }

        //Processes all player input
        private void HandleAllInput()
        {
            HandleZoomInput();
            HandleMovementInput();
            HandleStrafingInput();
            HandleBoostInput();

            HandleAttack();
            HandleSpecial();

            HandleEscapeMenuInput();
            HandleChatInput();
        }

        #endregion

        #region Movement Functions

        //Processes the zoom input
        private void HandleZoomInput()
        {
            float scrollDelta = zoomInput.normalized.y;
            PlayerCameraManager.instance.HandleZoomInput(scrollDelta);
        }

        //Processes the movement input
        private void HandleMovementInput()
        {
            verticalInput = movementInput.y;
            horizontalInput = movementInput.x;
        }

        //Processes the strafe input
        private void HandleStrafingInput()
        {
            if (strafeInput)
            {
                playerManager.canRotate = false;
            }
            else
            {
                playerManager.canRotate = true;
            }
        }

        //Processes the boost input
        private void HandleBoostInput()
        {
            if (boostInput)
            {
                playerManager.isBoosting = true;
            }
            else
            {
                playerManager.isBoosting = false;
            }
        }

        #endregion

        #region Attack Functions

        //Handles the attack input
        private void HandleAttack()
        {
            if(attackInput)
            {
                playerManager.playerCombatManager.PerformAttack();
                playerManager.isShooting = true;
            }
            else
            {
                playerManager.isShooting = false;
            }
        }

        //Handles the special input
        private void HandleSpecial()
        {
            if(specialInput)
            {
                specialInput = false;
                playerManager.playerCombatManager.PerformSpecial();
            }
        }

        #endregion

        #region Menu

        //Processes the escape input
        private void HandleEscapeMenuInput()
        {
            if (escapeInput)
            {
                escapeInput = false;

                /*if(GameOptions.instance.optionsGroup.alpha == 1f)
                {
                    GameOptions.instance.HideOptionsWindow();
                }
                else
                {
                    EscapeManager.instance.ToggleEscapeMenu();
                }*/
            }
        }

        //Processes the chat input
        private void HandleChatInput()
        {
            if (chatInput)
            {/*
                chatInput = false;

                if(!InternetManager.instance.CheckConnection())
                {
                    return;
                }

                if(PlayerUIManager.instance.chatUI.activeInHierarchy)
                {
                    if(string.IsNullOrEmpty(PlayerUIManager.instance.chatManager.playerMessage.text))
                    {
                        PlayerUIManager.instance.chatUI.SetActive(false);
                        PlayerUIManager.instance.minichatUI.SetActive(true);
                        PlayerUIManager.instance.chatManager.playerMessage.text = "";
                    }
                    else
                    {
                        PlayerUIManager.instance.chatManager.SendMessage();
                        PlayerUIManager.instance.chatManager.playerMessage.text = "";
                        PlayerUIManager.instance.chatManager.playerMessage.ActivateInputField();
                    }
                }
                else
                {
                    PlayerUIManager.instance.chatUI.SetActive(true);
                    PlayerUIManager.instance.minichatUI.SetActive(false);
                    PlayerUIManager.instance.chatManager.playerMessage.text = "";
                    PlayerUIManager.instance.chatManager.playerMessage.ActivateInputField();
                }*/
            }
        }

        #endregion
    }
}