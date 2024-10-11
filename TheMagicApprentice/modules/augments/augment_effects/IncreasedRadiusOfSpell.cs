using Godot;
using System;


/**
AugmentEffect to increase the radius of a spell
*/
[GlobalClass]
public partial class IncreasedRadiusOfSpell : OnCastAugmentEffect
{
    [Export]
    private SpellName _spellName = SpellName.BlackHole; ///< Which spell gets effected by the AugmentEffect

    [Export]
    private float _sizeIncreaseFactor = 1.3f; ///< factor by how much the size of the spell increases

    /**
    Adds itself to the OnCastAugment List of the correct Inventory spell
    */
    public override void Equip(SceneTree sceneTree)
    {
        InventorySpell inventorySpell = sceneTree.GetFirstNodeInGroup(Globals.GetGroupNameOfSpell(_spellName)) as InventorySpell;
        inventorySpell.AddOnCastAugmentEffect(this);
    }

    /**
    Increases the size of the spell
    */
    public override void OnCast(Spell spell, Vector2 playerPosition, Vector2 targetPosition)
    {
        spell.Scale *= _sizeIncreaseFactor;
    }


    public override string Description()
    {
        return "Increase the scale of " + _spellName.ToString() + " by " + (_sizeIncreaseFactor - 1).ToString("0.%");
    }
}
