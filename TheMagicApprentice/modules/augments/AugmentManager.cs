using Godot;
using System;
using System.Collections.Generic;


/**
This pseudo singleton class manages all the different Augments and AugmentEffect.
It loads all AugmentEffects at runtime and is used to automatically create a random augment with 1-3 AugmentEffects

It handles all complicated parts of Augment creation.

It is an autoload of the Game and thus always accessible using the static Instance member
*/
public partial class AugmentManager : Node
{

	public static AugmentManager Instance { get; private set; } ///< Instance of the Singleton

	private List<AugmentEffect> _augmentEffects = new(); ///< List of all possible AugmentEffects

	private static Random _random = new(); ///< random number generator
	
	/**
	Since this node is an autoload the ready function gets called exactly ones at the start of the game.
	Use this to set it to the static Instance of the class.
	*/
	public override void _Ready()
	{
		Instance = this;
		LoadAugmentEffects();
	}

	/**
	Automaticaly creates a random augment with amountAugmentEffects randomly selected AugmentEffects

	@param amountAugmentEffects should be between 1 and 3. If it is larger then 3 it will instead use 3 and if it is 0 then it will return an augment without effects
	*/
	public Augment CreateRandomAugment(uint amountAugmentEffects)
	{
		System.Diagnostics.Debug.Assert(amountAugmentEffects < 4, "amountAugmentEffects is larger then 3");
		System.Diagnostics.Debug.Assert(amountAugmentEffects > 0, "amountAugmentEffects is 0");

		Augment augment = new Augment();
		for (int i = 0; i < Math.Min(amountAugmentEffects, 3); i++) // use Math.Min to make sure we dont go out of bounce
		{
			augment.SetAugmentEffect(i, SelectRandomAugmentEffect());
			//augment.Description += "\n" + augment._augmentEffects[i].Description(); // build the description of the augment
		}
		return augment;
	}

	/**
	returns a random AugmentEffect from the list of all AugmentEffects
	*/
	private AugmentEffect SelectRandomAugmentEffect()
	{
		int randomIndex = _random.Next(_augmentEffects.Count); // create random index
		return _augmentEffects[randomIndex];
	}

	/**
	Fuses two Augments by overriding the AugmentEffect from fuseTo with index indexEffectToKeep with the AugmentEffect from fuseFrom at position indexEffectToKeep.
	Both target and sacrifice have to be mutable, as target gets changed and sacrifice gets deleted
	*/
	public void FuseAugments(Augment target, int indexEffectToOverride, Augment sacrifice, int indexEffectToKeep)
	{
		// if any of the indices is out of bounds do an early return
		if (indexEffectToOverride < 0 || indexEffectToKeep < 0 || target.GetAugmentEffect(indexEffectToOverride) is null || sacrifice.GetAugmentEffect(indexEffectToKeep) is null)
		{
			return;
		}
		// change the effect
		target.SetAugmentEffect(indexEffectToOverride, sacrifice.GetAugmentEffect(indexEffectToKeep));
		// Remove the AugmentEffect from sacrifice. This is actually not important but more of a security
		sacrifice.SetAugmentEffect(indexEffectToOverride, null);
	}

	/**
	Loads all AugmentEffect Resources from the correct folder
	*/
	private void LoadAugmentEffects()
    {
		// create emtpy list of augmentEffects
        _augmentEffects = new List<AugmentEffect>();

		// Open the directory containing all augmentEffect resources
		// Load them and add them to the list
		string folderPath = "res://modules/augments/augment_effects/resources/"; // sadly need to hardcode the path.
		using var directory = DirAccess.Open(folderPath); 
		if (directory != null)
		{
			directory.ListDirBegin();
			string fileName = directory.GetNext();
			while (fileName != "")
			{
				if (!directory.CurrentIsDir()) // make sure we skip directories (even though there should not be any)
				{
					_augmentEffects.Add( GD.Load<AugmentEffect>(folderPath + fileName) );
				}
				fileName = directory.GetNext();
			}
		}
		else // in case it fails we print an error message. TODO maybe add better error here, but not neccessary for now
		{
			GD.Print("Could not open augment effect directory. Maybe wrong name?");
		}
    }


	/**
	Getter for AugmentEffects. Is only used by tests
	*/
	public List<AugmentEffect> GetAugmentEffects()
	{
		return _augmentEffects;
	}
}
