using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class TruongUITextMeshPro : TruongUI
{
    [SerializeField] protected TextMeshProUGUI textTMP;
    public TextMeshProUGUI TextTMP => textTMP;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadTmp();
    }

    private void LoadTmp()
    {
        textTMP = GetComponentInChildren<TextMeshProUGUI>();
    }

    [Button]
    public virtual void AddText(string text)
    {
        this.textTMP.text = text;
    }

    [Button]
    public void ClearText()
    {
        this.textTMP.text = string.Empty;
    }
}