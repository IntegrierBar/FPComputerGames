using Godot;
using System;

/**
Class for the Node responsible for summoning the Moon Light spell
*/
public partial class InventoryMoonLight : InventorySpell
{
	/**
    Additionaly to base class also adds itself to the Moon Light Spell Group
    */
    public override void _Ready()
    {
        base._Ready();
        AddToGroup(Globals.MoonLightSpellGroup);
    }
}
