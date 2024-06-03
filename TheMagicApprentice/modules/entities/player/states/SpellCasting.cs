using Godot;
using System;

public partial class SpellCasting : State
{
    private double _timeLeft = 0.0;
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
        // TODO get the Spell by checking whether 1, 2 or 3 are pressed and then get all Data, like duration from the spell
        /*Spell spell;
        if (Input.IsActionPressed("spell1"))
        {
            
        }
        else if (Input.IsActionPressed("spell2"))
        {
            
        }
        else if (Input.IsActionPressed("spell3"))
        {
            
        }
        
        // if the spell is null, then we can imideately exit since that means we just tried to cast a spell that does not exist
        if (spell is null)
        {
            _timeLeft = 0.0;
            return;
        }
        // otherwise cast the spell and get spellcasting time from it
        spell.cast(Parent.Position, Parent.GetLocalMousePosition());
        */
        _timeLeft = 1.0;
        //Animations.Play("cast");
    }


    public override State ProcessPhysics(double delta)
    {
        _timeLeft -= delta;
        if (_timeLeft <= 0.0)
        {
            return Idle;
        }
        return base.ProcessPhysics(delta);
    }
}
