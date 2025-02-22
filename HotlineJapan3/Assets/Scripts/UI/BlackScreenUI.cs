using System;
using UnityEngine;
using UnityEngine.UI;

public class BlackScreenUI : MonoBehaviour
{
    public event Action OnImageBlack;
    public event Action OnImageTransparent;

    [SerializeField] private Image blackscreen;

    private float targetAlpha;
    [SerializeField] private float changeSpeed = 1;
    public const float DEFAULT_TIME_FRAME = 0.5f;
    private float alpha
    {
        get
        {
            return blackscreen.color.a;
        }
        set
        {
            Color newColor = blackscreen.color;
            newColor.a = value;
            blackscreen.color = newColor;
        }
    }

    public bool isChanging = false;

    private void Awake()
    {
        targetAlpha = alpha;
    }

    private void Update()
    {
        if (!isChanging)
            return;

        float currentChangeSpeed = targetAlpha == 0 ? -changeSpeed : changeSpeed;
        alpha += currentChangeSpeed * Time.unscaledDeltaTime;
        alpha = Mathf.Clamp01(alpha);

        if (alpha == targetAlpha)
        {
            isChanging = false;
            if (alpha == 0)
                OnImageTransparent?.Invoke();
            else
                OnImageBlack?.Invoke();
        }
    }

    public void ToBlack()
    {
        targetAlpha = 1f;
        alpha = 1f;
    }

    public void ToBlackOverTime(float timeFrame = DEFAULT_TIME_FRAME)
    {
        targetAlpha = 1f;
        isChanging = true;
    }

    public void FromBlack()
    {
        targetAlpha = 0f;
        alpha = 0f;
    }

    public void FromBlackOverTime(float timeFrame = DEFAULT_TIME_FRAME)
    {
        targetAlpha = 0f;
        isChanging = true;
    }
}
