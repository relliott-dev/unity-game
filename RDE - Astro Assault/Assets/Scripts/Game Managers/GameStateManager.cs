using UnityEngine;

namespace RDE
{
    /// <summary>
    /// 
    /// Manages the current state of the game
    /// Allows for setting and retrieving the game state, which can be used to control game behaviors
    /// 
    /// </summary>
    public class GameStateManager : MonoBehaviour
    {
        #region Variables

        public enum GameState
        {
            EscapeMenu,
            Gameplay,
            Loading,
            Chat,
            Menu
        }

        public static GameStateManager instance;

        private GameState gameState = GameState.Menu;

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
            DontDestroyOnLoad(gameObject);
        }

        #endregion

        #region Public Get/Set Methods

        //Get the Game State
        public GameState GetGameState()
        {
            return gameState;
        }

        //Set the Game State
        public void SetGameState(GameState newState)
        {
            gameState = newState;
        }

        #endregion
    }
}