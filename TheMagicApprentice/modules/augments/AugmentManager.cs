using Godot;
using System;
using System.Collections.Generic;


/**
This class manages all the different Augments and AugmentEffect.
It loads all AugmentEffects at runtime and is used to automatically create a random augment with 1-3 AugmentEffects

It handles all complicated parts of Augment creation.
*/
public partial class AugmentManager : Node
{

	public static AugmentManager Instance { get; private set; }

	private List<AugmentEffect> _augmentEffects;

	private static Random _random = new();
	
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

	@param amountAugmentEffects has to be between 1 and 3
	*/
	public Augment CreateRandomAugment(uint amountAugmentEffects)
	{
		System.Diagnostics.Debug.Assert(amountAugmentEffects < 4, "amountAugmentEffects is larger then 3");
		System.Diagnostics.Debug.Assert(amountAugmentEffects > 0, "amountAugmentEffects is 0");

		Augment augment = new Augment();
		for (int i = 0; i < amountAugmentEffects; i++)
		{
			augment._augmentEffects[i] = SelectRandomAugmentEffect();
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
				GD.Print(fileName);
				fileName = directory.GetNext();
			}
		}
		else // in case it fails we print an error message. TODO maybe add better error here, but not neccessary for now
		{
			GD.Print("Could not open augment effect directory. Maybe wrong name?");
		}
    }
}
