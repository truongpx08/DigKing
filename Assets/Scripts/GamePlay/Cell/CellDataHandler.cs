using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ECellType
{
    Thick,
    Thin
}

[System.Serializable]
public class CellData
{
    public int id;
    public ECellType type;
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

    public void AddData(int id, int x, int y)
    {
        this.data = new CellData
        {
            id = id,
            x = x,
            y = y,
            type = AddType(x, y)
        };
    }

    private ECellType AddType(int x, int y)
    {
        if (x == 0)
            return ECellType.Thin;
        if (x == MapGenerator.Width - 1)
            return ECellType.Thin;
        if (y == 0)
            return ECellType.Thin;
        if (y == MapGenerator.Height - 1)
            return ECellType.Thin;
        return ECellType.Thick;
    }
}