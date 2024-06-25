using Godot;
using System;

/**
Class for the Node responsible for summoning the Sun Beam Spell
*/
public partial class InventorySunBeam : InventorySpell
{
    /**
    Additionaly to base class also adds itself to the Sun Beam Spell Group
    */
    public override void _Ready()
    {
        base._Ready();
        AddToGroup(Globals.SunBeamSpellGroup);
    }
}
