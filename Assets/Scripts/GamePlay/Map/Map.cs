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
        return GetCellsWithType(ECellType.Thin);
    }

    [Button]
    public List<Cell> GetThickCell()
    {
        return GetCellsWithType(ECellType.Thick);
    }

    private List<Cell> GetCellsWithType(ECellType type)
    {
        List<Cell> cells = new List<Cell>();
        this.generator.CellList.ForEach(cell =>
        {
            if (cell.DataHandler.Data.type == type)
                cells.Add(cell);
        });
        return cells;
    }

    public Cell FindFirstUnprocessedThickCell()
    {
        // Find the first unprocessed thick cell  
        return generator.CellList.Find(item =>
            item.DataHandler.Data.type == ECellType.Thick && !item.IsProcessed);
    }

    public Cell GetRandomThinCell()
    {
        var list = GetThinCell();
        return list[Random.Range(0, list.Count)];
    }
}