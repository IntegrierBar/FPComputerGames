using Godot;
using System;

/**

*/
[GlobalClass]
public partial class InventoryItem : TextureRect
{
	[Export]
	public Augment Augment;

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
	Used by the engine for drag and drop
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

}
