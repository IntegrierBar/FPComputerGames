namespace Tests;

using GdUnit4;
using Godot;
using System;
using static GdUnit4.Assertions;

/**
 * Unit tests for the DirectionHelper class.
 * Tests the GetOppositeDirection function.
 */
[TestSuite]
public partial class TestDirections
{
	[TestCase]
	public void TestGetOppositeDirection()
	{
		AssertThat(DirectionHelper.GetOppositeDirection(Direction.UP)).IsEqual(Direction.DOWN);
		AssertThat(DirectionHelper.GetOppositeDirection(Direction.DOWN)).IsEqual(Direction.UP);
		AssertThat(DirectionHelper.GetOppositeDirection(Direction.LEFT)).IsEqual(Direction.RIGHT);
		AssertThat(DirectionHelper.GetOppositeDirection(Direction.RIGHT)).IsEqual(Direction.LEFT);
	}

	[TestCase]
	public void TestCalculateNewPosition()
	{
		Vector2I startPosition = new Vector2I(0, 0);

		Vector2I newPositionUp = DirectionHelper.CalculateNewPosition(startPosition, Direction.UP);
		AssertThat(newPositionUp).IsEqual(new Vector2I(0, -1));

		Vector2I newPositionDown = DirectionHelper.CalculateNewPosition(startPosition, Direction.DOWN);
		AssertThat(newPositionDown).IsEqual(new Vector2I(0, 1));

		Vector2I newPositionLeft = DirectionHelper.CalculateNewPosition(startPosition, Direction.LEFT);
		AssertThat(newPositionLeft).IsEqual(new Vector2I(-1, 0));

		Vector2I newPositionRight = DirectionHelper.CalculateNewPosition(startPosition, Direction.RIGHT);
		AssertThat(newPositionRight).IsEqual(new Vector2I(1, 0));
	}
}
