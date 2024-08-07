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
        this.cellRef.StateMachine.ChangeState(this.DataRef.type == ECellType.Thick ? ECellState.Think : ECellState.Thin);
    }

    private void AddName()
    {
        this.cellRef.name = $"Cell_{this.DataRef.x}_{this.DataRef.y}";
    }

    private void AddPosition()
    {
        var space = Map.Instance.Generator.Space;
        Vector3 position = new Vector3(this.DataRef.x / space, this.DataRef.y / space, 0);
        this.cellRef.transform.localPosition = position;
    }

    public void AddAdjacentCells()
    {
        this.MapGenerator.CellList.ForEach(cellItem =>
        {
            if (cellItem.IsThisCell(this.DataRef.x, this.DataRef.y + 1))
                this.DataRef.upCell = cellItem;
            if (cellItem.IsThisCell(this.DataRef.x, this.DataRef.y - 1))
                this.DataRef.downCell = cellItem;
            if (cellItem.IsThisCell(this.DataRef.x - 1, this.DataRef.y))
                this.DataRef.leftCell = cellItem;
            if (cellItem.IsThisCell(this.DataRef.x + 1, this.DataRef.y))
                this.DataRef.rightCell = cellItem;

            if (cellItem.IsThisCell(this.DataRef.x - 1, this.DataRef.y + 1))
                this.DataRef.upLeftCell = cellItem;
            if (cellItem.IsThisCell(this.DataRef.x + 1, this.DataRef.y + 1))
                this.DataRef.upRightCell = cellItem;
            if (cellItem.IsThisCell(this.DataRef.x - 1, this.DataRef.y - 1))
                this.DataRef.downLeftCell = cellItem;
            if (cellItem.IsThisCell(this.DataRef.x + 1, this.DataRef.y - 1))
                this.DataRef.downRightCell = cellItem;
        });
    }
}