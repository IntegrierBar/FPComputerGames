using Godot;
using System;
using System.Linq;

/**
AugmentEffect to increase the damage for all skills of the type _magicType
*/
[GlobalClass]
public partial class PercentDamageForOneType : AugmentEffect
{
    [Export]
    private MagicType _magicType = MagicType.SUN; ///< Which MagicType gets alls spells extra damage

    [Export]
    private double _damageIncreaseFaktor = 1.05; ///< factor of the damage increase

    public override void Equip(SceneTree sceneTree)
    {
        // Increase the damage of all spells of the Type _magicType
        foreach (InventorySpell spell in sceneTree.GetNodesInGroup(Globals.GetGroupNameOfSpellsOfType(_magicType)).OfType<InventorySpell>())
        {
            spell.Damage *= _damageIncreaseFaktor;
        }
    }


    public override string Description()
    {
        return "Increases damage of all " + _magicType.ToString() + " spells by " + (_damageIncreaseFaktor - 1).ToString("0.%");
    }
}
