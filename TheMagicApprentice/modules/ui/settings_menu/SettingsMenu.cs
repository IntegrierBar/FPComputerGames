public partial class SettingsMenu : BaseMenu
{
	public override void _Ready()
	{
		MenuType = MenuManager.MenuType.SettingsMenu;
		base._Ready();
	}

	private void OnBackButtonPressed()
	{
		PopMenu();
	}

	// This was outside the class before, let's move it inside
	private void _on_exit_button_pressed()
	{
		PopMenu();
	}

	// Add other settings-related functions here
}