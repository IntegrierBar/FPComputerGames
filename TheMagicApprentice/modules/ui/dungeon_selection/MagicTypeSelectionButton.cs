using Godot;
using System;

public partial class MagicTypeSelectionButton : TextureButton
{
	[Export]
	public MagicType MagicType { get; set; } = MagicType.SUN;
	[Export]
	public SpellName SpellName { get; set; } = SpellName.SunBasic;

	[Signal]
	public delegate void MagicTypeSelectedEventHandler(MagicType magicType);

	public override void _Ready()
	{
	
		// Get reference to player and skill tree
		var player = GetTree().GetFirstNodeInGroup(Globals.PlayerGroup) as Player;
		var skillTree = player.GetNode<SkillTree>("SkillTree");

		// Disable button if spell is not unlocked
		Disabled = !skillTree.IsUnlocked(SpellName);
		Connect("pressed", new Callable(this, nameof(OnButtonPressed)));
	}

	private void OnButtonPressed()
	{
		EmitSignal(SignalName.MagicTypeSelected, (int)MagicType);
	}
}
