using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class Cell : TruongMonoBehaviour
{
    [SerializeField] private CellStateMachine stateMachine;
    public CellStateMachine StateMachine => this.stateMachine;
    [SerializeField] private CellDataHandler dataHandler;
    public CellDataHandler DataHandler => this.dataHandler;
    [SerializeField] private SpriteRenderer model;
    [SerializeField] private bool isProcessed;
    public bool IsProcessed => this.isProcessed;
    public SpriteRenderer Model => this.model;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadStateMachine();
        LoadData();
    }

    private void LoadStateMachine()
    {
        this.stateMachine = GetComponentInChildren<CellStateMachine>();
    }

    private void LoadData()
    {
        this.dataHandler = GetComponentInChildren<CellDataHandler>();
    }

    public Cell FindNextUnprocessedThickCell()
    {
        // Define an array of neighboring cells  
        Cell[] neighbors =
        {
            this.dataHandler.Data.cellPosition2,
            this.dataHandler.Data.cellPosition8,
            this.dataHandler.Data.cellPosition4,
            this.dataHandler.Data.cellPosition6
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

    private bool IsUnprocessedThickCell(Cell cell)
    {
        return cell != null &&
               !cell.IsProcessed &&
               cell.stateMachine.CurrentState == ECellState.Thick;
    }

    public void SetIsProcessed(bool processed)
    {
        this.isProcessed = processed;
    }

    [Button]
    public bool IsBreakableBorder()
    {
        var cells = this.dataHandler.Data.Get8AdjacentCells();
        return cells.All(c => c == null || c.stateMachine.CurrentState != ECellState.Thick);
    }

    public bool IsThisCell(int x, int y)
    {
        return DataHandler.Data.x == x && DataHandler.Data.y == y;
    }

    public EDirectionType GetDirection(Cell adjacentCell)
    {
        var data = this.dataHandler.Data;
        if (data.cellPosition2 == adjacentCell) return EDirectionType.Position2;
        if (data.cellPosition8 == adjacentCell) return EDirectionType.Position8;
        if (data.cellPosition4 == adjacentCell) return EDirectionType.Position4;
        if (data.cellPosition6 == adjacentCell) return EDirectionType.Position6;

        if (data.cellPosition1 == adjacentCell) return EDirectionType.Position1;
        if (data.cellPosition3 == adjacentCell) return EDirectionType.Position3;
        if (data.cellPosition7 == adjacentCell) return EDirectionType.Position7;
        if (data.cellPosition9 == adjacentCell) return EDirectionType.Position9;
        return EDirectionType.None;
    }

    public Cell GetCellWithDirection(EDirectionType oppositeDirection)
    {
        var data = this.dataHandler.Data;
        return oppositeDirection switch
        {
            EDirectionType.Position2 => data.cellPosition2,
            EDirectionType.Position8 => data.cellPosition8,
            EDirectionType.Position4 => data.cellPosition4,
            EDirectionType.Position6 => data.cellPosition6,

            EDirectionType.Position1 => data.cellPosition1,
            EDirectionType.Position3 => data.cellPosition3,
            EDirectionType.Position7 => data.cellPosition7,
            EDirectionType.Position9 => data.cellPosition9,
            _ => throw new ArgumentOutOfRangeException(nameof(oppositeDirection), oppositeDirection, null)
        };
    }

    public EDirectionType GetRandom2468Direction()
    {
        EDirectionType[] list =
        {
            EDirectionType.Position2,
            EDirectionType.Position8,
            EDirectionType.Position4,
            EDirectionType.Position6,
        };
        return list[Random.Range(0, list.Length)];
    }

    public EDirectionType GetRandom1357Direction()
    {
        EDirectionType[] list =
        {
            EDirectionType.Position1,
            EDirectionType.Position3,
            EDirectionType.Position7,
            EDirectionType.Position9,
        };
        return list[Random.Range(0, list.Length)];
    }
}