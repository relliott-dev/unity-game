using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

namespace RDE
{
    public partial class GameOptions : MonoBehaviour
    {
        [SerializeField] private Dropdown resolutionsList;
        [SerializeField] private Dropdown monitorList;
        [SerializeField] private Toggle windowedToggle;

        private bool isFullScreen = true;

        private void LoadMonitorsInfo()
        {
            LoadMonitorList();
            LoadResolutionsList();
            SetCurrentMonitor();
            SetCurrentResolution();
        }

        private void LoadMonitorList()
        {
            monitorList.ClearOptions();
            for (int i = 0; i < Display.displays.Length; i++)
            {
                monitorList.options.Add(new Dropdown.OptionData(i.ToString()));
            }
        }

        private void LoadResolutionsList()
        {
            Resolution[] resArray = Screen.resolutions;
            List<string> resList = new List<string>();

            resolutionsList.ClearOptions();

            foreach (var res in resArray)
            {
                string checkRes = res.ToString().Replace(" ", "");
                checkRes = checkRes.Substring(0, Math.Max(checkRes.IndexOf('@'), 0));
                resList.Add(checkRes);
            }

            List<string> resList2 = resList.Distinct().ToList();
            resolutionsList.AddOptions(resList2);
        }

        private void SetCurrentMonitor()
        {
            for (var i = 0; i < monitorList.options.Count; i++)
            {
                if (monitorList.options[i].text == Display.main.systemWidth + "x" + Display.main.systemHeight)
                {
                    monitorList.value = i;
                    break;
                }
            }
        }

        private void SetCurrentResolution()
        {
            for (var i = 0; i < resolutionsList.options.Count; i++)
            {
                if (resolutionsList.options[i].text == Screen.width + "x" + Screen.height)
                {
                    resolutionsList.value = i;
                    break;
                }
            }
        }

        public void SetGFXMonitorSettings()
        {
            if (isEditor)
                return;

            int monitorSelection;
            string resolution;
            bool windowed;
            int x;
            int y;

            resolution = GetOptionValueByName(resolutionsList.gameObject.name);
            string[] res2 = resolution.Split('x');
            x = int.Parse(res2[0]);
            y = int.Parse(res2[1]);
            /*
            bool.TryParse(GetOptionValueByName(windowedToggle.gameObject.name), out windowed);

            int.TryParse(GetOptionValueByName(monitorList.gameObject.name), out monitorSelection);

            Debug.Log(windowed + " " + monitorSelection);

            if (monitorSelection < Display.displays.Length)
            {
                if (PlayerPrefs.GetInt("UnitySelectMonitor") != monitorSelection)
                    PlayerPrefs.SetInt("UnitySelectMonitor", monitorSelection);

                if (Screen.width != x || Screen.height != y || Screen.fullScreen != windowed)
                {
                    Screen.SetResolution(x, y, windowed);
                    LoadMonitorsInfo();
                    isFullScreen = windowed;
                }
            }*/
        }

        public void SetGFXMonitorChange()
        {
            if (isEditor)
                return;

            int monitorSelection;
            int.TryParse(GetOptionValueByName(monitorList.gameObject.name), out monitorSelection);

            if (PlayerPrefs.GetInt("UnitySelectMonitor") != monitorSelection)
            {
                PlayerPrefs.SetInt("UnitySelectMonitor", monitorSelection);
            }

            StartCoroutine(GetNewMonitorInfo(monitorSelection));
        }

        public void SetMonitorDefaults()
        {
            if (Display.displays.Length > 0)
            {
                if (PlayerPrefs.GetInt("UnitySelectMonitor") != 0)
                {
                    PlayerPrefs.SetInt("UnitySelectMonitor", 0);
                }
            }

            windowedToggle.isOn = false;

            StartCoroutine(GetNewMonitorInfo(0));
        }

        IEnumerator GetNewMonitorInfo(int newMonitor)
        {
            if (isEditor)
                yield break;

            int currMonitor = 0;
            bool windowed;
            bool.TryParse(GetOptionValueByName(windowedToggle.gameObject.name), out windowed);

            int x = Display.displays[newMonitor].systemWidth;
            int y = Display.displays[newMonitor].systemHeight;

            if (currMonitor != newMonitor || windowed)
            {
                if (Screen.width != x || Screen.height != y || Screen.fullScreen != !windowed)
                {
                    Screen.SetResolution(x, y, !windowed);
                    LoadMonitorsInfo();
                    isFullScreen = !windowed;
                }
            }
        }
    }
}