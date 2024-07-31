using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableParentHandler : TruongUIButton
{
    protected override void Start()
    {
        base.Start();
        AddActionToButton(OnClickButton);
    }

    private void OnClickButton()
    {
        var parentT = this.transform.parent;
        if (parentT == null) return;
        DisableGo(parentT);
    }
}