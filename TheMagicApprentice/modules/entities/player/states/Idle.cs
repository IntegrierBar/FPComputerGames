using Godot;
using System;

public partial class Idle : State
{
    /**
    References to all states we can transition into
    */
    [ExportGroup("States")]
    //[Export]
    //public State Idle;
    [Export]
    public State Moving;
    [Export]
    public State Dashing;
    [Export]
    public State SpellCasting;


    public override State ProcessPhysics(double delta)
    {
        // we out these checks into then Physics process function in order to take care of the case 
        // that we exit spellcasting or dashing state while pressing another movement button.
        if (Input.GetVector("left", "right", "up", "down") != Vector2.Zero)
        {
            return Moving;
        }
        if (Input.IsActionPressed("dash"))
        {
            return Dashing;
        }
        if (@Input.IsActionPressed("cast"))
        {
            return SpellCasting;
        }
        return null;
    }


    public override void UpdateAnimations()
    {
        
    }
}
