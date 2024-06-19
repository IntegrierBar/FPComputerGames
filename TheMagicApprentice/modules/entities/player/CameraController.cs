using GdUnit4.Asserts;
using Godot;
using System;

public partial class CameraController : Camera2D
{
	[Export]
	public float SmoothingFactor { get; set; } = 1f;

	[Export]
	public float MinDelta { get; set; } = 0.01f;

	private Node2D Player;
	private RoomHandler RoomHandler;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Player = GetTree().GetNodesInGroup("player")[0] as Node2D;
		RoomHandler = GetTree().GetNodesInGroup("room_handler")[0] as RoomHandler;
		JumpToPlayer();
		System.Diagnostics.Debug.Assert(Player is not null);
		RoomHandler.RoomInitialized += JumpToPlayer;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Vector2 diff = Player.Position - Position;
		if (diff.Length() < MinDelta)
		{
			Position = new Vector2(Player.Position.X, Player.Position.Y);
		}
		else
		{
			Position = new Vector2(Mathf.Lerp(Position.X, Player.Position.X, (float) delta * SmoothingFactor), Mathf.Lerp(Position.Y, Player.Position.Y, (float) delta * SmoothingFactor));
		}
	}
	
	private void JumpToPlayer()
	{
		Position = Player.Position;
	}
}
