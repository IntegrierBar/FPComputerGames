using Godot;

/**
 * Base class for all menu types in the game.
 * Provides common functionality and interface with the MenuManager.
 */
public partial class BaseMenu : CanvasLayer
{
	/** Reference to the MenuManager instance. */
	protected MenuManager MenuManager;

	/** The type of this menu. */
	public MenuManager.MenuType MenuType { get; protected set; }

	/**
	 * Initializes the menu by getting the MenuManager reference and calling SetupMenu.
	 */
	public override void _Ready()
	{
		MenuManager = GetTree().GetFirstNodeInGroup("menu_manager") as MenuManager;
		SetupMenu();
	}

	/**
	 * Virtual method for setting up the menu.
	 * Can be overridden in derived classes to provide specific setup logic.
	 */
	protected virtual void SetupMenu()
	{
		// This method can be overridden in derived classes if needed
	}

	/**
	 * Pushes a new menu onto the stack.
	 * 
	 * @param newMenu The type of menu to push.
	 */
	public void PushMenu(MenuManager.MenuType newMenu)
	{
		MenuManager.PushMenu(newMenu);
	}

	/**
	 * Pops the current menu from the stack.
	 */
	public void PopMenu()
	{
		MenuManager.PopMenu();
	}

	/**
	 * Sets a new root menu, clearing the existing menu stack.
	 * 
	 * @param newMenu The type of menu to set as the new root.
	 */
	public void SetRootMenu(MenuManager.MenuType newMenu)
	{
		MenuManager.SetRootMenu(newMenu);
	}
}
