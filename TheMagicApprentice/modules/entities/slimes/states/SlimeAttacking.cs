using Godot;
using System;

public partial class SlimeAttacking : State
{
	[ExportGroup("States")]
    [Export]
    public State Moving; ///< Reference to Moving state

	private double _timeLeft = 0.0; ///< time left in which the slime remains in the attacking state
	private Player _player; ///< reference to the player

	[Export]
	private HealthComponent _healthComponent; ///< Reference to Health component of the slime
	
	/**
	Sets player
	*/
	public override void _Ready()
	{
		_player = GetTree().GetFirstNodeInGroup("player") as Player;
		System.Diagnostics.Debug.Assert(_player is not null, "SlimeAttacking has not found a player!");
	}

	/**
	Calls update animation.
	Chooses whether to perform a ranged or a melee attack based on the attack range and executes the attack.
	*/
    public override void Enter()
    {
		UpdateAnimations();

		SlimeAttackRange slimeAttackRange = (Parent as Slime).GetSlimeAttackRange();
		if (slimeAttackRange == SlimeAttackRange.RANGED) 
		{
			AttackRanged(); // if the slime is ranged, use the ranged attack
		}
		else if (slimeAttackRange == SlimeAttackRange.MELEE)
		{
			AttackMelee(); // if the slime is melee, us the melee attack
		}
		else
		{
			GD.Print("Oh no, this slime does not have a known attack range!"); // this should not happen, I don't think it even can happen...
		}
        base.Enter();
    }

    /**
	Once the attack animation is done, return to the Moving state and set monitoring of the melee attack 
	hurt box to false so that the slime does not damage the player if they are colliding.
	While in Attacking state, move the slime according to the velocity set in the attack (no movement for ranged, to the position of the player for melee).
	*/    
	public override State ProcessPhysics(double delta)
	{
		_timeLeft -= delta; // count down time left in attack state
        if (_timeLeft <= 0.0)
        {
            return Moving; // return to moving state when the time is up
        }
		Parent.MoveAndSlide();

        return base.ProcessPhysics(delta);
	}

	/**
	Plays attack animation.
	Currently only slime magic type is considered.
	*/
    public override void UpdateAnimations()
    {
		string slimeMagicType = EntityTypeHelper.GetMagicTypeAsString((Parent as Slime).GetMagicType());
		String animation_name = slimeMagicType + "_jump"; // TODO: String name has to e adapted once there are different animations for melee and ranged attacks
		Animations.Play(animation_name);

        base.UpdateAnimations();
    }

    /**
	Executes the attack of the ranged slime.
	Constructs an attack that is given to the ranged projectile to do damage. 
	Ranged slimes do not move during its attack animation.
	Attacking calls a function that spawns a projectile that flies to the position of the PC.
	*/
    private void AttackRanged()
	{
		_timeLeft = 1; // duration of ranged attack animation
		Parent.Velocity = new Vector2(0.0f, 0.0f); // ranged slime does not move while attacking

		// NOTE: In the real animation, the projectile might not be send at the beginning of the attack animation. 
		// The function then has to be called in a different position
		Attack attack = BuildAttack(); // Builds an attack that can be used by the projectile to do damage
		SpawnRangedAttack(attack);
	}

	/**
	Plays attack animation and executes the attack of the melee slime.
	Function enables the MeleeAttackHurtBox so that the slime damages the PC if it collides with them.
	Constructs an attack that is given to the MeleeAttackHurtBox so that the slime can do damage when attacking the PC.
	Change the velocity of the melee slime such that it jumps to the position of the PC.
	*/
	private void AttackMelee()
	{
		_timeLeft = 1; // duration of melee attack animation

		// Creates an attack and gives it to the MeleeAttackHurtBox 
		Attack attack = BuildAttack();
		Parent.GetNode<MeleeAttackHurtBox>("MeleeAttackHurtBox").StartAttack(attack, _timeLeft);

		Vector2 vector_to_player = _player.GlobalPosition - Parent.GlobalPosition;
		Parent.Velocity = new Vector2 (vector_to_player[0] / (float)_timeLeft, vector_to_player[1] / (float)_timeLeft); // Ensures that the slime always exactly moves the distance towards the player when attacking
	}

	/**
	Sets parameters of the slimes attack. 
	Damage modifiers can also be added here. 
	*/
	private Attack BuildAttack()
	{
		double damage = (Parent as Slime).GetDamageValue();
	 	MagicType magicType = (Parent as Slime).GetMagicType();
		Attack attack = new(damage, magicType, _healthComponent);
		return attack;
	}

	/**
	Spawns the projectile for a ranged attack.
	Sets the necessary parameters for the ranged attack.
	*/
	private void SpawnRangedAttack(Attack attack)
	{
		PackedScene scene = GD.Load<PackedScene>("res://modules/entities/slimes/slime-attacks/RangedAttack.tscn");
		RangedAttack ranged_attack = scene.Instantiate() as RangedAttack;
		GetTree().Root.AddChild(ranged_attack); // Add the ranged attack projectile to the scene tree

		Vector2 vector_to_player = _player.GlobalPosition - Parent.GlobalPosition;
		ranged_attack.Init(attack, vector_to_player); // initialise the ranged attack with the build attack and the direction from the slime towards the player
		ranged_attack.Position = Parent.Position; // ranged attack spawns at the position of the slime
	}
}
