using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellReference : TruongMonoBehaviour
{
    [SerializeField] protected Cell cell;
    protected CellData DataRef => this.cell.DataHandler.Data;
    protected MapGenerator MapGenerator => Map.Instance.Generator;

    protected void LoadCellReference()
    {
        if (this.cell == null)
            this.cell = GetComponentInParent<Cell>();
    }
}