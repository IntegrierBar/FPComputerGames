using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

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
        IEnumerable<InventorySpell> spells = null;
        if (Input.IsActionPressed("spell1"))
        {
            spells = GetTree().GetNodesInGroup("spell1").OfType<InventorySpell>();
        }
        else if (CurseHandler.IsActive(Curse.SKILL_1_ONLY))
        {
            spells = null;
        }
        else if (Input.IsActionPressed("spell2"))
        {
            spells = GetTree().GetNodesInGroup("spell2").OfType<InventorySpell>();
        }
        else if (CurseHandler.IsActive(Curse.SKILL_3_DISABLED))
        {
            spells = null;
        }
        else if (Input.IsActionPressed("spell3"))
        {
            spells = GetTree().GetNodesInGroup("spell3").OfType<InventorySpell>();
        }

        // if the spell is null, then we can imideately exit since that means we just tried to cast a spell that does not exist
        if (spells is null || !spells.Any())
        {
            _timeLeft = 0.0;
            return;
        }
        
        // otherwise cast all spells
        foreach (InventorySpell spell in spells.OfType<InventorySpell>())
        {
            spell.Cast(Parent.Position, Parent.GetGlobalMousePosition());
        }
        // finally retrieve the CastTime from the first spell which is always the main spell
        _timeLeft = spells.First().CastTime;
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
