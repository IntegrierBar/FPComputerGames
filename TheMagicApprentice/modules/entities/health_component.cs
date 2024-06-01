using Godot;
using System;
using System.Collections.Generic;

public partial class HealthComponent : Area2D
{
	[Signal]
	public delegate void DeathEventHandler();

	[Export]
	private double MaxHP = 100;
	private double CurrentHP {get; set;}

	private Dictionary<MagicType, double> Armor;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		CurrentHP = MaxHP;

		Armor = new Dictionary<MagicType, double>
		{
			{MagicType.SUN, 30},
			{MagicType.COSMIC, 30},
			{MagicType.DARK, 30},
		};
	}

	
	public void TakeDamage(Attack attack)
	{
		if (Armor[attack.magicType] > 100.0)
		{
			if (attack.attacker is not null)
			{
				Attack reflectedAttack = new Attack(attack.damage * (Armor[attack.magicType]/100.0 - 1.0), attack.magicType, this);
				attack.attacker.TakeDamage(reflectedAttack);	// NOTE: if both target and attacker have armor over 100 this gives endles loop. However only the player is allowed to have more then 100 armor, so this is fine
			}
			return;
		}

		CurrentHP -= attack.damage * (1.0 - Armor[attack.magicType]/100.0);
		if (CurrentHP <= 0.0)
		{
			EmitSignal(SignalName.Death);
		}
	}

	/// Setter for MaxHP. Also automatically resets CurrentHP since this function is only called outside dungeon
	public void SetMaxHP(double newMaxHP)
	{
		System.Diagnostics.Debug.Assert(newMaxHP > 0);
		MaxHP = newMaxHP;
		CurrentHP = MaxHP;
	}

	/// Setter for Armor
	public void SetArmor(double armorSun, double armorCosmic, double armorDark)
	{
		Armor = new Dictionary<MagicType, double>
		{
			{MagicType.SUN, armorSun},
			{MagicType.COSMIC, armorCosmic},
			{MagicType.DARK, armorDark},
		};
	}

	/// Getter for CurrentHP. Is only used by tests
	public double GetCurrentHP()
	{
		return CurrentHP;
	}
}
