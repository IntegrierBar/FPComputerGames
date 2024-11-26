using Godot;
using System;
using System.Security.Cryptography;

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

/*!
\enum Curse
\brief Global Enum for the curses

The different types are MONSTER_HP_INCREASE.
*/
public enum Curse
{
    SKILL_3_DISABLED,
    SKILL_1_ONLY,
    MORE_VULNERABLE,
	MONSTER_BUFF,
    MORE_MONSTERS,
    TWO_BOSSES
}

/*!
\enum Direction
\brief Global Enum for the direction of movement

The different types are UP, DOWN, LEFT, RIGHT.
*/
public enum Direction
{
	UP,
	DOWN,
	LEFT,
	RIGHT,
}

/*!
\enum SpellName
\brief Global Enum for all spell names
This Enum is mostly used by the augments so that we don't have to use strings
*/
public enum SpellName
{
    SunBasic,
    CosmicBasic,
    DarkBasic,
    SunBeam,
    SummonSun,
    MoonLight,
    StarRain,
    DarkEnergyWave,
    BlackHole,
}

public static class EntityTypeHelper
{
    /**
    Returns the MagicType of the spellName.
    */
    public static MagicType GetMagicTypeOfSpell(SpellName spellName) => spellName switch
	{
		SpellName.SunBasic => MagicType.SUN,
		SpellName.SummonSun => MagicType.SUN,
		SpellName.SunBeam => MagicType.SUN,
		SpellName.CosmicBasic => MagicType.COSMIC,
		SpellName.MoonLight => MagicType.COSMIC,
		SpellName.StarRain => MagicType.COSMIC,
		SpellName.DarkBasic => MagicType.DARK,
		SpellName.DarkEnergyWave => MagicType.DARK,
		SpellName.BlackHole => MagicType.DARK,
		_ => MagicType.SUN,
	};

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
    Returns the MagicType that is weak against the MagicType magicType.
    Is used by the NewGameMenu to determine which MagicType the Intro Dungeon should have
    */
    public static MagicType GetWeakerMagicType(MagicType magicType) => magicType switch
    {
        MagicType.SUN => MagicType.COSMIC,
        MagicType.COSMIC => MagicType.DARK,
        MagicType.DARK => MagicType.SUN,
        _ => throw new ArgumentOutOfRangeException(nameof(magicType), magicType, null), // cannot happen, but must make compiler happy
    };

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



