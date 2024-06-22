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
	/* Commented out for now 
	private ISceneRunner _sceneRunner;
	private DungeonHandler dungeonHandler;
	private Node2D player;
	private Node roomHandler;

	[BeforeTest]
	public void SetupTest()
	{
		GD.Print("Setting up test environment...");

		// Load the scene using ISceneRunner
		_sceneRunner = ISceneRunner.Load("res://modules/rooms/scene.tscn");
		GD.Print("Scene loaded.");

		// Get references to the nodes
		dungeonHandler = _sceneRunner.GetProperty("DungeonHandler");
		player = _sceneRunner.GetProperty("Player");
		roomHandler = _sceneRunner.GetProperty("RoomHandler");

		GD.Print("Nodes references obtained.");
	}

	[AfterTest]
	public void TearDown()
	{
		GD.Print("Tearing down test environment...");

		// Clean up the scene runner
		//_sceneRunner.Free();
		//_sceneRunner = null;
		//dungeonHandler = null;
		//player = null;
		//roomHandler = null;

		GD.Print("Scene runner and nodes freed.");
	}

	[TestCase]
	public void TestLoadRoom()
	{
		GD.Print("Running TestLoadRoom...");

		// Use reflection to call the private method
		var loadRoomMethod = typeof(DungeonHandler).GetMethod("LoadRoom", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
		if (loadRoomMethod == null)
		{
			GD.PrintErr("LoadRoom method not found.");
			return;
		}
		loadRoomMethod.Invoke(dungeonHandler, new object[] { "Room2", Direction.LEFT });

		// Assert
		AssertThat(dungeonHandler.GetNode("Room1")).IsNotNull();
		AssertThat(player.Position).IsEqual(new Vector2(-359, 36)); // Assuming entrance position is (0, 0)
	}

	[TestCase]
	public void TestOnPlayerEnteredDoor()
	{
		GD.Print("Running TestOnPlayerEnteredDoor...");

		// Use reflection to call the private method
		var onPlayerEnteredDoorMethod = typeof(DungeonHandler).GetMethod("OnPlayerEnteredDoor", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
		if (onPlayerEnteredDoorMethod == null)
		{
			GD.PrintErr("OnPlayerEnteredDoor method not found.");
			return;
		}
		onPlayerEnteredDoorMethod.Invoke(dungeonHandler, new object[] { "Room2", Direction.RIGHT });
	}
	*/
}
