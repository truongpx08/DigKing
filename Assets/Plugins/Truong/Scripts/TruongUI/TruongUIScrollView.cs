using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

public class TruongUIScrollView : TruongUI
{
    [SerializeField] protected List<TruongMonoBehaviour> itemList;
    public List<TruongMonoBehaviour> ItemList => itemList;
    [SerializeField] private List<TruongMonoBehaviour> oldItemList;
    [SerializeField] protected Transform content;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadContent();
    }

    private void LoadContent()
    {
        this.content = GetComponentInChildrenWithName(TruongConstant.CONTENT).transform;
    }

    protected TruongMonoBehaviour GetOldItem(int siblingIndex)
    {
        if (siblingIndex > oldItemList.Count - 1) return null;

        var item = oldItemList.Find(item => item.transform.GetSiblingIndex() == siblingIndex);
        if (item == null) return null;
        EnableGo(item.gameObject);
        oldItemList.Remove(item);
        return item;
    }


    [Button]
    protected void DisableOldItems()
    {
        SaveItems();
        this.itemList.ForEach(item => { DisableGo(item.gameObject); });
        this.itemList.Clear();
    }

    private void SaveItems()
    {
        this.oldItemList.AddRange(this.itemList);
    }
}