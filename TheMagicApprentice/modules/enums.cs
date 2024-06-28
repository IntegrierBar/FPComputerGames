using Godot;
using System;

/*!
\enum MagicType
\brief Global Enum for the Magic Type

The different types are SUN, COSMIC DARK.
*/
public enum MagicType
{
	SUN,
	COSMIC,
	DARK,
}

/*!
\enum SlimeSize
\brief Global Enum for the size of slimes

The different types are LARGE and SMALL.
*/
public enum SlimeSize
{
	LARGE,
	SMALL,
}

/*!
\enum SlimeAttackRange
\brief Global Enum for the attack range of slimes

The different types are MELEE and RANGED.
*/
public enum SlimeAttackRange
{
	MELEE,
	RANGED,
}

public enum Direction
{
	UP,
	DOWN,
	LEFT,
	RIGHT,
}

public static class EntityTypeHelper
{
    /**
	Takes a magic type and returns it as a string, where all letters are lowercase.
    This is the way the magic types are written in e.g. the animation names.
	*/
    public static String GetMagicTypeAsString(MagicType magicType)
    {
        return magicType switch
        {
            MagicType.SUN => "sun",
            MagicType.COSMIC => "cosmic",
            MagicType.DARK => "dark",
            _ => throw new ArgumentOutOfRangeException(nameof(magicType), magicType, null),
        };
    }

    /**
    Returns a random magic type.
    */
    public static MagicType GetRandomMagicType()
    {
        return (MagicType)GD.RandRange(0, 2);
    }

    /**
	Takes a slime attack range and returns it as a string, where all letters are lowercase.
    This is the way the slime attack range should be written in e.g. the animation names.
	*/
    public static String GetSlimeAttackRangeAsString(SlimeAttackRange slimeAttackRange)
    {
        if (slimeAttackRange == SlimeAttackRange.RANGED)
        {
            return "ranged";
        }
        return "melee";
    }

    /**
	Takes a slime size and returns it as a string, where all letters are lowercase.
    This is the way the slime size should be written in e.g. the animation names.
	*/
    public static String GetSlimeSizeAsString(SlimeSize slimeSize)
    {
        if (slimeSize == SlimeSize.LARGE)
        {
            return "large";
        }
        return "small";
    }
}

public static class DirectionHelper
{
    public static Direction GetOppositeDirection(Direction direction)
    {
        return direction switch
        {
            Direction.UP => Direction.DOWN,
            Direction.DOWN => Direction.UP,
            Direction.LEFT => Direction.RIGHT,
            Direction.RIGHT => Direction.LEFT,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null),
        };
    }

    /**
	 * Calculates the new position in a grid based on the direction of movement.
	 * 
     * @param position The current position in the grid.
	 * @param direction The direction of movement.
	 * @return The new position in the grid.
	 */
	public static Vector2I CalculateNewPosition(Vector2I position, Direction direction)
	{
		Vector2I newPosition = position;
		switch (direction)
		{
			case Direction.UP: newPosition.Y--; break;
			case Direction.DOWN: newPosition.Y++; break;
			case Direction.LEFT: newPosition.X--; break;
			case Direction.RIGHT: newPosition.X++; break;
		}
		return newPosition;
	}
}
