using Godot;
using System;


/**
Class for the Node responsible for summoning the Dark Engery Wave spell
*/
public partial class InventoryDarkEnergyWave : InventorySpell
{
	/**
    Additionaly to base class also adds itself to the Dark Energy Wave Spell Group
    */
    public override void _Ready()
    {
        base._Ready();
        AddToGroup(Globals.DarkEnergyWaveSpellGroup);
    }
}
