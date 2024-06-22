namespace Tests;

using GdUnit4;
using Godot;
using System;
using static GdUnit4.Assertions;

/**
Unit tests for all Spell classes
*/
[TestSuite]
public class TestSpells
{
	/** 
	Test that _PhysicsProcess of Spell vlass correctly 
	*/
	[TestCase(1.0)]
    [TestCase(0.3)]
    [TestCase(1.3)]
    [TestCase(4.03)]
	public void TestSpellPhysicsProcess(double delta)
	{
		Spell spell = AutoFree(new Spell());
        spell.MaxLifeTimeInSeconds = 5;
        spell._Ready();
		spell._PhysicsProcess(delta);
        AssertFloat(spell.GetTimeUntilDeleteion()).IsEqual(5.0 - delta);
	}


	

	/**
	Test Sun CalculateAttack() function
	*/
	[TestCase(100.0, 10.0, 10.0, TestName = "simpel numbers")]
	[TestCase(100.0, 0.5, 100.0, TestName = "Test Mininium")]
	[TestCase(100.0, -0.5, 100.0, TestName = "Test negative distance")]
	public void TestSunCalculateAttack(double damage, double distanceToEnemySquared, double finalDamage)
	{
		Sun sun = AutoFree(new Sun());
		Attack attack = new Attack(damage, MagicType.SUN, null);
		sun.Init(attack, Vector2.Zero, Vector2.Zero);
		AssertThat(sun.CalculateAttack(distanceToEnemySquared).damage).IsEqual(finalDamage);
	}
}
