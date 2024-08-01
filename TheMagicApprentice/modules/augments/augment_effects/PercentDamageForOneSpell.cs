using Godot;
using System;

/**
AugmentEffect to increase the damage for one skill by _damageIncreaseFaktor
*/
[GlobalClass]
public partial class PercentDamageForOneSpell : AugmentEffect
{
    [Export]
    private SpellName _spellName = SpellName.SunBasic; ///< Name of the spell to get extra damage

    [Export]
    private double _damageIncreaseFaktor = 1.1; ///< factor of the damage increase

    public override void Equip(SceneTree sceneTree)
    {
        InventorySpell spell = sceneTree.GetFirstNodeInGroup(Globals.GetGroupNameOfSpell(_spellName)) as InventorySpell;
        System.Diagnostics.Debug.Assert(spell is not null, "spell name is wrong");
        spell.Damage *= _damageIncreaseFaktor;
    }


    public override string Description()
    {
        return "Increase the damage of " + _spellName.ToString() + " by " + (_damageIncreaseFaktor - 1).ToString("0.%");
    }
}
