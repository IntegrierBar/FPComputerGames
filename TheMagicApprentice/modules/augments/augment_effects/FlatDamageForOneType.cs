using Godot;
using System;
using System.Linq;

/**
AugmentEffect to increase the damage of all spells of a type by a flat amount
*/
public partial class FlatDamageForOneType : AugmentEffect
{
    [Export]
    private MagicType _magicType = MagicType.SUN;

    [Export]
    private double _flatDamageIncrease = 10;

    public override void Equip(SceneTree sceneTree)
    {
        // Increase the damage of all spells of the Type _magicType
        foreach (InventorySpell spell in sceneTree.GetNodesInGroup(Globals.GetGroupNameOfSpellsOfType(_magicType)).OfType<InventorySpell>())
        {
            spell.Damage += _flatDamageIncrease;
        }
    }
}
