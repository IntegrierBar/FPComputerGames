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
		PopMenu();
	}

	private void OnDungeonButtonPressed()
	{
		SetRootMenu(MenuManager.MenuType.MainGame);
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("esc") && !@event.IsEcho())
		{
			PushMenu(MenuManager.MenuType.PauseMenu);
		}
	}
}