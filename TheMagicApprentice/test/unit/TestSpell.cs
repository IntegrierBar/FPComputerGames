namespace Tests;

using GdUnit4;
using Godot;
using System;
using static GdUnit4.Assertions;

/**
Unit tests for the Spell base class
*/
[TestSuite]
public class TestSpell
{
	private Spell _spell;

    /**
    Create the Spell and manually call the _Ready() function
    */
	 [BeforeTest]
    public void SetupTest()
	{
        _spell = AutoFree(new Spell());
        _spell.MaxLifeTimeInSeconds = 5;
        _spell._Ready();
	}

	/* 
	Test that _PhysicsProcess correctly 
	*/
	[TestCase(1.0)]
    [TestCase(0.3)]
    [TestCase(1.3)]
    [TestCase(4.03)]
	public void TestPhysicsProcess(double delta)
	{
		_spell._PhysicsProcess(delta);
        AssertFloat(_spell.GetTimeUntilDeleteion()).IsEqual(5.0 - delta);
	}
	
}
