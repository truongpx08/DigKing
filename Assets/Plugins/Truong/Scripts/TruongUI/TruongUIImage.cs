using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TruongUIImage : TruongUI
{
    [SerializeField] protected Image image;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadImage();
    }

    protected virtual void LoadImage()
    {
        image = GetComponentInChildren<Image>();
    }

    public void AddSpriteToImage(Sprite sprite)
    {
        this.image.sprite = sprite;
    }
}