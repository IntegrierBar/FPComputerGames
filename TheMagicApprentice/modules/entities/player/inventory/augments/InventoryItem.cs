using Godot;
using System;

/**
This class is responsible for the items inside the inventory.
It contains the Augment as a variable.
This is basically a wrapper for Augments so that they can be used as Items inside the inventory
*/
[GlobalClass]
public partial class InventoryItem : TextureRect
{
	[Export]
	public Augment Augment; ///< Reference to the Augment

    /**
    When adding the Item to the tree we set the settings, texture and Tooltip
    */
    public override void _Ready()
    {
        if (Augment is not null)
		{
			ExpandMode = ExpandModeEnum.IgnoreSize;
			StretchMode = StretchModeEnum.KeepAspectCentered;
			Texture = GD.Load<Texture2D>("res://modules/entities/player/inventory/augments/augment.png");
			TooltipText = Augment.Description;
		}
    }

	/**
	Used by the engine for drag and drop. Creates the drag preview image the is moved with the mouse
	*/
    public override Variant _GetDragData(Vector2 atPosition)
    {
		var dragPreview = new TextureRect
        {
            Texture = Texture,
            ExpandMode = ExpandModeEnum.IgnoreSize,
            StretchMode = StretchModeEnum.KeepAspectCentered,
            CustomMinimumSize = Size*0.8f,
            Position = -atPosition
        };
        SetDragPreview(dragPreview);

		return this;
    }

    /**
	Rebuild the description of the Augment and set the tooltiptext
	*/
	public void RebuildDescription()
	{
		Augment.BuildDescription();
        TooltipText = Augment.Description;
	}
}
