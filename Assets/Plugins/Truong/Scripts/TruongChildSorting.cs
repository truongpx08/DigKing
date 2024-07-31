using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class TruongChildSorting : MonoBehaviour
{
    [Button]
    protected void Sort()
    {
        List<Transform> childTransforms = new List<Transform>();
        // Get all child transforms in the hierarchy
        for (int i = 0; i < transform.childCount; i++)
        {
            childTransforms.Add(transform.GetChild(i));
        }

        // Sort the child transforms by name
        var sorted = childTransforms.OrderBy(element => element.name).ToList();

        // Reset the positions of the child transforms according to the sorted order
        for (int i = 0; i < sorted.Count; i++)
        {
            sorted[i].SetSiblingIndex(i);
        }
    }
}