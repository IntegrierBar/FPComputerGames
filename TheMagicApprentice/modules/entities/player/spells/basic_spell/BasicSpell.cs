using Godot;
using System;


/**
The basic spell object of each element.
Setting the element will also change the color.

Note that since we use an Area2D which we manually move, if the spells moves ultra fast it can move over the enemies and thus miss them.
*/
public partial class BasicSpell : Spell
{
	[Export]
	public float SPEED = 600; ///< Speed of the spell. Do not set to high or else it might not hit enemies

	private Vector2 _direction; ///< Direction in which the spell moves


	/**
	Call after instantiating the base spell scene in order to set the Attack of the spell and change the animation depending on the magic type.
	*/
	public override void Init(Attack attack, Vector2 playerPosition, Vector2 targetPosition)
	{
		base.Init(attack, playerPosition, targetPosition);

		Position = playerPosition;
		_direction = (targetPosition-playerPosition).Normalized();

		LookAt(targetPosition); // make spell look in the correct direction

		// change the color depending on the magic type
		AnimatedSprite2D sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		String animations_name = EntityTypeHelper.GetMagicTypeAsString(_attack.magicType) + "_basic_spell";
		sprite.Play(animations_name);

		if (_direction.X < 0) // flip sprite vertically if the projectile flies to the left (so that down remains down in the image)
		{
			sprite.FlipV = true;
		}
	}

	/**
	Move the spell in _direction
	Count down the max life time of the spell and remove the spell once the time is up
	*/
    public override void _PhysicsProcess(double delta)
    {
		base._PhysicsProcess(delta);
        Position += (float)delta * SPEED * _direction;
    }


    /**
	Gets called when the spell hits a Health component since health components use area2Ds.
	Since the spells mask layer is set to the enemies layer, it cannot hit the player
	*/
    public override void OnAreaEntered(Area2D area)
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
