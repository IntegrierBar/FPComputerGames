using Godot;
using System;

/**
The spell object of the spell summon sun.
Deals damage every tick to enemies inside its radius
*/
public partial class Sun : Spell
{
	/**
	Positions the Sun at the mouse position
	*/
	public override void Init(Attack attack, Vector2 playerPosition, Vector2 targetPosition) 
	{
		base.Init(attack, playerPosition, targetPosition);

		Position = targetPosition;
	}

	/**
	Every physics update, deal damage to all enemies inside the sun
	*/
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

		foreach (var area in GetOverlappingAreas())
		{
			if (area is HealthComponent healthComponent)
			{
				double distanceToEnemySquared = (Position - healthComponent.GlobalPosition).LengthSquared(); // use squared distance since that is what physics dictate
				healthComponent.TakeDamage(CalculateAttack(distanceToEnemySquared));
			}
		}
    }

	/**
	Calculate the damage depending on the squared distance to the enemy
	TODO we might want to change this in the future for a better calculation. It probalby needs a scaling factor depending on the size of the sun
	*/
	public Attack CalculateAttack(double distanceToEnemySquared)
	{
		if (distanceToEnemySquared <= 1.0)
		{
			return _attack;
		}
		Attack attack = new Attack(_attack);
		attack.damage /=  distanceToEnemySquared;
		return attack;
	}

	/**
	Override OnAreaEntered to make it empty since we do not want to use it.
	All Damage is applied in _PhysicsProcess
	*/
    public override void OnAreaEntered(Area2D area)
    {
    }
}
