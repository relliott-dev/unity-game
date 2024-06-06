using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace RDE
{
    public partial class GameOptions : MonoBehaviour
    {
        #region Variables

        public static GameOptions instance;

        [Header("Controls/Objects")]
        [HideInInspector] public CanvasGroup optionsGroup;
        private ScrollRect scrollView;
        [SerializeField] private List<GameObject> formsWindow;
        [SerializeField] private GameObject firstButton;
        [SerializeField] private Toggle firstMenu;
        [SerializeField] private GameObject awaitingWindow;
        [SerializeField] private Text errorMsg;
        [SerializeField] private Text waitKeyMsg;

        [Header("Unallowed Input Keys")]
        [SerializeField] public List<KeyCode> unallowedKeys;

        [Header("Microphones")]
        [HideInInspector] public string chosenMicrophone;
        [SerializeField] private Dropdown microphoneList;

        [HideInInspector] public List<OptionItem> optionsList;
        [HideInInspector] public KeyCode[] keyList;
        private OptionItem awaitingInputOption;
        private string lastScene;

        private bool isStarted = false;
        private bool isEditor = false;
        private bool isAwaitingInput = false;

        #endregion

        #region UI Control

        void Awake()
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

        void Start()
        {
            DontDestroyOnLoad(gameObject);
            optionsGroup = GetComponentInChildren<CanvasGroup>();
            scrollView = GetComponentInChildren<ScrollRect>();
            StartGameOptions();
        }

        private void StartGameOptions()
        {
#if UNITY_EDITOR
            isEditor = true;
#else
            isEditor = false;
#endif

            keyList = (KeyCode[])Enum.GetValues(typeof(KeyCode));

            GetOptionsList();
            LoadMonitorsInfo();
            GetMicrophones();
            LoadOptions(true);
            ApplyOptions();
            HideOptionsWindow();

            isStarted = true;
            lastScene = SceneManager.GetActiveScene().name;
            SceneManager.sceneLoaded += OnLevelFinishedLoading;
        }

        public void ToggleOptionsWindow()
        {
            if (optionsGroup.alpha == 1f)
            {
                HideOptionsWindow();
            }
            else
            {
                ShowOptionsWindow();
            }
        }

        public void ShowOptionsWindow()
        {
            LoadOptions(false);
            ShowHideAwaitingKey(false);
            GetMicrophones();

            optionsGroup.alpha = 1f;
            optionsGroup.blocksRaycasts = true;
            ShowOptionsForm(0);
            firstMenu.group.SetAllTogglesOff();
            firstMenu.isOn = true;
            isAwaitingInput = false;
            EventSystem.current.SetSelectedGameObject(firstButton);
        }

        public void ShowOptionsForm(int index)
        {
            for (int i = 0; i < formsWindow.Count; i++)
            {
                formsWindow[i].SetActive(i == index);
            }

            scrollView.GetComponent<ScrollViewAdjuster>().UpdateScrollBars(formsWindow[index].transform);
        }

        public void HideOptionsWindow()
        {
            optionsGroup.alpha = 0f;
            optionsGroup.blocksRaycasts = false;

            if (GameStateManager.instance.GetGameState() == GameStateManager.GameState.Gameplay)
            {
                EscapeManager.instance.ToggleEscapeMenu();
            }

            isAwaitingInput = false;
        }

        public void CancelAndHide()
        {
            LoadOptions(false);
            ApplyOptions();
            HideOptionsWindow();
        }

        public void ApplyAndSaveOptions()
        {
            ApplyOptions();
            SaveOptions();
        }

        public void ApplySaveAndHideWinow()
        {
            ApplyOptions();
            SaveOptions();
            HideOptionsWindow();
        }

        public void SaveOptionsHideWindow()
        {
            SaveOptions();
            HideOptionsWindow();
        }

        public void ResetOptions()
        {
            SetToDefaults();
            ApplyOptions();
        }

        public void ResetCurrentFormOptions()
        {
            SetCurrentFormToDefaults();
            ApplyOptions();
        }

        void Update()
        {
            if (!isStarted)
                return;

            //@TODO
            if (isAwaitingInput) //Regular Key Input
            {
                for (int x = 0; x < keyList.Length; ++x)
                {
                    if (Input.GetKeyDown(keyList[x])) //Set Key & Invoke the Callback Function
                    {
                        if (CheckIfKeyAlreadyUsed(keyList[x], awaitingInputOption.gameObject.name))
                        {
                            ShowError("Key already used.");
                            break;
                        }
                        if (awaitingInputOption.inputKeyButton.IsKeyAllowed(keyList[x]) && CheckIfGlobalKeyAllowed(keyList[x]) && keyList[x] != KeyCode.Escape)
                        {
                            awaitingInputOption.currentValue = KeyCodeToString(keyList[x]);
                            awaitingInputOption.inputKeyButton.shownKey.text = KeyCodeToString(keyList[x]); //Show Key
                            awaitingInputOption.callBack.Invoke(awaitingInputOption); //Apply Key
                            ShowHideAwaitingKey(false);
                            isAwaitingInput = false;
                            break;
                        }
                    }
                }
            }
        }

        //@TODO
        public bool CheckIfGlobalKeyAllowed(KeyCode key)
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

        //@TODO
        public void SetAwaitingInputKey(string OptionName, GameObject from)
        {
            int index = GetOptionIndexByName(OptionName);
            if (index == -1)
                return;

            awaitingInputOption = optionsList[index];
            isAwaitingInput = true;

            ShowHideAwaitingKey(true);
            waitKeyMsg.text = "Waiting for key: " + awaitingInputOption.inputKeyButton.description.text;
        }

        //@TODO
        private void ShowHideAwaitingKey(bool show)
        {
            awaitingWindow.SetActive(show);
        }

        //@TODO
        public void ShowError(string errorMsg2)
        {
            errorMsg.gameObject.SetActive(true);
            errorMsg.text = errorMsg2;
            StartCoroutine(FadeError());
        }

        //@TODO
        private IEnumerator FadeError()
        {
            for (float x = 2f; x >= 0; x -= Time.deltaTime) //2 Second Fade
            {
                errorMsg.color = new Color(164, 0, 0, x); //Red Color
                yield return null;
            }
            errorMsg.gameObject.SetActive(false);
        }

        //@TODO
        private bool CheckIfKeyAlreadyUsed(KeyCode key, string currOptionName)
        {
            int x = GetOptionIndexByName(currOptionName);
            if (x >= 0)
            {
                //Check If Current Option Allows Sharing this key
                if (optionsList[x].inputKeyButton.IsKeyShared(key))
                {
                    return false;
                }
            }

            //Check if already used
            for (var i = 0; i < optionsList.Count; i++)
            {
                if (optionsList[i].inputKeyButton != null)
                {
                    if (optionsList[i].inputKeyButton.gameObject.name != currOptionName)
                    {
                        if (optionsList[i].inputKeyButton.shownKey.text == KeyCodeToString(key))
                        {
                            return true;
                        }

                    }
                }
            }
            return false;
        }

        #endregion

        #region Core Functions

        private void GetOptionsList()
        {
            optionsList.Clear();

            var foundOptions = FindObjectsOfType<OptionItem>();
            for (var i = 0; i < foundOptions.Length; i++)
            {
                optionsList.Add(foundOptions[i]);
            }
        }

        public void LoadOptions(bool isStart)
        {
            string file = GetSettingFilePath();

            if (!File.Exists(file))
            {
                LoadAsDefaults(isStart);
            }
            else
            {
                SimpleINI iniParser = new SimpleINI();
                iniParser.LoadIniFile(file, true);

                float percentChk = optionsList.Count * 0.9f;
                if (iniParser.GetVariableCount() < (int)percentChk || iniParser.GetVariableCount() > optionsList.Count)
                {
                    LoadAsDefaults(isStart);
                    return;
                }

                for (var i = 0; i < optionsList.Count; i++)
                {
                    //Slider
                    if (optionsList[i].slider != null)
                    {
                        if (optionsList[i].slider.gameObject != null)
                        {
                            string gobjName = optionsList[i].slider.gameObject.name;
                            string section = Enum.GetName(typeof(OptionType), optionsList[i].type);
                            string value = iniParser.GetStringValue(section, gobjName + "-Bar");
                            value = CleanUpStr(value);
                            float value2;
                            if (!float.TryParse(value, out value2))
                            {
                                float.TryParse(optionsList[i].defaultValue, out value2);
                            }
                            optionsList[i].slider.value = value2;
                            optionsList[i].currentValue = value2.ToString("0.0000");
                            continue;
                        }
                    }

                    //Toggle
                    if (optionsList[i].toggle != null)
                    {
                        if (optionsList[i].toggle.gameObject != null)
                        {
                            string gobjName = optionsList[i].toggle.gameObject.name;
                            string section = Enum.GetName(typeof(OptionType), optionsList[i].type);
                            string value = iniParser.GetStringValue(section, gobjName + "-Check");
                            value = CleanUpStr(value);
                            bool value2;
                            if (value == "") value = "false";
                            if (!bool.TryParse(value, out value2))
                            {
                                bool.TryParse(optionsList[i].defaultValue, out value2);
                            }
                            optionsList[i].toggle.isOn = value2;
                            optionsList[i].currentValue = value2.ToString();
                            continue;
                        }
                    }

                    //TextMeshPro
                    if (optionsList[i].textMeshPro != null)
                    {
                        if (optionsList[i].textMeshPro.gameObject != null)
                        {
                            string gobjName = optionsList[i].textMeshPro.gameObject.name;
                            string section = Enum.GetName(typeof(OptionType), optionsList[i].type);
                            string value = iniParser.GetStringValue(section, gobjName + "-TextMeshPro");
                            value = CleanUpStr(value);
                            if (value == "")
                            {
                                value = optionsList[i].defaultValue;
                            }
                            optionsList[i].textMeshPro.text = value;
                            optionsList[i].currentValue = value.ToString();
                            continue;
                        }
                    }

                    //InputField Text  
                    if (optionsList[i].inputfield != null)
                    {
                        if (optionsList[i].inputfield.gameObject != null)
                        {
                            string gobjName = optionsList[i].inputfield.gameObject.name;
                            string section = Enum.GetName(typeof(OptionType), optionsList[i].type);
                            string value = iniParser.GetStringValue(section, gobjName + "-InputText");
                            value = CleanUpStr(value);
                            if (value == "")
                            {
                                value = optionsList[i].defaultValue;
                            }
                            optionsList[i].inputfield.text = value;
                            optionsList[i].currentValue = value.ToString();
                            continue;
                        }
                    }

                    //Input Key Button
                    if (optionsList[i].inputKeyButton != null)
                    {
                        if (optionsList[i].inputKeyButton.gameObject != null)
                        {
                            string gobjName = optionsList[i].inputKeyButton.gameObject.name;
                            string section = Enum.GetName(typeof(OptionType), optionsList[i].type);
                            string value = iniParser.GetStringValue(section, gobjName + "-InputKey");
                            value = CleanUpStr(value);
                            if (value == "") //Invalid Data Load Default
                            {
                                value = optionsList[i].defaultValue;
                            }
                            optionsList[i].inputKeyButton.shownKey.text = value;
                            optionsList[i].currentValue = value.ToString();
                            continue;
                        }
                    }

                    //Dropdown
                    if (optionsList[i].dropdown != null)
                    {
                        if (optionsList[i].dropdown.gameObject != null)
                        {
                            string gobjName = optionsList[i].dropdown.gameObject.name;
                            string section = Enum.GetName(typeof(OptionType), optionsList[i].type);
                            string value = iniParser.GetStringValue(section, gobjName + "-Dropdown");
                            value = CleanUpStr(value);
                            if (value == "")
                            {
                                value = optionsList[i].defaultValue;
                            }
                            //Find Option Index
                            for (var x = 0; x < optionsList[i].dropdown.options.Count; x++)
                            {
                                if (optionsList[i].dropdown.options[x].text.ToLower() == value.ToLower())
                                {
                                    optionsList[i].dropdown.value = x;
                                    optionsList[i].currentValue = value;
                                    continue;
                                }
                            }
                            continue;
                        }
                    }
                }
            }
        }

        public void LoadAsDefaults(bool isStart)
        {
            SetToDefaults();
            ApplyOptions();
            SaveOptions();
        }

        public void SaveOptions()
        {
            string file = GetSettingFilePath();
            if (File.Exists(file))
                File.Delete(file);

            SimpleINI iniParser = new SimpleINI();
            iniParser.Initialize();
            iniParser.AddCategory("Video");
            iniParser.AddCategory("Audio");
            iniParser.AddCategory("Gameplay");
            iniParser.AddCategory("Controls");
            iniParser.AddCategory("Accessibility");

            for (var i = 0; i < optionsList.Count; i++)
            {
                //Slider
                if (optionsList[i].slider != null)
                {
                    if (optionsList[i].slider.gameObject != null)
                    {
                        string gobjName = optionsList[i].slider.gameObject.name;
                        string section = Enum.GetName(typeof(OptionType), optionsList[i].type);
                        string value = optionsList[i].slider.value.ToString("0.0000");

                        iniParser.AddSetStringValue(section, gobjName + "-Bar", value);
                        continue;
                    }
                }

                //Dropdown
                if (optionsList[i].dropdown != null)
                {
                    if (optionsList[i].dropdown.gameObject != null)
                    {
                        string gobjName = optionsList[i].dropdown.gameObject.name;
                        string section = Enum.GetName(typeof(OptionType), optionsList[i].type);
                        int x = optionsList[i].dropdown.value;

                        string value1 = "";
                        if (optionsList[i].dropdown.options.Count != 0)
                            value1 = optionsList[i].dropdown.options[x].text;

                        iniParser.AddSetStringValue(section, gobjName + "-Dropdown", value1);
                        continue;
                    }
                }

                //Toggle
                if (optionsList[i].toggle != null)
                {
                    if (optionsList[i].toggle.gameObject != null)
                    {
                        string gobjName = optionsList[i].toggle.gameObject.name;
                        string section = Enum.GetName(typeof(OptionType), optionsList[i].type);
                        string value = optionsList[i].toggle.isOn.ToString();
                        iniParser.AddSetStringValue(section, gobjName + "-Check", value);
                        continue;
                    }
                }

                //TextMeshPro
                if (optionsList[i].textMeshPro != null)
                {
                    if (optionsList[i].textMeshPro.gameObject != null)
                    {
                        string gobjName = optionsList[i].textMeshPro.gameObject.name;
                        string section = Enum.GetName(typeof(OptionType), optionsList[i].type);
                        string value = optionsList[i].textMeshPro.text;
                        iniParser.AddSetStringValue(section, gobjName + "-TextMeshPro", value);
                        continue;
                    }
                }

                //Input Text
                if (optionsList[i].inputfield != null)
                {
                    if (optionsList[i].inputfield.gameObject != null)
                    {
                        string gobjName = optionsList[i].inputfield.gameObject.name;
                        string section = Enum.GetName(typeof(OptionType), optionsList[i].type);
                        string value = optionsList[i].inputfield.text;
                        iniParser.AddSetStringValue(section, gobjName + "-InputText", value);
                        continue;
                    }
                }

                //Input Key Button
                if (optionsList[i].inputKeyButton != null)
                {
                    if (optionsList[i].inputKeyButton.gameObject != null)
                    {
                        string gobjName = optionsList[i].inputKeyButton.gameObject.name;
                        string section = Enum.GetName(typeof(OptionType), optionsList[i].type);
                        string value = optionsList[i].inputKeyButton.shownKey.text;
                        iniParser.AddSetStringValue(section, gobjName + "-InputKey", value);
                        continue;
                    }
                }
            }

            iniParser.WriteIniFile(file);
        }

        private void ApplyOptions()
        {
            bool requiresRestart = false;

            SetGFXMonitorSettings();

            for (var i = 0; i < optionsList.Count; i++)
            {
                optionsList[i].ApplyOption();

                if (optionsList[i].requiresRestart)
                    requiresRestart = true;
            }

            LoadMonitorsInfo();

            if (requiresRestart)
                ShowError("Some settings require you to restart the game.");
        }

        private void ApplyOptionsSceneChange()
        {
            for (var i = 0; i < optionsList.Count; i++)
            {
                optionsList[i].ApplyOption();
            }
        }

        private void SetToDefaults()
        {
            for (var i = 0; i < optionsList.Count; i++)
            {
                //Slider
                if (optionsList[i].slider != null)
                {
                    if (optionsList[i].slider.gameObject != null)
                    {
                        float value2 = 0f;

                        if (!string.IsNullOrEmpty(optionsList[i].defaultValue))
                            float.TryParse(optionsList[i].defaultValue, out value2);

                        optionsList[i].slider.value = value2;
                        optionsList[i].currentValue = value2.ToString("0.0000");
                        continue;
                    }
                }

                //Toggle
                if (optionsList[i].toggle != null)
                {
                    if (optionsList[i].toggle.gameObject != null)
                    {
                        bool value2 = false;

                        if (!string.IsNullOrEmpty(optionsList[i].defaultValue))
                            bool.TryParse(optionsList[i].defaultValue, out value2);

                        optionsList[i].toggle.isOn = value2;
                        optionsList[i].currentValue = value2.ToString();
                        continue;
                    }
                }

                //TextMeshPro
                if (optionsList[i].textMeshPro != null)
                {
                    if (optionsList[i].textMeshPro.gameObject != null)
                    {
                        string value;
                        value = optionsList[i].defaultValue;
                        optionsList[i].textMeshPro.text = value;
                        optionsList[i].currentValue = value.ToString();
                        continue;
                    }
                }

                //Input Text
                if (optionsList[i].inputfield != null)
                {
                    if (optionsList[i].inputfield.gameObject != null)
                    {
                        string value;
                        value = optionsList[i].defaultValue;
                        optionsList[i].inputfield.text = value;
                        optionsList[i].currentValue = value.ToString();
                        continue;
                    }
                }

                //Input Key Button
                if (optionsList[i].inputKeyButton != null)
                {
                    if (optionsList[i].inputKeyButton.gameObject != null)
                    {
                        string value;
                        value = optionsList[i].defaultValue;
                        optionsList[i].inputKeyButton.shownKey.text = TranslateKeyName(value);
                        optionsList[i].currentValue = value.ToString();
                        continue;
                    }
                }

                //Dropdown
                if (optionsList[i].dropdown != null)
                {
                    if (optionsList[i].dropdown.gameObject != null)
                    {
                        string value;
                        value = optionsList[i].defaultValue;

                        for (var x = 0; x < optionsList[i].dropdown.options.Count; x++)
                        {
                            if (optionsList[i].dropdown.options[x].text == value)
                            {
                                optionsList[i].dropdown.value = x;
                                optionsList[i].currentValue = value;
                                continue;
                            }
                        }
                        continue;
                    }
                }
            }

            SetMonitorDefaults();
        }

        private void SetCurrentFormToDefaults()
        {
            var foundOptions = FindObjectsOfType<OptionItem>();

            for (var i = 0; i < foundOptions.Length; i++)
            {
                //Slider
                if (foundOptions[i].slider != null)
                {
                    if (foundOptions[i].slider.gameObject != null)
                    {
                        float value2;
                        float.TryParse(foundOptions[i].defaultValue, out value2);
                        foundOptions[i].slider.value = value2;
                        foundOptions[i].currentValue = value2.ToString("0.0000");
                        continue;
                    }
                }

                //Toggle
                if (foundOptions[i].toggle != null)
                {
                    if (foundOptions[i].toggle.gameObject != null)
                    {
                        bool value2;
                        bool.TryParse(foundOptions[i].defaultValue, out value2);
                        foundOptions[i].toggle.isOn = value2;
                        foundOptions[i].currentValue = value2.ToString();
                        continue;
                    }
                }

                //TextMeshPro
                if (foundOptions[i].textMeshPro != null)
                {
                    if (foundOptions[i].textMeshPro.gameObject != null)
                    {
                        string value;
                        value = foundOptions[i].defaultValue;
                        foundOptions[i].textMeshPro.text = value;
                        foundOptions[i].currentValue = value.ToString();
                        continue;
                    }
                }

                //Input Text
                if (foundOptions[i].inputfield != null)
                {
                    if (foundOptions[i].inputfield.gameObject != null)
                    {
                        string value;
                        value = foundOptions[i].defaultValue;
                        foundOptions[i].inputfield.text = value;
                        foundOptions[i].currentValue = value.ToString();
                        continue;
                    }
                }

                //Input Key Button
                if (foundOptions[i].inputKeyButton != null)
                {
                    if (foundOptions[i].inputKeyButton.gameObject != null)
                    {
                        string value;
                        value = foundOptions[i].defaultValue;
                        foundOptions[i].inputKeyButton.shownKey.text = TranslateKeyName(value);
                        foundOptions[i].currentValue = value.ToString();
                        continue;
                    }
                }

                //Dropdown
                if (foundOptions[i].dropdown != null)
                {
                    if (foundOptions[i].dropdown.gameObject != null)
                    {
                        string value;
                        value = foundOptions[i].defaultValue;

                        for (var x = 0; x < foundOptions[i].dropdown.options.Count; x++)
                        {
                            if (foundOptions[i].dropdown.options[x].text == value)
                            {
                                foundOptions[i].dropdown.value = x;
                                foundOptions[i].currentValue = value;
                                continue;
                            }
                        }
                        continue;
                    }
                }
            }
        }

        private string GetSettingFilePath()
        {
            return Path.Combine(Application.dataPath, "options.ini"); ;
        }

        void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
        {
            if (!isStarted)
                return;

            if (lastScene != SceneManager.GetActiveScene().name)
                ApplyOptionsSceneChange();

            lastScene = SceneManager.GetActiveScene().name;
        }

        void OnDisable()
        {
            SceneManager.sceneLoaded -= OnLevelFinishedLoading;
        }

        #endregion

        #region Utility

        public string CleanUpStr(string input)
        {
            string result;
            char[] trimChars = { ' ' };
            result = input.TrimEnd(trimChars);
            return result;
        }

        public string GetOptionValue(OptionItem item)
        {
            //Slider
            if (item.slider != null)
            {
                if (item.slider.gameObject != null)
                    return item.slider.value.ToString();
            }

            //Dropdown
            if (item.dropdown != null)
            {
                if (item.dropdown.gameObject != null)
                {
                    if (item.dropdown.options.Count > 0)
                    {
                        int x = item.dropdown.value;
                        return item.dropdown.options[x].text;
                    }
                    else
                        return "";
                }
            }

            //Toggle
            if (item.toggle != null)
            {
                if (item.toggle.gameObject != null)
                    return item.toggle.isOn.ToString();
            }

            //Input Text
            if (item.inputfield != null)
            {
                if (item.inputfield.gameObject != null)
                    return item.inputfield.text;
            }

            //Input Key Button
            if (item.inputKeyButton != null)
            {
                if (item.inputKeyButton.gameObject != null)
                    return item.inputKeyButton.shownKey.text;
            }

            //TextMeshPro
            if (item.textMeshPro != null)
            {
                if (item.textMeshPro.gameObject != null)
                    return item.textMeshPro.text;
            }

            return "";
        }

        public OptionItem GetOptionByName(string name)
        {
            for (var i = 0; i < optionsList.Count; i++)
            {
                if (optionsList[i].gameObject.name == name)
                {
                    return optionsList[i];
                }
            }
            return null;
        }

        public string GetOptionValueByName(string name)
        {
            return GetOptionValue(GetOptionByName(name));
        }

        public void SetOptionValueByName(string name, string value)
        {
            OptionItem temp = GetOptionByName(name);

            if (temp == null)
                return;

            //Slider
            if (temp.slider != null)
            {
                if (temp.slider.gameObject != null)
                {
                    float value2 = 0.0f;

                    if (!string.IsNullOrEmpty(value))
                        float.TryParse(value, out value2);

                    temp.slider.value = value2;
                    temp.currentValue = value2.ToString("0.0000");
                    return;
                }
            }

            //Toggle
            if (temp.toggle != null)
            {
                if (temp.toggle.gameObject != null)
                {
                    bool value2 = false;

                    if (!string.IsNullOrEmpty(value))
                        bool.TryParse(value, out value2);

                    temp.toggle.isOn = value2;
                    temp.currentValue = value2.ToString();
                    return;
                }
            }

            //TextMeshPro
            if (temp.textMeshPro != null)
            {
                if (temp.textMeshPro.gameObject != null)
                {
                    temp.textMeshPro.text = value;
                    temp.currentValue = value.ToString();
                    return;
                }
            }

            //Input Text
            if (temp.inputfield != null)
            {
                if (temp.inputfield.gameObject != null)
                {
                    temp.inputfield.text = value;
                    temp.currentValue = value.ToString();
                    return;
                }
            }

            //Input Key Button
            if (temp.inputKeyButton != null)
            {
                if (temp.inputKeyButton.gameObject != null)
                {
                    temp.inputKeyButton.shownKey.text = TranslateKeyName(value);
                    temp.currentValue = value.ToString();
                    return;
                }
            }

            //Dropdown
            if (temp.dropdown != null)
            {
                if (temp.dropdown.gameObject != null)
                {
                    for (var x = 0; x < temp.dropdown.options.Count; x++)
                    {
                        if (temp.dropdown.options[x].text == value)
                        {
                            temp.dropdown.value = x;
                            temp.currentValue = value;
                            continue;
                        }
                    }
                    return;
                }
            }
        }

        public void ApplyOptionByName(string name)
        {
            OptionItem temp = GetOptionByName(name);
            if (temp == null)
                return;

            temp.ApplyOption();
        }

        public void SetOptionToDefaultByName(string name)
        {
            OptionItem temp = GetOptionByName(name);
            if (temp == null)
                return;

            //Slider
            if (temp.slider != null)
            {
                if (temp.slider.gameObject != null)
                {
                    float value2 = 0.0f;

                    if (!string.IsNullOrEmpty(temp.defaultValue))
                        float.TryParse(temp.defaultValue, out value2);

                    temp.slider.value = value2;
                    temp.currentValue = value2.ToString("0.0000");
                    return;
                }
            }

            //Toggle
            if (temp.toggle != null)
            {
                if (temp.toggle.gameObject != null)
                {
                    bool value2 = false;

                    if (!string.IsNullOrEmpty(temp.defaultValue))
                        bool.TryParse(temp.defaultValue, out value2);

                    temp.toggle.isOn = value2;
                    temp.currentValue = value2.ToString();
                    return;
                }
            }

            //TextMeshPro
            if (temp.textMeshPro != null)
            {
                if (temp.textMeshPro.gameObject != null)
                {
                    string value;
                    value = temp.defaultValue;
                    temp.textMeshPro.text = value;
                    temp.currentValue = value.ToString();
                    return;
                }
            }

            //Input Text
            if (temp.inputfield != null)
            {
                if (temp.inputfield.gameObject != null)
                {
                    string value;
                    value = temp.defaultValue;
                    temp.inputfield.text = value;
                    temp.currentValue = value.ToString();
                    return;
                }
            }

            //Input Key Button
            if (temp.inputKeyButton != null)
            {
                if (temp.inputKeyButton.gameObject != null)
                {
                    string value;
                    value = temp.defaultValue;
                    temp.inputKeyButton.shownKey.text = TranslateKeyName(value);
                    temp.currentValue = value.ToString();
                    return;
                }
            }

            //Dropdown
            if (temp.dropdown != null)
            {
                if (temp.dropdown.gameObject != null)
                {
                    string value;
                    value = temp.defaultValue;

                    for (var x = 0; x < temp.dropdown.options.Count; x++)
                    {
                        if (temp.dropdown.options[x].text == value)
                        {
                            temp.dropdown.value = x;
                            temp.currentValue = value;
                            continue;
                        }
                    }
                    return;
                }
            }
        }

        public string GetOptionsNameByIndex(int index)
        {
            if (optionsList[index].gameObject != null)
            {
                return optionsList[index].gameObject.name;
            }

            return "";
        }

        public int GetOptionIndexByName(string name)
        {
            for (var i = 0; i < optionsList.Count; i++)
            {
                if (optionsList[i].gameObject.name == name)
                {
                    return i;
                }
            }
            return -1;
        }

        public void EnableDisableOptionByName(string name, bool isEnabled)
        {
            int index = GetOptionIndexByName(name);
            if (index == -1)
                return;

            if (optionsList[index] != null)
            {
                optionsList[index].enabled = isEnabled;
            }
        }

        public string KeyCodeToString(KeyCode key)
        {
            string output;
            output = key.ToString();
            output = output.Replace("Mouse0", "Left Mouse");
            output = output.Replace("Mouse1", "Right Mouse");
            output = output.Replace("Mouse2", "Middle Mouse");
            if (output == "Alpha0") output = "0";
            if (output == "Alpha1") output = "1";
            if (output == "Alpha2") output = "2";
            if (output == "Alpha3") output = "3";
            if (output == "Alpha4") output = "4";
            if (output == "Alpha5") output = "5";
            if (output == "Alpha6") output = "6";
            if (output == "Alpha7") output = "7";
            if (output == "Alpha8") output = "8";
            if (output == "Alpha9") output = "9";
            if (output == "Exclaim") output = "!";
            if (output == "DoubleQuote") output = "\"";
            if (output == "Hash") output = "#";
            if (output == "Dollar") output = "$";
            if (output == "Percent") output = "%";
            if (output == "Ampersand") output = "&";
            if (output == "Quote") output = "\'";
            if (output == "LeftParen") output = "(";
            if (output == "RightParen") output = ")";
            if (output == "Asterisk") output = "*";
            if (output == "Plus") output = "+";
            if (output == "Minus") output = "-";
            if (output == "Comma") output = ",";
            if (output == "Peroid") output = ".";
            if (output == "Slash") output = "/";
            if (output == "Colon") output = ":";
            if (output == "Semicolon") output = ";";
            if (output == "Less") output = "<";
            if (output == "Greater") output = ">";
            if (output == "Equals") output = "=";
            if (output == "Question") output = "?";
            if (output == "At") output = "@";
            if (output == "LeftBracket") output = "[";
            if (output == "RightBracket") output = "]";
            if (output == "Backslash") output = "\\";
            if (output == "Caret") output = "^";
            if (output == "Underscore") output = "_";
            if (output == "Backquote") output = "`";
            if (output == "LeftCurlyBracket") output = "{";
            if (output == "RightCurlyBracket") output = "}";
            if (output == "Tilde") output = "~";
            if (output == "Pipe") output = "|";
            return output;
        }

        public KeyCode StringToKeyCode(string key)
        {
            string output;
            output = key;
            output = output.Replace("Left Mouse", "Mouse0");
            output = output.Replace("Right Mouse", "Mouse1");
            output = output.Replace("Middle Mouse", "Mouse2");
            if (output == "0") output = "Alpha0";
            if (output == "1") output = "Alpha1";
            if (output == "2") output = "Alpha2";
            if (output == "3") output = "Alpha3";
            if (output == "4") output = "Alpha4";
            if (output == "5") output = "Alpha5";
            if (output == "6") output = "Alpha6";
            if (output == "7") output = "Alpha7";
            if (output == "8") output = "Alpha8";
            if (output == "9") output = "Alpha9";
            if (output == "!") output = "Exclaim";
            if (output == "\"") output = "DoubleQuote";
            if (output == "#") output = "Hash";
            if (output == "$") output = "Dollar";
            if (output == "%") output = "Percent";
            if (output == "&") output = "Ampersand";
            if (output == "\'") output = "Quote";
            if (output == "(") output = "LeftParen";
            if (output == ")") output = "RightParen";
            if (output == "*") output = "Asterisk";
            if (output == "+") output = "Plus";
            if (output == "-") output = "Minus";
            if (output == ",") output = "Comma";
            if (output == ".") output = "Peroid";
            if (output == "/") output = "Slash";
            if (output == ":") output = "Colon";
            if (output == ";") output = "Semicolon";
            if (output == "<") output = "Less";
            if (output == ">") output = "Greater";
            if (output == "?") output = "Question";
            if (output == "@") output = "At";
            if (output == "[") output = "LeftBracket";
            if (output == "]") output = "RightBracket";
            if (output == "\\") output = "Backslash";
            if (output == "^") output = "Caret";
            if (output == "_") output = "Underscore";
            if (output == "`") output = "Backquote";
            if (output == "{") output = "LeftCurlyBracket";
            if (output == "}") output = "RightCurlyBracket";
            if (output == "~") output = "Tilde";
            if (output == "|") output = "Pipe";

            KeyCode temp;
            Enum.TryParse(output, out temp);
            return temp;
        }

        public string TranslateKeyName(string key)
        {
            string output;
            output = key.ToString();
            output = output.Replace("Mouse0", "Left Mouse");
            output = output.Replace("Mouse1", "Right Mouse");
            output = output.Replace("Mouse2", "Middle Mouse");
            if (output == "Alpha0") output = "0";
            if (output == "Alpha1") output = "1";
            if (output == "Alpha2") output = "2";
            if (output == "Alpha3") output = "3";
            if (output == "Alpha4") output = "4";
            if (output == "Alpha5") output = "5";
            if (output == "Alpha6") output = "6";
            if (output == "Alpha7") output = "7";
            if (output == "Alpha8") output = "8";
            if (output == "Alpha9") output = "9";
            if (output == "Exlaim") output = "!";
            if (output == "DoubleQuote") output = "\"";
            if (output == "Hash") output = "#";
            if (output == "Dollar") output = "$";
            if (output == "Percent") output = "%";
            if (output == "Ampersand") output = "&";
            if (output == "Quote") output = "\'";
            if (output == "LeftParen") output = "(";
            if (output == "RightParen") output = ")";
            if (output == "Asterisk") output = "*";
            if (output == "Plus") output = "+";
            if (output == "Minus") output = "-";
            if (output == "Comma") output = ",";
            if (output == "Peroid") output = ".";
            if (output == "Slash") output = "/";
            if (output == "Colon") output = ":";
            if (output == "Semicolon") output = ";";
            if (output == "Less") output = "<";
            if (output == "Greater") output = ">";
            if (output == "Question") output = "?";
            if (output == "At") output = "@";
            if (output == "LeftBracket") output = "[";
            if (output == "RightBracket") output = "]";
            if (output == "Backslash") output = "\\";
            if (output == "Caret") output = "^";
            if (output == "Underscore") output = "_";
            if (output == "Backquote") output = "`";
            if (output == "LeftCurlyBracket") output = "{";
            if (output == "RightCurlyBracket") output = "}";
            if (output == "Tilde") output = "~";
            if (output == "Pipe") output = "|";
            return output;
        }

        public string GetKeyStringFromOption(string name)
        {
            string output;
            output = GetOptionValueByName(name);
            KeyCode temp = StringToKeyCode(output);
            output = temp.ToString();
            return output;
        }

        #endregion
    }

    [Serializable]
    public class OptionCallback : UnityEvent<OptionItem>
    { }

    public enum OptionType
    {
        Video,
        Audio,
        Gameplay,
        Controls,
        Accessibility
    }
}