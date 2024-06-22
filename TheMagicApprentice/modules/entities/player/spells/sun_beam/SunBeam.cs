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
}
