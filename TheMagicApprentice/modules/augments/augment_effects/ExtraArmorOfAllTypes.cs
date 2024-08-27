using Godot;
using System;


/**
AugmentEffect to increase the armor of all types
*/
[GlobalClass]
public partial class ExtraArmorOfAllTypes : AugmentEffect
{
    [Export]
    private double _extraArmor = 7; ///< How much extra armor gets added to the player. Use 7, since then player needs all 15 slots to get 100 armor.

    /**
    Get the players Healthcomponent and modify it
    */
    public override void Equip(SceneTree sceneTree)
    {
        HealthComponent healthComponentPlayer = (sceneTree.GetFirstNodeInGroup(Globals.PlayerGroup) as Player).GetNode("HealthComponent") as HealthComponent;
        System.Diagnostics.Debug.Assert(healthComponentPlayer is not null, "Could not get HealthComponent of player");

        healthComponentPlayer.ModifyArmor(MagicType.SUN, _extraArmor);
        healthComponentPlayer.ModifyArmor(MagicType.COSMIC, _extraArmor);
        healthComponentPlayer.ModifyArmor(MagicType.DARK, _extraArmor);
    }

    /**
    remember to remove the armor when unequiping the item
    */
    public override void UnEquip(SceneTree sceneTree)
    {
        HealthComponent healthComponentPlayer = (sceneTree.GetFirstNodeInGroup(Globals.PlayerGroup) as Player).GetNode("HealthComponent") as HealthComponent;
        System.Diagnostics.Debug.Assert(healthComponentPlayer is not null, "Could not get HealthComponent of player");

        healthComponentPlayer.ModifyArmor(MagicType.SUN, -_extraArmor);
        healthComponentPlayer.ModifyArmor(MagicType.COSMIC, -_extraArmor);
        healthComponentPlayer.ModifyArmor(MagicType.DARK, -_extraArmor);
    }


    public override string Description()
    {
        return "Adds " + _extraArmor + " armor to all types";
    }
}
