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

public class DirectionHelper
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
}
