using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RDE
{
    /// <summary>
    /// 
    /// Manages the patch notes in the game
    /// Generates the patch notes using a list of title, subtitles and patches
    /// 
    /// @TODO:
    /// - Change scroll bars
    /// - Change font
    /// 
    /// </summary>
    public class PatchNotesManager : MonoBehaviour
    {
        #region Variables

        [SerializeField] private List<PatchNoteData> patchNotesData;

        [Header("UI Objects")]
        [SerializeField] private GameObject titlePrefab;
        [SerializeField] private GameObject subtitlePrefab;
        [SerializeField] private GameObject patchPrefab;
        private ScrollRect scrollRect;

        #endregion

        #region Base Methods

        private void Awake()
        {
            scrollRect = GetComponent<ScrollRect>();

            if (titlePrefab == null)
            {
                Debug.LogError("PatchNotesManager Error: Title Prefab is not assigned");
            }
            if (subtitlePrefab == null)
            {
                Debug.LogError("PatchNotesManager Error: Subtitle Prefab is not assigned");
            }
            if (subtitlePrefab == null)
            {
                Debug.LogError("PatchNotesManager Error: Patch Prefab is not assigned");
            }
        }

        private void Start()
        {
            GeneratePatchNotes();
            Canvas.ForceUpdateCanvases();
            scrollRect.verticalNormalizedPosition = 1f;
        }

        private void OnEnable()
        {
            scrollRect.verticalNormalizedPosition = 1f;
        }

        #endregion

        #region Generate Patch Notes

        // Generates the patch notes dynamically
        private void GeneratePatchNotes()
        {
            foreach (var patchVersion in patchNotesData)
            {
                GameObject patchTitle = Instantiate(titlePrefab, scrollRect.content);
                TextMeshProUGUI titleText = patchTitle.GetComponent<TextMeshProUGUI>();
                titleText.text = "Game Patch " + patchVersion.patchVersion + " - Released " + patchVersion.patchMonth + "/" + patchVersion.patchDay + "/" + patchVersion.patchYear;

                AddSpacer(20f);

                if (patchVersion.generalUpdates.Count > 0)
                {
                    GameObject patchSubtitleUpdate = Instantiate(subtitlePrefab, scrollRect.content);
                    TextMeshProUGUI subtitleText = patchSubtitleUpdate.GetComponent<TextMeshProUGUI>();
                    subtitleText.text = "General Updates";

                    AddSpacer(1f);

                    foreach (var patchEntry in patchVersion.generalUpdates)
                    {
                        GameObject patchUpdate = Instantiate(patchPrefab, scrollRect.content);
                        TextMeshProUGUI patchText = patchUpdate.GetComponent<TextMeshProUGUI>();
                        patchText.text = "• " + patchEntry;
                    }

                    AddSpacer(5f);
                }

                AddSpacer(5f);

                if (patchVersion.newFeatures.Count > 0)
                {
                    GameObject patchSubtitleUpdate = Instantiate(subtitlePrefab, scrollRect.content);
                    TextMeshProUGUI subtitleText = patchSubtitleUpdate.GetComponent<TextMeshProUGUI>();
                    subtitleText.text = "New Features";

                    AddSpacer(1f);

                    foreach (var patchEntry in patchVersion.newFeatures)
                    {
                        GameObject patchUpdate = Instantiate(patchPrefab, scrollRect.content);
                        TextMeshProUGUI patchText = patchUpdate.GetComponent<TextMeshProUGUI>();
                        patchText.text = "• " + patchEntry;
                    }

                    AddSpacer(5f);
                }

                AddSpacer(5f);

                if (patchVersion.bugFixes.Count > 0)
                {
                    GameObject patchSubtitleUpdate = Instantiate(subtitlePrefab, scrollRect.content);
                    TextMeshProUGUI subtitleText = patchSubtitleUpdate.GetComponent<TextMeshProUGUI>();
                    subtitleText.text = "Bug Fixes";

                    AddSpacer(1f);

                    foreach (var patchEntry in patchVersion.bugFixes)
                    {
                        GameObject patchUpdate = Instantiate(patchPrefab, scrollRect.content);
                        TextMeshProUGUI patchText = patchUpdate.GetComponent<TextMeshProUGUI>();
                        patchText.text = "• " + patchEntry;
                    }

                    AddSpacer(5f);
                }

                AddSpacer(5f);

                if (patchVersion.knownIssues.Count > 0)
                {
                    GameObject patchSubtitleUpdate = Instantiate(subtitlePrefab, scrollRect.content);
                    TextMeshProUGUI subtitleText = patchSubtitleUpdate.GetComponent<TextMeshProUGUI>();
                    subtitleText.text = "Known Issues";

                    AddSpacer(1f);

                    foreach (var patchEntry in patchVersion.knownIssues)
                    {
                        GameObject patchUpdate = Instantiate(patchPrefab, scrollRect.content);
                        TextMeshProUGUI patchText = patchUpdate.GetComponent<TextMeshProUGUI>();
                        patchText.text = "• " + patchEntry;
                    }

                    AddSpacer(5f);
                }

                AddSpacer(5f);
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
    }
}