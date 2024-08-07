using Godot;
using System;
using System.Linq;


/**
The AugmentInventory is the root node of the augment inventory.
It handles the creation of all InventorySlots and the adding of new augments to the inventory.
The scene is a child of the player so that it is always loaded as the scene containes all augment data.
Usually the visibility and the processing is disabled. Except if the player clicks the "Open Augment Inventory" button.
It can then be closed again using ESC.
*/
[GlobalClass]
public partial class AugmentInventory : CanvasLayer
{
	[Export]
	private int _numberOfSlots = 10*7; ///< How many empty slots should be initialized at the start of the game
	[Export]
	private float _minSize = 64; ///< Minimum size of the individual InventorySlots. When changing this remember to also change custim_minimum_size of the ScrollContainer accordingly
	private GridContainer _inactiveAugments; ///< Reference to the GridContainer Node that contains all inactive augment slot
	private HBoxContainer _activeAugments; ///< Referennce to the HBoxContainer that containes all active augment slots
	
	/**
	Gets the Reference to the Grid and fills it with _numberOfSlots many emtpy slots and gets reference to the active augments and creates the 5 active augment slots
	*/
	public override void _Ready()
	{
		_inactiveAugments = GetNode<GridContainer>("%InactiveAugments");
		_activeAugments = GetNode<HBoxContainer>("%ActiveAugments");
		System.Diagnostics.Debug.Assert(_inactiveAugments is not null, "InactiveAugments in AugmentInventory is null");
		System.Diagnostics.Debug.Assert(_activeAugments is not null, "ActiveAugments in AugmentInventory is null");

		// create the inventory for the inactive augments
		for (int i = 0; i < _numberOfSlots; i++)
		{
			var inventorySlot = new InventorySlot();
			_inactiveAugments.AddChild(inventorySlot);
			inventorySlot.Init(new Vector2(_minSize, _minSize), -1); // set the slots as inactive
		}

		// create the inventory for the active augments
		Player player = GetTree().GetFirstNodeInGroup(Globals.PlayerGroup) as Player;
		System.Diagnostics.Debug.Assert(player is not null, "Cannot get player in _Ready function in AugmentInventory");
		for (int i = 0; i < 5; i++)
		{
			var inventorySlot = new InventorySlot();
			_activeAugments.AddChild(inventorySlot);
			inventorySlot.Init(new Vector2(_minSize, _minSize), i); // set the slots as active and give it the correct index
			inventorySlot.EquipAugmentInSlot += player.EquipAugmentInSlot; // connect its the signal for equiping augments to the player so that augments are equiped
		}
    }

    /**
	If Esc is pressed the AugmentInventory becomes invisible again and stops processing
	*/
    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsAction("esc"))
		{
			SetVisibility(false);
		}
    }

	/**
	Set the visibility and the ProcessMode of the AugmentInventory. I.e. enable and disable it.
	*/
	public void SetVisibility(bool isVisible)
	{
		Visible = isVisible;
		if (isVisible)
		{
			ProcessMode = ProcessModeEnum.Always;
		}
		else
		{
			ProcessMode = ProcessModeEnum.Disabled;
		}
	}

    /**
	Adds a new augment to the inventory by finding an empty slot in the Grid, creating and InventoryItem with the Augment and putting it in the slot
	In case all slots are filled it creates a new row of slots in the inventory.
	*/
    public void AddAugmentToInventory(Augment augment)
	{
		var inventorySlot = FindEmptyInventorySlot();
        InventoryItem inventoryItem = new InventoryItem
        {
            Augment = augment
        };
		inventorySlot.AddChild(inventoryItem); // add the item to the empty slot
    }

	/**
	Finds the first empty InventorySlot. If all Slots are full it automatically generates Grid.Columns many new slots 
	*/
	private InventorySlot FindEmptyInventorySlot()
	{
		var listOfAllInventorySlots = _inactiveAugments.GetChildren().OfType<InventorySlot>();

		InventorySlot emptySlot = null;
		foreach (var slot in listOfAllInventorySlots)
		{
			// If we find an empty slot, get the reference and stop
			if (slot.GetChildCount() == 0)
			{
				emptySlot = slot;
				break;
			}
		}
		// If we did not find an empty slot we need to create more slots
		if (emptySlot is null)
		{
			emptySlot = new InventorySlot();
			_inactiveAugments.AddChild(emptySlot);
			emptySlot.Init(new Vector2(_minSize, _minSize), -1); // set the slots as inactive

			for (int i = 1; i < _inactiveAugments.Columns; i++) // start at 1 not 0 since we already added one
			{
				var inventorySlot = new InventorySlot();
				_inactiveAugments.AddChild(inventorySlot);
				inventorySlot.Init(new Vector2(_minSize, _minSize), -1); // set the slots as inactive
			}
		}

		return emptySlot;
	}

	/**
	Adds a random augment to the inventory
	*/
	public void AddRandomAugment()
	{
		uint numberOfEffects = GD.Randi() % 3 + 1;
		Augment augment = AugmentManager.Instance.CreateRandomAugment(numberOfEffects);
		AddAugmentToInventory(augment);
	}

	/**
	Getter for _inactiveAguments.
	Only used be tests
	*/
	public GridContainer GetInactiveAugments()
	{
		return _inactiveAugments;
	}

	/**
	Getter for _activeAguments.
	Only used be tests
	*/
	public HBoxContainer GetActiveAugments()
	{
		return _activeAugments;
	}
}
