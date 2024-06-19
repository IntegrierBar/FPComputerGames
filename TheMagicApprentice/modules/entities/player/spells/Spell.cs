using Godot;
using System;


/**
The spell base class.
Every spell inherits this class.
*/
[GlobalClass]
public partial class Spell : Area2D
{
	[Export]
	public double MaxLifeTimeInSeconds = 5.0; ///< How long the spell exists at maximum until it is removed from the world
	public double _timeLeftUntilDeletion; ///< Time left until deletion

	/**
	When the scene is added to the scene tree, we initialize _duration with the value from Duration
	*/
    public override void _Ready()
    {
        _timeLeftUntilDeletion = MaxLifeTimeInSeconds;
    }

	/**
	Every Spell has to overide this method as it is used to initialize the spell on creation
	*/
	public virtual void Init(Attack attack, Vector2 direction) {}

    /**
	If the spell has reached its maximal duration, we delete it
	*/
    public override void _PhysicsProcess(double delta)
    {
		_timeLeftUntilDeletion -= delta;
		if (_timeLeftUntilDeletion <= 0)
		{
			//GD.Print("deleted");
			QueueFree();
		}
    }

	/**
	Getter for _timeLeftUntilDeletion.
	Is only used in tests.
	*/
	public double GetTimeUntilDeleteion()
	{
		return _timeLeftUntilDeletion;
	}
}
