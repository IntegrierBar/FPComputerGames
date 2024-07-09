using Godot;
using System;


/**
Class of the individual Star of the spell star rain

On creation gets a random offset from the player and moves towards the mouse position at cast time
*/
public partial class Star : Spell
{
	[Export]
	public float SPEED = 500; ///< Speed of the spell. Do not set to high or else it might not hit enemies

	private Vector2 _direction; ///< Direction in which to spell moves

	
	/**
	Similar to BasicSpell but add a random offset to the starting position
	*/
	public override void Init(Attack attack, Vector2 playerPosition, Vector2 targetPosition)
	{
		base.Init(attack, playerPosition, targetPosition);

		var rng = new RandomNumberGenerator();

		Vector2 offset = new Vector2(rng.RandfRange(-10, 10),  rng.RandfRange(-10, 10));
		Position = playerPosition + offset;
		_direction = (targetPosition-playerPosition).Normalized(); // TODO think abouth how to set the direction according to the offset

		LookAt(targetPosition); // make spell look in the correct direction
	}

	/**
	Change position of the spell.
	Count down the max life time of the spell and remove the spell once the time is up -> TODO: this max time is not implemented!
	*/
    public override void _PhysicsProcess(double delta)
    {
		base._PhysicsProcess(delta);
        Position += (float)delta * SPEED * _direction;
    }


    /**
	Gets called when the spell hits a Health component.
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
		// TODO: Spell should also disappear when hitting a wall
	}
}
