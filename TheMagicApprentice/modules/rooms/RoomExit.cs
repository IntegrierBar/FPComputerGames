using Godot;
using System;

public partial class RoomExit : Area2D
{
	[Signal]
	public delegate void PlayerEnteredDoorEventHandler(string targetRoom, Direction direction); ///< Signal emitted when the player enters the door.

	[Export]
	public string TargetRoom { get; set; } ///< The name of the target room to load.

	[Export]
	public Direction Direction { get; set; } ///< The direction from which the player entered the door.

	/**
	 * Called when the node is added to the scene.
	 * Connects the BodyEntered signal to the OnBodyEntered method.
	 */
	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
	}

	/**
	 * Callback for when a body enters the area.
	 * 
	 * @param body The body that entered the area.
	 */
	private void OnBodyEntered(Node body)
	{
		GD.Print("Body entered.");
		if (body.Name == "Player")
		{
			GD.Print("PlayerExit detected. Sending signal.");
			CallDeferred(nameof(EmitPlayerEnteredDoorSignal));
		}
	}

	private void EmitPlayerEnteredDoorSignal()
	{
		EmitSignal(nameof(PlayerEnteredDoor), TargetRoom, (int)Direction);
	}
}
