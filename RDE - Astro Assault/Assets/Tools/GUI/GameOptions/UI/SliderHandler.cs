using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SliderHandler : MonoBehaviour
{
    private Slider slider;
    private TextMeshProUGUI sliderValue;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        sliderValue = GetComponentInChildren<TextMeshProUGUI>();
        slider.onValueChanged.AddListener(delegate { DisplayText(); });
        DisplayText();
    }

    private void DisplayText()
    {
        float normalizedValue = (slider.value - slider.minValue) / (slider.maxValue - slider.minValue);
        float percentage = normalizedValue * 100f;
        sliderValue.text = "%" + percentage.ToString("F0");
    }
}