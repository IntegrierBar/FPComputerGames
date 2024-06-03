using Godot;
using System;

[GlobalClass]
public partial class State : Node
{
	/**
	Reference to the parent of the scene, i.e. the entity that this state machine belongs to.
	*/
	public CharacterBody2D Parent;

	/**
	Reference to the AnimationPlayer of the entity
	*/
	public AnimationPlayer Animations;


	public virtual void Enter() {}

	public virtual void Exit() {}

	public virtual State ProcessInput(InputEvent @event) 
	{
		return null;
	}

	public virtual State ProcessFrame(double delta)
	{
		UpdateAnimations();
		return null;
	}

	public virtual State ProcessPhysics(double delta)
	{
		return null;
	}

	/**
	Called in ProcessFrame. Is resposible for playing the correct animation for the state
	*/
	public virtual void UpdateAnimations() {}
}
