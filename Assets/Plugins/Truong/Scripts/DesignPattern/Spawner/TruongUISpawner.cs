using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TruongUISpawner : TruongSpawner
{
    protected override void ResetTransformObj(Transform obj)
    {
        var rectTransform = GetRectTransform(obj);
        rectTransform.localPosition = Vector3.zero;
        rectTransform.localRotation = Quaternion.identity;
        rectTransform.localScale = Vector3.one;
    }

    private Transform GetRectTransform(Transform obj)
    {
        var rectTransform = obj.GetComponent<RectTransform>();
        return rectTransform ? rectTransform : obj.gameObject.AddComponent<RectTransform>();
    }
}