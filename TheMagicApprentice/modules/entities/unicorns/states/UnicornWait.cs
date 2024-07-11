using Godot;
using System;

public partial class UnicornWait : State
{
	[ExportGroup("States")]
    [Export]
    public State ChargeAttack; ///< Reference to ChargeAttack state
    [Export]
    public State ShootingAttack; ///< Reference to ShootingAttack state 
	[Export]
    public State StompingAttack; ///< Reference to StompingAttack state 

	[Export]
	public float SPEED = 10; ///< Speed of the unicorn when it waits until its next attack
	[Export]
	public float WaitTime = 2; ///< Duration the unicorn spends in the wait state between attacks

	private double _timeLeft = 0.0; ///< time left in which the unicorn waits until its next attack

	private Player _player; ///< reference to the player


	/**
    Set player so that the distance to the player can be determined later. 
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
    When the unicorn enters the state, set timeLeft so that the unicorn remains in the Wait state the 
	for the correct time duration.
	Also update animations (call of update animations might have to be changed later).
    */
    public override void Enter()
    {
		_timeLeft = WaitTime;
		UpdateAnimations();
        base.Enter();
    }

	/**
	When the unicorn leaves the state, reset the velocity to zero to ensure that the unicorn does not do
	any unintended movements when in the next state. 
	*/
    public override void Exit()
    {
		Parent.Velocity = new Vector2(0.0f, 0.0f);
        base.Exit();
    }

    /**
    Count down the time left in the wait state. 
	If the time is smaller or equal to zero, call funciotn SelectNextAttack to determine which attack is
	to be performed next and then change to that attack.
	If the unicorn remains in the wait state because the time is not up yet, the unicorn moves towards the current position of the player slowly. 
    */
    public override State ProcessPhysics(double delta)
	{
		if (_player is null)
		{
			return null;
		}
		Vector2 vector_to_player = (_player as CharacterBody2D).GlobalPosition - Parent.GlobalPosition;

		_timeLeft -= delta;

		if (_timeLeft <= 0) // if the time in the wait state is over, transition to the next attack
		{
			State nextAttackState = SelectNextAttack(vector_to_player);
			return nextAttackState;
		}

		// The unicorn is still in the wait state
		Parent.Velocity = vector_to_player.Normalized() * SPEED; // unicorn moves towards the player (at a very slow speed)
		Parent.MoveAndSlide();
		return null;
	}

	/**
    Selects the next attack of the unicorn out of the three attacks: stomping attack, shooting attack 
	and charge attack. 
	If the player is within the melee attack range of the unicorn, the unicorn will perform a stomping attack.
	If the player is outside of the melee attack range of the unicorn, the unicorn will change to
	one of the ranged attacks: stooting attack or charge attack. A random number is generated to decide 
	which ranged attack is used. 
    */
	private State SelectNextAttack(Vector2 vector_to_player)
	{
		if (vector_to_player.Length() <= (Parent as Unicorn).MeleeAttackRange) // use the melee attack StompingAttack if the player is withing the melee attack range
		{
			return StompingAttack;
		}
		
		// the player is outside of the melee attack range
		uint randomNumber0or1 = GD.Randi() % 2; // use a random number to decide which of the two ranged attacks is going to be used
		if (randomNumber0or1 == 0)
		{
			return ChargeAttack;
		}
		else
		{
			return ShootingAttack;
		}
	}

	/**
    Update animations to the wait animation, depending on the magic type of the unicorn.
	Note: When the proper animation is done, there should be eight versions for the different directions. 
	Then, this function has to be updated accordingly. 
	At the moment, there are no animations.
    */
    public override void UpdateAnimations()
    {
		string unicornMagicType = EntityTypeHelper.GetMagicTypeAsString((Parent as Unicorn).GetMagicType());
		String animation_name = unicornMagicType + "_wait";
        base.UpdateAnimations();
    }
}
