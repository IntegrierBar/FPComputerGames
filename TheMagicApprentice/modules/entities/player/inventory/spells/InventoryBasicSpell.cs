using Godot;
using System;

public partial class InventoryBasicSpell : InventorySpell
{
    /**
	Create the base spell of the correct type
	*/
    public override void Cast(Vector2 playerPosition, Vector2 targetPosition)
    {
        base.Cast(playerPosition, targetPosition);

		// instanciate the spell and cast it to the correct class
		BasicSpell spell = _spellScene.Instantiate() as BasicSpell;
		System.Diagnostics.Debug.Assert(spell is not null); // check that it is not null

		GetTree().Root.AddChild(spell);
        Attack attack = new Attack(Damage, MagicType, PlayerHealthComponent);
        spell.Init(attack, playerPosition, targetPosition);
        //spell.Position = playerPosition;

    }
}