using Godot;
using System;


/**
The spell object of the spell black hole
*/
public partial class BlackHole : Spell
{
	public override void Init(Attack attack, Vector2 playerPosition, Vector2 targetPosition) 
	{
		base.Init(attack, playerPosition, targetPosition);

		Position = targetPosition;
	}
}
