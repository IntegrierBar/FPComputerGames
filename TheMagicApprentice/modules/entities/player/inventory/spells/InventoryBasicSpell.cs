using Godot;
using System;

public partial class InventoryBasicSpell : InventorySpell
{
    /**
    Additionaly to base class also adds itself to the correct Basic Spell Group
    */
    public override void _Ready()
    {
        base._Ready();
        switch (MagicType)
        {
            case MagicType.SUN:
                AddToGroup(Globals.SunBasicSpellGroup);
                break;
            case MagicType.COSMIC:
                AddToGroup(Globals.CosmicBasicSpellGroup);
                break;
            case MagicType.DARK:
                AddToGroup(Globals.DarkBasicSpellGroup);
                break;
        }

    }
}
