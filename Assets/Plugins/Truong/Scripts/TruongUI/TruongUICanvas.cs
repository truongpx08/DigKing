using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruongUICanvas : TruongUI
{
    [SerializeField] protected Canvas canvas;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadCanvas();
    }

    private void LoadCanvas()
    {
        this.canvas = GetComponentInChildren<Canvas>();
    }
}