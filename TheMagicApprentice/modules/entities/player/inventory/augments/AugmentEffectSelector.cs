using Godot;
using System;


/**
This class extends the OptionButton. It allows you to choose between the AugmentEffects of one Augment.
It is used in the Fuse Augment System.
*/
[GlobalClass]
public partial class AugmentEffectSelector : OptionButton
{
	
	/**
	This function is used to generate the Items of the OptionButton using an Augment.
	Since this function gets called by the signal EquipAugmentInSlot, it needs to take an int that we dont care about.
	*/
	public void SelectAugment(Augment augment, int _)
	{
		Clear(); // Clear all previous Items from the OptionButton
		// If the augment is null, then we just unequiped the augment from the slot and thus do not display anything anymore
		if (augment is null)
		{
			return;
		}
		// Otherwise loop through all AugmentEffects of the Augment and add them as an Item to the OptionButton
		foreach (AugmentEffect augmentEffect in augment._augmentEffects)
		{
			if (augmentEffect is not null)
			{
				AddItem(augmentEffect.Description());
			}
		}
	}

}
