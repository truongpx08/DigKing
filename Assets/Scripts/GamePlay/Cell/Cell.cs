using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class Cell : TruongMonoBehaviour
{
    [SerializeField] private CellDataHandler dataHandler;
    public CellDataHandler DataHandler => this.dataHandler;
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
        this.dataHandler = GetComponentInChildren<CellDataHandler>();
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
            this.dataHandler.Data.upCell,
            this.dataHandler.Data.downCell,
            this.dataHandler.Data.leftCell,
            this.dataHandler.Data.rightCell
        };

        foreach (var cell in neighbors)
        {
            if (IsUnprocessedThickCell(cell))
            {
                return cell; // Return the first found unprocessed thick cell  
            }
        }

        Debug.Log($"No unprocessed thick cell found for x:{this.dataHandler.Data.x} y:{this.dataHandler.Data.y}.");
        return null; // Return null if no unprocessed thick cell is found  
    }

    public Cell[] Get4AdjacentCells()
    {
        // Define an array of neighboring cells  
        Cell[] neighbors =
        {
            this.dataHandler.Data.upCell,
            this.dataHandler.Data.downCell,
            this.dataHandler.Data.leftCell,
            this.dataHandler.Data.rightCell
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
        neighbors[0] = this.dataHandler.Data.upLeftCell;
        neighbors[1] = this.dataHandler.Data.upRightCell;
        neighbors[2] = this.dataHandler.Data.downLeftCell;
        neighbors[3] = this.dataHandler.Data.downRightCell;

        // Điền các ô lân cận còn lại vào mảng neighbors  
        Array.Copy(_4Neighbors, 0, neighbors, 4, 4);

        return neighbors; // Trả về mảng ô lân cận  
    }

    private bool IsUnprocessedThickCell(Cell cell)
    {
        return cell != null &&
               !cell.IsProcessed &&
               cell.dataHandler.Data.type == ECellType.Thick;
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
        return DataHandler.Data.x == x && DataHandler.Data.y == y;
    }

    public EDirectionType GetDirection(Cell adjacentCell)
    {
        var data = this.dataHandler.Data;
        if (data.upCell == adjacentCell) return EDirectionType.Up;
        if (data.downCell == adjacentCell) return EDirectionType.Down;
        if (data.leftCell == adjacentCell) return EDirectionType.Left;
        if (data.rightCell == adjacentCell) return EDirectionType.Right;
        return EDirectionType.Down;
    }

    public Cell GetCellWithDirection(EDirectionType oppositeDirection)
    {
        var data = this.dataHandler.Data;
        return oppositeDirection switch
        {
            EDirectionType.Up => data.upCell,
            EDirectionType.Down => data.downCell,
            EDirectionType.Left => data.leftCell,
            EDirectionType.Right => data.rightCell,
            _ => throw new ArgumentOutOfRangeException(nameof(oppositeDirection), oppositeDirection, null)
        };
    }
}