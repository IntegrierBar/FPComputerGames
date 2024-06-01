using Godot;
using System;


public partial class Attack
{
    public double damage;
    public MagicType magicType;
    public HealthComponent attacker;

    public Attack(double damageValue, MagicType type, HealthComponent attackerNode)
    {
        damage = damageValue;
        magicType = type;
        attacker = attackerNode;
    }
}
