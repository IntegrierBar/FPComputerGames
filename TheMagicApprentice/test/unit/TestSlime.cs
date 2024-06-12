namespace Tests;

using GdUnit4;
using Godot;
using System;
using static GdUnit4.Assertions;

/**
Unit tests for all slime states.
So far, I could only think of one test for the idle slime state. 
*/
[TestSuite]
public class TestSlime
{
	private SlimeIdle _slimeIdle;

	 [BeforeTest]
    public void SetupTest()
	{
		_slimeIdle = AutoFree(new SlimeIdle());
		_slimeIdle.SPEED = 20;
		_slimeIdle.IdleAnimationDuration = 1.0;
		_slimeIdle.JumpAnimationDuration = 1.0;
	}

	/* 
	Test ChangeRandomWalk function from SlimeIdle state. Check that after calling the function once, 
	the direction is not zero (slime should move araound). The second time the function is called the 
	direction is zero (slime remains in a position). The third timee the function is called, the slime
	changed back to moving around and the direction should no longer be zero.
	*/
	[TestCase]
	public void TestIdleStateChangeRandomWalk()
	{
		AssertThat(_slimeIdle.ChangeRandomWalk()).IsNotEqual(Vector2.Zero);
		AssertThat(_slimeIdle.ChangeRandomWalk()).IsEqual(Vector2.Zero);
		AssertThat(_slimeIdle.ChangeRandomWalk()).IsNotEqual(Vector2.Zero);
	}
	
}
