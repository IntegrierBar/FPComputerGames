using Godot;
using System;


/**
The spell object of the spell dark energy wave.

Every tick it increases in size.
Enemies hit are pushed away.
*/
public partial class DarkEnergyWave : Spell
{
	public override void Init(Attack attack, Vector2 playerPosition, Vector2 targetPosition) 
	{
		base.Init(attack, playerPosition, targetPosition);

		Position = playerPosition;
	}

	/**
	Increase the size every tick
	*/
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
		Scale *= (float)1.2; // use the scale property to push the wave
    }



    /**
	If we hit an enemy we push them back
	*/
    public override void OnAreaEntered(Area2D area)
	{
		if (area is HealthComponent healthComponent) // check if area is a health component and if true cast it as a healthcomponent under the name healthComponent
		{
			healthComponent.TakeDamage(_attack);
			(healthComponent.GetParent() as CharacterBody2D).Velocity += (healthComponent.Position - Position).Normalized() * 10; // pushes the enemy. Needs to be testest (TODO)
		}
	}
}
