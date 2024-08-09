using System;

public class DirectionUtils
{
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

    public static EDirectionType GetOppositeDirection(EDirectionType directionType)
    {
        return directionType switch
        {
            EDirectionType.Up => EDirectionType.Down,
            EDirectionType.Down => EDirectionType.Up,
            EDirectionType.Left => EDirectionType.Right,
            EDirectionType.Right => EDirectionType.Left,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}