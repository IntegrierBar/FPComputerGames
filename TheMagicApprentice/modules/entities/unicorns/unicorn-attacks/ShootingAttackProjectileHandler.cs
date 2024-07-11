using Godot;
using System;

public partial class ShootingAttackProjectileHandler : Node
{

	[Export]
	public int AmountProjectilesToSpawn = 10; ///< How many projectiles are created 
	private int _projectilesLeftToSpawn;	///< Variable to track how many projectiles still have to be spawned

	private Attack _attack;
	private Vector2 _unicornPosition;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
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
		PackedScene scene = GD.Load<PackedScene>("res://modules/entities/unicorns/unicorn-attacks/ShootingAttackProjectile.tscn");
		ShootingAttackProjectile projectile = scene.Instantiate() as ShootingAttackProjectile;
		GetTree().Root.AddChild(projectile); // Add the ranged attack projectile to the scene tree

		projectile.Init(_attack);
		projectile.Position = _unicornPosition; // ranged attack spawns at the position of the slime
	}

}
