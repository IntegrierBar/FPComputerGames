using Godot;
using Godot.Collections;
using System;

/**
AugmentEffect to cast one spell, when casting another.
The UnEquiping is handled in the player class.

The basic spells are never the _castedSpell.
Combinations only exist in one direction.
*/
[GlobalClass]
public partial class CastAdditionalSpell : AugmentEffect
{
    [Export]
    private SpellName _castedSpell = SpellName.SunBasic; ///< Name of the spell on which we want to cast another

    [Export]
    private SpellName _additionalSpell = SpellName.SunBeam; ///< Name of the spell which gets additionaly casted

    /**
    If _castedSpell is in a Spell Group, we also add _additionalSpell
    */
    public override void Equip(SceneTree sceneTree)
    {
        // get reference to the InventorySpells of _castedSpell and _additionalSpell
        InventorySpell castedSpell = sceneTree.GetFirstNodeInGroup(Globals.GetGroupNameOfSpell(_castedSpell)) as InventorySpell;
        InventorySpell additionalSpell = sceneTree.GetFirstNodeInGroup(Globals.GetGroupNameOfSpell(_additionalSpell)) as InventorySpell;

        // loop over all spell groups and if the first spell is inside the group, add the second spell to the group
        foreach (var spellGroup in new string[3] {Globals.Spell1, Globals.Spell2, Globals.Spell3})
        {
            if (castedSpell.IsInGroup(spellGroup))
            {
                additionalSpell.AddToGroup(spellGroup);
            }
        }
    }

    /**
    We need to manually unequip _additionalSpell from the group if _castedSpell is in the group.
    This is ugly and should be changed once skill tree exists. But it works for how, since cyclic dependancies do not exist
    TODO this should not be neccessary anymore
    */
    public override void UnEquip(SceneTree sceneTree)
    {
        // get reference to the InventorySpells of _castedSpell and _additionalSpell
        InventorySpell castedSpell = sceneTree.GetFirstNodeInGroup(Globals.GetGroupNameOfSpell(_castedSpell)) as InventorySpell;
        InventorySpell additionalSpell = sceneTree.GetFirstNodeInGroup(Globals.GetGroupNameOfSpell(_additionalSpell)) as InventorySpell;

        // loop over all spell groups and if the first spell is inside the group, remove the second spell from the group
        foreach (var spellGroup in new string[3] {Globals.Spell1, Globals.Spell2, Globals.Spell3})
        {
            if (castedSpell.IsInGroup(spellGroup))
            {
                additionalSpell.RemoveFromGroup(spellGroup);
            }
        }
    }



    public override string Description()
    {
        return "Cast " + _additionalSpell.ToString() + " when casting " + _castedSpell.ToString();
    }
}
