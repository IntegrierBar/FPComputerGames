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
	private float _minSize = 64; ///< Minimum size of the individual InventorySlots. When changing this remember to also change it in all manually instanced Slots
	private GridContainer _inactiveAugments; ///< Reference to the GridContainer Node that contains all inactive augment slot
	private HBoxContainer _activeAugments; ///< Referennce to the HBoxContainer that containes all active augment slots
	private HBoxContainer _fuseAugments; ///< Reference to the HBoxContainer that contains the system for fusing augments
	
	/**
	Gets the Reference to the Grid and fills it with _numberOfSlots many emtpy slots and gets reference to the active augments and creates the 5 active augment slots
	*/
	public override void _Ready()
	{
		_inactiveAugments = GetNode<GridContainer>("%InactiveAugments");
		_activeAugments = GetNode<HBoxContainer>("%ActiveAugments");
		_fuseAugments = GetNode<HBoxContainer>("%FuseAugments");
		System.Diagnostics.Debug.Assert(_inactiveAugments is not null, "InactiveAugments in AugmentInventory is null");
		System.Diagnostics.Debug.Assert(_activeAugments is not null, "ActiveAugments in AugmentInventory is null");
		System.Diagnostics.Debug.Assert(_fuseAugments is not null, "FuseAugments in AugmentInventory is null");

		// create the inventory for the inactive augments.
		/*for (int i = 0; i < _numberOfSlots; i++)
		{
			var inventorySlot = new InventorySlot();
			_inactiveAugments.AddChild(inventorySlot);
			inventorySlot.Init(new Vector2(_minSize, _minSize), -1); // set the slots as inactive
		}*/

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
	Gets called when the left button of the menu is pressed.
	Handles transition to Fuse Augments and Skill Tree
	*/
	public void LeftButtonPressed()
	{
		if (_activeAugments.IsVisibleInTree())
		{
			// Switch to Fuse Augment Menu
			_activeAugments.Visible = false;
			_fuseAugments.Visible = true;
			GetNode<Button>("%LeftButton").Text = "Skill Tree";
			GetNode<Button>("%RightButton").Text = "Active Augments";
		}
		else
		{
			// TODO Go to skill tree
		}
	}

	/**
	Gets called when right button is pressed.
	Handles transition to Skill Tree and Active Augments
	*/
	public void RightButtonPressed()
	{
		if (_activeAugments.IsVisibleInTree())
		{
			// TODO Go to skill tree
		}
		else // _fuseAugments is currently shown
		{
			_activeAugments.Visible = true;
			_fuseAugments.Visible = false;
			GetNode<Button>("%LeftButton").Text = "Fuse Augments";
			GetNode<Button>("%RightButton").Text = "Skill Tree";
		}
	}

	/**
	Fuse the currently selected Augments in the way specified by the 2 OptionButtons
	*/
	public void FuseSelectedAugments()
	{
		var fuseTo = GetNode<InventorySlot>("MarginContainer/MarginContainer2/VBoxContainer/HBoxContainer/FuseAugments/FuseTo");
		var fuseFrom = GetNode<InventorySlot>("MarginContainer/MarginContainer2/VBoxContainer/HBoxContainer/FuseAugments/FuseFrom");

		int fuseToEffectIndex = GetNode<AugmentEffectSelector>("MarginContainer/MarginContainer2/VBoxContainer/HBoxContainer/FuseAugments/EffectToOverride").Selected;
		int fuseFromEffectIndex = GetNode<AugmentEffectSelector>("MarginContainer/MarginContainer2/VBoxContainer/HBoxContainer/FuseAugments/EffectToKeep").Selected;

		// If one of the indices is less then 0 then nothing is selected, therefore we do nothing
		if (fuseToEffectIndex < 0 || fuseFromEffectIndex < 0)
		{
			return;
		}
		// since both indices are >= 0, something is selected, which means augments are inside the slots, therefore this is valid
		InventoryItem fuseToItem = fuseTo.GetChild<InventoryItem>(0);
		InventoryItem fuseFromItem = fuseFrom.GetChild<InventoryItem>(0);

		// Override the effect of the augment with the selected AugmentEffect
		AugmentEffect selectedAugmentEffect = fuseFromItem.Augment._augmentEffects[fuseFromEffectIndex];
		fuseToItem.Augment._augmentEffects[fuseToEffectIndex] = selectedAugmentEffect;
		fuseToItem.RebuildDescription(); // update the tool tip text

		// Finally destroy the Augment from which we have taken the effect
		fuseFromItem.QueueFree();
		fuseFrom.EquipAugment(null); // let the slot, and thus the OptionButton, know it is empty now

		// call EquipAugment even thougt it is already a child so that the OptionButton gets updated
		fuseTo.EquipAugment(fuseToItem);
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
