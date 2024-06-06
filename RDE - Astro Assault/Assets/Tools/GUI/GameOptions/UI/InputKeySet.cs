using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RDE
{
    [RequireComponent(typeof(Button))]
    public class InputKeySet : MonoBehaviour
    {
        //Settings
        public Button inputButton;
        public Text shownKey;
        public Text description;
        public bool isEnabled;
        [SerializeField] public List<KeyCode> unallowedKeys;
        [SerializeField] public List<KeyCode> sharedKeys;

        void Awake()
        {
            inputButton = gameObject.GetComponent<Button>();
            inputButton.onClick.AddListener(HandleInputKey);
        }

        void HandleInputKey()
        {
            GameOptions.instance.SetAwaitingInputKey(gameObject.name, gameObject);
        }

        public bool IsKeyAllowed(KeyCode key)
        {
            bool result = true;
            for (int x = 0; x < unallowedKeys.Count; ++x)
            {
                if (unallowedKeys[x] == key)
                {
                    result = false;
                    break;
                }
            }
            return result;
        }

        public bool IsKeyShared(KeyCode key)
        {
            bool result = false;
            for (int x = 0; x < sharedKeys.Count; ++x)
            {
                if (sharedKeys[x] == key)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        public void SetEnabled(bool isEnabled2)
        {
            isEnabled = isEnabled2;
            inputButton.enabled = isEnabled;
        }
    }
}