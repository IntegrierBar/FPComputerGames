using Godot;
using System;

public partial class PlayerDeath : State
{
    [Export]
    public double DeathAnimationTime = 1.0; ///< Duration of the death animation 
    private double _timeLeft = 0.0; 

    
    public override void Enter()
    {
        //Animations.Play("death");
        _timeLeft = DeathAnimationTime;
        GetTree().Paused = true;
        base.Enter();
    }

    public override State ProcessPhysics(double delta)
    {
        _timeLeft -= delta;
        if (_timeLeft <= 0)
        {
            // TODO need to connect this somehow to a screen where player can choose what to do now
        }
        return base.ProcessPhysics(delta);
    }
}
