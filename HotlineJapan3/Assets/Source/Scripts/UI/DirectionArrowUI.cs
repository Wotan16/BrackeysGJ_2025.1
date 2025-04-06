using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DirectionArrowUI : MonoBehaviour
{
    public Transform targetTransform;
    [SerializeField] private RectTransform iconTransform;
    [SerializeField] private Canvas canvas;
    private Rect canvasRect;
    [SerializeField] private float offset;

    private void Awake()
    {
        canvasRect = canvas.GetComponent<RectTransform>().rect;
    }

    private void Start()
    {
        iconTransform.gameObject.SetActive(false);
        LevelController.Instance.OnLevelCopleted += Instance_OnLevelCopleted;
    }

    private void Instance_OnLevelCopleted()
    {
        targetTransform = FindFirstObjectByType<VictoryArea>().transform;
        iconTransform.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (targetTransform == null)
            return;

        Vector3 targetScreenPosition = Camera.main.WorldToScreenPoint(targetTransform.position);

        Vector2 newPosition = Vector2.zero;
        RotateArrowTowardsTarget();

        if (IsScreenPositionInsideCanvas(targetScreenPosition))
        {
            newPosition = targetScreenPosition;
        }
        else
        {
            float x = Mathf.Clamp(targetScreenPosition.x, 0 + offset, canvasRect.width - offset);
            float y = Mathf.Clamp(targetScreenPosition.y, 0 + offset, canvasRect.height - offset);
            newPosition = new Vector2(x, y);
        }

        iconTransform.position = newPosition;
    }

    private void RotateArrowTowardsTarget()
    {
        Vector3 playerScreenPosition = Camera.main.WorldToScreenPoint(PlayerController.Instance.transform.position);
        Vector2 playerToTargetVector = iconTransform.position - playerScreenPosition;
        Quaternion newRotation = Quaternion.FromToRotation(Vector3.up, playerToTargetVector);
        iconTransform.rotation = newRotation;
    }

    private bool IsScreenPositionInsideCanvas(Vector2 screenPosition)
    {
        bool xValid = screenPosition.x > 0 && screenPosition.x < canvasRect.width;
        bool yValid = screenPosition.y > 0 && screenPosition.y < canvasRect.height;
        return xValid && yValid;
    }
}
