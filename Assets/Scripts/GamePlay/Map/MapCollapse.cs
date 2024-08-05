using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

public class MapCollapse : TruongMonoBehaviour
{
    [SerializeField] private List<Cell> cellList = new();
    [ShowInInspector]
    private Dictionary<int, List<Cell>> cellDictionary = new();
    [ShowInInspector]
    private Dictionary<int, List<Cell>> borderDictionary = new();
    [ShowInInspector]
    private Dictionary<int, List<Cell>> nextCellOfBorderDictionary = new();
    [ShowInInspector]
    private Dictionary<int, List<Cell>> dicCheck = new();
    [ShowInInspector]
    private Dictionary<int, List<int>> mergeDictionary = new();
    [ShowInInspector]
    private Dictionary<int, List<int>> resultListIndexDictionary = new();
    [ShowInInspector]
    private Dictionary<int, List<Cell>> resultListCellDictionary = new();
    [SerializeField] private int maxAreaKey;

    [Button]
    public void Collapse()
    {
        Map.Instance.Generator.CellList.ForEach(cell => cell.SetIsProcessed(false));
        cellDictionary.Clear();
        ProcessOneList();
    }

    private void ProcessOneList()
    {
        cellList.Clear();

        Cell firstCell = Map.Instance.FindFirstUnprocessedThickCell();

        if (firstCell != null)
        {
            ProcessCell(firstCell);
        }
        else
        {
            Debug.Log("No unprocessed thick cells left.");
            SetBorderDictionary();
            SetNextCellOfBorderDictionary();
            MergeCell();
            CalculateResultListIndex();
            CalculateResultListCell();
            DestroySmallerAreas();
            BreakRemainingThinCells();
        }
    }

    private void BreakRemainingThinCells()
    {
        var unbreakableKeys = resultListIndexDictionary[maxAreaKey].ToHashSet();

        foreach (var (key, cells) in this.nextCellOfBorderDictionary)
        {
            // Nếu key đã có trong danh sách không thể phá hủy, bỏ qua  
            if (unbreakableKeys.Contains(key)) continue;

            foreach (var cell in cells)
            {
                // Kiểm tra kiểu của ô trước khi gọi phương thức  
                if (cell.Data.ModelData.type != ECellType.Thin) continue;

                // Kiểm tra xem ô có phải là biên có thể phá hủy không  
                if (cell.IsBreakableBorder())
                {
                    cell.StateMachine.ChangeState(ECellState.Disabled);
                }
            }
        }
    }

    private void DestroySmallerAreas()
    {
        // Track the maximum count and the associated key  
        int maxCount = -1;
        this.maxAreaKey = -1;

        // Find the key with the maximum area count  
        foreach (var item in this.resultListCellDictionary)
        {
            if (item.Value.Count > maxCount)
            {
                maxCount = item.Value.Count;
                maxAreaKey = item.Key;
            }
        }

        // If no max key found, exit early  
        if (maxAreaKey == -1) return;

        // Disable all areas except the one with the maximum count  
        foreach (var item in this.resultListCellDictionary)
        {
            if (item.Key != maxAreaKey)
            {
                // Disable the cells in the smaller areas  
                foreach (var cell in item.Value)
                {
                    cell.StateMachine.ChangeState(ECellState.Disabled);
                }
            }
        }
    }

    private void CalculateResultListCell()
    {
        // Clear the existing result list cell dictionary  
        this.resultListCellDictionary.Clear();

        // Use a foreach loop for better readability and performance  
        foreach (var itemDictionary in resultListIndexDictionary)
        {
            var indexList = itemDictionary.Value;
            var list = new List<Cell>();

            // Loop through the indexes and accumulate cells  
            foreach (var index in indexList)
            {
                // Try to get cells for the index  
                if (this.cellDictionary.TryGetValue(index, out var cells) && cells != null)
                {
                    // Add the cells to the list  
                    list.AddRange(cells);
                }
            }

            // Assign the populated list to the result dictionary  
            this.resultListCellDictionary[itemDictionary.Key] = list;
        }
    }

    private void CalculateResultListIndex()
    {
        // Duyệt qua từng mục trong mergeDictionary  
        foreach (var mergeItem in mergeDictionary)
        {
            var merged = false; // Theo dõi xem có mục nào đã được gộp hay chưa  

            // Chuyển đổi resultListIndexDictionary tại chỉ số j thành HashSet  
            for (int j = 0; j < this.resultListIndexDictionary.Count; j++)
            {
                // Chuyển đổi resultItem thành HashSet để sử dụng UnionWith  
                var resultItem = new HashSet<int>(this.resultListIndexDictionary[j]);

                // Lấy danh sách từ mergeItem  
                var mergeValues = mergeItem.Value; // mergeItem.Value chứa List<int>  

                // Kiểm tra xem có thể gộp không  
                if (resultItem.Overlaps(mergeValues)) // Kiểm tra sự giao nhau  
                {
                    merged = true;

                    // Gộp tất cả các mục mới vào resultItem  
                    resultItem.UnionWith(mergeValues); // Gộp các mục của mergeValues vào resultItem  
                    this.resultListIndexDictionary[j] = resultItem.ToList(); // Chuyển đổi lại thành List  
                    break; // Đã gộp xong, không cần kiểm tra nữa  
                }
            }

            // Nếu chưa gộp, thêm một mục mới  
            if (!merged)
            {
                this.resultListIndexDictionary.Add(mergeItem.Key,
                    new List<int>(mergeItem.Value));
            }
        }
    }


    private void MergeCell()
    {
        mergeDictionary.Clear();

        for (int i = 0; i < borderDictionary.Count; i++)
        {
            var list0 = new List<int> { i }; // Create a copy of the current list  
            mergeDictionary[i] = list0;

            for (int j = 0; j < nextCellOfBorderDictionary.Count; j++)
            {
                var checkItem = nextCellOfBorderDictionary[j];
                if (j == i) continue;
                // Check if there are any common cells  
                bool canMerge = borderDictionary[i].Any(borderCell => checkItem.Contains(borderCell));

                if (canMerge)
                {
                    // Combine the cell lists  
                    var list = new List<int>(mergeDictionary[i]) { j }; // Create a copy of the current list  
                    mergeDictionary[i] = list;
                }
            }
        }
    }

    private void SetNextCellOfBorderDictionary()
    {
        for (int count = 0; count < borderDictionary.Count; count++)
        {
            var border = GetNextCellOfBorder(borderDictionary[count]);
            nextCellOfBorderDictionary[count] = border;
        }
    }

    private List<Cell> GetNextCellOfBorder(List<Cell> list)
    {
        var resultSet = new HashSet<Cell>(); // Sử dụng HashSet để kiểm tra sự tồn tại một cách nhanh chóng  

        foreach (var cell in list)
        {
            if (cell != null)
            {
                AddCellToSet(cell.Data.ModelData.upCell);
                AddCellToSet(cell.Data.ModelData.downCell);
                AddCellToSet(cell.Data.ModelData.leftCell);
                AddCellToSet(cell.Data.ModelData.rightCell);
            }
        }

        return new List<Cell>(resultSet); // Chuyển đổi lại HashSet thành List  

        void AddCellToSet(Cell cell)
        {
            if (cell != null) // Chỉ thêm nếu cell không null  
            {
                resultSet.Add(cell); // HashSet tự handle việc trùng lặp  
            }
        }
    }


    private void SetBorderDictionary()
    {
        int count = 0;
        cellDictionary.ForEach(item =>
        {
            var border = GetBorder(item.Value);
            borderDictionary[count] = border;
            count++;
        });
    }

    private List<Cell> GetBorder(List<Cell> list)
    {
        var borderList = new List<Cell>();

        var xValues = new Dictionary<int, (int maxY, Cell cellMaxY, int minY, Cell cellMinY)>();
        var yValues = new Dictionary<int, (int maxX, Cell cellMaxX, int minX, Cell cellMinX)>();

        // Traverse the cells once to collect information about maxY, minY, maxX, minX  
        foreach (var cell in list)
        {
            int x = cell.Data.ModelData.x;
            int y = cell.Data.ModelData.y;

            // Update information for x  
            if (!xValues.ContainsKey(x))
            {
                xValues[x] = (int.MinValue, null, int.MaxValue, null);
            }

            var xValue = xValues[x]; // Get the temporary value  
            if (y > xValue.maxY)
            {
                xValue.maxY = y;
                xValue.cellMaxY = cell;
            }

            if (y < xValue.minY)
            {
                xValue.minY = y;
                xValue.cellMinY = cell;
            }

            xValues[x] = xValue; // Assign back the modified value  

            // Update information for y  
            if (!yValues.ContainsKey(y))
            {
                yValues[y] = (int.MinValue, null, int.MaxValue, null);
            }

            var yValue = yValues[y]; // Get the temporary value  
            if (x > yValue.maxX)
            {
                yValue.maxX = x;
                yValue.cellMaxX = cell;
            }

            if (x < yValue.minX)
            {
                yValue.minX = x;
                yValue.cellMinX = cell;
            }

            yValues[y] = yValue; // Assign back the modified value  
        }

        // Add maximum and minimum cells to borderList from x-values  
        foreach (var (maxY, cellMaxY, minY, cellMinY) in xValues.Values)
        {
            if (cellMaxY != null && !borderList.Contains(cellMaxY))
            {
                borderList.Add(cellMaxY);
            }

            if (cellMinY != null && !borderList.Contains(cellMinY))
            {
                borderList.Add(cellMinY);
            }
        }

        // Add maximum and minimum cells to borderList from y-values  
        foreach (var (maxX, cellMaxX, minX, cellMinX) in yValues.Values)
        {
            if (cellMaxX != null && !borderList.Contains(cellMaxX))
            {
                borderList.Add(cellMaxX);
            }

            if (cellMinX != null && !borderList.Contains(cellMinX))
            {
                borderList.Add(cellMinX);
            }
        }

        return borderList;
    }

    private void ProcessCell(Cell cell)
    {
        cell.SetIsProcessed(true); // Mark the cell as processed  
        cellList.Add(cell);
        ProcessNextCell(cell); // Get the next thick cell  
    }

    private void ProcessNextCell(Cell currentCell)
    {
        // Find the next unprocessed thick cell  
        var nextCell = currentCell.FindNextUnprocessedThickCell();
        if (nextCell != null)
        {
            ProcessCell(nextCell); // Process the next cell  
        }
        else
        {
            Debug.Log("Completed a list.");
            AddCurrentCellListToDictionary();
            ProcessOneList(); //Loop
        }
    }

    private void AddCurrentCellListToDictionary()
    {
        cellDictionary[cellDictionary.Count] = new List<Cell>(cellList);
    }
}