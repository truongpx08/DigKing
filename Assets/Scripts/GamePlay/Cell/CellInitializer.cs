using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

public class CellInitializer : CellReference
{
    [Button]
    public void Initialize()
    {
        AddPosition();
        AddName();
        this.cell.StateMachine.ChangeState(this.Data.type == ECellType.Thick ? ECellState.Think : ECellState.Thin);
    }

    private void AddName()
    {
        this.cell.name = $"Cell_{this.Data.x}_{this.Data.y}";
    }

    private void AddPosition()
    {
        var space = Map.Instance.Generator.Space;
        Vector3 position = new Vector3(this.Data.x / space, this.Data.y / space, 0);
        this.cell.transform.localPosition = position;
    }

    public void AddAdjacentCells()
    {
        this.MapGenerator.CellList.ForEach(cellItem =>
        {
            if (cellItem.IsThisCell(this.Data.x, this.Data.y + 1))
                this.Data.upCell = cellItem;
            if (cellItem.IsThisCell(this.Data.x, this.Data.y - 1))
                this.Data.downCell = cellItem;
            if (cellItem.IsThisCell(this.Data.x - 1, this.Data.y))
                this.Data.leftCell = cellItem;
            if (cellItem.IsThisCell(this.Data.x + 1, this.Data.y))
                this.Data.rightCell = cellItem;

            if (cellItem.IsThisCell(this.Data.x - 1, this.Data.y + 1))
                this.Data.upLeftCell = cellItem;
            if (cellItem.IsThisCell(this.Data.x + 1, this.Data.y + 1))
                this.Data.upRightCell = cellItem;
            if (cellItem.IsThisCell(this.Data.x - 1, this.Data.y - 1))
                this.Data.downLeftCell = cellItem;
            if (cellItem.IsThisCell(this.Data.x + 1, this.Data.y - 1))
                this.Data.downRightCell = cellItem;
        });
    }
}