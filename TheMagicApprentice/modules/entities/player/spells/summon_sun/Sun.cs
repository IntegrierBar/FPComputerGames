using Godot;
using System;
using System.Linq;

/**
The spell object of the spell summon sun.
Deals damage every tick to enemies inside its radius
*/
public partial class Sun : Spell
{
	/**
	Positions the Sun at the mouse position.
	Create the Attack is the dmg per second. Since damage is applied 60 times per second, we need to divide by 60
	*/
	public override void Init(Attack attack, Vector2 playerPosition, Vector2 targetPosition) 
	{
		base.Init(attack, playerPosition, targetPosition);
		_attack.damage /= 60.0;

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
				double distanceToEnemySquared = (Position - healthComponent.GlobalPosition).Length(); // use squared distance since that is what physics dictate
				healthComponent.TakeDamage(CalculateAttack(distanceToEnemySquared));
			}
		}
    }

	/**
	Calculate the damage depending on the distance to the enemy.
	Use linear scaling since otherwise not fun.
	*/
	public Attack CalculateAttack(double distanceToEnemy)
	{
		// TODO once the sun spell is finalized remove the calculations here and hardcode the values.
		double radiusSun = 15.0;
		double maximumReach = 150.0;
		double a = 1.0/(radiusSun - maximumReach);
		double b = - maximumReach / (radiusSun - maximumReach);
		if (distanceToEnemy <= radiusSun) // the number here is the radius of the sun. Inside this, the enemy takes the full damage. Outside the radius scales down quadratically. TODO if sun is changed. need to change here as well
		{
			return _attack;
		}
		Attack attack = new Attack(_attack);
		attack.damage *= a * distanceToEnemy + b;
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
