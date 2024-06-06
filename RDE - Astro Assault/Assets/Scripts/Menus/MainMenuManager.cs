using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

namespace RDE
{
    /// <summary>
    /// 
    /// Manages the main menu interface of the game
    /// This class handles the opening and closing of different menu screens and performs actions related to player management and game settings
    /// It serves as a central hub for user navigation within the main menu
    /// 
    /// </summary>
    public class MainMenuManager : MonoBehaviour
    {
        #region Variables

        public static class MenuOptions
        {
            public const string MainMenu = "Main Menu";
            public const string NewPlayer = "New Game";
            public const string LoadPlayer = "Load Game";
            public const string OnlineMenu = "Online Menu";
            public const string LevelMenu = "Level Menu";
            public const string Options = "Options";
            public const string PatchNotes = "Patch Notes";
            public const string Credits = "Credits";
            public const string Quit = "Quit";
        }

        [Serializable]
        public struct MenuButtonPair
        {
            public GameObject selectMenu;
            public Button selectButton;
            public TMP_InputField selectInputField;
        }

        public static MainMenuManager instance;

        [SerializeField] private List<MenuButtonPair> menuButtonPairs;

        [Header("UI Objects")]
        [SerializeField] private GameObject startMenu;
        [SerializeField] private TextMeshProUGUI playerWarning;
        public TMP_InputField playerName;

        [Header("Helper Variables")]
        [HideInInspector] public string playerClass;
        [HideInInspector] public int sceneIndex;

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
            CheckForNullComponent(startMenu, "Start Menu");
            CheckForNullComponent(playerWarning, "Player Warning Text");
            CheckForNullComponent(playerName, "Player Name Input Field");

            playerName.characterLimit = 12;
            playerName.contentType = TMP_InputField.ContentType.Alphanumeric;
        }

        private void CheckForNullComponent(UnityEngine.Object component, string componentName)
        {
            if (component == null)
            {
                Debug.LogError($"MainMenuManager: {componentName} is not assigned in the Inspector");
            }
        }

        #endregion

        #region Handle Menus

        //Method called from button clicks to determine what menu to open
        public void ClickButton(string menuOption)
        {
            CloseAllMenus();

            SoundFXManager.instance.PlaySound(SoundFXManager.instance.GetOpenMenuSFX());

            switch (menuOption)
            {
                case MenuOptions.MainMenu:
                    OpenMenu(0);
                    SaveGameManager.instance.LoadAllPlayerSlots();
                    break;
                case MenuOptions.NewPlayer:
                    OpenMenu(1);
                    break;
                case MenuOptions.LoadPlayer:
                    OpenMenu(2);
                    break;
                case MenuOptions.OnlineMenu:
                    OpenMenu(3);
                    break;
                case MenuOptions.LevelMenu:
                    OpenMenu(4);
                    break;
                case MenuOptions.Options:
                    GameOptions.instance.ToggleOptionsWindow();
                    break;
                case MenuOptions.PatchNotes:
                    OpenMenu(5);
                    break;
                case MenuOptions.Credits:
                    OpenMenu(6);
                    break;
                case MenuOptions.Quit:
                    Application.Quit();
                    break;
            }
        }

        //Closes all menus and opens desired menu and also selects correct UI object
        private void OpenMenu(int index)
        {
            CloseAllMenus();

            if (menuButtonPairs[index].selectMenu != null)
            {
                menuButtonPairs[index].selectMenu.SetActive(true);
                Button selectButton = menuButtonPairs[index].selectButton;
                TMP_InputField selectInputField = menuButtonPairs[index].selectInputField;

                if (selectButton != null)
                {
                    selectButton.Select();
                }
                else if (selectInputField != null)
                {
                    selectInputField.Select();
                }
            }
        }

        #endregion

        #region Helper Methods

        //Sets warning text for creating a new player
        public void SetNewPlayerWarning(string warningText)
        {
            playerWarning.text = warningText;
        }

        //Sets characters class
        public void SetPlayerClass(string className)
        {
            playerClass = className;
        }

        //Sets Scene Index
        public void SetSceneIndex(int index)
        {
            sceneIndex = index;
        }

        //Closes all menus, resets states and opens main menu
        private void CloseAllMenus()
        {
            SetNewPlayerWarning("");
            playerName.text = "";

            foreach (MenuButtonPair pair in menuButtonPairs)
            {
                if (pair.selectMenu != null)
                {
                    pair.selectMenu.SetActive(false);
                }
            }

            startMenu.SetActive(false);
            GameOptions.instance.HideOptionsWindow();
        }

        #endregion
    }
}