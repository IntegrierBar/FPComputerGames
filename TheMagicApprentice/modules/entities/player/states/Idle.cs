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


    public override State ProcessInput(InputEvent @event)
    {
        //GD.Print("called input method");
        if (Input.GetVector("left", "right", "up", "down") != Vector2.Zero)
        {
            //GD.Print("returned Moving");
            return Moving;
        }
        return null;
    }


    public override void UpdateAnimations()
    {
        
    }
}
