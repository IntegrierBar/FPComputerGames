using Godot;
using System;

public partial class SlimeMoving : State
{
	[ExportGroup("States")]
    [Export]
    public State Idle; ///< Reference to Idle state
    [Export]
    public State Attacking; ///< Reference to Attacking state 

	[Export]
	public float SPEED = 50; ///< Speed of the slime when it moves towards the player

	private Node _player;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_player = GetTree().GetFirstNodeInGroup("player");
		if (_player is null)
		{
			GD.Print("No player found!");
		}
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
			if (vector_to_player.Length() > (Parent as Slime).GetViewRange())
			{
				return Idle;
			}

			Parent.Velocity = vector_to_player.Normalized() * SPEED;
			Parent.MoveAndSlide();
			
			String animation_name = (Parent as Slime).GetMagicTypeAsString() + "_jump";
			Animations.Play(animation_name);
		}
		return null;
	}
}
