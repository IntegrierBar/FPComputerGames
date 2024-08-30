using Godot;
using System;
using System.Linq;

/**
AugmentEffect to increase the damage of all spells in slot _slot by a flat amount.
(Uses all spells currently in the spell group)
*/
[GlobalClass]
public partial class FlatDamageForSpellSlot : AugmentEffect
{
    [Export]
    private uint _slot = 0; ///< Index of which slot gets changed

    [Export]
    private double _flatDamageIncrease = 15; ///< The flat increase of the spelldamage

    public override void Equip(SceneTree sceneTree)
    {
        // Increase the damage of all spells of the Type _magicType
        foreach (InventorySpell spell in sceneTree.GetNodesInGroup(Globals.GetGroupNameOfSpellsInSlot(_slot)).OfType<InventorySpell>())
        {
            spell.Damage += _flatDamageIncrease;
        }
    }


    public override string Description()
    {
        return "Increases the damage of spells in slot " + (_slot + 1) + " by " + _flatDamageIncrease;
    }
}
