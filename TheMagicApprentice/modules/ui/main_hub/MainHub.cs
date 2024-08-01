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

	private void OnDungeonButtonPressed()
	{
		SetRootMenu(MenuManager.MenuType.MainGame);
	}

	/**
	Open the augment inventory by calling the OpenAugmentInventory function of the player which sets the visibility of the Inventory to true
	*/
	private void OpenAugmentInventory()
	{
		(GetTree().GetFirstNodeInGroup(Globals.PlayerGroup) as Player).OpenAugmentInventory();
	}
}
