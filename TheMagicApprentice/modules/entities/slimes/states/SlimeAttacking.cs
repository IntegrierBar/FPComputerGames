using Godot;
using System;

public partial class SlimeAttacking : State
{
	[ExportGroup("States")]
    [Export]
    public State Moving; ///< Reference to Moving state

	private double _timeLeft = 0.0;
	private Player _player;

	[Export]
	private HealthComponent _healthComponent; ///< Reference to Health component of the slime
	
	/**
	Sets player
	*/
	public override void _Ready()
	{
		_player = GetTree().GetFirstNodeInGroup("player") as Player;
		if (_player is null)
		{
			GD.Print("No player found!");
		}
	}

    public override void Enter()
    {
		String slimeAttackRange = (Parent as Slime).GetSlimeAttackRangeAsString();
		if (slimeAttackRange == "ranged")
		{
			AttackRanged();
		}
		else if (slimeAttackRange == "melee")
		{
			AttackMelee();
		}
		else
		{
			GD.Print("Oh no, this slime does not have a known attack range!");
		}
        base.Enter();
    }

    /**
	Once the attack animation is done, return to the Moving state and set monitoring of the melee attack 
	hurt box to false so that the slime does not damage the player if they are colliding.
	*/    public override State ProcessPhysics(double delta)
	{
		_timeLeft -= delta;
        if (_timeLeft <= 0.0)
        {
			Parent.GetNode<MeleeAttackHurtBox>("MeleeAttackHurtBox").Monitoring = false; // Deactivate SlimesHurtBox (does nothing for ranged slimes but necessary for melee slimes)
            return Moving;
        }
        return base.ProcessPhysics(delta);
	}

	/**
	Plays attack animation and executes the attack of the ranged slime.
	Constructs an attack that is given to the ranged projectile to do damage. 
	Ranged slimes do not move during its attack animation.
	Attacking spawns a projectile that flies to the position of the PC.
	*/
	private void AttackRanged()
	{
		_timeLeft = 1; // duration of ranged attack animation

		Attack attack = BuildAttack(); // Builds an attack that can be used by the projectile to do damage

		String animation_name = (Parent as Slime).GetMagicTypeAsString() + "_jump"; // TODO: String name has to e adapted once there are different animations for melee and ranged attacks
		Animations.Play(animation_name);

		// NOTE: In the real animation, the projectile might not be send at the beginning of the attack animation. 
		// This part then needs to be handled differently.
		Vector2 vector_to_player = (_player as CharacterBody2D).Position - Parent.Position;
		PackedScene scene = GD.Load<PackedScene>("res://modules/entities/slimes/slime-attacks/RangedAttack.tscn");
		RangedAttack ranged_attack = scene.Instantiate() as RangedAttack;
		GetTree().Root.AddChild(ranged_attack); // TODO In the future they schoudl not be added to root but to dungeon so that they get deleted when teh dungeon gets deleted
		ranged_attack.Init(attack, vector_to_player);
		ranged_attack.Position = (Parent as Slime).Position;
	}

	/**
	Plays attack animation and executes the attack of the melee slime.
	Function enables the MeleeAttackHurtBox so that the slime damages the PC if it collides with them.
	Constructs an attack that is given to the MeleeAttackHurtBox so that the slime can do damage when attacking the PC.
	When attacking, the melee slime jumps to the position of the PC.
	*/
	private void AttackMelee()
	{
		_timeLeft = 1; // duration of melee attack animation

		String animation_name = (Parent as Slime).GetMagicTypeAsString() + "_jump"; // TODO: String name has to e adapted once there are different animations for melee and ranged attacks
		Animations.Play(animation_name);

		Parent.GetNode<MeleeAttackHurtBox>("MeleeAttackHurtBox").Monitoring = true;

		// Creates an attack and gives it to the MeleeAttackHurtBox 
		Attack attack = BuildAttack();
		Parent.GetNode<MeleeAttackHurtBox>("MeleeAttackHurtBox").SetAttack(attack);

		Vector2 vector_to_player = (_player as CharacterBody2D).Position - Parent.Position;
		Parent.Velocity = new Vector2 (vector_to_player[0] / (float)_timeLeft, vector_to_player[1] / (float)_timeLeft); // Ensures that the slime always exactly moves the distance towards the player when attacking
		Parent.MoveAndSlide();
	}

	/**
	Sets parameters of the slimes attack. 
	Damage modifiers can also be added here. 
	*/
	private Attack BuildAttack()
	{
		double damage = (Parent as Slime).GetDamageValue();
	 	MagicType magicType = (Parent as Slime).GetMagicType();
		Attack attack = new Attack(damage, magicType, _healthComponent);
		return attack;
	}
}
