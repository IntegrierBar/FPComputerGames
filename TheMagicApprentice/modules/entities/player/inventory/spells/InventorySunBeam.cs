using Godot;
using System;

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
