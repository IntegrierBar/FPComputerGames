using Godot;
using System;

/**
Dash state of the player charackter.
During dashing the hitbox of the player is disabled
*/
public partial class PlayerDashing : State
{
    [Export]
    public CollisionShape2D HitBox; ///< Reference of the HitBox of the player 
    [Export]
    public double SPEED = 400; ///< Speed of the dash
    [Export]
    public double DASH_TIME = 0.3; ///< Duration of the dash 
    private double _timeLeft = 0;

    
    [ExportGroup("States")]
    [Export]
    public State Idle; ///< Reference to the Idle state
    //[Export]
    //public State Moving;
    //[Export]
    //public State Dashing;
    //[Export]
    //public State SpellCasting;

    public override void Enter()
    {
        base.Enter();
        _timeLeft = DASH_TIME;
        Parent.Velocity = Input.GetVector("left", "right", "up", "down") * (float)SPEED;

        HitBox.Disabled = true;
    }

    public override void Exit()
    {
        HitBox.Disabled = false;
        base.Exit();
    }

    public override State ProcessPhysics(double delta)
    {
        _timeLeft -= delta;
        if (_timeLeft <= 0.0)
        {
            // always return to idle state as described in 
            return Idle;
        }

        Parent.MoveAndSlide();

        return base.ProcessPhysics(delta);
    }
}
