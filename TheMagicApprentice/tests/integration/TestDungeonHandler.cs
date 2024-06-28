namespace Tests;

using GdUnit4;
using Godot;
using System;
using static GdUnit4.Assertions;
using GdUnit4.Executions;
using GdUnit4.Executions.Monitors;
using System.Threading.Tasks;

/**
Integration test for the player scene.
*/

[TestSuite]
public partial class TestDungeonHandler
{
	private ISceneRunner _sceneRunner;
	private DungeonHandler dungeonHandler;

	[BeforeTest]
	public void SetupTest()
	{
		GD.Print("Setting up test environment...");

		// Load the scene using ISceneRunner
		_sceneRunner = ISceneRunner.Load("res://main_game.tscn");
		GD.Print("Scene loaded.");

		// Get references to the nodes
		dungeonHandler = _sceneRunner.FindChild("DungeonHandler") as DungeonHandler;
	}

	[AfterTest]
	public void TearDown()
	{
		GD.Print("Tearing down test environment...");

		// Clean up the scene runner
		_sceneRunner = null;
		dungeonHandler = null;

		GD.Print("Scene runner and nodes freed.");
	}

	[TestCase]
	public void TestLoadRoom()
	{
		dungeonHandler.LoadDungeon(Dungeons.IntroDungeon);
		
		AssertThat(dungeonHandler.GetCurrentRoomPosition()).IsEqual(new Vector2I(0, 0));

		var loadRoomMethod = typeof(DungeonHandler).GetMethod("LoadRoom", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
		if (loadRoomMethod == null)
		{
			GD.PrintErr("LoadRoom method not found.");
			return;
		}
		loadRoomMethod.Invoke(dungeonHandler, new object[] { new Vector2I(0, 1), Direction.DOWN });

		AssertThat(dungeonHandler.GetCurrentRoomPosition()).IsEqual(new Vector2I(0, 1));
	}

	[TestCase]
	public void TestOnPlayerEnteredDoor()
	{
		dungeonHandler.LoadDungeon(Dungeons.IntroDungeon);
		
		AssertThat(dungeonHandler.GetCurrentRoomPosition()).IsEqual(new Vector2I(0, 0));

		var onPlayerEnteredDoorMethod = typeof(DungeonHandler).GetMethod("OnPlayerEnteredDoor", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
		if (onPlayerEnteredDoorMethod == null)
		{
			GD.PrintErr("OnPlayerEnteredDoor method not found.");
			return;
		}
		onPlayerEnteredDoorMethod.Invoke(dungeonHandler, new object[] { Direction.DOWN });

		AssertThat(dungeonHandler.GetCurrentRoomPosition()).IsEqual(new Vector2I(0, 1));
	}
}
