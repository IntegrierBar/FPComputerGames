using Godot;
using System;


/**
AugmentEffect to increase the Armor of one magic type
*/
[GlobalClass]
public partial class ExtraArmorOfType : AugmentEffect
{
    [Export]
    private MagicType _magicType = MagicType.SUN; ///< Type of armor we want to effect

    [Export]
    private double _extraArmor = 12; ///< How much armor gets added. Use 12 so that with 9 player has over 100 Armor of the type.

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


    public override string Description()
    {
        return "Adds " + _extraArmor + " armor to type " + _magicType.ToString();
    }
}
