using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class ScrollViewAdjuster : MonoBehaviour
{
    private ScrollRect scrollView;
    private RectTransform contentRect;
    private Scrollbar scrollbar;

    void Awake()
    {
        scrollView = gameObject.GetComponent<ScrollRect>();
        contentRect = scrollView.content.GetComponent<RectTransform>();
        scrollbar = scrollView.verticalScrollbar;
    }

    public void UpdateScrollBars(Transform currentForm)
    {
        float newHeight = currentForm.GetComponent<RectTransform>().sizeDelta.y;
        contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, newHeight);
        scrollbar.value = 1f;
    }
}