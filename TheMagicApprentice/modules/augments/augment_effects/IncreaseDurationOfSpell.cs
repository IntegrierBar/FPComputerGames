using Godot;
using System;


/**
AugmentEffect to increase the duration of a spell
*/
[GlobalClass]
public partial class IncreaseDurationOfSpell : OnCastAugmentEffect
{
    [Export]
    private SpellName _spellName = SpellName.BlackHole; ///< Which spell gets effected by the AugmentEffect

    [Export]
    private float _durationIncreaseFactor = 1.4f; ///< factor by how much the size of the spell increases

    /**
    Adds itself to the OnCastAugment List of the correct Inventory spell
    */
    public override void Equip(SceneTree sceneTree)
    {
        InventorySpell inventorySpell = sceneTree.GetFirstNodeInGroup(Globals.GetGroupNameOfSpell(_spellName)) as InventorySpell;
        inventorySpell.AddOnCastAugmentEffect(this);
    }

    /**
    Increases the duration of the spell
    */
    public override void OnCast(Spell spell)
    {
        spell._timeLeftUntilDeletion *= _durationIncreaseFactor;
    }


    public override string Description()
    {
        return "Increase the duration of " + _spellName.ToString() + " by " + (_durationIncreaseFactor - 1).ToString("0.%");
    }
}
