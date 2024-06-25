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
}
