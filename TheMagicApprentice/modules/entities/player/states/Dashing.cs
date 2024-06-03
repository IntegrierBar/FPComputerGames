using Godot;
using System;

public partial class Dashing : State
{
    [Export]
    public CollisionShape2D HitBox;
    [Export]
    public double SPEED = 400;
    [Export]
    public double DASH_TIME = 0.3;
    private double _timeLeft = 0;

    /**
    References to all states we can transition into
    */
    [ExportGroup("States")]
    [Export]
    public State Idle;
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
