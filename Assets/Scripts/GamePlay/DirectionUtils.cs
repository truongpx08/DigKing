using System;

public class DirectionUtils
{
    public static bool IsOppositeDirection(EDirectionType newDirection, EDirectionType oldDirection)
    {
        switch (oldDirection)
        {
            case EDirectionType.Position2:
                if (newDirection == EDirectionType.Position8) return true;
                break;
            case EDirectionType.Position8:
                if (newDirection == EDirectionType.Position2) return true;
                break;
            case EDirectionType.Position4:
                if (newDirection == EDirectionType.Position6) return true;
                break;
            case EDirectionType.Position6:
                if (newDirection == EDirectionType.Position4) return true;
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
            EDirectionType.Position2 => EDirectionType.Position8,
            EDirectionType.Position8 => EDirectionType.Position2,
            EDirectionType.Position4 => EDirectionType.Position6,
            EDirectionType.Position6 => EDirectionType.Position4,
            EDirectionType.Position1 => EDirectionType.Position9,
            EDirectionType.Position3 => EDirectionType.Position7,
            EDirectionType.Position7 => EDirectionType.Position3,
            EDirectionType.Position9 => EDirectionType.Position1,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}