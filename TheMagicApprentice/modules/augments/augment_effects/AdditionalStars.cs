using Godot;
using System;


/**
AugmentEffect to increase the amount of stars spawned by StarRain by a faktor
*/
[GlobalClass]
public partial class AdditionalStars : AugmentEffect
{
    [Export]
    private double _factor = 1.1; ///< multiplicative factor of how many more stars get spawned

    /**
    Change the static member AmountStarsToSpawn of InventoryStarRain
    */
    public override void Equip(SceneTree sceneTree)
    {
        (sceneTree.GetFirstNodeInGroup(Globals.StarRainSpellGroup) as InventoryStarRain).AmountStarsToSpawn *= _factor;
    }

    /**
    Reverts the change to AmountStarsToSpawn
    */
    public override void UnEquip(SceneTree sceneTree)
    {
        (sceneTree.GetFirstNodeInGroup(Globals.StarRainSpellGroup) as InventoryStarRain).AmountStarsToSpawn /= _factor;
    }

    public override string Description()
    {
        return "Cast " + (_factor - 1).ToString("0.%") + " more stars";
    }
}
