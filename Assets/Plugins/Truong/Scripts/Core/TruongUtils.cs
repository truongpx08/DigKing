using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

public class TruongUtils : MonoBehaviour
{
    #region Get Component

    protected T GetComponentInChildrenOfFather<T>()
    {
        return this.transform.parent.GetComponentInChildren<T>();
    }

    protected bool HasComponent<T>()
    {
        return GetComponent<T>() != null;
    }


    protected T GetComponentInChildrenWithName<T>(string goName)
    {
        return GetComponentInChildrenWithName(goName).GetComponent<T>();
    }

    protected T GetComponentInChildrenMayBeOff<T>()
    {
        var list = GetAllChildrenWithAllGenerations();
        return list.Find(item => item.GetComponent<T>() != null).GetComponent<T>();
    }

    protected List<T> GetComponentsInChildrenMayBeOff<T>()
    {
        var list = GetAllChildrenWithAllGenerations();
        List<T> result = new List<T>();
        list.ForEach(item =>
        {
            if (!IsObjNull(item.GetComponent<T>()))
                result.Add(item.GetComponent<T>());
        });
        return result;
    }

    protected GameObject GetComponentInChildrenWithName(string goName)
    {
        var list = GetAllChildrenWithAllGenerations();
        return list.Find(item => item.name == goName)?.gameObject;
    }

    protected T GetComponentInBro<T>()
    {
        var list = GetBro();
        return list.Find(item => item.GetComponent<T>() != null).GetComponent<T>();
    }

    private List<Transform> GetBro()
    {
        var list = GetAllChildrenOfParent(this.transform.parent);
        list.Remove(this.transform);
        return list;
    }

    protected List<Transform> GetAllChildrenWithAllGenerations()
    {
        var value = new List<Transform>();
        LoopGetChild(this.transform, value);
        return value;
    }

    private void LoopGetChild(Transform parent, List<Transform> list)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            list.Add(child);
            if (child.childCount > 0)
                LoopGetChild(child, list);
        }
    }

    protected List<Transform> GetAllChildrenWithGeneration1()
    {
        return GetAllChildrenOfParent(this.transform);
    }

    protected List<Transform> GetAllChildrenOfParent(Transform parent)
    {
        if (parent == null) return null;
        var value = new List<Transform>();
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            value.Add(child);
        }

        return value;
    }

    #endregion

    #region Handle Resources

    protected T LoadPrefabInResources<T>() where T : Object
    {
        var list = Resources.LoadAll<T>(TruongConstant.PREFABS).ToList();
        return list.FirstOrDefault();
    }

    protected List<Sprite> LoadSpriteListInResources(string folderName)
    {
        return Resources.LoadAll<Sprite>(TruongPath.GetSpriteInResourcePath(folderName)).ToList();
    }

    #endregion

    #region Call functions

    /// <summary>
    /// If the function is called on a copy that has been destroyed, it is ignored
    /// </summary>
    /// <param name="action"></param>
    protected void CallWhileExists(Action action)
    {
        if (this == null) return;
        action?.Invoke();
    }

    protected void CallWithDelay(float delay, Action action)
    {
        StartCoroutine(IECallWithDelay(delay, action));
    }

    protected IEnumerator IECallWithDelay(float delay, Action action)
    {
        yield return new WaitForSeconds(delay);
        action?.Invoke();
    }

    protected void CallAfterOneFrame(Action action)
    {
        StartCoroutine(IECall());

        IEnumerator IECall()
        {
            yield return null;
            action?.Invoke();
        }
    }

    #endregion

    #region Other

    public string GetName()
    {
        return this.name;
    }

    protected bool IsObjNull(object obj)
    {
        return obj == null;
    }

    protected Vector2 GetInputMousePosition()
    {
        return Camera.main!.ScreenToWorldPoint(Input.mousePosition);
    }

    protected void DeleteDuplicate<T>()
    {
        //To prevent Unity from creating multiple copies of the same component in inspector at runtime
        Component c = gameObject.GetComponent<T>() as Component;

        if (c != null)
        {
            Destroy(c);
        }
    }

    #endregion
}