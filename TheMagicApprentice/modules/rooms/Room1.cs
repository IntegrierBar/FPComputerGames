using Godot;
using System;
using System.ComponentModel;

// This script was used to spawn slimes into room 1 to test their functionality. It can be removed or overwritten.  

public partial class Room1 : Node2D
{

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
		PackedScene scene = GD.Load<PackedScene>("res://modules/entities/slimes/slime.tscn");

		Slime slime_sun_ranged = scene.Instantiate() as Slime;
		Slime slime_sun_melee = scene.Instantiate() as Slime;
		Slime slime_dark_melee = scene.Instantiate() as Slime;
		AddChild(slime_dark_melee);
		AddChild(slime_sun_melee);
		AddChild(slime_sun_ranged);

		slime_sun_ranged.SetSlimeProperties(MagicType.SUN, SlimeSize.SMALL, SlimeAttackRange.RANGED,new Vector2(-75.0f, 75.0f));
		slime_sun_melee.SetSlimeProperties(MagicType.SUN, SlimeSize.SMALL, SlimeAttackRange.MELEE, new Vector2(50.0f, 125.0f));
		slime_dark_melee.SetSlimeProperties(MagicType.DARK, SlimeSize.SMALL, SlimeAttackRange.MELEE, new Vector2(200.0f, 60.0f));
		

	}
}
