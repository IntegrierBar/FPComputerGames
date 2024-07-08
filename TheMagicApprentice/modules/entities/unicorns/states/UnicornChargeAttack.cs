using Godot;
using System;

public partial class UnicornChargeAttack : State
{
	[ExportGroup("States")]
    [Export]
    public State Wait; ///< Reference to Wait state

	[Export]
	public float CHARGESPEED = 300; ///< Speed with which the unicorn charges at the player

	private Player _player; ///< reference to the player
	private double _timeLeft = 0.0; ///< time left in which the unicorn remains in the charge attack state

	/**
    Set player so that the distance to the player can be determined later. 
    */
	public override void _Ready()
	{
		_player = GetTree().GetFirstNodeInGroup("player") as Player;
		System.Diagnostics.Debug.Assert(_player is not null, "UnicornChargeAttack has not found a player!");
	}

	/**
    When entering the state, the duration of the charge is determined at first 
    */
	public override void Enter()
    {
		_timeLeft = DetermineChargeTimeAndSetDirection();
		UpdateAnimations();
		EnableChargedAttackHitbox();
        base.Enter();
    }

	/**
	When the unicorn leaves the state, reset the velocity to zero to ensure that the unicorn does not do
	any unintended movements when in the next state. 
	*/
    public override void Exit()
    {
		Parent.Velocity = new Vector2(0.0f, 0.0f); // set velocity back to zero when leaving the state
        base.Exit();
    }

	/**
    Count down the time left in the Charge Attack state. When the time left has reached zero, 
	return to the wait state. 
    */
	public override State ProcessPhysics(double delta)
	{
		_timeLeft -= delta; // count down time left in charge attack state
		if (_timeLeft <= 0)
		{
			return Wait; // if the time is up return to the wait state
		}
		Parent.MoveAndSlide();
		return null;
	}

	/**
    This function will later detemine how long the unicorn will charge, depending on how far the player
	is away from the unicorn (the unicorn has to reach the player) and on the distance to the walls behind 
	player (the unicorn should aboid a collision with the wall). Animation duration might also play a role. 
    */
	private float DetermineChargeTimeAndSetDirection()
	{
		if (_player is null) // if there is no player, set the time left to zero so that we leave the state again. 
		{
			return 0;
		}

		// player is not null from here on
		Vector2 vector_to_player = _player.GlobalPosition - Parent.GlobalPosition;
		Parent.Velocity = vector_to_player.Normalized() * CHARGESPEED;
		// duration calculation follows afterwards and has to be added later
		return vector_to_player.Length() / CHARGESPEED * 1.1f;
	}

	/**
    This function should handle the hit box needed for the charge attack to damage the player. 
	Implementation will be done later.
    */
	private void EnableChargedAttackHitbox()
	{
		// This is where the damage part of the charged attack should be later on. Implementation will follow.
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
}
