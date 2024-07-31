using System.Collections;
using System.Collections.Generic;
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

    public Cell[] GetAdjacentCells()
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
}