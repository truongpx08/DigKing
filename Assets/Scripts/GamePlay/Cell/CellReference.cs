using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellReference : TruongMonoBehaviour
{
    [SerializeField] protected Cell cellRef;
    protected CellData DataRef => this.cellRef.DataHandler.Data;
    protected MapGenerator MapGenerator => Map.Instance.Generator;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadCell();
    }

    private void LoadCell()
    {
        this.cellRef = GetComponentInParent<Cell>();
    }
}