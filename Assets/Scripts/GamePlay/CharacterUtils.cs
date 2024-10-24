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
            EDirectionType.Position2 => currentCell.cellPosition2,
            EDirectionType.Position8 => currentCell.cellPosition8,
            EDirectionType.Position4 => currentCell.cellPosition4,
            EDirectionType.Position6 => currentCell.cellPosition6,
            EDirectionType.Position1 => currentCell.cellPosition1,
            EDirectionType.Position3 => currentCell.cellPosition3,
            EDirectionType.Position7 => currentCell.cellPosition7,
            EDirectionType.Position9 => currentCell.cellPosition9,
            _ => throw new ArgumentOutOfRangeException(nameof(moveType), moveType, null)
        };
    }
}