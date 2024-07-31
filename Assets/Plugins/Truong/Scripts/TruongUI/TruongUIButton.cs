using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class TruongUIButton : TruongUI
{
    [SerializeField] protected Button button;
    public Button Button => button;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadButton();
    }

    private void LoadButton()
    {
        button = GetComponentInChildren<Button>();
    }

    protected void CleanActionButton()
    {
        this.button.onClick.RemoveAllListeners();
    }

    public void AddActionToButton(UnityAction action)
    {
        this.button.onClick.AddListener(action);
    }
}