using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruongUICanvasLayerHigher : TruongUICanvas
{
    [SerializeField] private Canvas parentCanvas;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadParentCanvas();
    }

    private void LoadParentCanvas()
    {
        this.parentCanvas = transform.parent.GetComponentInParent<Canvas>();
    }

    protected override void SetVariableToDefault()
    {
        base.SetVariableToDefault();
        UpgradeChildLayerHigherThanParent();
    }

    private void UpgradeChildLayerHigherThanParent()
    {
        if (this.canvas == null) return;
        if (this.parentCanvas == null) return;
        this.canvas.sortingOrder = parentCanvas.sortingOrder + 1;
    }
}