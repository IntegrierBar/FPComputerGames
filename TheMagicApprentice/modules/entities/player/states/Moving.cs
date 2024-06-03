using Godot;
using System;

public partial class Moving : State
{
    [Export]
    public double SPEED = 100;
    /**
    References to all states we can transition into
    */
    [ExportGroup("States")]
    [Export]
    public State Idle;
    //[Export]
    //public State Moving;
    [Export]
    public State Dashing;
    [Export]
    public State SpellCasting;


    public override State ProcessInput(InputEvent @event)
    {
        if (@event.IsActionPressed("dash"))
        {
            return Dashing;
        }
        if (@event.IsActionPressed("cast"))
        {
            return SpellCasting;
        }
        return base.ProcessInput(@event);
    }

    public override State ProcessPhysics(double delta)
    {
        Vector2 direction = Input.GetVector("left", "right", "up", "down");
        if (direction == Vector2.Zero)
        {
            //GD.Print("returned Idle from Moving");
            return Idle;
        }

        Parent.Velocity = (float)SPEED * direction;
        Parent.MoveAndSlide();
        
        return null;
    }
}
