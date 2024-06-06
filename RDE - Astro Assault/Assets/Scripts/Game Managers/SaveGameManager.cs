using Crosstales.BWF;
using Crosstales.BWF.Model.Enum;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RDE
{
    /// <summary>
    /// 
    /// Manages save game functionality
    /// This class handles creation, loading, saving, and deletion of character data, as well as management of UI elements related to save slots
    /// 
    /// @TODO:
    /// - Optimize code for loading/populating slots
    /// - Stop SavePeriodically when exiting to main menu
    /// 
    /// </summary>
    public class SaveGameManager : MonoBehaviour
    {
        #region Variables

        public static SaveGameManager instance;

        [HideInInspector] public PlayerManager playerManager;
        private SaveFileDataWriter saveFileDataWriter;

        [SerializeField] private List<PlayerCharacter> playerCharacters;
        [HideInInspector] public PlayerSaveData currentPlayerData;
        [HideInInspector] public List<PlayerSaveData> playerData;

        [Header("Player Prefab")]
        [SerializeField] private GameObject playerPrefab;

        [Header("UI Objects")]
        [SerializeField] private GameObject saveSlotsParent;
        [SerializeField] private GameObject playerSaveSlotPrefab;

        #endregion

        #region Base Methods

        private void Awake()
        {
            if(instance == null)
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

            ValidatePlayerCharacters();

            saveFileDataWriter = new SaveFileDataWriter("Saves");
        }

        private void ValidatePlayerCharacters()
        {
            foreach (PlayerCharacter.ClassType classType in Enum.GetValues(typeof(PlayerCharacter.ClassType)))
            {
                if (!playerCharacters.Any(pc => pc.classType == classType))
                {
                    Debug.LogError("Missing PlayerCharacter for class type: " + classType);
                }
            }
        }

        #endregion

        #region Save/Load Methods

        //Creates a new player
        public void CreateNewPlayer(bool onlineMode)
        {
            bool isNotOk = BWFManager.Instance.Contains(MainMenuManager.instance.playerName.text, ManagerMask.BadWord);

            if(MainMenuManager.instance.playerName.text.Length < 1)
            {
                MainMenuManager.instance.SetNewPlayerWarning("Player name not long enough");
            }
            else if(PlayerNameExists(MainMenuManager.instance.playerName.text))
            {
                MainMenuManager.instance.SetNewPlayerWarning("Player name already exists");
            }    
            else if (isNotOk)
            {
                MainMenuManager.instance.SetNewPlayerWarning("Please do not use bad language");
            }
            else
            {
                currentPlayerData = new PlayerSaveData();
                currentPlayerData.playerName = MainMenuManager.instance.playerName.text;
                currentPlayerData.playerClass = MainMenuManager.instance.playerClass;
                currentPlayerData.timeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                saveFileDataWriter.CreateNewFile(currentPlayerData.playerName, currentPlayerData);
                LoadAllPlayerSlots();

                if(onlineMode)
                {
                    MainMenuManager.instance.ClickButton("Online Menu");
                }
                else
                {
                    MainMenuManager.instance.ClickButton("Level Menu");
                }
            }
        }

        //Loads a player
        public void LoadPlayer(bool onlineMode)
        {
            if (currentPlayerData == null)
            {
                Debug.LogWarning("Load Error: Failed to load player data for save file");
                return;
            }

            SoundFXManager.instance.PlaySound(SoundFXManager.instance.GetOpenMenuSFX());

            StartCoroutine(LoadWorldSceneAsync(onlineMode));
        }

        //Saves a player
        public void SavePlayer()
        {
            if (playerManager == null)
            {
                Debug.LogWarning("Save Error: Player does not exist for character " + currentPlayerData.playerName);
                return;
            }

            if (!saveFileDataWriter.CheckFileExists(currentPlayerData.playerName))
            {
                Debug.LogWarning("Save Error: File does not exist for character " + currentPlayerData.playerName);
                return;
            }

            playerManager.SaveGame(ref currentPlayerData);
            currentPlayerData.timeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            int saveIndex = playerData.FindIndex(saveData => saveData.playerName == currentPlayerData.playerName);

            if (saveIndex != -1)
            {
                playerData[saveIndex] = currentPlayerData;
            }

            saveFileDataWriter.CreateNewFile(currentPlayerData.playerName, currentPlayerData);
        }

        //Deletes a player
        public void DeleteCharacter()
        {
            if (!saveFileDataWriter.CheckFileExists(currentPlayerData.playerName))
            {
                Debug.LogWarning("Delete Error: File does not exist for character " + currentPlayerData.playerName);
                return;
            }

            saveFileDataWriter.DeleteSaveFile(currentPlayerData.playerName);

            LoadAllPlayerSlots();

            MainMenuManager.instance.ClickButton("Main Menu");
        }

        #endregion

        #region Character Slots

        //Loads all character save slots from save files
        public void LoadAllPlayerSlots()
        {
            string[] saveFiles = saveFileDataWriter.GetAllSaveFiles();
            playerData.Clear();

            foreach (string saveFile in saveFiles)
            {
                PlayerSaveData characterSaveData = saveFileDataWriter.LoadSaveFile(Path.GetFileName(saveFile));

                if (characterSaveData != null)
                {
                    playerData.Add(characterSaveData);
                }
            }

            if (playerData.Count == 0)
            {
                currentPlayerData = null;
            }

            playerData.Sort((a, b) => DateTime.Compare(DateTime.Parse(b.timeStamp), DateTime.Parse(a.timeStamp)));
            PopulateCharacterSaveSlots();
        }

        //Populates UI with character save slots
        private void PopulateCharacterSaveSlots()
        {
            InternetManager.instance.onlineMode = false;

            foreach (Transform child in saveSlotsParent.transform)
            {
                Destroy(child.gameObject);
            }

            for (int i = 0; i < playerData.Count; i++)
            {
                GameObject saveSlotPrefab = Instantiate(playerSaveSlotPrefab, saveSlotsParent.transform);
                PlayerSaveSlot saveSlot = saveSlotPrefab.GetComponent<PlayerSaveSlot>();
                saveSlot.PopulateSlot(playerData[i]);

                if (i == 0)
                {
                    saveSlot.SelectSlot();
                    currentPlayerData = playerData[i];
                }
            }
        }

        #endregion

        #region Helper Methods

        //Manages the asynchronous loading of the world scene with a loading screen
        private IEnumerator LoadWorldSceneAsync(bool onlineMode)
        {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(MainMenuManager.instance.sceneIndex);
            loadOperation.allowSceneActivation = false;

            while (!loadOperation.isDone)
            {
                if (loadOperation.progress >= 0.9f)
                {
                    loadOperation.allowSceneActivation = true;
                }

                yield return null;
            }

            if(onlineMode)
            {

            }
            else
            {
                Instantiate(playerPrefab);
            }
            GameStateManager.instance.SetGameState(GameStateManager.GameState.Gameplay);
        }

        //Checks if a character with the specified name already exists in the save data
        private bool PlayerNameExists(string characterName)
        {
            return playerData.Exists(saveData => saveData.playerName == characterName);
        }

        #endregion
    }
}