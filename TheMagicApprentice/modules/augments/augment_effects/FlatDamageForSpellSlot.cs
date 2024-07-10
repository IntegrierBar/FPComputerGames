using Godot;
using System;
using System.Linq;

/**
AugmentEffect to increase the damage of all spells in slot _slot by a flat amount
*/
public partial class FlatDamageForSpellSlot : AugmentEffect
{
    [Export]
    private uint _slot = 0;

    [Export]
    private double _flatDamageIncrease = 20;

    public override void Equip(SceneTree sceneTree)
    {
        // Increase the damage of all spells of the Type _magicType
        foreach (InventorySpell spell in sceneTree.GetNodesInGroup(Globals.GetGroupNameOfSpellsInSlot(_slot)).OfType<InventorySpell>())
        {
            spell.Damage += _flatDamageIncrease;
        }
    }
}
