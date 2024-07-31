using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class TruongUITouchAbstract : TruongMonoBehaviour, IPointerDownHandler,
    IPointerUpHandler //required interface when using the OnPointerExit method.
{
    private Action onPointerDown;
    private Action onPointerUp;
    [SerializeField] private bool isShowLog;

    protected override void SetVariableToDefault()
    {
        base.SetVariableToDefault();
        isShowLog = false;
    }

    protected override void Start()
    {
        base.Start();
        AddAction();
    }

    protected abstract void AddAction();

    protected void AddActionToPointerUp(Action action)
    {
        this.onPointerUp += action;
    }

    protected void AddActionToPointerDown(Action action)
    {
        this.onPointerDown += action;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        onPointerDown?.Invoke();
        ShowLogOnPointerDown();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        onPointerUp?.Invoke();
        ShowLogOnPointerUp();
    }

    private void ShowLogOnPointerDown()
    {
        if (!this.isShowLog) return;
        Debug.Log("Pointer Down");
    }

    private void ShowLogOnPointerUp()
    {
        if (!this.isShowLog) return;
        Debug.Log("Pointer Up");
    }
}