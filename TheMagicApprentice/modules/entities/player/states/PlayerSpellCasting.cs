using Godot;
using System;
using System.ComponentModel.DataAnnotations;

public partial class PlayerSpellCasting : State
{
    private double _timeLeft = 0.0; ///< Varaible to track how long we remain in this state

    [ExportGroup("States")]
    [Export]
    public State Idle; ///< Reference to the Idle state
    //[Export]
    //public State Moving;
    //[Export]
    //public State Dashing;
    //[Export]
    //public State SpellCasting;

    
    /*!
    Get the correct spell by checking whether spell1, spell2 or spell3 was cast.
    Then cast it and set the time left to the duration given by the spell.
    TODO: Spells are not yet implemented so this does nothing.
    */
    public override void Enter()
    {
        base.Enter();
        Godot.Collections.Array<Node> spells = null;
        if (Input.IsActionPressed("spell1"))
        {
            spells = GetTree().GetNodesInGroup("spell1");
        }
        else if (Input.IsActionPressed("spell2"))
        {
            spells = GetTree().GetNodesInGroup("spell2");
        }
        else if (Input.IsActionPressed("spell3"))
        {
            spells = GetTree().GetNodesInGroup("spell3");
        }
        
        // if the spell is null, then we can imideately exit since that means we just tried to cast a spell that does not exist
        if (spells is null || spells.Count == 0)
        {
            _timeLeft = 0.0;
            return;
        }
        // otherwise cast the spell and get spellcasting time from it
        foreach (InventorySpell spell in spells)
        {
            spell.Cast(Parent.Position, Parent.GetGlobalMousePosition());
        }
        
        _timeLeft = 0.2;
        Animations.Play("cast");
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
