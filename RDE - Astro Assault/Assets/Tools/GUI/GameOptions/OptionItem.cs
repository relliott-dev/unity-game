using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RDE
{
    [System.Serializable]
    public class OptionItem : MonoBehaviour
    {
        public OptionType type;
        [HideInInspector] public Slider slider;
        [HideInInspector] public Toggle toggle;
        [HideInInspector] public Dropdown dropdown;
        [HideInInspector] public InputField inputfield;
        [HideInInspector] public InputKeySet inputKeyButton;
        [HideInInspector] public TMP_Text textMeshPro;

        public string currentValue;
        public string defaultValue;
        public bool requiresRestart = false;

        public OptionCallback callBack;

        private void Awake()
        {
            slider = GetComponent<Slider>();
            if (slider != null)
                return;

            toggle = GetComponent<Toggle>();
            if (toggle != null)
                return;

            dropdown = GetComponent<Dropdown>();
            if (dropdown != null)
                return;

            inputfield = GetComponent<InputField>();
            if (inputfield != null)
                return;

            inputKeyButton = GetComponent<InputKeySet>();
            if (inputKeyButton != null)
                return;

            textMeshPro = GetComponent<TMP_Text>();
        }

        public void ApplyOption()
        {
            if (callBack != null)
            {
                if (callBack.GetPersistentEventCount() == 0)
                {
                    return;
                }
                if (callBack.GetPersistentMethodName(0) == "")
                {
                    return;
                }

                if (callBack.GetPersistentMethodName(0) != "")
                    callBack.Invoke(this);
            }
        }
    }
}