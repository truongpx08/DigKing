using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellReference : TruongMonoBehaviour
{
    [SerializeField] protected Cell cellRef;
    protected CellData DataRef => this.cellRef.DataHandler.Data;
    protected MapGenerator MapGenerator => Map.Instance.Generator;

    protected void LoadCellReference()
    {
        if (this.cellRef == null)
            this.cellRef = GetComponentInParent<Cell>();
    }
}