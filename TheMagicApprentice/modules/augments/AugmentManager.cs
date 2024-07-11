using Godot;
using System;


/**
This class manages all the different Augments and AugmentEffect.
It loads all AugmentEffects at runtime and is used to automatically create a random augment with 1-3 AugmentEffects

It handles all complicated parts of Augment creation.
*/
public partial class AugmentManager : Node
{

	public static AugmentManager Instance { get; private set; }
	
	/**
	Since this node is an autoload the ready function gets called exactly ones at the start of the game.
	Use this to set it to the static Instance of the class.
	*/
	public override void _Ready()
	{
		Instance = this;
	}
}
