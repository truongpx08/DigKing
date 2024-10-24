using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CellData
{
    public int x;
    public int y;
    public Cell cellPosition2;
    public Cell cellPosition8;
    public Cell cellPosition4;
    public Cell cellPosition6;
    public Cell cellPosition1;
    public Cell cellPosition3;
    public Cell cellPosition7;
    public Cell cellPosition9;

    public Cell[] GetAdjacentCellsWithPosition2468()
    {
        // Define an array of neighboring cells  
        Cell[] neighbors =
        {
            cellPosition2,
            cellPosition8,
            cellPosition4,
            cellPosition6
        };

        return neighbors; // Return null if no unprocessed thick cell is found  
    }

    public Cell[] Get8AdjacentCells()
    {
        // Khai báo mảng cho các ô lân cận  
        var neighbors = new Cell[8];

        // Lấy 4 ô lân cận  
        var _4Neighbors = GetAdjacentCellsWithPosition2468();

        // Điền các ô chéo vào mảng neighbors  
        neighbors[0] = cellPosition1;
        neighbors[1] = cellPosition3;
        neighbors[2] = cellPosition7;
        neighbors[3] = cellPosition9;

        // Điền các ô lân cận còn lại vào mảng neighbors  
        Array.Copy(_4Neighbors, 0, neighbors, 4, 4);

        return neighbors; // Trả về mảng ô lân cận  
    }
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