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

	/**
	Tests both GetSpellFromIndex and its inverse
	*/
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
		if (spellName is not null) // only check the inverse function if spellName is not null
		{
			AssertThat(_skillTree.GetIndexFromSpell((SpellName)spellName)).IsEqual(spellIndex);
		}
		
	}

	[TestCase("SunBasic", SpellName.SunBasic)]
	[TestCase("SummonSun", SpellName.SummonSun)]
	[TestCase("SunBeam", SpellName.SunBeam)]
	[TestCase("CosmicBasic", SpellName.CosmicBasic)]
	[TestCase("MoonLight", SpellName.MoonLight)]
	[TestCase("StarRain", SpellName.StarRain)]
	[TestCase("DarkBasic", SpellName.DarkBasic)]
	[TestCase("DarkEnergyWave", SpellName.DarkEnergyWave)]
	[TestCase("BlackHole", SpellName.BlackHole)]
	public void TestConvertStringNameToSpellName(string stringName, SpellName spellName)
	{
		AssertThat(SkillTree.ConvertStringNameToSpellName(stringName)).IsEqual(spellName);
	}
}
