using Godot;

public partial class MainMenu : BaseMenu
{
	public override void _Ready()
	{
		MenuType = MenuManager.MenuType.MainMenu;
		base._Ready();
	}

	private void OnExitButtonPressed()
	{
		GetTree().Quit();
	}
	
	private void OnContinueButtonPressed()
	{
		PushMenu(MenuManager.MenuType.MainHub);
	}

	private void OnSettingsButtonPressed()
	{
		PushMenu(MenuManager.MenuType.SettingsMenu);
	}
}
