using Godot;
using System;

public partial class RangedAttack : Area2D
{
    private Attack _attack;	///< Contains damage, type and caster reference for damage calculation
	private Vector2 _direction; ///< Direction in which to attack moves

	public float SPEED = 400; ///< Speed of the attack. Do not set too high or evading might be too difficult

	/**
	Set attack and direction which are given to the Init function. 
	LookAt is used to ensure that the sprite with the projectile looks in the correct direction.
	The colour of the sprite is adapted based on the Magic type the attack has. 
	NOTE: if the projectiles get changed, maybe a change in png becomes necessary here
	*/
    public void Init(Attack attack, Vector2 direction)
    {
        _attack = attack;
		_direction = direction.Normalized();

		LookAt(GlobalPosition + direction); // makes attack look in the correct direction TODO: check that it is the correct direction, otherwise the texture has to be rotated

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
		}

    }

	/**
	Change position of the projectile.
	*/
    public override void _PhysicsProcess(double delta)
    {
        Position += (float)delta * SPEED * _direction;
    }

    /**
	Gets called when the projectile hits a Health component.
	Since the projectile's mask layer is set to the player layer, it cannot hit other slimes
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
