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
            EDirectionType.Up => currentCell.upCell,
            EDirectionType.Down => currentCell.downCell,
            EDirectionType.Left => currentCell.leftCell,
            EDirectionType.Right => currentCell.rightCell,
            _ => throw new ArgumentOutOfRangeException(nameof(moveType), moveType, null)
        };
    }
}