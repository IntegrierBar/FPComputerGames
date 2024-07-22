using Godot;
using System;

public partial class ShootingAttackProjectile : Area2D
{
    
    [Export]
	public float SPEED = 300; ///< Speed of the attack. Do not set too high or evading might be too difficult
	[Export]
	public double MaxLifeTimeInSeconds = 5; ///< maximum life time of the projectile, the projectile is remove afterwards
    [Export]
    public double HomingTimer = 1; ///< time after spawn in which the projectile changes its direction according to the position of the player 

    private Attack _attack;	///< Contains damage, type and caster reference for damage calculation
    private Vector2 _direction; ///< Direction in which to attack moves

    private double _maxLifeTimeSeconds; ///< maximum life time of this projectile
    private double _homingTimer; ///< time after spawn in which this projectile shows the homing behaviour
    private float _accelerationSpeed = 1200;  ///< factor for the acceleration towards the player

    private Player _player; ///< reference to the player

    private RandomNumberGenerator _random;  ///< random number generator needed for the random initial direction of the projectiles

    /**
	Initialise the random number generator needed to generate a random direction for the projectile.
    Set player so that the direction from the unicorn to the player can be determined later. 
	*/
    public override void _Ready()
    {
        _random = new RandomNumberGenerator(); // necessary for generating some random numbers

        _player = GetTree().GetFirstNodeInGroup("player") as Player;
		System.Diagnostics.Debug.Assert(_player is not null, "ShottingAttackProjectile has not found a player!");
    }

    /**
	Initialise the projectiles parameters.
    Set the attack so that the projectile can damage the player when hitting them.
    Set max life time in seconds after which the projectile is removed again.
    Initialise the direction of the projectile as a random direction.
    Set the homing timer, which is the time the projectile is accelerated towards the current position of the player
	*/
    public void Init(Attack attack)
    {
        _attack = attack;
        _maxLifeTimeSeconds = MaxLifeTimeInSeconds;
        _direction = new Vector2(_random.RandfRange(-1, 1), _random.RandfRange(-1, 1)).Normalized(); // Start direction of the projectil is random
        _homingTimer = HomingTimer;
    }


    /**
	Change position of the projectile.
    Count down homing timer.
    As long as the homing timer is not up, accelerate the projectile in the direction of the current
    position of the player. Once the homing timer is up, the projectile keeps the last direction. 
	Count down the max life time of the projectile and remove the projectile once the time is up
	*/
    public override void _PhysicsProcess(double delta)
    {
        _homingTimer -= delta; // count down homing timer
        if (_homingTimer > 0) // as long as the homing timer is not up yet, accelerate the projectile towards the current player position
        {
            Vector2 velocity = _direction * SPEED; // calculate veclocity from current position and speed
            velocity += (_player.GlobalPosition-GlobalPosition).Normalized() * _accelerationSpeed * (float)delta; // update velocity by accelarating it towards the current player position, v = v_0 + a*t
            _direction = velocity.Normalized(); // the new direction is the new velocity normalised
        } 
        Position += (float)delta * SPEED * _direction; // update the position based on the direction and the speed
        LookAt(GlobalPosition + _direction); // makes attack look in the correct direction

        _maxLifeTimeSeconds -= delta; // count down life time of the projectile
        if (_maxLifeTimeSeconds <= 0.0) // remove projectile when its time is up
		{
			QueueFree();
		}
    }

    /**
	Gets called when the spell hits a Health component since health components use area2Ds.
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
	}

    /**
	Since parts of the tilemap that have a collision layer are not area2D nodes, body entered is necessary to use.
	This function detects collisions with all types of 2D nodes.
	Check if the projectile entered a part of the tilemap, which means a wall or object, and remove the projectile. 
	This requires mask 1 (Collision) to be set!
	*/
	public void OnBodyEntered(Node2D body)
	{
		if (body is TileMap)
		{
			QueueFree();
		}
	}
}
