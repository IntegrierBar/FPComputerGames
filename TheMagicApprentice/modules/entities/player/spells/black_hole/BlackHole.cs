using Godot;
using System;


/**
The spell object of the spell black hole
TODO: Currently the gravity does not affect anything. we probalby have to use gravity or something like that inside the slime class
*/
public partial class BlackHole : Spell
{
	public override void Init(Attack attack, Vector2 playerPosition, Vector2 targetPosition) 
	{
		base.Init(attack, playerPosition, targetPosition);

		Position = targetPosition;
	}

	public override void OnAreaEntered(Area2D area)
	{
		// Print the name of the area
		GD.Print($"Area entered: {area.Name}");

		// Keep the original functionality from the base class
		base.OnAreaEntered(area);
	}
}
