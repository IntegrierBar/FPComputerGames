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
}
