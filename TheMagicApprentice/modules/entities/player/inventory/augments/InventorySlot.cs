using Godot;
using System;



/**
The InventorySlot class exists
*/
[GlobalClass]
public partial class InventorySlot : PanelContainer
{


	public void Init(Vector2 minSize)
	{
		CustomMinimumSize = minSize;
	}

	/**
	Return 0 if data is an InventoryItem and the slot is emtpy
	*/
    public override bool _CanDropData(Vector2 atPosition, Variant data)
    {
		if (data.VariantType == Variant.Type.Object && data.AsGodotObject() is InventoryItem) // first check if it is a GodotObject then check if it is an InventoryItem
		{
			return GetChildCount() == 0; // the slot is empty iff it does not have a child
			
		}
		return false;
    }

	/**
	Reparent the InventoryItem to this slot
	*/
    public override void _DropData(Vector2 atPosition, Variant data)
    {
        (data.AsGodotObject() as InventoryItem)?.Reparent(this);
    }
}
