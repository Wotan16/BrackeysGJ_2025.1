using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderValueUI : MonoBehaviour
{
    [SerializeField]
    private float modifier = 1f;

    [SerializeField]
    private TextMeshProUGUI valueText;


    public void UpdateText(float value)
    {
        float newValue = (float)Math.Round(value * modifier, 2);
        valueText.text = newValue.ToString();
    }
}
