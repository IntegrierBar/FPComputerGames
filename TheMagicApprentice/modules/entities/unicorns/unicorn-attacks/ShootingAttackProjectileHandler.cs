using Godot;
using System;

public partial class ShootingAttackProjectileHandler : Node
{

	[Export]
	public int AmountProjectilesToSpawn = 10; ///< How many projectiles are created 
	private int _projectilesLeftToSpawn;	///< Variable to track how many projectiles still have to be spawned

	private Attack _attack; ///< attack that is given to the projectiles to damage the player
	private Vector2 _unicornPosition; ///< position of the unicorn at the beginning of the shooting attack

	/**
	As long as there are still projectiles that need to be spawned, spawn one projectile per physics
	frame and reduce the number of projectiles left to spawn by one. 
	*/
	public override void _PhysicsProcess(double delta)
	{
		if (_projectilesLeftToSpawn > 0)
		{
			SpawnProjectile();
			_projectilesLeftToSpawn -= 1;
		} 
	}

	/**
	Attack is currently set from the Attacking state in the ranged attack function, so right before the attack is made.
	*/
    public void SetAttack(Attack attack)
    {
        _attack = attack;
    }

	/**
	Starts shooting attack by setting the projectiles left to spawn and initialising the attack used for the projectiles.
	*/
	public void StartShootingAttack(Attack attack, Vector2 unicornPosition)
	{
		SetAttack(attack);
		_projectilesLeftToSpawn = AmountProjectilesToSpawn;
		_unicornPosition = unicornPosition;
	}

	/**
	Spawns a projectile. 
	*/
	private void SpawnProjectile()
	{
		// Get a reference to the current room. The current room is the child of the RoomHandler
		Node2D room = GetTree().GetFirstNodeInGroup(Globals.RoomHandlerGroup)?.GetChild(0) as Node2D;
		if (room is not null) // only cast if room exists
		{
			PackedScene scene = GD.Load<PackedScene>("res://modules/entities/unicorns/unicorn-attacks/ShootingAttackProjectile.tscn");
			ShootingAttackProjectile projectile = scene.Instantiate() as ShootingAttackProjectile;
			room.AddChild(projectile); // Add the ranged attack projectile to the scene tree

			projectile.Init(_attack); // Iniitialise the attack such that the projectile can damage the player on impact
			projectile.GlobalPosition = _unicornPosition; // ranged attack spawns at the position of the slime
		}
	}

}
