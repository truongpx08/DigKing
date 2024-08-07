using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterVirtual
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

    public static bool IsOppositeDirection(EDirectionType newDirection, EDirectionType oldDirection)
    {
        switch (oldDirection)
        {
            case EDirectionType.Up:
                if (newDirection == EDirectionType.Down) return true;
                break;
            case EDirectionType.Down:
                if (newDirection == EDirectionType.Up) return true;
                break;
            case EDirectionType.Left:
                if (newDirection == EDirectionType.Right) return true;
                break;
            case EDirectionType.Right:
                if (newDirection == EDirectionType.Left) return true;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return false;
    }
}