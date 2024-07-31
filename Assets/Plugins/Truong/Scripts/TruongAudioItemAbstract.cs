using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class TruongAudioItemAbstract : TruongMonoBehaviour
{
    [SerializeField] private bool isActive;
    [SerializeField] private Transform slide;
    [SerializeField] private Transform onPoint;
    [SerializeField] private Transform offPoint;
    [SerializeField] private float duration;

    [SerializeField] private TruongUIButtonHandler button;
    public TruongUIButtonHandler Button => button;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadSlide();
        LoadOnPoint();
        LoadOffPoint();
        LoadCloseButton();
    }

    private void LoadCloseButton()
    {
        button = GetComponentInChildren<TruongUIButtonHandler>();
    }

    protected override void SetVariableToDefault()
    {
        base.SetVariableToDefault();
        duration = 0.25f;
    }

    private void LoadOffPoint()
    {
        offPoint = transform.Find("OffPoint");
    }

    private void LoadOnPoint()
    {
        onPoint = transform.Find("OnPoint");
    }

    private void LoadSlide()
    {
        slide = transform.Find("Slide");
    }

    protected override void Start()
    {
        base.Start();
        SetActiveOnEnable();
        Button.AddActionOnClick(OnClickButton);
    }

    protected abstract void SetActiveOnEnable();

    protected virtual void SetActiveOnEnable(bool value)
    {
        this.isActive = value;
        if (this.isActive)
            TurnOn();
        else
            TurnOff();
    }

    public void OnClickButton()
    {
        Switch();
    }


    private void Switch()
    {
        isActive = !isActive;
        OnValueChange(isActive);
        if (isActive)
            TurnOn(duration);
        else
            TurnOff(duration);
    }

    protected abstract void OnValueChange(bool active);

    [Button]
    private void TurnOn(float time)
    {
        slide.transform.DOMove(offPoint.position, time);
    }

    [Button]
    private void TurnOff(float time)
    {
        slide.transform.DOMove(onPoint.position, time);
    }

    private void TurnOn()
    {
        slide.transform.position = offPoint.position;
    }

    [Button]
    private void TurnOff()
    {
        slide.transform.position = onPoint.position;
    }
}