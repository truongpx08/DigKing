using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TruongUIButtonHandler : TruongUIButton
{
    [SerializeField] private bool playSfxOnClick;
    public bool PlaySfxOnClick => playSfxOnClick;
    [HideInInspector] public bool isClipName;
    [HideInInspector] public bool isClipPath;
    [HideInInspector] public string clipName;
    [HideInInspector] public string clipPath;

    protected override void Awake()
    {
        base.Awake();
        AddActionOnClick(PlaySfx);
    }

    private void PlaySfx()
    {
        if (!playSfxOnClick) return;
        if (isClipPath)
            TruongCommon.TruongSfx.PlayWithAddressable(clipPath);
        if (isClipName)
            TruongCommon.TruongSfx.Play(clipName);
    }


    public void AddActionOnClick(UnityAction action)
    {
        if (!Button) return;
        Button.onClick.AddListener(action);
    }

    public void SetInteractable(bool value)
    {
        this.Button.interactable = value;
    }

    public void RemoveAllActionOnClick()
    {
        if (!Button) return;
        Button.onClick.RemoveAllListeners();
    }
}