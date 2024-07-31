using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TruongUIButtonHold : TruongUIButton, IPointerDownHandler, IPointerUpHandler
{
    private Coroutine coroutine;
    private Action _action;

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");
        TryStopCoroutine();
        this.coroutine = StartCoroutine(IEDelayCall());
    }

    private void TryStopCoroutine()
    {
        if (coroutine != null) StopCoroutine(coroutine);
    }

    private IEnumerator IEDelayCall()
    {
        yield return new WaitForSeconds(0.5f);
        while (true)
        {
            _action?.Invoke();
            yield return new WaitForSeconds(0.1f);
        }
    }


    public void OnPointerUp(PointerEventData eventData)
    {
        TryStopCoroutine();
    }

    public void ListenHoldEvent(Action action)
    {
        this._action = action;
    }
}