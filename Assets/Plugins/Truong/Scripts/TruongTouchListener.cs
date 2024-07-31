using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruongTouchListener : MonoBehaviour
{
    private bool interactable;
    private Action actionOnClick;

    public void SetInteractable(bool value)
    {
        this.interactable = value;
    }

    public void OnMouseDown()
    {
        Debug.Log("Touch");
        if (!this.interactable) return;
        Action();
    }

    public void Action()
    {
        actionOnClick?.Invoke();
    }

    public void AddActionOnClick(Action onClick)
    {
        this.actionOnClick = onClick;
    }
}