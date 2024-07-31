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

    [Button]
    public void GenerateMap()
    {
        RemoveChildren();
        this.cellList.Clear();
        var count = 0;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var cell = Instantiate(cellPrefab, this.transform);
                cell.Data.AddData(count, x, y);
                cell.Initializer.Initialize();

                this.cellList.Add(cell);
                count++;
            }
        }

        this.cellList.ForEach(cell => { cell.Initializer.AddAdjacentCells(); });
    }


    private void RemoveChildren()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    public Cell FindCell(int x, int y)
    {
        return this.cellList.Find(cell => cell.Data.ModelData.x == x && cell.Data.ModelData.y == y);
    }
}