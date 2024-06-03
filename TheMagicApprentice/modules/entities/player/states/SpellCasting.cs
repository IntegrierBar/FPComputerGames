using Godot;
using System;

public partial class SpellCasting : State
{
    /**
    References to all states we can transition into
    */
    [ExportGroup("States")]
    [Export]
    public State Idle;
    [Export]
    public State Moving;
    [Export]
    public State Dashing;
    //[Export]
    //public State SpellCasting;
}
