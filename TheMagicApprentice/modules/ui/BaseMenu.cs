using Godot;

public partial class BaseMenu : Control
{
	protected MenuManager MenuManager;
	public MenuManager.MenuType MenuType { get; protected set; }

	public override void _Ready()
	{
		MenuManager = GetNode<MenuManager>("/root/MenuManager");
		SetupMenu();
	}

	protected virtual void SetupMenu()
	{
		// This method can be overridden in derived classes if needed
	}

	public void PushMenu(MenuManager.MenuType newMenu)
	{
		MenuManager.PushMenu(newMenu);
	}

	public void PopMenu()
	{
		MenuManager.PopMenu();
	}
}
