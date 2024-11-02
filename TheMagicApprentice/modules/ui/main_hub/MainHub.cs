using Godot;

public partial class MainHub : BaseMenu
{
	public override void _Ready()
	{
		MenuType = MenuManager.MenuType.MainHub;
		base._Ready();
	}

	private void OnExitButtonPressed()
	{
		SetRootMenu(MenuManager.MenuType.MainMenu);
	}

	/**
	Open the dungeon selection menu by calling the OpenDungeonSelection function of the player which sets the visibility of the Menu to true
	*/
	private void OnDungeonButtonPressed()
	{
		SetRootMenu(MenuManager.MenuType.MainHub);
		PushMenu(MenuManager.MenuType.DungeonSelection);
	}

	/**
	Open the augment inventory by calling the OpenAugmentInventory function of the player which sets the visibility of the Inventory to true
	*/
	private void OpenAugmentInventory()
	{
		(GetTree().GetFirstNodeInGroup(Globals.PlayerGroup) as Player).OpenAugmentInventory();
	}

	/**
	Open the fuse augment menu by calling the OpenFuseAugments function of the player which sets the visibility of the Menu to true
	*/
	private void OpenFuseAugments()
	{
		(GetTree().GetFirstNodeInGroup(Globals.PlayerGroup) as Player).OpenFuseAugments();
	}

	/**
	Open the skill tree by calling the OpenSkillTree function of the player which sets the visibility of the Skill Tree to true
	*/
	private void OpenSkillTree()
	{
		(GetTree().GetFirstNodeInGroup(Globals.PlayerGroup) as Player).OpenSkillTree();
	}
}
