using Godot;
using System;


[GlobalClass]
public partial class Attack : Node
{
    public float damage;
    public Enums.MagicType magicType;
    public Node2D attacker;
}
