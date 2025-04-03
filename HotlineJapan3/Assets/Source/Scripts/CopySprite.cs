using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class CopySprite : MonoBehaviour
{
    [SerializeField] private SpriteRenderer targetSP;
    [SerializeField] private SpriteRenderer selfSP;

    private void Start()
    {
        if (selfSP == null)
        {
            selfSP = GetComponent<SpriteRenderer>();
        }
        
        targetSP.RegisterSpriteChangeCallback(OnSpriteChangedCallback);
        CopyTargetSprite();
    }

    private void OnSpriteChangedCallback(SpriteRenderer arg0)
    {
        CopyTargetSprite();
    }

    private void CopyTargetSprite()
    {
        selfSP.sprite = targetSP.sprite;
    }

    private void OnValidate()
    {
        CopyTargetSprite();
    }
}
