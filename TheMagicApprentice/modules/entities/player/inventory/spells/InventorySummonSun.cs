using Godot;
using System;

/**
Class for the Node responsible for summoning the Sun spell
*/
public partial class InventorySummonSun : InventorySpell
{
	/**
    Additionaly to base class also adds itself to the Summon Sun Spell Group
    */
    public override void _Ready()
    {
        base._Ready();
        AddToGroup(Globals.SummonSunSpellGroup);
    }
}
