using Godot;
using System;
using System.Linq;

/**
AugmentEffect to increase the damage for all skills of the type _magicType
*/
public partial class PercentDamageForOneType : AugmentEffect
{
    [Export]
    private MagicType _magicType = MagicType.SUN;

    [Export]
    private double _damageIncreaseFaktor = 1.05;

    public override void Equip(SceneTree sceneTree)
    {
        // Get the name of the group
        string magicTypeGroupName = "";
        switch (_magicType)
        {
            case MagicType.SUN:
                magicTypeGroupName = Globals.SunSpellGroup;
                break;
            case MagicType.COSMIC:
                magicTypeGroupName = Globals.CosmicSpellGroup;
                break;
            case MagicType.DARK:
                magicTypeGroupName = Globals.DarkSpellGroup;
                break;
        }
        
        // Increase the damage
        foreach (InventorySpell spell in sceneTree.GetNodesInGroup(magicTypeGroupName).OfType<InventorySpell>())
        {
            spell.Damage *= _damageIncreaseFaktor;
        }
    }
}
