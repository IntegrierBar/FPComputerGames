using Godot;
using System;


/**
Health bar skript
*/
public partial class Healthbar : TextureProgressBar
{
	private TextureProgressBar _healthbar;
	private Timer _timer;

	private double _healthPoints;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_healthbar = GetNode<TextureProgressBar>("Healthbar");
		_timer = GetNode<Timer>("Timer");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void InitHealthbar(double health)
	{
		_healthPoints = health;
		MaxValue = _healthPoints;
		Value = _healthPoints;
		_healthbar.MaxValue = _healthPoints;
		_healthbar.Value = _healthPoints;
	}

	public void SetHealthPoints(double newHealth)
	{
		double oldHealth = _healthPoints;
		_healthPoints = Math.Min(MaxValue, newHealth);
		_healthbar.Value = _healthPoints;

		if (_healthPoints <= 0)
		{
			QueueFree();
		}
		if (_healthPoints < oldHealth)
		{
			_timer.Start();
		}
		else
		{
			Value = _healthPoints;
		}
	}

	public void OnTimerTimeout()
	{
		Value = _healthPoints;
	}
}
