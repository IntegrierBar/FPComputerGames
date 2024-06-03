using Godot;
using System;

public partial class Moving : State
{
    [Export]
    public double SPEED = 100; ///< Movement speed 
    
    
    [ExportGroup("States")]
    [Export]
    public State Idle; ///< Reference to Idle state 
    //[Export]
    //public State Moving;
    [Export]
    public State Dashing; ///< Reference to Dashing state
    [Export]
    public State SpellCasting; ///< Reference to SpellCasting state 


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
