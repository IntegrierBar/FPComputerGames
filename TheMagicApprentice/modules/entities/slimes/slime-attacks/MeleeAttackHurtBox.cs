using Godot;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;

public partial class MeleeAttackHurtBox : Area2D
{
    private Attack _attack;
    private double _timeLeft;

    /**
	Monitoring is set to false so that the collisions from slime and PC only damage the PC if the slime is in the Attacking state.
    Monitoring is toggled on in StartAttack function. 
	*/
    public override void _Ready()
    {
        CallDeferred("SetMonitoringAndPhysics", false); 
        base._Ready();
    }

    /**
	Count down the time left in the attack state. When the attack state is over, end the attack.
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
	The slime should only be able to detect collisions with the PC.
    If the slime collides with the PC, the damage function is called.
    Afterwards Monitoring is set to false again to prevent the player from taking damage from the same attack a second time.
	*/
    public void OnAreaEntered(Area2D area)
    {
        GD.Print("hit area");
		GD.Print(area);
        if (area is HealthComponent healthComponent) // check if area is a health component and if true cast it as a healthcomponent under the name healthComponent
		{
			healthComponent.TakeDamage(_attack);
            EndAttack(); // End attack turns monitoring off, this ensures that the PC can only be damaged once per attack
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
    with the player can be detected. The time left is also set to track how long the slime will remain 
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
