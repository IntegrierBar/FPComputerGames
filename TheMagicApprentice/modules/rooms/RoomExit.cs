using Godot;
using System;

/**
A room exit node that emits a signal when the player enters the door. Once a detection with the player is detected,
a signal is emitted with the direction from which the player entered the door, which is used by DungeonHandler to load the next room.
*/
public partial class RoomExit : Area2D
{
	[Signal]
	public delegate void PlayerEnteredDoorEventHandler(Direction direction); ///< Signal emitted when the player enters the door.

	[Export]
	public Direction Direction { get; set; } ///< The direction from which the player entered the door.

	private ColorRect _colorRect; ///< The color rect of the room exit.

	public override void _Ready()
	{
		_colorRect = GetNode<ColorRect>("ColorRect");
		_colorRect.Visible = true;
	}

	/**
	 * Called when the room is initialized.
	 * Connects the BodyEntered signal to the OnBodyEntered method to detect Player Exits.
	 */
	public void RegisterExit()
	{
		BodyEntered += OnBodyEntered;
		_colorRect.Visible = false;
	}

	public void UnregisterExit()
	{
		BodyEntered -= OnBodyEntered;
		_colorRect.Visible = true;
	}

	/**
	 * Callback for when a body enters the area.
	 * 
	 * @param body The body that entered the area.
	 */
	private void OnBodyEntered(Node body)
	{
		if (body.Name == "Player")
		{
			UnregisterExit();
			CallDeferred(nameof(EmitPlayerEnteredDoorSignal));
		}
	}

	private void EmitPlayerEnteredDoorSignal()
	{
		EmitSignal(nameof(PlayerEnteredDoor), (int)Direction);
	}
}
