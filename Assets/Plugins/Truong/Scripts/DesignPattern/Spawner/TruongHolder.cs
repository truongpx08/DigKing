using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// Contains Items
/// </summary>
public class TruongHolder : TruongGameObject
{
    [SerializeField] protected List<Transform> items;
    public List<Transform> Items => items;

    protected override void SetVariableToDefault()
    {
        base.SetVariableToDefault();
        items = new List<Transform>();
    }

    [Button]
    public void AddItem(Transform item)
    {
        item.transform.SetParent(this.transform);
        items.Add(item);
    }

    [Button]
    public Transform GetAvailableItemForReuse(Transform fromPrefab)
    {
        if (fromPrefab == null) return null;
        foreach (var item in items)
        {
            if (item == null) continue;
            if (item.GetComponent<TruongId>().Id != fromPrefab.GetInstanceID()) continue;
            if (item.gameObject.activeSelf) continue;

            EnableGo(item);
            return item;
        }

        return null;
    }

    public Transform GetDefaultOrFirstItem()
    {
        if (this.transform.childCount == 0) return null;
        return this.transform.GetChild(0);
    }
}