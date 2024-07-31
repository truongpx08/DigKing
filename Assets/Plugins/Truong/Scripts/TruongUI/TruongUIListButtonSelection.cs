using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruongUIListButtonSelection : TruongUI
{
    [SerializeField] protected List<TruongUIButtonSelectionV2> selectionButtonList;
    public List<TruongUIButtonSelectionV2> SelectionButtonList => selectionButtonList;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadSelectionButtonList();
    }

    private void LoadSelectionButtonList()
    {
        this.selectionButtonList.Clear();
        GetAllChildrenWithGeneration1().ForEach(item =>
        {
            var component = item.GetComponent<TruongUIButtonSelectionV2>();
            if (component != null)
                this.selectionButtonList.Add(component);
        });
    }
}