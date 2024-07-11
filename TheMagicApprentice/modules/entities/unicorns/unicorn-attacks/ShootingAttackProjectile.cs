using Godot;
using System;

public partial class ShootingAttackProjectile : Area2D
{
    
    [Export]
	public float SPEED = 200; ///< Speed of the attack. Do not set too high or evading might be too difficult
	[Export]
	public double MaxLifeTimeInSeconds = 10;
    [Export]
    public double HomingTimer = 1.0;

    private Attack _attack;	///< Contains damage, type and caster reference for damage calculation
    private Vector2 _direction; ///< Direction in which to attack moves

    private double _maxLifeTimeSeconds;
    private double _homingTimer;
    private float _accelerationSpeed = 1200;

    private Player _player;

    private RandomNumberGenerator _random;

    public override void _Ready()
    {
        _random = new RandomNumberGenerator(); // necessary for generating some random numbers

        _player = GetTree().GetFirstNodeInGroup("player") as Player;
		System.Diagnostics.Debug.Assert(_player is not null, "ShottingAttackProjectilehas not found a player!");
    }

    public void Init(Attack attack)
    {
        _attack = attack;
        _maxLifeTimeSeconds = MaxLifeTimeInSeconds;
        _direction = new Vector2(_random.RandfRange(-1, 1), _random.RandfRange(-1, 1)).Normalized(); // TODO: might want to change to ensure that projectiles cannot be initialised in opposite direction of player
        _homingTimer = HomingTimer;
    }


    /**
	Change position of the projectile.
	Count down the max life time of the projectile and remove the projectile once the time is up
	*/
    public override void _PhysicsProcess(double delta)
    {
        _homingTimer -= delta;
        if (_homingTimer > 0)
        {
            Vector2 velocity = _direction * SPEED;
            velocity += (_player.GlobalPosition-GlobalPosition).Normalized() * _accelerationSpeed * (float)delta;
            _direction = velocity.Normalized();
        } 
        Position += (float)delta * SPEED * _direction;
        LookAt(GlobalPosition + _direction); // makes attack look in the correct direction

        _maxLifeTimeSeconds -= delta;
        if (_maxLifeTimeSeconds <= 0.0)
		{
			QueueFree();
		}
    }

    /**
	Gets called when the spell hits a Health component.
	Projectile mask is set such, that it can only hit the player. 
	*/
    public void OnAreaEntered(Area2D area)
	{
		if (area is HealthComponent healthComponent) // check if area is a health component and if true cast it as a healthcomponent under the name healthComponent
		{
			healthComponent.TakeDamage(_attack);
			// once the spell has hit something we delete it
			QueueFree();
		}
		// TODO: Spell should also disappear when hitting a wall
	}
}
