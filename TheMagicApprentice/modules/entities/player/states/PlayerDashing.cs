using Godot;
using System;

/**
Dash state of the player charackter.
During dashing the hitbox of the player is disabled
*/
public partial class PlayerDashing : State
{
    [Export]
    public CollisionShape2D HitBox; ///< Reference of the HitBox of the player 
    [Export]
    public double SPEED = 300; ///< Speed of the dash
    [Export]
    public double DASH_TIME = 0.3; ///< Duration of the dash 
    private double _timeLeft = 0;

    
    [ExportGroup("States")]
    [Export]
    public State Idle; ///< Reference to the Idle state
    //[Export]
    //public State Moving;
    //[Export]
    //public State Dashing;
    //[Export]
    //public State SpellCasting;

    private Movement _movement; ///< reference to the movement component

    /**
    When entering the dash state we disable the Hitbox and set the Velocity of the Player 
    */
    public override void Enter()
    {
        base.Enter();
        _movement = Parent.GetNode<Movement>("Movement");
        _timeLeft = DASH_TIME;

        HitBox.Disabled = true;
    }

    /**
    When exiting the dash state we enable the Hitbox again
    */
    public override void Exit()
    {
        HitBox.Disabled = false;
        base.Exit();
    }

    public override State ProcessPhysics(double delta)
    {
        _timeLeft -= delta;
        if (_timeLeft <= 0.0)
        {
            // always return to idle state as described in 
            return Idle;
        }

        _movement.SetMovement(Input.GetVector("left", "right", "up", "down"), (float)SPEED);
        Parent.MoveAndSlide();
        UpdateAnimations();

        return base.ProcessPhysics(delta);
    }

    /**
    Change the animation depending on the current movement of the player
    */
    public override void UpdateAnimations()
    {
        base.UpdateAnimations();

        string animationName = "dash_"; 
        if (Parent.Velocity.X > 0)
        {
            animationName += "right";
        }
        else if (Parent.Velocity.X < 0)
        {
            animationName += "left";
        }
        else if (Parent.Velocity.Y < 0)
        {
            animationName += "up";
        }
        else
        {
            animationName += "down";
        }

        Animations.Play(animationName);
    }
}
