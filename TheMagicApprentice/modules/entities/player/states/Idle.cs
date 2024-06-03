using Godot;
using System;

public partial class Idle : State
{
    /// References to all states we can transition into
    [ExportGroup("States")]
    //[Export]
    //public State Idle;
    [Export]
    public State Moving;
    [Export]
    public State Dashing;
    [Export]
    public State SpellCasting;


    public override State ProcessInput(InputEvent @event)
    {
        return base.ProcessInput(@event);
    }


    public override void UpdateAnimations()
    {
        
    }
}
