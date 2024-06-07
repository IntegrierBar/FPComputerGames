using Godot;
using System;
using System.Collections.Generic;

public partial class HealthComponent : Area2D
{
    [Signal]
    public delegate void DeathEventHandler(); ///< Signal that gets emitted if the entities health reaches 0 

    [Export]
    private double MaxHP = 100; ///< Maximum HP of the entity 
	private double _currentHP; ///< current HP of the entitiy 

    private Dictionary<MagicType, double> Armor = new Dictionary<MagicType, double> {
            {MagicType.SUN, 0},
            {MagicType.COSMIC, 0},
            {MagicType.DARK, 0},
        }; ///< Armor of the entity 


    /**
	During initialization we set the currentHP to the MaxHP
	*/
    public override void _Ready()
    {
        _currentHP = MaxHP;
    }


    /**
	Gets called whenever the hitbox of the Healthcomponent collides with a hurtbox.
	Processes the Attack and if currentHP reaches zero emits the signal Death.
	*/
    public void TakeDamage(Attack attack)
    {
        if (Armor[attack.magicType] > 100.0)
        {
            if (attack.attacker is not null)
            {
                Attack reflectedAttack = new Attack(attack.damage * (Armor[attack.magicType] / 100.0 - 1.0), attack.magicType, this);
                attack.attacker.TakeDamage(reflectedAttack);    // NOTE: if both target and attacker have armor over 100 this gives endles loop. However only the player is allowed to have more then 100 armor, so this is fine
            }
            return;
        }

        _currentHP -= attack.damage * (1.0 - Armor[attack.magicType] / 100.0);
        if (_currentHP <= 0.0)
        {
            EmitSignal(SignalName.Death);
        }
    }

    /**
	Setter for MaxHP. Also automatically resets CurrentHP since this function is only called outside dungeon
	*/
    public void SetMaxHP(double newMaxHP)
    {
        System.Diagnostics.Debug.Assert(newMaxHP > 0);
        MaxHP = newMaxHP;
        _currentHP = MaxHP;
    }

    /**
	Setter for Armor
	*/
    public void SetArmor(double armorSun, double armorCosmic, double armorDark)
    {
        Armor = new Dictionary<MagicType, double>
        {
            {MagicType.SUN, armorSun},
            {MagicType.COSMIC, armorCosmic},
            {MagicType.DARK, armorDark},
        };
    }

    /**
	Getter for CurrentHP. Is only used by tests
	*/
    public double GetCurrentHP()
    {
        return _currentHP;
    }
}
