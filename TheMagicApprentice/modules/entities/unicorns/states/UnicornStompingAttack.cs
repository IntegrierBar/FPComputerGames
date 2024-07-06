using Godot;
using System;

public partial class UnicornStompingAttack : State
{
	[ExportGroup("States")]
    [Export]
    public State Wait; ///< Reference to Wait state

	private Player _player; ///< reference to the player
	private double _timeLeft = 0.0; ///< time left in which the unicorn remains in the stomping attack state

	[Export]
	public double StompingAnimationDuration;

	/**
    Set player so that the distance or direction to the player can be determined later. 
    */
	public override void _Ready()
	{
		_player = GetTree().GetFirstNodeInGroup("player") as Player;
		if (_player is null)
		{
			GD.Print("No player found!");
		}
	}

	/**
    When entering the stomping attack state, set the time left in the state depending on the animation duration.
	Update the animations and call the function that handles the effect of the stomping attack.
    */
	public override void Enter()
    {
		_timeLeft = StompingAnimationDuration;
		UpdateAnimations();
		StompOnGround();
        base.Enter();
    }

	/**
    Count down the time left in the Stomping Attack state. When the time left has reached zero, 
	return to the wait state. 
    */
	public override State ProcessPhysics(double delta)
	{
		_timeLeft -= delta;
		if (_timeLeft <= 0)
		{
			return Wait;
		}
		return null;
	}

	/**
    Update animations to the stomping attack animation, depending on the magic type of the unicorn.
	Note: When the proper animation is done, there should be eight versions for the different directions. 
	Then, this function has to be updated accordingly. 
	At the moment, there are no animations.
    */
    public override void UpdateAnimations()
    {
		string unicornMagicType = EntityTypeHelper.GetMagicTypeAsString((Parent as Unicorn).GetMagicType());
		String animation_name = unicornMagicType + "_stomping_attack";
        base.UpdateAnimations();
    }

	/**
    This function should handle the hitbox of the attack, probably also the animation that has to be 
	displayed and ensure that the attack can hurt the player.
	Implementation will be done later.
    */
	private void StompOnGround()
	{
		// here the stomping attack damage and hitbox and stuff like that should be handled
	}
}
