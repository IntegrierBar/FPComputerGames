namespace Tests;

using GdUnit4;
using Godot;
using System;
using static GdUnit4.Assertions;
using GdUnit4.Executions;
using GdUnit4.Executions.Monitors;
using System.Threading.Tasks;

/**
Integration test for the CurseHandler.
*/

[TestSuite]
public partial class TestCurseHandler
{
	private ISceneRunner _sceneRunner;
	private CurseHandler curseHandler;

	[BeforeTest]
	public void SetupTest()
	{
		GD.Print("Setting up test environment...");

		// Load the scene using ISceneRunner
		_sceneRunner = ISceneRunner.Load("res://main_game.tscn");
		GD.Print("Scene loaded.");

		// Get references to the nodes
		curseHandler = _sceneRunner.FindChild("CurseHandler") as CurseHandler;

		// Ensure all curses are deactivated at the beginning
		CurseHandler.DeactivateCurse(Curse.SKILL_3_DISABLED);
		CurseHandler.DeactivateCurse(Curse.SKILL_1_ONLY);
		CurseHandler.DeactivateCurse(Curse.MORE_VULNERABLE);
		CurseHandler.DeactivateCurse(Curse.MONSTER_BUFF);
		CurseHandler.DeactivateCurse(Curse.MORE_MONSTERS);
		CurseHandler.DeactivateCurse(Curse.TWO_BOSSES);
		GD.Print("set all to false.");
	}

	[AfterTest]
	public void TearDown()
	{
		GD.Print("Tearing down test environment...");

		// Clean up the scene runner
		_sceneRunner = null;
		curseHandler = null;

		GD.Print("Scene runner and nodes freed.");
	}

	[TestCase]
	public void TestIsActive()
	{
		// Initially, all curses should be inactive
		AssertThat(CurseHandler.IsActive(Curse.SKILL_3_DISABLED)).IsFalse();
		AssertThat(CurseHandler.IsActive(Curse.SKILL_1_ONLY)).IsFalse();
		AssertThat(CurseHandler.IsActive(Curse.MORE_VULNERABLE)).IsFalse();
		AssertThat(CurseHandler.IsActive(Curse.MONSTER_BUFF)).IsFalse();
		AssertThat(CurseHandler.IsActive(Curse.MORE_MONSTERS)).IsFalse();
		AssertThat(CurseHandler.IsActive(Curse.TWO_BOSSES)).IsFalse();

		// Activate each curse and test
		CurseHandler.ActivateCurse(Curse.SKILL_3_DISABLED);
		AssertThat(CurseHandler.IsActive(Curse.SKILL_3_DISABLED)).IsTrue();

		CurseHandler.ActivateCurse(Curse.SKILL_1_ONLY);
		AssertThat(CurseHandler.IsActive(Curse.SKILL_1_ONLY)).IsTrue();

		CurseHandler.ActivateCurse(Curse.MORE_VULNERABLE);
		AssertThat(CurseHandler.IsActive(Curse.MORE_VULNERABLE)).IsTrue();

		CurseHandler.ActivateCurse(Curse.MONSTER_BUFF);
		AssertThat(CurseHandler.IsActive(Curse.MONSTER_BUFF)).IsTrue();

		CurseHandler.ActivateCurse(Curse.MORE_MONSTERS);
		AssertThat(CurseHandler.IsActive(Curse.MORE_MONSTERS)).IsTrue();

		CurseHandler.ActivateCurse(Curse.TWO_BOSSES);
		AssertThat(CurseHandler.IsActive(Curse.TWO_BOSSES)).IsTrue();
	}

	[TestCase]
	public void TestSetCurseActive()
	{
		// Test setting a curse to active
		AssertThat(CurseHandler.IsActive(Curse.SKILL_3_DISABLED)).IsFalse();

		CurseHandler.ActivateCurse(Curse.SKILL_3_DISABLED);
		AssertThat(CurseHandler.IsActive(Curse.SKILL_3_DISABLED)).IsTrue();

		// Test setting a curse back to inactive
		CurseHandler.DeactivateCurse(Curse.SKILL_3_DISABLED);
		AssertThat(CurseHandler.IsActive(Curse.SKILL_3_DISABLED)).IsFalse();
	}
}
