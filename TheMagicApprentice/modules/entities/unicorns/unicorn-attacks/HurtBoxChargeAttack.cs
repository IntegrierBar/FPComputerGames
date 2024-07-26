using Godot;
using System;

public partial class HurtBoxChargeAttack : Area2D
{
    private Attack _attack; ///< attack from the unicorn that is applied to the player when hit by the charge attack
    private double _timeLeft; ///< time left in the charge state

    /**
	Monitoring is set to false so that the collisions from unicorn and PC only damage the PC if the 
    unicorn is performing an attack.
	*/
    public override void _Ready()
    {
        CallDeferred("SetMonitoringAndPhysics", false); 
        base._Ready();
    }

    /**
	Count down the time left in the charge attack state. When the charge attack is over, end the attack.
	*/
    public override void _PhysicsProcess(double delta)
    {
        _timeLeft -= delta;
        if (_timeLeft <= 0)
        {
            EndAttack(); 
        }
        base._PhysicsProcess(delta);
    }

    /**
	The unicorn should only be able to detect collisions with the PC.
    If the unicorn collides with the PC, the damage function is called.
    The player can be hit by the unicorns charge attack several times -> invicibility frames for the 
    player will probably be necessary.
	*/
    public void OnAreaEntered(Area2D area)
    {
        if (area is HealthComponent healthComponent) // check if area is a health component and if true cast it as a healthcomponent under the name healthComponent
		{
			healthComponent.TakeDamage(_attack);
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
	Start attack begins the attack by setting the attack and enabling monitoring such that collisions 
    with the player can be detected. The time left is also set to track how long the unicorn will remain 
    in the attack state.
	*/
    public void StartAttack(Attack attack, double timeLeft)
    {
        SetAttack(attack);
        CallDeferred("SetMonitoringAndPhysics", true); 
        _timeLeft = timeLeft;
    }


    /**
	When the attack is ended, disable the monitoring again so that the player cannot be hurt. 
    */
    public void EndAttack()
    {
        CallDeferred("SetMonitoringAndPhysics", false); 
    }

    /**
	Sets Monitoring and enables or disables the physics process of the hurt box. This function 
    always has to be called deferred to avoid changes in the hitboxes while calculations are done.
	*/
    private void SetMonitoringAndPhysics(bool enabled)
    {
        Monitoring = enabled;
        SetPhysicsProcess(enabled);
    }

}
