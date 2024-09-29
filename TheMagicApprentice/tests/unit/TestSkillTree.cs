namespace Tests;

using GdUnit4;
using Godot;
using System;
using static GdUnit4.Assertions;

/**
Unit tests for the SkillTree Class
*/
[TestSuite]
public partial class TestSkillTree 
{
	private SkillTree _skillTree;

	[BeforeTest]
	public void SetupTest()
	{
		_skillTree = AutoFree(new SkillTree());
	}

	[TestCase(0, SpellName.SunBasic, TestName = "SunBasic")]
	[TestCase(1, SpellName.SunBeam, TestName = "SunBeam")]
	[TestCase(2, SpellName.SummonSun, TestName = "SummonSun")]
	[TestCase(3, SpellName.CosmicBasic, TestName = "CosmicBasic")]
	[TestCase(4, SpellName.MoonLight, TestName = "MoonLight")]
	[TestCase(5, SpellName.StarRain, TestName = "StarRain")]
	[TestCase(6, SpellName.DarkBasic, TestName = "DarkBasic")]
	[TestCase(7, SpellName.DarkEnergyWave, TestName = "DarkEnergyWave")]
	[TestCase(8, SpellName.BlackHole, TestName = "BlackHole")]
	[TestCase(9, null, TestName = "Number too high")]
	[TestCase(-1, null, TestName = "Number too low")]
	public void TestGetSpellFromIndex(int spellIndex, SpellName? spellName)
	{
		AssertThat(_skillTree.GetSpellFromIndex(spellIndex)).IsEqual(spellName);
	}
}
