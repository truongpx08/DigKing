using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class TruongCanvasScaler : TruongMonoBehaviour
{
    private enum ScreenType
    {
        None,
        Landscape,
        Portrait,
    }

    [SerializeField] private CanvasScaler canvasScaler;

    [SerializeField] private ScreenType screenType;
    private float defaultWidth;
    private float defaultHeight;
    [Range(0, 1)]
    [SerializeField] private float matchForLongScreen;
    [Range(0, 1)]
    [SerializeField] private float matchForWideScreen;
    private float currentWidth;
    private float currentHeight;


    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadCanvasScaler();
    }

    protected override void SetVariableToDefault()
    {
        base.SetVariableToDefault();
        this.defaultWidth = this.canvasScaler.referenceResolution.x;
        this.defaultHeight = this.canvasScaler.referenceResolution.y;
        SetScreenType();
    }

    private void LoadCanvasScaler()
    {
        this.canvasScaler = GetComponent<CanvasScaler>();
    }

    protected override void Start()
    {
        UpdateMatch();
    }

    [Button]
    private void UpdateMatch()
    {
        this.currentWidth = Screen.width;
        this.currentHeight = Screen.height;
        float currentAspect = currentWidth / currentHeight;
        float defaultAspect = defaultWidth / defaultHeight;
        switch (screenType)
        {
            case ScreenType.None:
                break;
            case ScreenType.Landscape:
                canvasScaler.matchWidthOrHeight =
                    currentAspect < defaultAspect ? matchForWideScreen : matchForLongScreen;
                break;
            case ScreenType.Portrait:
                canvasScaler.matchWidthOrHeight =
                    currentAspect < defaultAspect ? matchForLongScreen : matchForWideScreen;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void SetScreenType()
    {
        this.screenType = Screen.width - Screen.height < 0 ? ScreenType.Portrait : ScreenType.Landscape;
    }
}