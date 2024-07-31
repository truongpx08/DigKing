using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TruongPrefabs : TruongMonoBehaviour
{
    [SerializeField] private string prefabName;
    [SerializeField] protected string prefabPath;
    [SerializeField] protected List<Transform> items;
    public List<Transform> Items => items;

    public void LoadPrefabWithName(string prefabNameValue)
    {
        this.prefabName = prefabNameValue;
        LoadPrefabWithPath(TruongPath.GetPrefabInResourcePath(this.prefabName));
    }

    public void LoadPrefabWithPath(string prefabPathValue)
    {
        this.prefabPath = prefabPathValue;
        LoadPrefabInResource();
    }

    private void LoadPrefabInResource()
    {
        items = Resources.LoadAll<Transform>(prefabPath).ToList();
        CheckNull(items);
    }

    public Transform GetPrefabWithName(string value)
    {
        return items.Find(p => p.name == value);
    }

    public Transform GetDefaultPrefab()
    {
        return items[0];
    }

    private void CheckNull(object obj)
    {
        if (obj != null) return;
        Debug.LogError("object is null");
    }
}