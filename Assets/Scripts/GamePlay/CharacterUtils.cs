using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterUtils
{
    public static Cell GetNextCellToMove(CellData currentCell, EDirectionType moveType)
    {
        return moveType switch
        {
            EDirectionType.Up => currentCell.cellPosition2,
            EDirectionType.Down => currentCell.cellPosition8,
            EDirectionType.Left => currentCell.cellPosition4,
            EDirectionType.Right => currentCell.cellPosition6,
            _ => throw new ArgumentOutOfRangeException(nameof(moveType), moveType, null)
        };
    }
}