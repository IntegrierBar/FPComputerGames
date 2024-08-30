using Godot;
using System;

[Tool]
public partial class FlashlightTextureGenerator : Node
{
	[Export] public int TestProperty { get; set; } = 0;

	public override void _Ready()
	{
		GD.Print("FlashlightTextureGenerator is ready!");
	}
}
