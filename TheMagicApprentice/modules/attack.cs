using Godot;
using System;


/**
Basic class that encapsulates everything responsible for an attack
*/
public partial class Attack
{
    public double damage; ///< The damage value of the attack 
    public MagicType magicType; ///< The magic type of the attack 
    public HealthComponent attacker; ///< Reference to the HealthComponent of the attacker 

    public Attack(double damageValue, MagicType type, HealthComponent attackerNode)
    {
        damage = damageValue;
        magicType = type;
        attacker = attackerNode;
    }

    /**
    Copy constructor
    */
    public Attack(Attack attack)
        : this(attack.damage, attack.magicType, attack.attacker)
    {}
}
