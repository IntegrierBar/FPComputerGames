using Godot;
using System;

public partial class UnicornShootingAttack : State
{
	[ExportGroup("States")]
    [Export]
    public State Wait; ///< Reference to Wait state

	[Export]
	public double ShootingAnimationDuration; ///< Duration of the shooting attack animation

	private Player _player; ///< reference to the player
	private double _timeLeft = 0.0; ///< time left in which the unicorn remains in the charge attack state

	/**
    Set player so that the distance to the player can be determined later. 
    */
	public override void _Ready()
	{
		_player = GetTree().GetFirstNodeInGroup("player") as Player;
		System.Diagnostics.Debug.Assert(_player is not null, "UnicornShootingAttack has not found a player!");
	}

	/**
    When entering the shooting attack state, set the time left in the state, which depends on the animation duration.
	Update the animation and call the function, that handles the shooting of the projectiles.
    */
	public override void Enter()
    {
		_timeLeft = ShootingAnimationDuration;
		UpdateAnimations();
		ShootProjectiles();
        base.Enter();
    }

	/**
    Count down the time left in the Shooting Attack state. When the time left has reached zero, 
	return to the wait state. 
    */
	public override State ProcessPhysics(double delta)
	{
		_timeLeft -= delta; // count down time left in the shooting attack state
		if (_timeLeft <= 0)
		{
			return Wait; // if the time is up return to the wait state
		}
		return null;
	}

	/**
    Update animations to the charge attack animation, depending on the magic type of the unicorn.
	Note: When the proper animation is done, there should be eight versions for the different directions. 
	Then, this function has to be updated accordingly. 
	At the moment, there are no animations.
    */
    public override void UpdateAnimations()
    {
		string unicornMagicType = EntityTypeHelper.GetMagicTypeAsString((Parent as Unicorn).GetMagicType());
		String animation_name = unicornMagicType + "_charge_attack";
        base.UpdateAnimations();
    }

	/**
    This function should handle the spawning of the projectiles, make sure, that the projectiles
	move in the correct direction and ensure that they damage the player when hitting them.
	Implementation will be done later.
    */
	private void ShootProjectiles()
	{
		// this is where the shooting attack projectiles are spawned. Implementation will follow.
	}
}
