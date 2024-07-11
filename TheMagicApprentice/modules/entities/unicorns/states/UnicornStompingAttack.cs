using Godot;
using System;

public partial class UnicornStompingAttack : State
{
	[ExportGroup("States")]
    [Export]
    public State Wait; ///< Reference to Wait state

	[Export]
	public double StompingAnimationDuration; ///< Duration of the stomping attack animation. 
	[Export]
	public double StompingDelayTime; ///< Time after which the unicorn hits the ground with its hooves in the animation

	private Player _player; ///< reference to the player
	private double _timeLeft = 0.0; ///< time left in which the unicorn remains in the stomping attack state

	[Export]
	private HealthComponent _healthComponent; ///< Reference to Health component of the unicorn

	/**
    Set player so that the distance or direction to the player can be determined later. 
    */
	public override void _Ready()
	{
		_player = GetTree().GetFirstNodeInGroup("player") as Player;
		System.Diagnostics.Debug.Assert(_player is not null, "UnicornStompingAttack has not found a player!");
	}

	/**
    When entering the stomping attack state, set the time left in the state depending on the animation duration.
	Update the animations and call the function that handles the effect of the stomping attack.
    */
	public override void Enter()
    {
		_timeLeft = StompingAnimationDuration;
		UpdateAnimations();
		EnableStompingHurtbox();
        base.Enter();
    }

	/**
    Count down the time left in the Stomping Attack state. When the time left has reached zero, 
	return to the wait state. 
    */
	public override State ProcessPhysics(double delta)
	{
		_timeLeft -= delta; // count down time left in stomping attack state
		if (_timeLeft <= 0)
		{
			return Wait; // if the time in the stomping attack state is over, return to the Wait state
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
    This function activates the hurt box of the stomping attack. The hurtbox is only activated after a delay
	depending on the dtomping animation but this is handled by the hurtbox.
    */
	private void EnableStompingHurtbox()
	{
		Parent.GetNode<HurtBoxStompingAttack>("HurtBoxStompingAttack").StartAttack(BuildAttack(), StompingDelayTime);
	}

	/**
	Sets parameters of the unicorn attack. 
	Damage modifiers can also be added here. 
	*/
	private Attack BuildAttack()
	{
		double damage = (Parent as Unicorn).GetDamageValue();
	 	MagicType magicType = (Parent as Unicorn).GetMagicType();
		Attack attack = new(damage, magicType, _healthComponent);
		return attack;
	}
}
