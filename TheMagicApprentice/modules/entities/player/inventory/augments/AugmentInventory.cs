using Godot;
using System;

public partial class AugmentInventory : Control
{
	[Export]
	private int _numberOfSlots = 8*6; ///< How many empty slots should be initialized at the start of the game
	[Export]
	private float _minSize = 64; ///< Minimum size of the individual InventorySlots
	private GridContainer _grid; ///< Reference to the GridContainer Node
	
	/**
	Creates the Reference to the Grid and fills the grid with _numberOfSlots many emtpy slots
	*/
	public override void _Ready()
	{
		_grid = GetNode<GridContainer>("Grid");
		System.Diagnostics.Debug.Assert(_grid is not null, "Grid in AugmentInventory is null");

		for (int i = 0; i < _numberOfSlots; i++)
		{
			var inventorySlot = new InventorySlot();
			_grid.AddChild(inventorySlot);
			inventorySlot.Init(new Vector2(_minSize, _minSize));
		}

		// for testing purposes create a test Item
        var testInventoryItem = new InventoryItem
        {
            Augment = AugmentManager.Instance.CreateRandomAugment(3)
        };
		_grid.GetChild(0).AddChild(testInventoryItem);
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
