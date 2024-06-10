using Godot;
using System;

public partial class SlimeIdle : State
{
	[ExportGroup("States")]
    [Export]
    public State Moving; ///< Reference to Moving state
    [Export]
    public State Attacking; ///< Reference to Attacking state 

	[Export]
	public float speed = 20; ///< Speed of the slime when it is idle

	private Node _player;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_player = GetTree().GetFirstNodeInGroup("player");
		if (_player is null)
		{
			GD.Print("No player found!");
		}
		GD.Print("Hello!");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override State ProcessPhysics(double delta)
	{
		if (_player is not null)
		{
			Vector2 vector_to_player = (_player as CharacterBody2D).Position - Parent.Position;
			if (vector_to_player.Length() <= (Parent as Slime).GetAttackRangeF())
			{
				return Attacking;
			}
			if (vector_to_player.Length() <= (Parent as Slime).GetViewRange())
			{
				return Moving;
			}
		}
		String animations_name = (Parent as Slime).GetMagicTypeAsString() + "_idle";
		Animations.Play(animations_name);
		// TODO: else do random walk
		// Note: ensure that a new direction is not picked every physics frame, that looks really weird

		return null;
	}
}
