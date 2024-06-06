using UnityEngine;
using UnityEngine.SceneManagement;

namespace RDE
{
    /// <summary>
    /// 
    /// Manages the player's UI components in the game
    /// This script acts as a central hub for various UI elements related to the player HUD,
    /// including chat, map, interaction prompts, and more
    /// 
    /// @TODO:
    /// - Save the data on crash
    /// - Disable player input on crash
    /// 
    /// </summary>
    public class PlayerUIManager : MonoBehaviour
    {
        public static PlayerUIManager instance;

        [HideInInspector] public PlayerManager playerManager;

        public PlayerUIHUDManager playerUIHUDManager;
        [HideInInspector] public PlayerChatManager chatManager;

        [Header("Windows")]
        public GameObject chatUI;
        public GameObject minichatUI;
        public GameObject noInternetUI;
        public GameObject deathUI;

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

            playerUIHUDManager = GetComponentInChildren<PlayerUIHUDManager>();
            chatManager = GetComponentInChildren<PlayerChatManager>(true);

            if (playerUIHUDManager == null || chatManager == null)
            {
                Debug.LogError("PlayerUIManager: Missing one or more essential components");
            }

            if (chatUI == null || minichatUI == null || noInternetUI == null ||
                deathUI == null)
            {
                Debug.LogError("PlayerUIManager: Missing one or more UI objects");
            }
        }

        public void CrashedInternet()
        {
            noInternetUI.SetActive(true);
        }

        public void BackToMainMenu()
        {
            SceneManager.LoadScene(0);
        }
    }
}