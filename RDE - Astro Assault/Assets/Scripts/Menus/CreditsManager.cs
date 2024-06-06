using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RDE
{
    /// <summary>
    /// 
    /// Manages the credits in the game
    /// Generates the credits using a list of title and descriptions
    /// Allows for automated vertical scrolling with an option for the user to manually control it
    /// 
    /// @TODO:
    /// - Change scroll bars
    /// - Change font
    /// 
    /// </summary>
    public class CreditsManager : MonoBehaviour, IBeginDragHandler, IEndDragHandler
    {
        #region Variables

        [SerializeField] private List<CreditData> creditsData;

        [Header("UI Objects")]
        [SerializeField] private GameObject titlePrefab;
        [SerializeField] private GameObject namePrefab;
        private ScrollRect scrollRect;

        [Header("Settings")]
        [SerializeField, Range(5f, 200f)] private float scrollSpeed = 50f;
        [SerializeField, Range(0.5f, 5f)] private float delayBeforeStart = 1.5f;

        [Header("Helper Variables")]
        private bool isScrolling;
        private Coroutine scrollingCoroutine;

        #endregion

        #region Base Methods

        private void Awake()
        {
            scrollRect = GetComponent<ScrollRect>();

            if (titlePrefab == null)
            {
                Debug.LogError("CreditsManager Error: Title Prefab is not assigned");
            }
            if (namePrefab == null)
            {
                Debug.LogError("CreditsManager Error: Name Prefab is not assigned");
            }
        }

        private void Start()
        {
            GenerateCredits();
            Canvas.ForceUpdateCanvases();
            scrollRect.verticalNormalizedPosition = 1f;
        }

        private void OnEnable()
        {
            isScrolling = false;
            scrollRect.verticalNormalizedPosition = 1f;
            if (scrollingCoroutine != null)
            {
                StopCoroutine(scrollingCoroutine);
            }
            scrollingCoroutine = StartCoroutine(StartScrollingAfterDelay());
        }

        private void Update()
        {
            if (isScrolling && scrollRect.verticalNormalizedPosition > 0f)
            {
                scrollRect.verticalNormalizedPosition -= Time.deltaTime * scrollSpeed / scrollRect.content.rect.height;
            }
        }

        #endregion

        #region Generate Credits

        // Generates the credits dynamically
        private void GenerateCredits()
        {
            AddSpacer(scrollRect.GetComponent<RectTransform>().rect.height);

            foreach (var titleEntry in creditsData)
            {
                GameObject titleCredit = Instantiate(titlePrefab, scrollRect.content);
                TextMeshProUGUI titleText = titleCredit.GetComponent<TextMeshProUGUI>();
                titleText.text = titleEntry.title;

                foreach (var nameEntry in titleEntry.names)
                {
                    GameObject nameCredit = Instantiate(namePrefab, scrollRect.content);
                    TextMeshProUGUI nameText = nameCredit.GetComponent<TextMeshProUGUI>();
                    nameText.text = nameEntry;
                }

                AddSpacer(30f);
            }
        }

        //Adds a spacer GameObject with a given height
        private void AddSpacer(float height)
        {
            GameObject spacer = new GameObject("Spacer", typeof(RectTransform));
            spacer.transform.SetParent(scrollRect.content, false);
            RectTransform rt = spacer.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, height);
        }

        #endregion

        #region Drag and Scroll Methods

        //Stops the automatic scrolling when the user starts dragging
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (scrollingCoroutine != null)
            {
                StopCoroutine(scrollingCoroutine);
            }
            isScrolling = false;
        }

        //Resumes automatic scrolling when the user stops dragging
        public void OnEndDrag(PointerEventData eventData)
        {
            scrollingCoroutine = StartCoroutine(StartScrollingAfterDelay());
        }

        //Starts the scrolling of the credits after a specified delay
        private IEnumerator StartScrollingAfterDelay()
        {
            yield return new WaitForSeconds(delayBeforeStart);
            isScrolling = true;
        }

        #endregion
    }
}