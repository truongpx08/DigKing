using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CellData
{
    public int x;
    public int y;
    public Cell upCell;
    public Cell downCell;
    public Cell leftCell;
    public Cell rightCell;
    public Cell upLeftCell;
    public Cell upRightCell;
    public Cell downLeftCell;
    public Cell downRightCell;
}

public class CellDataHandler : CellReference
{
    [SerializeField] private CellData data;
    public CellData Data => this.data;

    public void AddData(int x, int y)
    {
        this.data = new CellData
        {
            x = x,
            y = y,
        };
    }
}