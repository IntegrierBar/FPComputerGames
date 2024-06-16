using Godot;
using System;

public partial class SlimeDeath : State
{
	 [Export]
    public double DeathAnimationTime = 0.6; ///< Duration of the death animation 
    private double _timeLeft = 0.0; 

    /**
	Set duration of death animation and play death animation.
    Call function to ensure that slime cannot be hit by attacks once it entered the death state.
	*/
	public override void Enter()
    {
        _timeLeft = DeathAnimationTime;
		UpdateAnimations();
        CallDeferred("DisableBoxes"); // has to be called deferred because the function should not be executed during a calculation
		
        base.Enter();
    }

    /**
	Once the duration of the death animation has passed, remove the slime from the scene tree.
	*/
    public override State ProcessPhysics(double delta)
    {
        _timeLeft -= delta;
        if (_timeLeft <= 0)
        {
            Parent.QueueFree();
        }
        return base.ProcessPhysics(delta);
    }

    /**
	play death animation. Animation name has to be constructed from the slimes properties. 
	Currently used properties: magic type. 
	*/
    public override void UpdateAnimations()
    {
        string slimeMagicType = EntityTypeHelper.GetMagicTypeAsString((Parent as Slime).GetMagicType());
        string animationName = slimeMagicType + "_death"; // TODO: when animations also consider size and attack range, this part has to be changed!
		Animations.Play(animationName);

        base.UpdateAnimations();
    }

    /**
    Disable hit- and collision box of the slime upon death. 
    Has to be called deferred to avoid errors with collision calculations.
    */
    private void DisableBoxes()
    {
        GetNode<CollisionShape2D>("%CollisionShapeSlime").Disabled = true;
        GetNode<CollisionShape2D>("%HitBoxSlime").Disabled = true;
    }
}
