using Godot;
using System;

public partial class SlimeDeath : State
{
	 [Export]
    public double DeathAnimationTime = 0.6; ///< Duration of the death animation 
    private double _timeLeft = 0.0; 

	public override void Enter()
    {
        _timeLeft = DeathAnimationTime;
		string animationName = (Parent as Slime).GetMagicTypeAsString() + "_death"; // TODO: when animations also consider size and attack range, this part has to be changed!
		Animations.Play(animationName);
		
        base.Enter();
    }

public override State ProcessPhysics(double delta)
    {
        _timeLeft -= delta;
        if (_timeLeft <= 0)
        {
            Parent.QueueFree();
        }
        return base.ProcessPhysics(delta);
    }
}