using UnityEngine;
using UnityEngine.UI;

namespace RDE
{
    /// <summary>
    /// 
    /// This class manages the visual representation of a stat bar in the game's user interface
    /// It controls a UI slider to reflect the current and maximum values of a specified stat
    /// The class also allows for dynamic resizing of the stat bar based on the stat values
    /// 
    /// </summary>
    public class UI_StatBar : MonoBehaviour
    {
        private Slider slider;
        private RectTransform rectTransform;

        [Header("Bar Settings")]
        [SerializeField] protected float widthScaleMultiplier = 3f;

        protected virtual void Awake()
        {
            slider = GetComponent<Slider>();
            rectTransform = GetComponent<RectTransform>();

            if (slider == null)
            {
                Debug.LogError("UI_StatBar: Slider component not found on the object");
                return;
            }
            
            if (rectTransform == null)
            {
                Debug.LogError("UI_StatBar: RectTransform component not found on the object");
                return;
            }
        }

        //Updates the current value of the stat bar
        public virtual void SetStat(float newValue)
        {
            slider.value = newValue;
        }

        //Updates the maximum value of the stat bar and scales the length of the bar if scaleBarLengthWithStats is true
        public virtual void SetMaxStat(float maxValue)
        {
            slider.maxValue = maxValue;

            rectTransform.sizeDelta = new Vector2(maxValue * widthScaleMultiplier, rectTransform.sizeDelta.y);
        }
    }
}