using Godot;
using System;


/**
The spell base class.
Every spell inherits this class.
*/
[GlobalClass]
public partial class Spell : Area2D
{
	protected Attack _attack;	///< Contains damage, type and caster reference for damage calculation


	[Export]
	public double MaxLifeTimeInSeconds = 5.0; ///< How long the spell exists at maximum until it is removed from the world
	public double _timeLeftUntilDeletion; ///< Time left until deletion

	/**
	When the scene is added to the scene tree, we initialize _duration with the value from Duration and connect the OnAreaEntered Method to the AreaEntered signal
	*/
    public override void _Ready()
    {
        _timeLeftUntilDeletion = MaxLifeTimeInSeconds;
		AreaEntered += OnAreaEntered; 
    }

	/**
	Every Spell has to overide this method as it is used to initialize the spell on creation
	*/
	public virtual void Init(Attack attack, Vector2 playerPosition, Vector2 targetPosition) 
	{
		_attack = attack;
	}

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
	Default implementation of the OnAreaEntered method for all Spells
	Simply checks if the area is a HealthComponent and calls TakeDamage().
	*/
	public virtual void OnAreaEntered(Area2D area)
	{
		if (area is HealthComponent healthComponent) // check if area is a health component and if true cast it as a healthcomponent under the name healthComponent
		{
			healthComponent.TakeDamage(_attack);
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
