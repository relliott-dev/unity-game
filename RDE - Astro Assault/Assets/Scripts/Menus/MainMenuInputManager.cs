using UnityEngine;

namespace RDE
{
    /// <summary>
    /// 
    /// Manages input interactions within the main menu
    /// Handles button selections and navigations based on player input controls
    /// 
    /// </summary>
    public class MainMenuInputManager : MonoBehaviour
    {
        #region Variables

        private PlayerControls playerControls;

        [Header("Input")]
        private bool cancelInput = false;

        #endregion

        #region Base Methods

        private void OnEnable()
        {
            if (playerControls == null)
            {
                playerControls = new PlayerControls();
                playerControls.UI.Cancel.performed += i => cancelInput = true;
            }

            playerControls.Enable();
        }

        private void OnDisable()
        {
            playerControls.Disable();
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
            HandleAllMenuInput();
        }

        #endregion

        #region Input Methods

        //Handles all menu inputs
        private void HandleAllMenuInput()
        {
            HandleCancelInput();
        }

        //Processes the cancel input
        private void HandleCancelInput()
        {
            if (cancelInput)
            {
                cancelInput = false;

                SoundFXManager.instance.PlaySound(SoundFXManager.instance.GetCloseMenuSFX());

                MainMenuManager.instance.ClickButton("Main Menu");
            }
        }

        #endregion
    }
}