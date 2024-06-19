using Godot;
using System;
using System.Linq;


/**
The spell object of the spell sun beam
*/
public partial class SunBeam : Spell
{
	public override void Init(Attack attack, Vector2 playerPosition, Vector2 targetPosition) 
	{
		base.Init(attack, playerPosition, targetPosition);

		Position = playerPosition;

		LookAt(targetPosition); // make spell look in the correct direction
	}
	

/*
	public override void OnAreaEntered(Area2D area)
	{
		GD.Print("hit area");
		GD.Print(area);
		if (area is HealthComponent healthComponent) // check if area is a health component and if true cast it as a healthcomponent under the name healthComponent
		{
			healthComponent.TakeDamage(_attack);
			GD.Print("dealt Damage");
			GD.Print(_attack.damage);
			GD.Print(healthComponent.GetCurrentHP());
			// once the spell has hit something we delete it
			//QueueFree();
		}
	}
*/
}
