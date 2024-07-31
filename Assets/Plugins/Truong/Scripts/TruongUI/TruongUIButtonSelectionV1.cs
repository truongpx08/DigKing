using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// When clicking on the button, it will automatically change between select and deselect
/// </summary>
public class TruongUIButtonSelectionV1 : TruongUIButtonSelectionAbstract
{
    protected override void OnClickButton()
    {
        if (this.isSelected)
        {
            Deselect();
            return;
        }

        Select();
    }
}