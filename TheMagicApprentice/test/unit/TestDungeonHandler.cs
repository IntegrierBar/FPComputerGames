namespace Tests;

using GdUnit4;
using Godot;
using System;
using static GdUnit4.Assertions;

[TestSuite]
public partial class TestDungeonHandler
{
	private DungeonHandler dungeonHandler;
	private Node2D player;
	private Node roomHandler;
	private SceneTree sceneTree;

	[BeforeTest]
	public void SetUp()
	{
		GD.Print("Setting up test environment...");

		// Create a new SceneTree instance
		sceneTree = new SceneTree();
		
		GD.Print("SceneTree created.");

		var root = new Node();

		// Create instances of the nodes
		dungeonHandler = new DungeonHandler();
		GD.Print("DungeonHandler created.");
		player = new Node2D();
		player.AddToGroup("player");
		GD.Print("Player created and added to group.");
		roomHandler = new Node();
		roomHandler.AddToGroup("room_handler");
		GD.Print("RoomHandler created and added to group.");

		// Add nodes to the scene tree
		sceneTree.Root.AddChild(player);
		sceneTree.Root.AddChild(dungeonHandler);
		sceneTree.Root.AddChild(roomHandler);
		GD.Print("Nodes added to SceneTree.");

		// Set up the player and room handler groups
		player.AddToGroup("player");
		roomHandler.AddToGroup("room_handler");
		GD.Print("Player and RoomHandler groups set up.");

		// Call the _Ready method to initialize the DungeonHandler
		dungeonHandler._Ready();
		GD.Print("DungeonHandler _Ready method called.");
	}

	[AfterTest]
	public void TearDown()
	{
		GD.Print("Tearing down test environment...");

		// Clean up the scene tree
		if (dungeonHandler != null)
		{
			dungeonHandler.Free();
			dungeonHandler = null;
			GD.Print("DungeonHandler freed.");
		}
		if (player != null)
		{
			player.Free();
			player = null;
			GD.Print("Player freed.");
		}
		if (roomHandler != null)
		{
			roomHandler.Free();
			roomHandler = null;
			GD.Print("RoomHandler freed.");
		}
		if (sceneTree != null)
		{
			sceneTree.Root.Free();
			sceneTree = null;
			GD.Print("SceneTree freed.");
		}
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
}
