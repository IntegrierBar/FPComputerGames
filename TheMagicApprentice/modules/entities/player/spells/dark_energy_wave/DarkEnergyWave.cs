using Godot;
using System;
using System.Collections.Generic;

/**
The spell object of the spell dark energy wave.

Every tick it increases in size.
Enemies hit are pushed away.
*/
public partial class DarkEnergyWave : Spell
{
	private Sprite2D _sprite;
	private CpuParticles2D _particles;
	private CollisionShape2D _collisionShape;
	private float _initialSize = 10f; ///< Initial size in pixels
	private float _elapsedTime = 0f; ///< Time elapsed since spell creation
	private Dictionary<CharacterBody2D, Vector2> _pushedSlimes = new Dictionary<CharacterBody2D, Vector2>(); ///< Tracks pushed slimes and their velocities
	private float _initialPushVelocity = 350f; ///< Initial velocity applied to pushed slimes
	private float _pushTime = 0.6f; ///< Duration of the push effect

	public override void Init(Attack attack, Vector2 playerPosition, Vector2 targetPosition) 
	{
		base.Init(attack, playerPosition, targetPosition);

		GlobalPosition = playerPosition;
		 _sprite = GetNode<Sprite2D>("Sprite2D");
		_particles = GetNode<CpuParticles2D>("Particles");
		_collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
		UpdateSpriteSize(_initialSize);
	}

	/**
	Increase the size every tick
	*/
	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		
		_elapsedTime += (float)delta;
		float newSize = CalculateSize(_elapsedTime);
		UpdateSize(newSize);

		UpdatePushedSlimes((float)delta);
	}

	/**
	Clean up the spell when it's removed from the scene
	*/
	public override void _ExitTree()
	{
		base._ExitTree();
		UpdateSize(0);
	}

	/**
	If we hit an enemy we push them back
	*/
	public override void OnAreaEntered(Area2D area)
	{
		if (area is HealthComponent healthComponent) // check if area is a health component and if true cast it as a healthcomponent under the name healthComponent
		{

			CharacterBody2D slime = healthComponent.GetParent() as CharacterBody2D;
			if (slime != null && !_pushedSlimes.ContainsKey(slime))
			{
				healthComponent.TakeDamage(_attack);
				Vector2 pushDirection = (slime.GlobalPosition - GlobalPosition).Normalized();
				_pushedSlimes[slime] = pushDirection * _initialPushVelocity;
			}
		}
	}

	/**
	Calculates the size of the spell based on elapsed time
	*/
	private float CalculateSize(float time)
	{
		float initialVelocity = _particles.InitialVelocityMax;
		float maxLinearAccel = (float) Mathf.Abs(_particles.LinearAccelMax);
		
		// Simplified calculation assuming constant acceleration
		float distance = initialVelocity * time + maxLinearAccel * time * time;
		
		return _initialSize + distance; // Size in pixels
	}

	/**
	Updates both sprite and collision shape size
	*/
	private void UpdateSize(float size)
	{
		UpdateSpriteSize(size);
		UpdateCollisionShapeSize(size);
	}

	/**
	Updates the sprite size based on the given size
	*/
	private void UpdateSpriteSize(float size)
	{
		Vector2 textureSize = _sprite.Texture.GetSize();
		float scale = size / Mathf.Max(textureSize.X, textureSize.Y);
		_sprite.Scale = new Vector2(scale, scale);
	}

	/**
	Updates the collision shape size based on the given size
	*/
	private void UpdateCollisionShapeSize(float size)
	{
		if (_collisionShape.Shape is CircleShape2D circleShape)
		{
			circleShape.Radius = size / 2; // Divide by 2 because the radius is half the diameter
		}
	}

	/**
	Updates the positions and velocities of pushed slimes
	*/
	private void UpdatePushedSlimes(float delta)
	{
		List<CharacterBody2D> slimesToRemove = new List<CharacterBody2D>();

		foreach (var kvp in _pushedSlimes)
		{
			CharacterBody2D slime = kvp.Key;
			Vector2 pushVelocity = kvp.Value;

			// Update slime position
			slime.GlobalPosition += pushVelocity * delta;

			// Decay push velocity
			pushVelocity = pushVelocity.MoveToward(Vector2.Zero, _initialPushVelocity / _pushTime * delta);

			if (pushVelocity.LengthSquared() < 1f)
			{
				slimesToRemove.Add(slime);
			}
			else
			{
				_pushedSlimes[slime] = pushVelocity;
			}
		}

		// Remove slimes with zero push velocity
		foreach (var slime in slimesToRemove)
		{
			_pushedSlimes.Remove(slime);
		}
	}
}
