using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// These panel is not affected by safe area
/// </summary>
public class TruongSafeAreaExcluder : TruongMonoBehaviour
{
    private RectTransform panel;
    private Vector2 originAnchorMin;
    private Vector2 originAnchorMax;
    private float value;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadPanel();
    }

    protected override void SetVariableToDefault()
    {
        base.SetVariableToDefault();
        this.value = -10;
        this.originAnchorMin = panel.anchorMin;
        this.originAnchorMax = panel.anchorMax;
        this.panel.offsetMin = new Vector3(this.value, this.value);
        this.panel.offsetMax = new Vector3(-this.value, -this.value);
    }

    private void LoadPanel()
    {
        this.panel = GetComponent<RectTransform>();
    }

    public void Restore(Vector2 anchorMin, Vector2 anchorMax)
    {
        this.panel.anchorMin = new Vector2(this.originAnchorMin.x + (this.originAnchorMin.x - anchorMin.x),
            this.originAnchorMin.y + (this.originAnchorMin.y - anchorMin.y));
        this.panel.anchorMax = new Vector2(this.originAnchorMax.x + (this.originAnchorMax.x - anchorMax.x),
            this.originAnchorMax.y + (this.originAnchorMax.y - anchorMax.y));
    }
}