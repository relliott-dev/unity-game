using System.Collections;
using UnityEngine;

namespace RDE
{
    /// <summary>
    /// 
    /// Manages internet connectivity for the game
    /// Allows for checking the internet connection and optionally notifying the player of its status
    /// 
    /// </summary>
    public class InternetManager : MonoBehaviour
    {
        #region Variables

        public static InternetManager instance;

        [Header("Settings")]
        [SerializeField, Range(1f, 30f)] private float checkInterval = 5f;

        [Header("Helper Variables")]
        [HideInInspector] public bool onlineMode = false;

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

        #region Connection Methods

        //Checks if connected to the internet
        public bool CheckConnection()
        {
            if(Application.internetReachability == NetworkReachability.NotReachable)
            {
                PlayerUIManager.instance.CrashedInternet();
                onlineMode = false;
                return false;
            }
            else
            {
                return true;
            }
        }

        //Checks if connected to the internet periodically
        public void CheckConnectionPeriodically()
        {
            onlineMode = true;
            StartCoroutine(CheckInternetConnectionPeriodically());
        }

        //Coroutine to check if connected to the internet periodically
        private IEnumerator CheckInternetConnectionPeriodically()
        {
            while(onlineMode && CheckConnection())
            {
                yield return new WaitForSeconds(checkInterval);
            }
        }
        
        #endregion
    }
}