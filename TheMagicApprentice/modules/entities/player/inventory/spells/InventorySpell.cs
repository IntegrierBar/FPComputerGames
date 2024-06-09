using Godot;
using System;


/**
Base class for all spells inside the inventory
*/
[GlobalClass]
public partial class InventorySpell : Node
{
	[Export]
	public double BaseDamage = 10; ///< base damage of the skill
	public double Damage = 10; ///< actual damage of the spell
	[Export]
	public MagicType MagicType = MagicType.SUN; ///< Magic type of the spell
	[Export]
	public HealthComponent Caster; ///< Reference to the caster (i.e. the player)


	[Export]
	protected PackedScene _spellScene; // Packed Scene of the spells actual scene

	/**
	Casts the spell by instanciating the scene and initializing the spell
	*/
	public virtual void Cast(Vector2 casterPosition, Vector2 targetPosition) {}

	/**
	Equips the spell by adding it to the correct group
	*/
	public virtual void Equip(int slot) 
	{
		AddToGroup("spell" + slot);
	}


	/**
	Unequip the spell by removing it from all spell groups
	*/
	public virtual void UnEquip()
	{
		RemoveFromGroup("spell1");
		RemoveFromGroup("spell2");
		RemoveFromGroup("spell3");
	}
}
