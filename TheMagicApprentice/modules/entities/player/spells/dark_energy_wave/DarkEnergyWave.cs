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
		Scale *= (float)1.05; // use the scale property to push the wave
    }



    /**
	If we hit an enemy we push them back
	*/
    public override void OnAreaEntered(Area2D area)
	{
		if (area is HealthComponent healthComponent) // check if area is a health component and if true cast it as a healthcomponent under the name healthComponent
		{
			healthComponent.TakeDamage(_attack);

			// pushes the enemy. TODO This still need extensive work. We might have to change how movement works. We probably want to use acceleration instead of velocity for movement
			//(healthComponent.GetParent() as CharacterBody2D).Position += (healthComponent.Position - Position).Normalized() * 40;
			(healthComponent.GetParent() as CharacterBody2D).MoveAndCollide((healthComponent.GlobalPosition - GlobalPosition).Normalized() * 100);  
		}
	}
}
