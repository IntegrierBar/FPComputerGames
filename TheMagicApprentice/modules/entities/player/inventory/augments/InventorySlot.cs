using Godot;
using System;


/**
The InventorySlot class.
An inventorySlot contains an item, if it has a child. Which is then the item it contains.
Uses Godots internal _CanDropData and _DropData function to handle the drag and drop logic
*/
[GlobalClass]
public partial class InventorySlot : PanelContainer
{
	[Signal]
	public delegate void EquipAugmentInSlotEventHandler(Augment augment, int slotIndex); ///< Signal that gets emitted if the entities health reaches 0 

	private int _activeAugmentSlot = -1; ///< Determines whether this augment slot is an active or inactive augment slot, i.e. whether the augment effects get applied, negative means it is an inactive slot, otherwise it is the index of the slot

	/**
	Initialize the data of the InventorySlot

	@param minSize determines the minimum size of the slot
	@param active sets _activeAugmentSlot
	*/
	public void Init(Vector2 minSize, int activeSlot)
	{
		CustomMinimumSize = minSize;
		_activeAugmentSlot = activeSlot;
	}

	/**
	Return true if data is an InventoryItem
	*/
    public override bool _CanDropData(Vector2 atPosition, Variant data)
    {
		if (data.VariantType == Variant.Type.Object && data.AsGodotObject() is InventoryItem) // first check if it is a GodotObject then check if it is an InventoryItem
		{
			return true;
			
		}
		return false;
    }
	
	/**
	Reparent the InventoryItem to this slot.
	Incease there was another InventoryItem in the slot, they switch places
	*/
    public override void _DropData(Vector2 atPosition, Variant data)
    {
        InventoryItem inventoryItem = data.AsGodotObject() as InventoryItem;
		System.Diagnostics.Debug.Assert(inventoryItem is not null, "Conversion to InventoryItem failed"); // I am relatively sure this should never happen. In case it ever happens, we need to have an early return for this


		// get the reference to the other item incase it exists
		InventoryItem itemInCurrentSlot = null; 
		if (GetChildCount() > 0)
		{
			itemInCurrentSlot = GetChild(0) as InventoryItem;
		}

		// Get the previous slot of the item
		InventorySlot previousSlotOfItem = inventoryItem.GetParent<InventorySlot>();

		// Equip the new item in this slot and the previous item in the other slot
		EquipAugment(inventoryItem);
		previousSlotOfItem.EquipAugment(itemInCurrentSlot);
    }

	/**
	Reparent the InventoryItem and emit the signal EquiptAugmentInSlot
	*/
	public void EquipAugment(InventoryItem augmentItem)
	{
		augmentItem?.Reparent(this);
		EmitSignal(SignalName.EquipAugmentInSlot, augmentItem?.Augment, _activeAugmentSlot);
	}
}
