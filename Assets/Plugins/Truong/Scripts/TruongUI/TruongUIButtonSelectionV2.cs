using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// When clicking the button, it will automatically unselect other buttons
/// Requires adding TruongUIListButtonSelection to parent
/// </summary>
public abstract class TruongUIButtonSelectionV2 : TruongUIButtonSelectionAbstract
{
    [SerializeField] private TruongUIListButtonSelection selectionButtonList;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadSelectionButtonList();
    }

    private void LoadSelectionButtonList()
    {
        this.selectionButtonList = GetComponentInParent<TruongUIListButtonSelection>();
    }

    protected override void OnClickButton()
    {
        Select();
        UnselectOtherButton();
    }

    private void UnselectOtherButton()
    {
        if (this.selectionButtonList == null) return;
        this.selectionButtonList.SelectionButtonList.ForEach(item =>
        {
            if (item == this) return;
            item.Deselect();
        });
    }
}