using Godot;
using System;

public partial class Movement : Node
{
	[Export] public float MaxSpeed = 100.0f;
	[Export] public float Acceleration = 500.0f;
	[Export] public float Deceleration = 500.0f;

	private CharacterBody2D _parent;
	private Vector2 _currentDirection = Vector2.Zero;
    private float _currentSpeed = 0f;
	private Vector2 _velocity = Vector2.Zero;
	private Vector2 _externalForce = Vector2.Zero;

	public override void _Ready()
	{
		_parent = GetParent<CharacterBody2D>();
	}

	public override void _PhysicsProcess(double delta)
    {
        ApplyMovement(delta);
		_parent.MoveAndSlide();
    }

    public void SetMovement(Vector2 direction, float speed)
    {
        _currentDirection = direction.Normalized();
        _currentSpeed = speed;
    }

    private void ApplyMovement(double delta)
    {
        if (_currentDirection != Vector2.Zero)
        {
            _velocity = _velocity.MoveToward(_currentDirection * _currentSpeed, (float)(Acceleration * delta));
        }
        else
        {
            _velocity = _velocity.MoveToward(Vector2.Zero, (float)(Deceleration * delta));
        }

        _velocity += _externalForce;
        _externalForce = _externalForce.MoveToward(Vector2.Zero, (float)(Deceleration * delta));

        _parent.Velocity = _velocity;
        _parent.MoveAndSlide();
		_currentDirection = Vector2.Zero;
		_currentSpeed = 0f;
    }

	public void ApplyForce(Vector2 force)
	{
		_externalForce += force;
	}
}
