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
    public enum ArrowOrientation
    {
        Up,
        Down,
        Left,
        Right
    }

    [Serializable]
    private class OrientationIconPair
    {
        public ArrowOrientation orientation;
        public GameObject imageObject;
    }
    [SerializeField] private List<OrientationIconPair> pairs;

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
        ArrowOrientation orientation = GetArrowOrientation();
        SetArrowDirection(orientation);

        Vector2 newPosition = Vector2.zero;

        if (IsScreenPositionInsideCanvas(targetScreenPosition))
        {
            newPosition = GetPositionWithOffset(targetScreenPosition, orientation);
        }
        else
        {
            float x = Mathf.Clamp(targetScreenPosition.x, 0 + offset, canvasRect.width - offset);
            float y = Mathf.Clamp(targetScreenPosition.y, 0 + offset, canvasRect.height - offset);
            newPosition = new Vector2(x, y);
        }

        iconTransform.position = newPosition;
    }

    private ArrowOrientation GetArrowOrientation()
    {
        Vector2 iconToTargetVector = targetTransform.position - PlayerController.Instance.transform.position;
        if (Mathf.Abs(iconToTargetVector.x) > Mathf.Abs(iconToTargetVector.y))
        {
            if (iconToTargetVector.x > 0)
                return ArrowOrientation.Right;
            else
                return ArrowOrientation.Left;
        }
        else
        {
            if (iconToTargetVector.y > 0)
                return ArrowOrientation.Up;
            else
                return ArrowOrientation.Down;
        }
    }

    private Vector2 GetPositionWithOffset(Vector2 position, ArrowOrientation arrowOrientation)
    {
        float angle = 0;
        switch (arrowOrientation)
        {
            case ArrowOrientation.Down:
                angle = 0;
                break;
            case ArrowOrientation.Left:
                angle = 90;
                break;
            case ArrowOrientation.Up:
                angle = 180;
                break;
            case ArrowOrientation.Right:
                angle = 270;
                break;
        }
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.back);
        Vector2 offsetVector = (rotation * Vector2.up) * offset;
        return position + offsetVector;
    }

    private void SetArrowDirection(ArrowOrientation arrowOrientation)
    {
        foreach(var pair in pairs)
        {
            pair.imageObject.SetActive(pair.orientation == arrowOrientation);
        }
    }

    private bool IsScreenPositionInsideCanvas(Vector2 screenPosition)
    {
        bool xValid = screenPosition.x > 0 && screenPosition.x < canvasRect.width;
        bool yValid = screenPosition.y > 0 && screenPosition.y < canvasRect.height;
        return xValid && yValid;
    }
}
