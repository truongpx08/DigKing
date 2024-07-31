using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class TruongSafeArea : TruongMonoBehaviour
{
    private RectTransform panel;
    [Tooltip("These panel is not affected by safe area")]
    [SerializeField] private TruongSafeAreaExcluder[] excludedPanels;


    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadPanel();
        LoadExcludedPanels();
    }

    private void LoadExcludedPanels()
    {
        this.excludedPanels = GetComponentsInChildren<TruongSafeAreaExcluder>().ToArray();
    }

    private void LoadPanel()
    {
        this.panel = GetComponent<RectTransform>();
    }

    protected override void Start()
    {
        base.Start();
        ApplySafeArea();
    }

    private void ApplySafeArea()
    {
        var safeArea = Screen.safeArea;
        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;

        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;

        this.panel.anchorMin = anchorMin;
        this.panel.anchorMax = anchorMax;

        ExcludeSafeArea();
    }

    private void ExcludeSafeArea()
    {
        if (excludedPanels == null)
            return;
        foreach (var item in excludedPanels)
            item.Restore(this.panel.anchorMin, this.panel.anchorMax);
    }
}