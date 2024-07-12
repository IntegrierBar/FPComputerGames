using Godot;

public partial class PauseMenu : BaseMenu
{
	public override void _Ready()
	{
		MenuType = MenuManager.MenuType.PauseMenu;
		base._Ready();
		//GetTree().Paused = true;
	}

	private void OnResumeButtonPressed()
	{
		//GetTree().Paused = false;
		PopMenu();
	}

	private void OnSettingsButtonPressed()
	{
		PushMenu(MenuManager.MenuType.SettingsMenu);
	}

	private void OnExitButtonPressed()
	{
		//GetTree().Paused = false;
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("esc"))
		{
			PopMenu();
		}
	}
}
