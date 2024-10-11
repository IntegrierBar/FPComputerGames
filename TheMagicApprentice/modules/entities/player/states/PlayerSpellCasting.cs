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
        InventorySpell spell = null;
        IEnumerable<InventorySpell> spellsToCast = null;
        UISpellSlot uISpellBox = null;
        if (Input.IsActionPressed("spell1"))
        {
            spell = (Parent as Player).GetPlayerSkill(0);
            spellsToCast = GetTree().GetNodesInGroup("spell1").OfType<InventorySpell>();
            uISpellBox = GetTree().GetFirstNodeInGroup(Globals.SpellSlot1) as UISpellSlot;
        }
        else if (CurseHandler.IsActive(Curse.SKILL_1_ONLY))
        {
            spell = null;
        }
        else if (Input.IsActionPressed("spell2"))
        {
            spell = (Parent as Player).GetPlayerSkill(1);
            spellsToCast = GetTree().GetNodesInGroup("spell2").OfType<InventorySpell>();
            uISpellBox = GetTree().GetFirstNodeInGroup(Globals.SpellSlot2) as UISpellSlot;
        }
        else if (CurseHandler.IsActive(Curse.SKILL_3_DISABLED))
        {
            spell = null;
        }
        else if (Input.IsActionPressed("spell3"))
        {
            spell = (Parent as Player).GetPlayerSkill(2);
            spellsToCast = GetTree().GetNodesInGroup("spell3").OfType<InventorySpell>();
            uISpellBox = GetTree().GetFirstNodeInGroup(Globals.SpellSlot3) as UISpellSlot;
        }

        // if the spell is null, then we can imideately exit since that means we just tried to cast a spell that does not exist. But should technically not happen, thanks to CanEnter
        if (spell is null)
        {
            _timeLeft = 0.0;
            return;
        }

        System.Diagnostics.Debug.Assert(uISpellBox is not null, "uISpellBox is null in PlayerSpellCasting");
        
        // otherwise cast the spells
        foreach (var inventorySpell in spellsToCast)
        {
            inventorySpell.Cast(Parent.GlobalPosition, Parent.GetGlobalMousePosition());
        }
        

        // finally retrieve the CastTime from the first spell which is always the main spell
        _timeLeft = spell.CastTime;

        uISpellBox.Cast(spell.CoolDown); // show in UI that spell was cast
        Animations.Play("cast");
    }

    public override bool CanEnter()
    {
        InventorySpell spell = null;
        UISpellSlot uISpellBox = null;
        if (Input.IsActionPressed("spell1"))
        {
            spell = (Parent as Player).GetPlayerSkill(0);
            uISpellBox = GetTree().GetFirstNodeInGroup(Globals.SpellSlot1) as UISpellSlot;
        }
        else if (Input.IsActionPressed("spell2"))
        {
            spell = (Parent as Player).GetPlayerSkill(1);
            uISpellBox = GetTree().GetFirstNodeInGroup(Globals.SpellSlot2) as UISpellSlot;
        }
        else if (Input.IsActionPressed("spell3"))
        {
            spell = (Parent as Player).GetPlayerSkill(2);
            uISpellBox = GetTree().GetFirstNodeInGroup(Globals.SpellSlot3) as UISpellSlot;
        }

        if (spell is null || uISpellBox.IsOnCooldown())
        {
            return false;
        }
        return true;
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
