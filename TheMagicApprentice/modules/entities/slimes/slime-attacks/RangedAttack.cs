using Godot;
using System;

public partial class RangedAttack : Area2D
{
    private Attack _attack;	///< Contains damage, type and caster reference for damage calculation
	private Vector2 _direction; ///< Direction in which to attack moves

	private double _maxLifeTimeSeconds;

	[Export]
	public float SPEED = 100; ///< Speed of the attack. Do not set too high or evading might be too difficult
	[Export]
	public double MaxLifeTimeInSeconds = 5;

	/**
	Set attack and direction which are given to the Init function. 
	LookAt is used to ensure that the sprite with the projectile looks in the correct direction.
	The colour of the sprite is adapted based on the Magic type the attack has. 
	NOTE: if the projectiles get changed, maybe a change in png becomes necessary here
	*/
    public void Init(Attack attack, Vector2 direction)
    {
		_maxLifeTimeSeconds = MaxLifeTimeInSeconds;
        _attack = attack;
		_direction = direction.Normalized();

		LookAt(GlobalPosition + direction); // makes attack look in the correct direction 

		// change the color depending on the magic type
		AnimatedSprite2D sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		String animations_name = EntityTypeHelper.GetMagicTypeAsString(_attack.magicType) + "_projectile";
		sprite.Play(animations_name);

		if (_direction.X < 0) // flip sprite vertically if the projectile flies to the left (so that down remains down in the image)
		{
			sprite.FlipV = true;
		}
		/*
		Sprite2D sprite = GetNode<Sprite2D>("Sprite2D");
		switch (_attack.magicType) // change colour of the sprite depending on the magic type
		{
			case MagicType.SUN:
			{
				sprite.Modulate = new Color("ORANGE");
				break;
			}
			case MagicType.COSMIC:
			{
				sprite.Modulate = new Color("CYAN");
				break;
			}
			default:
			{
				sprite.Modulate = new Color("PURPLE");
				break;
			}
		}*/

    }

	/**
	Change position of the projectile.
	Count down the max life time of the projectile and remove the projectile once the time is up
	*/
    public override void _PhysicsProcess(double delta)
    {
        Position += (float)delta * SPEED * _direction;

		_maxLifeTimeSeconds -= delta;
		if (_maxLifeTimeSeconds <= 0.0)
		{
			QueueFree();
		}
    }

    /**
	Gets called when the projectile hits a Health component since health components use area2Ds.
	Since the projectile's mask layer is set to the player layer, it cannot hit other slimes.
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
