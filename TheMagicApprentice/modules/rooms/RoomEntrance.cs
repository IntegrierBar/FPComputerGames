using Godot;
using System;

public partial class RoomEntrance : Node2D
{
	[Export]
	public Direction Direction { get; set; } ///< The direction of the room entrance.
}
