using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Map : TruongSingleton<Map>
{
    [SerializeField] private MapGenerator generator;
    public MapGenerator Generator => this.generator;
    [SerializeField] private MapCollapse collapse;
    public MapCollapse Collapse => this.collapse;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadCollapse();
        LoadGenerator();
    }

    private void LoadCollapse()
    {
        this.collapse = GetComponentInChildren<MapCollapse>();
    }

    private void LoadGenerator()
    {
        this.generator = GetComponentInChildren<MapGenerator>();
    }

    [Button]
    public List<Cell> GetThinCell()
    {
        return GetCellsWithType(ECellState.Thin);
    }

    [Button]
    public List<Cell> GetThickCell()
    {
        return GetCellsWithType(ECellState.Thick);
    }

    private List<Cell> GetCellsWithType(ECellState type)
    {
        List<Cell> cells = new List<Cell>();
        this.generator.CellList.ForEach(cell =>
        {
            if (cell.StateMachine.CurrentState == type)
                cells.Add(cell);
        });
        return cells;
    }

    public Cell FindFirstUnprocessedThickCell()
    {
        // Find the first unprocessed thick cell  
        return generator.CellList.Find(item =>
            item.StateMachine.CurrentState == ECellState.Thick && !item.IsProcessed);
    }

    public Cell GetRandomThinCellWithoutCharacter()
    {
        var cellList = GetThinCell();
        //Remove the character from the list
        var playerCell = Player.Instance.DataHandler.Data.currentCell;
        if (playerCell != null)
            cellList.Remove(Player.Instance.DataHandler.Data.currentCell);
        if (EnemiesSpawner.Instance.EnemyList.Count > 0)
        {
            EnemiesSpawner.Instance.EnemyList.ForEach(item =>
            {
                var enemyCell = item.DataHandler.Data.currentCell;
                if (enemyCell != null)
                    cellList.Remove(enemyCell);
            });
        }

        return cellList[Random.Range(0, cellList.Count)];
    }
}