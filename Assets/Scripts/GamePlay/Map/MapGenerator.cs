using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class MapGenerator : TruongMonoBehaviour
{
    [SerializeField] private List<Cell> cellList;
    public List<Cell> CellList => this.cellList;

    [SerializeField] private Cell cellPrefab;
    [SerializeField] private int width;
    public int Width => width;
    [SerializeField] private int height;
    public int Height => height;
    [SerializeField] private float space;
    public float Space => space;

    public void Initial()
    {
        if (this.cellList.Count == 0)
            GenerateMap();
        else
            ResetMap();
    }

    private void ResetMap()
    {
        foreach (var cell in this.cellList)
        {
            cell.StateMachine.ChangeState(ECellState.Initial);
        }
    }

    [Button]
    private void GenerateMap()
    {
        // Clear existing cell list and initialize the capacity  
        this.cellList.Clear();
        this.cellList.Capacity = width * height;

        // Initialize the cells  
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var cell = Instantiate(cellPrefab, this.transform);
                cell.DataHandler.AddData(x, y);
                cell.StateMachine.ChangeState(ECellState.Initial);
                this.cellList.Add(cell);
            }
        }

        foreach (var cell in this.cellList)
        {
            cell.StateMachine.InitialState.AddAdjacentCells();
        }
    }
}