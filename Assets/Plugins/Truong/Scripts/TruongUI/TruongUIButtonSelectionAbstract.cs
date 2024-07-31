using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class TruongUIButtonSelectionAbstract : TruongUIButton
{
    [SerializeField] private Transform offImage;
    [SerializeField] private Transform onImage;
    [SerializeField] protected bool isSelected;
    public bool IsSelected => isSelected;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadOffImage();
        LoadOnImage();
    }

    protected override void Start()
    {
        base.Start();
        Deselect();
        AddActionToButton(OnClickButton);
    }

    protected abstract void OnClickButton();

    private void LoadOnImage()
    {
        this.onImage = transform.Find(TruongConstant.OnImage);
    }

    private void LoadOffImage()
    {
        this.offImage = transform.Find(TruongConstant.OffImage);
    }


    [Button]
    public virtual void Select()
    {
        SetIsSelected(true);
        EnableGo(this.onImage);
        DisableGo(this.offImage);
    }


    [Button]
    public virtual void Deselect()
    {
        SetIsSelected(false);
        EnableGo(this.offImage);
        DisableGo(this.onImage);
    }

    private void SetIsSelected(bool value)
    {
        this.isSelected = value;
        OnSelectionChanged();
    }

    protected virtual void OnSelectionChanged()
    {
        // For Override
    }
}