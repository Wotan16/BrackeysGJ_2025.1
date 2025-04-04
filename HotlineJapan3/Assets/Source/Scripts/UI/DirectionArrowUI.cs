using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DirectionArrowUI : MonoBehaviour
{
    public Transform targetTransform;
    [SerializeField] private RectTransform iconTransform;
    [SerializeField] private Canvas canvas;
    private RectTransform rectTransform;
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
        rectTransform = canvas.GetComponent<RectTransform>();
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

        float x = Mathf.Clamp(targetScreenPosition.x, 0 + offset, rectTransform.rect.width - offset);
        float y = Mathf.Clamp(targetScreenPosition.y, 0 + offset, rectTransform.rect.height - offset);
        Vector2 newPosition = new Vector2(x, y);
        iconTransform.position = newPosition;

        Vector2 iconToTargetVector = targetTransform.position - PlayerController.Instance.transform.position;
        if (Mathf.Abs(iconToTargetVector.x) > Mathf.Abs(iconToTargetVector.y))
        {
            if (iconToTargetVector.x > 0)
                SetArrowDirection(ArrowOrientation.Right);
            else
                SetArrowDirection(ArrowOrientation.Left);
        }
        else
        {
            if (iconToTargetVector.y > 0)
                SetArrowDirection(ArrowOrientation.Up);
            else
                SetArrowDirection(ArrowOrientation.Down);
        }
    }

    //private void SetArrowDirection(ArrowOrientation arrowOrientation)
    //{
    //    float angle = 0;
    //    switch (arrowOrientation)
    //    {
    //        case ArrowOrientation.Up:
    //            angle = 0;
    //            break;
    //            case ArrowOrientation.Right:
    //            angle = 90;
    //            break;
    //        case ArrowOrientation.Down:
    //            angle = 180;
    //            break;
    //        case ArrowOrientation.Left:
    //            angle = 270;
    //            break;
    //    }
    //    Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.back);
    //    iconTransform.rotation = rotation;
    //}

    private void SetArrowDirection(ArrowOrientation arrowOrientation)
    {
        foreach(var pair in pairs)
        {
            pair.imageObject.SetActive(pair.orientation == arrowOrientation);
        }
    }

    private bool IsScreenPositionInsideCanvas(Vector2 screenPosition)
    {
        bool xValid = screenPosition.x > 0 && screenPosition.x < rectTransform.rect.width;
        bool yValid = screenPosition.y > 0 && screenPosition.y < rectTransform.rect.height;
        return xValid && yValid;
    }
}
