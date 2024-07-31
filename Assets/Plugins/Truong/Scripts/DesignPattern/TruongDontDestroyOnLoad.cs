using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruongDontDestroyOnLoad : TruongMonoBehaviour
{
    protected override void Awake()
    {
        base.Awake();
        SetDontDestroyOnLoadObj();
    }

    private void SetDontDestroyOnLoadObj()
    {
        DontDestroyOnLoad(this.gameObject);
        SetParentTransform();
    }

    private void SetParentTransform()
    {
        //Don't destroy on load only works on root objects so let's force this transform to be a root object:
        this.transform.parent = null;
    }
}