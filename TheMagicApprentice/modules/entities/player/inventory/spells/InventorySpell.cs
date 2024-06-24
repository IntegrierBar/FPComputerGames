using Godot;
using System;


/**
Base class for all spells inside the inventory
*/
[GlobalClass]
public partial class InventorySpell : Node
{
	[Export]
	public double CastTime = 0.3; ///< How long it takes to cast the spell
	[Export]
	public double CoolDown = 1;	///< How long until the spell can be used again

	[Export]
	public double BaseDamage = 30; ///< base damage of the skill
	public double Damage; ///< actual damage of the spell
	[Export]
	public MagicType MagicType = MagicType.SUN; ///< Magic type of the spell

	[Export]
	protected HealthComponent _playerHealthComponent; ///< Reference to the players HealthComponent


	[Export]
	protected PackedScene _spellScene; // Packed Scene of the spells actual scene


	/**
	call ResetDamage in order to set Damage to BaseDamage.
	Adds the spell to the correct base groups depending on the MagicType
	*/
    public override void _Ready()
    {
		// For debugging purposes make sure that the exports have been set
		System.Diagnostics.Debug.Assert(_playerHealthComponent is not null, "_playerHealthComponent in InventorySpell is null"); 
		System.Diagnostics.Debug.Assert(_spellScene is not null, "_spellScene in InventorySpell is null"); 


        base._Ready();
		ResetDamage();

		switch (MagicType)
		{
			case MagicType.SUN:
				AddToGroup(Globals.SunSpellGroup);
				break;
			case MagicType.COSMIC:
				AddToGroup(Globals.CosmicSpellGroup);
				break;
			case MagicType.DARK:
				AddToGroup(Globals.DarkSpellGroup);
				break;
		}
    }


    /**
	Casts the spell by instanciating the scene and initializing the spell
	*/
    public virtual void Cast(Vector2 playerPosition, Vector2 targetPosition) 
	{
		Spell spell = _spellScene.Instantiate() as Spell;
		System.Diagnostics.Debug.Assert(spell is not null, "_spellScene is empty, Could not create szene"); // check that it is not null

        Attack attack = new Attack(Damage, MagicType, _playerHealthComponent);
        spell.Init(attack, playerPosition, targetPosition);

		// TODO in the future do not add to Tree Root but to Room
        GetTree().Root.AddChild(spell);
	}


	/**
	Reset Damage to BaseDamage
	*/
	public void ResetDamage()
	{
		System.Diagnostics.Debug.Assert(BaseDamage >= 0, "BaseDamage is negative"); // check that it is non negative
		Damage = BaseDamage;
	}
}
