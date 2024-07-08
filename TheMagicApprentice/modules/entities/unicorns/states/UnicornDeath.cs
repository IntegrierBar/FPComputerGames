using Godot;
using System;

public partial class UnicornDeath : State
{
	[Export]
    public double DeathAnimationTime = 1.0; ///< Duration of the death animation 
    private double _timeLeft = 0.0; ///< time left in the death state

	 /**
	Set duration of death animation and play death animation.
    Call function to ensure that unicorn cannot be hit by attacks once it entered the death state.
	*/
	public override void Enter()
    {
        _timeLeft = DeathAnimationTime;
		UpdateAnimations();
        CallDeferred("DisableBoxes"); // has to be called deferred because the function should not be executed during a calculation
		
        base.Enter();
    }

	/**
	Once the duration of the death animation has passed, remove the unicorn from the scene tree.
    A signal or a funciton have to be called then, to tell the dungeon that it was successfully cleared.
	*/
    public override State ProcessPhysics(double delta)
    {
        _timeLeft -= delta; // count down time left in death state
        if (_timeLeft <= 0)
        {
            Parent.QueueFree(); // when the time is up remove the unicorn from the scene tree
			DungeonCleared(); // This is a dummy function that does nothing yet. But the signal to the dungeon for succesfully clearing the dungeon should go here
        }
        return base.ProcessPhysics(delta);
    }

	/**
    Update animations to the death animation, depending on the magic type of the unicorn.
	At the moment, there are no animations.
    */
    public override void UpdateAnimations()
    {
		string unicornMagicType = EntityTypeHelper.GetMagicTypeAsString((Parent as Unicorn).GetMagicType());
		String animation_name = unicornMagicType + "_charge_attack";
        base.UpdateAnimations();
    }

    /**
    Dummy function that does nothing yet. At this position, the dungeon should be informed that the player
    has succesfully cleared the dungeon and the player can be returned to the main hub. Might be 
    implemented as a signal later, which would make this function obsolete. 
    */
	private void DungeonCleared()
	{
		// There should probably be a signal here, that informs the dungeon that it has been cleared and
		// returns the player to the main hub.
	}

	/**
    Disable hit- and collision box of the unicorn upon death. 
    Has to be called deferred to avoid errors with collision calculations.
    */
    private void DisableBoxes()
    {
        GetNode<CollisionShape2D>("%CollisionShapeUnicorn").Disabled = true;
        GetNode<CollisionShape2D>("%HitBoxUnicorn").Disabled = true;
    }
}
