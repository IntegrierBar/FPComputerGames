using Godot;
using System;


/**
Class for the Node responsible for summoning the Black Hole spell
*/
public partial class InventoryBlackHole : InventorySpell
{
	/**
    Additionaly to base class also adds itself to the Black Hole Spell Group
    */
    public override void _Ready()
    {
        base._Ready();
        AddToGroup(Globals.BlackHoleSpellGroup);
    }
}
