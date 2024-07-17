using Godot;
using System;


/**
Class for the Node responsible for summoning the Star Rain spell
Instead of instantly spawning the spell once, it spawnes the spell every 
*/
public partial class InventoryStarRain : InventorySpell
{
	private bool _casting = false; ///< true if the node is currently spawning 
	[Export]
	public double AmountStarsToSpawn = 10; ///< How many stars are created. Uses double instead of int to allow easy manipulation from augments
	private int _starsLeftToSpawn;	///< Variable to track how many stars still have to be spawned

	private Vector2 _playerPosition;	///< Saves the player position when the spell is cast, so that all Stars get the same info
	private Vector2 _targetPosition;	///< Saves the target position when the spell is cast, so that all Stars get the same info

	/**
    Additionaly to base class also adds itself to the Star Rain Spell Group
    */
    public override void _Ready()
    {
        base._Ready();
        AddToGroup(Globals.StarRainSpellGroup);
    }


    public override void Cast(Vector2 playerPosition, Vector2 targetPosition)
    {
        _playerPosition = playerPosition;
		_targetPosition = targetPosition;
		_starsLeftToSpawn = (int)Math.Round(AmountStarsToSpawn); // Round up when calculating how many stars to spawn
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_starsLeftToSpawn > 0)
		{
			base.Cast(_playerPosition, _targetPosition); // we can use the base implementation of Cast to create the individual stars
			_starsLeftToSpawn -= 1;
		}
    }
}
