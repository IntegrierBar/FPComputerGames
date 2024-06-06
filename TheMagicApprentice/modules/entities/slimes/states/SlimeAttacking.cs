using Godot;
using System;

public partial class SlimeAttacking : State
{
	[ExportGroup("States")]
    [Export]
    public State Moving; ///< Reference to Moving state
    [Export]
    public State Idle; ///< Reference to Idle state 

	private double _timeLeft = 0;
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

    public override void Enter()
    {
		Attack();
        base.Enter();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override State ProcessPhysics(double delta)
	{
		_timeLeft -= delta;
        if (_timeLeft <= 0.0)
        {
            return Moving; // TODO: should slime return to idle or moving state after performing its attack? 
        }
        return base.ProcessPhysics(delta);
	}

	/**
	Plays attack animation and executes the attack of the slime
	*/
	private void Attack()
	{
		_timeLeft = 1;

		String animation_name = (Parent as Slime).GetMagicTypeAsString() + "_jump";
		Animations.Play(animation_name);
		// The attack of the slime has to be implemented here. It should be taken into consideration 
		// to have to different attack states for melee and ranged slimes. I am not sure yet. 
	}
}
