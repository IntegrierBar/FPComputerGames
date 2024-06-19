using Godot;
using System;

public partial class InventorySunBeam : InventorySpell
{
	/**
	Create the base spell of the correct type
	*/
    public override void Cast(Vector2 playerPosition, Vector2 targetPosition)
    {
        base.Cast(playerPosition, targetPosition);

		// instanciate the spell and cast it to the correct class
		SunBeam spell = _spellScene.Instantiate() as SunBeam;
		System.Diagnostics.Debug.Assert(spell is not null); // check that it is not null


        Attack attack = new Attack(Damage, MagicType, PlayerHealthComponent);
        spell.Init(attack, playerPosition, targetPosition);
        GetTree().Root.AddChild(spell);
        //spell.Position = playerPosition;

    }
}
