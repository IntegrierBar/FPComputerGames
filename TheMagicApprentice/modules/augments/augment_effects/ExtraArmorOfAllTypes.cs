using Godot;
using System;


/**
AugmentEffect to increase the armor of all types
*/
public partial class ExtraArmorofAllTypes : AugmentEffect
{
    [Export]
    private double _extraArmor = 5;

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
}
