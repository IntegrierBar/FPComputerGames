using Godot;

public partial class PauseMenu : BaseMenu
{
	public override void _Ready()
	{
		MenuType = MenuManager.MenuType.PauseMenu;
		base._Ready();
	}

	private void OnResumeButtonPressed()
	{
		PopMenu();
	}

	private void OnSettingsButtonPressed()
	{
		PushMenu(MenuManager.MenuType.SettingsMenu);
	}

	private void OnExitButtonPressed()
	{
		SetRootMenu(MenuManager.MenuType.MainHub);
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("esc"))
		{
			PopMenu();
		}
	}
}
