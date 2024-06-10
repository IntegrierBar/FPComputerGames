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
	private Attack _attack; 
	
	// Called when the node enters the scene tree for the first time.
	/**
	Sets player and constructs an attack that is given to the MeleeAttackHurtBox so that it can do damage when attacking the PC
	*/
	public override void _Ready()
	{
		_player = GetTree().GetFirstNodeInGroup("player");
		if (_player is null)
		{
			GD.Print("No player found!");
		}

		// Creates an attack and gives it to the MeleeAttackHurtBox
		// NOTE: this implementation does not allow for the attack of the slime to change during its lifetime, only before it is spawned! 
		BuildAttack();
		Parent.GetNode<MeleeAttackHurtBox>("MeleeAttackHurtBox").SetAttack(_attack);
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

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override State ProcessPhysics(double delta)
	{
		_timeLeft -= delta;
        if (_timeLeft <= 0.0)
        {
			Parent.GetNode<MeleeAttackHurtBox>("MeleeAttackHurtBox").Monitoring = false; // Deactivate SlimesHurtBox (does nothing for ranged slimes but necessary for melee slimes)
            return Moving; // TODO: should slime return to idle or moving state after performing its attack? 
        }
        return base.ProcessPhysics(delta);
	}

	/**
	Plays attack animation and executes the attack of the ranged slime.
	Ranged slimes do not move during its attack animation.
	Attacking spawns a projectile that flies to the position of the PC.
	*/
	private void AttackRanged()
	{
		_timeLeft = 1; // duration of ranged attack animation

		String animation_name = (Parent as Slime).GetMagicTypeAsString() + "_jump"; // TODO: String name has to e adapted once there are different animations for melee and ranged attacks
		Animations.Play(animation_name);

		// NOTE: In the real animation, the projectile might not be send at the beginning of the attack animation. 
		// This part then needs to be handled differently.
		Vector2 vector_to_player = (_player as CharacterBody2D).Position - Parent.Position;
		PackedScene scene = GD.Load<PackedScene>("res://modules/entities/slimes/slime-attacks/RangedAttack.tscn");
		RangedAttack attack = scene.Instantiate() as RangedAttack;
		attack.Init(_attack, vector_to_player);
		attack.Position = (Parent as Slime).Position;
	}

	/**
	Plays attack animation and executes the attack of the melee slime.
	Function enables the MeleeAttackHurtBox so that the slime damages the PC if it collides with them.
	When attacking, the melee slime jumps to the position of the PC.
	*/
	private void AttackMelee()
	{
		_timeLeft = 1; // duration of melee attack animation

		String animation_name = (Parent as Slime).GetMagicTypeAsString() + "_jump"; // TODO: String name has to e adapted once there are different animations for melee and ranged attacks
		Animations.Play(animation_name);

		Parent.GetNode<MeleeAttackHurtBox>("MeleeAttackHurtBox").Monitoring = true;

		Vector2 vector_to_player = (_player as CharacterBody2D).Position - Parent.Position;
		Parent.Velocity = new Vector2 (vector_to_player[0] / (float)_timeLeft, vector_to_player[1] / (float)_timeLeft); // Ensures that the slime always exactly moves the distance towards the player when attacking
		Parent.MoveAndSlide();
	}

	/**
	Sets parameters of the slimes attack. 
	Damage modifiers can also be added here. 
	*/
	private void BuildAttack()
	{
		_attack.attacker = Parent.GetNode<HealthComponent>("HealthComponent");
		_attack.damage = (Parent as Slime).GetDamageValue();
		_attack.magicType = (Parent as Slime).GetMagicType();
	}
}
