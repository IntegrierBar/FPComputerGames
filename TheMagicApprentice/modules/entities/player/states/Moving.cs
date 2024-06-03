using Godot;
using System;

public partial class Moving : State
{
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

    public override State ProcessPhysics(double delta)
    {
        if (Input.GetVector("left", "right", "up", "down") == Vector2.Zero)
        {
            //GD.Print("returned Idle from Moving");
            return Idle;
        }
        return null;
    }
}
