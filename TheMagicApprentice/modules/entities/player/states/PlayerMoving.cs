using Godot;
using System;

public partial class PlayerMoving : State
{
    [Export]
    public double SPEED = 100; ///< Movement speed 
    [Export]
    public AudioStreamPlayer2D WalkingSound;
    
    
    [ExportGroup("States")]
    [Export]
    public State Idle; ///< Reference to Idle state 
    //[Export]
    //public State Moving;
    [Export]
    public State Dashing; ///< Reference to Dashing state
    [Export]
    public State SpellCasting; ///< Reference to SpellCasting state 


    public override State ProcessInput(InputEvent @event)
    {
        if (@event.IsActionPressed("dash"))
        {
            return Dashing;
        }
        if (@event.IsActionPressed("cast"))
        {
            return SpellCasting;
        }
        return base.ProcessInput(@event);
    }


    public override State ProcessPhysics(double delta)
    {
        Vector2 direction = Input.GetVector("left", "right", "up", "down");
        if (direction == Vector2.Zero)
        {
            return Idle;
        }

        Parent.Velocity = (float)SPEED * direction;
        Parent.MoveAndSlide();
        
        UpdateAnimations();
        return null;
    }

    /**
    Change the animation depending on the current movement of the player
    */
    public override void UpdateAnimations()
    {
        base.UpdateAnimations();

        string animationName = "walk_"; 
        if (Parent.Velocity.X > 0)
        {
            animationName += "right";
        }
        else if (Parent.Velocity.X < 0)
        {
            animationName += "left";
        }
        else if (Parent.Velocity.Y < 0) // Note: Up in godot is negative y axis
        {
            animationName += "up";
        }
        else
        {
            animationName += "down";
        }

        Animations.Play(animationName);
    }

    /**
    Start playing the walking sound when entering
    */
    public override void Enter()
    {
        base.Enter();
        WalkingSound.Play();
    }

    /**
    Stops the walking sound from playing when we leave the state
    */
    public override void Exit()
    {
        base.Exit();
        WalkingSound.Stop();
    }
}
