using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace RDE
{
    public class EscapeManager : MonoBehaviour
    {
        public static EscapeManager instance;

        private CanvasGroup escapeGroup;
        [SerializeField] private GameObject firstButton;

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
            escapeGroup = GetComponent<CanvasGroup>();
            escapeGroup.alpha = 0f;
        }

        public void ToggleEscapeMenu()
        {
            if (escapeGroup.alpha == 1f)
            {
                escapeGroup.alpha = 0f;
                escapeGroup.blocksRaycasts = false;
            }
            else
            {
                escapeGroup.alpha = 1f;
                escapeGroup.blocksRaycasts = true;
                EventSystem.current.SetSelectedGameObject(firstButton);
            }
        }

        public void Resume()
        {
            escapeGroup.alpha = 0f;
            escapeGroup.blocksRaycasts = false;
        }

        public void Profile()
        {

        }

        public void Social()
        {

        }

        public void Options()
        {
            escapeGroup.alpha = 0f;
            escapeGroup.blocksRaycasts = false;
            GameOptions.instance.ToggleOptionsWindow();
        }

        public void Exit()
        {
            ToggleEscapeMenu();
            GameStateManager.instance.SetGameState(GameStateManager.GameState.Menu);
            SceneManager.LoadScene(0);
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}