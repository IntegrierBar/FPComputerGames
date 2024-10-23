using Godot;
using Godot.Collections;
using System;

public partial class SpellInventory : Control
{
    /**
	This function calls the SpellInventory to change the icons of the spells when the spells are changed
    Find the correct spell slot that was changed and call the spell slot to change the icon.
    Note: icon can be null if a spell is removed and no new spell is added! This is intended behaviour.
	*/
	public void SetUIIcon(int nrSpellSlot, Texture2D icon)
    {
        String skillNodeName = "%SpellSlot" + nrSpellSlot;
		UISpellSlot skillSlotNode = GetNode<UISpellSlot>(skillNodeName);
        skillSlotNode.SetSpell(icon);
    }
}
