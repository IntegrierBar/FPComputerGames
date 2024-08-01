using Godot;
using System;

public partial class Movement : Node
{
	[Export] public float MaxSpeed = 100.0f;
	[Export] public float Acceleration = 500.0f;
	[Export] public float Deceleration = 500.0f;

	private CharacterBody2D _parent;
	private Vector2 _velocity = Vector2.Zero;
	private Vector2 _externalForce = Vector2.Zero;

	public override void _Ready()
	{
		_parent = GetParent<CharacterBody2D>();
	}

	public void ApplyMovement(Vector2 direction, double delta, float speed)
	{
		if (direction != Vector2.Zero)
		{
			_velocity = _velocity.MoveToward(direction * speed, (float)(Acceleration * delta));
		}
		else
		{
			_velocity = _velocity.MoveToward(Vector2.Zero, (float)(Deceleration * delta));
		}

		_velocity += _externalForce;
		_externalForce = _externalForce.MoveToward(Vector2.Zero, (float)(Deceleration * delta));

		_parent.Velocity = _velocity;
		_parent.MoveAndSlide();
	}

	public void ApplyForce(Vector2 force)
	{
		_externalForce += force;
	}
}
