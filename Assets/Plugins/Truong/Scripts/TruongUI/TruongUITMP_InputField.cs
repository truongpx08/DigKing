using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class TruongUITMP_InputField : TruongUI
{
    [SerializeField] private TMP_InputField inputField;
    public TMP_InputField InputField => inputField;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadInputField();
    }


    private void LoadInputField()
    {
        this.inputField = GetComponentInChildren<TMP_InputField>();
    }

    public void SetTextInput(string value)
    {
        this.inputField.text = value;
    }

    public void ClearInputText()
    {
        SetTextInput(String.Empty);
    }

    public void Select()
    {
        this.inputField.ActivateInputField();
    }
}