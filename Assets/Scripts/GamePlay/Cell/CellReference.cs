using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellReference : TruongMonoBehaviour
{
    [SerializeField] protected Cell cell;
    protected CellModelData Data => this.cell.Data.ModelData;
    protected MapGenerator MapGenerator => Map.Instance.Generator;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadCell();
    }

    private void LoadCell()
    {
        this.cell = GetComponentInParent<Cell>();
    }
}