using Godot;
using System;

public partial class HurtBoxStompingAttack : Area2D
{
    [Export]
    public Timer timer;  ///< timer used to track the delay time of the stomping attack (damage should only be applied when the unicorn hits the ground)
    private Attack _attack; ///< attack from the unicorn that is applied to the player when hit by the stomping attack
    private double _activeHurtboxTimeLeft; ///< time left in which the hurtbox is active. This time period is rather short.

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
	Count down the time left in which the hurtbox should stay active. When the time is up end the attack (disable the hurtbox).
	*/
    public override void _PhysicsProcess(double delta)
    {
        _activeHurtboxTimeLeft -= delta;
        if (_activeHurtboxTimeLeft <= 0)
        {
            EndAttack();
        }
        base._PhysicsProcess(delta);
    }

    /**
	The unicorn should only be able to detect collisions with the PC.
    If the unicorns hurtbox on the ground collides with the PC, the damage function is called.
    The player can only be hit once by the stomping attack, so the attack ends when the player was damaged.
	*/
    public void OnAreaEntered(Area2D area)
    {
        GD.Print("hit area");
		GD.Print(area);
        if (area is HealthComponent healthComponent) // check if area is a health component and if true cast it as a healthcomponent under the name healthComponent
		{
			healthComponent.TakeDamage(_attack);
            EndAttack();
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
	Start attack begins the attack by setting the attack. Also sets the timer to the delay time, after 
    which the damafinf phase of the attack begins. Starts the timer. 
	*/
    public void StartAttack(Attack attack, double delayTime)
    {
        SetAttack(attack);
        timer.WaitTime = delayTime;
        timer.Start();
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
        GetNode<CollisionShape2D>("CollisionShapeStompingAttack").Disabled = !enabled;
    }

    /**
	The timer ends when the unicorn hits the floor with its hooves. Then the damage should be applied in 
    the area around the unicorn. Therefore the Hurtbox is activated when the timer runs out and a short
    period of time is set, in which the hurtbox is active. 
	*/
    public void OnTimerTimeout()
    {
        _activeHurtboxTimeLeft = 0.1;
        CallDeferred("SetMonitoringAndPhysics", true);
    }
}
