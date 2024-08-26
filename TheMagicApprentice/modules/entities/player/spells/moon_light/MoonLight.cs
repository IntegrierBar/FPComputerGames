using Godot;
using System;

/**
The spell object of the moolight spell.
*/
public partial class MoonLight : Spell
{
	[Export]
	public double boost = 1.5; ///< boost factor by how much the player is boosted
	
	// TODO implement the boost
	public override void Init(Attack attack, Vector2 playerPosition, Vector2 targetPosition) 
	{
		base.Init(attack, playerPosition, targetPosition);
		Position = playerPosition;
	}
}
