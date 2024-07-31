using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class TruongId : TruongMonoBehaviour
{
    [SerializeField] private int id;
    public int Id => id;

    [Button]
    public void SetId(int value)
    {
        this.id = value;
    }
}