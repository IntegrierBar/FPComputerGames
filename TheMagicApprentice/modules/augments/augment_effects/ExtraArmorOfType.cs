using Godot;
using System;


/**
AugmentEffect to increase the Armor of one magic type
*/
public partial class ExtraArmorOfType : AugmentEffect
{
    [Export]
    private MagicType _magicType = MagicType.SUN;

    [Export]
    private double _extraArmor = 10;

    /**
    Get the players Healthcomponent and modify it
    */
    public override void Equip(SceneTree sceneTree)
    {
        HealthComponent healthComponentPlayer = (sceneTree.GetFirstNodeInGroup(Globals.PlayerGroup) as Player).GetNode("HealthComponent") as HealthComponent;
        System.Diagnostics.Debug.Assert(healthComponentPlayer is not null, "Could not get HealthComponent of player");

        healthComponentPlayer.ModifyArmor(_magicType, _extraArmor);
    }

    /**
    remember to remove the armor when unequiping the item
    */
    public override void UnEquip(SceneTree sceneTree)
    {
        HealthComponent healthComponentPlayer = (sceneTree.GetFirstNodeInGroup(Globals.PlayerGroup) as Player).GetNode("HealthComponent") as HealthComponent;
        System.Diagnostics.Debug.Assert(healthComponentPlayer is not null, "Could not get HealthComponent of player");

        healthComponentPlayer.ModifyArmor(_magicType, -_extraArmor);
    }
}
