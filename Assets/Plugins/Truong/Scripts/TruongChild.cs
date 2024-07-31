using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class TruongChild : TruongMonoBehaviour
{
    protected GameObject CreateChild(string objName)
    {
        if (string.IsNullOrEmpty(objName)) return null;
        if (transform.Find(objName) != null) return null;
        if (this.gameObject.scene != SceneManager.GetActiveScene()) return null;

        GameObject newGo = null;
        newGo = new GameObject
        {
            name = objName,
            transform =
            {
                parent = this.transform
            }
        };
        ResetTransformObj(newGo);
        return newGo.gameObject;
    }

    protected virtual void ResetTransformObj(GameObject obj)
    {
        ResetTransformObj(obj.transform);
    }

    protected virtual void ResetTransformObj(Transform obj)
    {
        obj.localPosition = Vector3.zero;
        obj.localRotation = Quaternion.identity;
        obj.localScale = Vector3.one;
    }
}