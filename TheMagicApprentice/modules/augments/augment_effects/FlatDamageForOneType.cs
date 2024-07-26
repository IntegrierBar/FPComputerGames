using Godot;
using System;
using System.Linq;

/**
AugmentEffect to increase the damage of all spells of a type by a flat amount
*/
[GlobalClass]
public partial class FlatDamageForOneType : AugmentEffect
{
    [Export]
    private MagicType _magicType = MagicType.SUN; ///< The MagicType of which the spells get extra damage

    [Export]
    private double _flatDamageIncrease = 10; ///< The flat increase of the spelldamage

    public override void Equip(SceneTree sceneTree)
    {
        // Increase the damage of all spells of the Type _magicType
        foreach (InventorySpell spell in sceneTree.GetNodesInGroup(Globals.GetGroupNameOfSpellsOfType(_magicType)).OfType<InventorySpell>())
        {
            spell.Damage += _flatDamageIncrease;
        }
    }


    public override string Description()
    {
        return "Increases damage of all " + _magicType.ToString() + " spells by " + _flatDamageIncrease;
    }
}
