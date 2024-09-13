using Godot;
using System;
using System.Collections.Generic;

/**
The spell object of the spell black hole
TODO: Currently the gravity does not affect anything. we probalby have to use gravity or something like that inside the slime class
*/
public partial class BlackHole : Spell
{
	
	[Export] private float _pullStrength = 50f;
	[Export] private float _maxPullStrength = 100f;
	[Export] private float _minPullStrength = 10f;
	[Export] private float _maxPullDistance = 200f;

	CpuParticles2D _particles;
	Area2D _gravityArea;
	private List<Slime> _affectedSlimes = new List<Slime>();
	private double startingDamage;
	

	public override void Init(Attack attack, Vector2 playerPosition, Vector2 targetPosition) 
	{
		base.Init(attack, playerPosition, targetPosition);

		GlobalPosition = targetPosition;
		_particles = GetNode<CpuParticles2D>("Particles");
		startingDamage = _attack.damage;

		_gravityArea = GetNode<Area2D>("GravityArea");
		_gravityArea.AreaEntered += OnGravityAreaEntered;
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		StopParticles();
	}

	// Modified: Check if the area is a Slime and add it to the list
	private void OnGravityAreaEntered(Area2D area)
	{
		if (area.GetParent() is Slime slime)
		{
			_affectedSlimes.Add(slime);
		}
	}

	private void OnGravityAreaExited(Area2D area)
	{
		if (area.GetParent() is Slime slime)
		{
			_affectedSlimes.Remove(slime);
		}
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
		PullSlimes((float)delta);


		_attack.damage = startingDamage * delta;
		foreach (var area in GetOverlappingAreas())
		{
			if (area is HealthComponent healthComponent)
			{
				
				healthComponent.TakeDamage(_attack);
				
			}
		}
	}

	private void PullSlimes(float delta)
	{
		foreach (var slime in _affectedSlimes.ToArray())
		{
			if (IsInstanceValid(slime))
			{
				Vector2 direction = GlobalPosition - slime.GlobalPosition;
				float distance = direction.Length();
				direction = direction.Normalized();

				// Calculate pull strength based on distance
				float pullStrength = Mathf.Lerp(_maxPullStrength, _minPullStrength, distance / _maxPullDistance);
				pullStrength = Mathf.Clamp(pullStrength, _minPullStrength, _maxPullStrength);

				// Move the slime towards the black hole
				slime.GlobalPosition += direction * pullStrength * delta;
			}
			else
			{
				_affectedSlimes.Remove(slime);
			}
		}
	}

	private void StopParticles()
	{
		if (_particles != null)
		{
			Vector2 globalPos = _particles.GlobalPosition;
			_particles.GetParent().RemoveChild(_particles);
			GetTree().Root.AddChild(_particles);
			_particles.GlobalPosition = globalPos;
			_particles.Emitting = false;
			GetTree().CreateTimer(_particles.Lifetime).Connect("timeout", Callable.From(() => _particles.QueueFree()));
		}
	}

	/**
	Override OnAreaEntered to make it empty since we do not want to use it.
	All Damage is applied in _PhysicsProcess
	*/
	public override void OnAreaEntered(Area2D area)
	{
	}
}
