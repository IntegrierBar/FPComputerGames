using Godot;
using System;


/**
AugmentEffect to increase the HP of player by percentage
*/
[GlobalClass]
public partial class HPIncreaseAugmentEffect : AugmentEffect
{
    [Export]
    private double _HPIncreaseFaktor = 1.2; ///< Factor by how much the HP gets increased

    /**
    Get the players Healthcomponent and modify it
    */
    public override void Equip(SceneTree sceneTree)
    {
        HealthComponent healthComponentPlayer = (sceneTree.GetFirstNodeInGroup(Globals.PlayerGroup) as Player).GetNode("HealthComponent") as HealthComponent;
        System.Diagnostics.Debug.Assert(healthComponentPlayer is not null, "Could not get HealthComponent of player");

        healthComponentPlayer.SetMaxHP(healthComponentPlayer.GetMaxHP() * _HPIncreaseFaktor);
    }

    /**
    remember to remove the armor when unequiping the item
    */
    public override void UnEquip(SceneTree sceneTree)
    {
        HealthComponent healthComponentPlayer = (sceneTree.GetFirstNodeInGroup(Globals.PlayerGroup) as Player).GetNode("HealthComponent") as HealthComponent;
        System.Diagnostics.Debug.Assert(healthComponentPlayer is not null, "Could not get HealthComponent of player");

        healthComponentPlayer.SetMaxHP(healthComponentPlayer.GetMaxHP() / _HPIncreaseFaktor);
    }


    public override string Description()
    {
        return "Increases HP by " + (_HPIncreaseFaktor - 1).ToString("0.%");
    }
}
