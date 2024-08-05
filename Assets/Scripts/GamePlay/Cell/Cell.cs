using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class Cell : TruongMonoBehaviour
{
    [SerializeField] private CellData data;
    public CellData Data => this.data;
    [SerializeField] private CellInitializer initializer;
    public CellInitializer Initializer => this.initializer;
    [SerializeField] private SpriteRenderer model;
    [SerializeField] private bool isProcessed;
    public bool IsProcessed => this.isProcessed;
    public SpriteRenderer Model => this.model;
    [SerializeField] private CellStateMachine stateMachine;
    public CellStateMachine StateMachine => this.stateMachine;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadStateMachine();
        LoadData();
        LoadInitializer();
    }

    private void LoadStateMachine()
    {
        this.stateMachine = GetComponentInChildren<CellStateMachine>();
    }

    private void LoadData()
    {
        this.data = GetComponentInChildren<CellData>();
    }

    private void LoadInitializer()
    {
        this.initializer = GetComponentInChildren<CellInitializer>();
    }

    public Cell FindNextUnprocessedThickCell()
    {
        // Define an array of neighboring cells  
        Cell[] neighbors =
        {
            this.data.ModelData.upCell,
            this.data.ModelData.downCell,
            this.data.ModelData.leftCell,
            this.data.ModelData.rightCell
        };

        foreach (var cell in neighbors)
        {
            if (IsUnprocessedThickCell(cell))
            {
                return cell; // Return the first found unprocessed thick cell  
            }
        }

        Debug.Log($"No unprocessed thick cell found for x:{this.data.ModelData.x} y:{this.data.ModelData.y}.");
        return null; // Return null if no unprocessed thick cell is found  
    }

    public Cell[] Get4AdjacentCells()
    {
        // Define an array of neighboring cells  
        Cell[] neighbors =
        {
            this.data.ModelData.upCell,
            this.data.ModelData.downCell,
            this.data.ModelData.leftCell,
            this.data.ModelData.rightCell
        };

        return neighbors; // Return null if no unprocessed thick cell is found  
    }

    public Cell[] Get8AdjacentCells()
    {
        // Khai báo mảng cho các ô lân cận  
        var neighbors = new Cell[8];

        // Lấy 4 ô lân cận  
        var _4Neighbors = Get4AdjacentCells();

        // Điền các ô chéo vào mảng neighbors  
        neighbors[0] = this.data.ModelData.upLeftCell;
        neighbors[1] = this.data.ModelData.upRightCell;
        neighbors[2] = this.data.ModelData.downLeftCell;
        neighbors[3] = this.data.ModelData.downRightCell;

        // Điền các ô lân cận còn lại vào mảng neighbors  
        Array.Copy(_4Neighbors, 0, neighbors, 4, 4);

        return neighbors; // Trả về mảng ô lân cận  
    }

    private bool IsUnprocessedThickCell(Cell cell)
    {
        return cell != null &&
               !cell.IsProcessed &&
               cell.data.ModelData.type == ECellType.Thick;
    }

    public void SetIsProcessed(bool processed)
    {
        this.isProcessed = processed;
    }

    [Button]
    public bool IsBreakableBorder()
    {
        var cells = Get8AdjacentCells();
        return cells.All(c => c == null || c.stateMachine.CurrentState != ECellState.Think);
    }

    public bool IsThisCell(int x, int y)
    {
        return Data.ModelData.x == x && Data.ModelData.y == y;
    }
}