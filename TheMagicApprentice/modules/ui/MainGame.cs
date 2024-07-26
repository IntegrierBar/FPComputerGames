using Godot;

public partial class MainGame : BaseMenu
{
	public override void _Ready()
	{
		MenuType = MenuManager.MenuType.MainGame;
		base._Ready();
	}

	private void OnExitButtonPressed()
	{
		PopMenu();
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("esc"))
		{
			PushMenu(MenuManager.MenuType.PauseMenu);
		}
	}
}
