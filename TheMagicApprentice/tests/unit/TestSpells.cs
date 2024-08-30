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
	Test Sun CalculateAttack() function.
	Remeber that the damage calculated by CalculateAttack is 1/60 of the damage given to the spell
	*/
	[TestCase(60.0, 1.0, 1.0, TestName = "inside sun")]
	[TestCase(60.0, 15.1, 1.0, TestName = "just in front of sun")]
	[TestCase(60.0, 149, 0.0, TestName = "far away from sun")]
	[TestCase(60.0, -10.0, 1.0, TestName = "negative distance")]
	public void TestSunCalculateAttack(double damage, double distanceToEnemySquared, double finalDamage)
	{
		Sun sun = AutoFree(new Sun());
		Attack attack = new Attack(damage, MagicType.SUN, null);
		sun.Init(attack, Vector2.Zero, Vector2.Zero);
		AssertThat(sun.CalculateAttack(distanceToEnemySquared).damage).IsEqualApprox(finalDamage, 1e-1);
	}
}
