using Godot;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;

public partial class MeleeAttackHurtBox : Area2D
{
    private Attack _attack;
    private double _timeLeft;

    /**
	Monitoring is set to false so that the collisions from slime and PC only damage the PC if the slime is in the Attacking state.
    Monitoring is toggled on in the SlimeAttacking state. 
	*/
    public override void _Ready()
    {
        CallDeferred("SetMonitoringAndPhysics", false); 
        base._Ready();
    }

    public override void _PhysicsProcess(double delta)
    {
        _timeLeft -= delta;
        if (_timeLeft <= 0)
        {
            CallDeferred("SetMonitoringAndPhysics", false); 
        }
        base._PhysicsProcess(delta);
    }

    /**
	The slime should only be able to detect collisions with the PC.
    If the slime collides with the PC, the damage function is called.
    Afterwards Monitoring is set to false again to prevent the player from taking damage from the same attack a second time.
    If the slime does not hit the PC, monitoring is turned of in the SlimeAttacking state again.
	*/
    public void OnAreaEntered(Area2D area)
    {
        GD.Print("hit area");
		GD.Print(area);
        if (area is HealthComponent healthComponent) // check if area is a health component and if true cast it as a healthcomponent under the name healthComponent
		{
			healthComponent.TakeDamage(_attack);
            CallDeferred("SetMonitoringAndPhysics", false); // Ensures that the PC can only be damaged once per attack
		}
    }

    /**
	Attack is currently set from the Attacking state in the ranged attack function, so right before the attack is made.
	*/
    public void SetAttack(Attack attack)
    {
        _attack = attack;
    }

    public void StartAttack(Attack attack, double timeLeft)
    {
        SetAttack(attack);
        CallDeferred("SetMonitoringAndPhysics", true); 
        _timeLeft = timeLeft;
    }

    public void EndAttack()
    {
        CallDeferred("SetMonitoringAndPhysics", false); 
    }

    private void SetMonitoringAndPhysics(bool enabled)
    {
        Monitoring = enabled;
        SetPhysicsProcess(enabled);
    }
}
