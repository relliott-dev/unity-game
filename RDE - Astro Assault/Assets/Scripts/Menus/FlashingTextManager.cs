using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections;

namespace RDE
{
    /// <summary>
    /// 
    /// Manages the flashing effect of UI text elements to draw attention
    /// The effect includes transitioning the alpha value of the text between a specified minimum and maximum value
    /// It starts flashing when hovered over with a mouse or when programmatically selected, and stops when deselected
    /// 
    /// </summary>
    public class FlashingTextManager : MonoBehaviour, IPointerEnterHandler, ISelectHandler, IDeselectHandler
    {
        #region Variables

        [Header("Settings")]
        [SerializeField] private float minAlpha = 0.2f;
        [SerializeField] private float maxAlpha = 0.9f;
        [SerializeField] private float timerAlpha = 1f;

        private TextMeshProUGUI text;
        private Color currentColor;
        private Coroutine flashingCoroutine;
        private float startTime;
        private bool isSelected;

        #endregion

        #region Base Methods

        private void Awake()
        {
            text = GetComponent<TextMeshProUGUI>();
            currentColor = text.color;
            SetAlpha(1f);
        }

        #endregion

        #region Flashing Effect

        //Starts the flashing effect
        private void StartFlashing()
        {
            if (flashingCoroutine == null)
            {
                startTime = Time.time;
                flashingCoroutine = StartCoroutine(FlashingEffect());
            }
        }

        //Stops the flashing effect
        private void StopFlashing()
        {
            if (flashingCoroutine != null)
            {
                StopCoroutine(flashingCoroutine);
                flashingCoroutine = null;
            }
            SetAlpha(1f);
        }

        //Sets the alpha transparency
        private void SetAlpha(float alpha)
        {
            text.color = new Color(currentColor.r, currentColor.g, currentColor.b, Mathf.Clamp(alpha, minAlpha, maxAlpha));
        }

        //Coroutine for ping pong flash effect
        private IEnumerator FlashingEffect()
        {
            while (true)
            {
                float chAlpha = minAlpha + Mathf.PingPong((Time.time - startTime) / timerAlpha, maxAlpha - minAlpha);
                SetAlpha(chAlpha);
                yield return null;
            }
        }

        #endregion

        #region Handler Methods

        //Mouse Over Event
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!isSelected)
            {
                EventSystem.current.SetSelectedGameObject(gameObject);
            }
        }

        //Select Event
        public void OnSelect(BaseEventData eventData)
        {
            isSelected = true;
            StartFlashing();
            SoundFXManager.instance.PlaySound(SoundFXManager.instance.GetHoverMenuSFX());
        }

        //Deselect Event
        public void OnDeselect(BaseEventData eventData)
        {
            isSelected = false;
            StopFlashing();
        }

        //Disable Event
        private void OnDisable()
        {
            isSelected = false;
            StopFlashing();
        }

        #endregion
    }
}