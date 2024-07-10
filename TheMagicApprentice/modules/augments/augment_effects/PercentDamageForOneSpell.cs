using Godot;
using System;

/**
AugmentEffect to increase the damage for one skill by _damageIncreaseFaktor
*/
public partial class PercentDamageForOneSpell : AugmentEffect
{
    [Export]
    private SpellName _spellName = SpellName.SunBasic;

    [Export]
    private double _damageIncreaseFaktor = 1.1;

    public override void Equip(SceneTree sceneTree)
    {
        InventorySpell spell = sceneTree.GetFirstNodeInGroup(Globals.GetGroupNameOfSpell(_spellName)) as InventorySpell;
        System.Diagnostics.Debug.Assert(spell is not null, "spell name is wrong");
        spell.Damage *= _damageIncreaseFaktor;
    }
}
