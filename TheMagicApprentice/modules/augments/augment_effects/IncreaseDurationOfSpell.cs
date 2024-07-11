using Godot;
using System;

public partial class IncreaseDurationOfSpell : OnCastAugmentEffect
{
    [Export]
    private SpellName _spellName = SpellName.BlackHole; ///< Which spell gets effected by the AugmentEffect

    [Export]
    private float _durationIncreaseFactor = 2.5f; ///< factor by how much the size of the spell increases

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
}
