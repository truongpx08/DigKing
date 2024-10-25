using System;
using System.Collections.Generic;

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
            EDirectionType.None => EDirectionType.None,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public static List<EDirectionType> GetPopOutDirection(EDirectionType directionType)
    {
        List<EDirectionType> directions = new List<EDirectionType>();
        switch (directionType)
        {
            case EDirectionType.Position1:
                directions.AddRange(new[]
                    { EDirectionType.Position3, EDirectionType.Position7, EDirectionType.Position9 });
                break;
            case EDirectionType.Position3:
                directions.AddRange(new[]
                    { EDirectionType.Position1, EDirectionType.Position9, EDirectionType.Position7 });
                break;
            case EDirectionType.Position7:
                directions.AddRange(new[]
                    { EDirectionType.Position1, EDirectionType.Position9, EDirectionType.Position3 });
                break;
            case EDirectionType.Position9:
                directions.AddRange(new[]
                    { EDirectionType.Position7, EDirectionType.Position3, EDirectionType.Position1 });
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return directions;
    }
}