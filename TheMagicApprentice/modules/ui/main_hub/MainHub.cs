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
}
