using Godot;
using System;

[GlobalClass]
public partial class State : Node
{
	
	public CharacterBody2D Parent; ///<	Reference to the parent of the scene, i.e. the entity that this state machine belongs to. 

	
	public AnimationPlayer Animations; ///<	Reference to the AnimationPlayer of the entity 


	/**
	Called everytime we enter the state
	*/
	public virtual void Enter() {}


	/**
	Called everytime we exit the state
	*/
	public virtual void Exit() {}

	/**
	If the state is the current state this function gets called whenever there is an unhandled input
	*/
	public virtual State ProcessInput(InputEvent @event) 
	{
		return null;
	}


	/**
	If the state is the current state this function gets called every frame
	*/
	public virtual State ProcessFrame(double delta)
	{
		return null;
	}

	/**
	If the state is the current state this function gets called every physics update
	*/
	public virtual State ProcessPhysics(double delta)
	{
		return null;
	}

	/**
	Called in ProcessFrame. Is resposible for playing the correct animation for the state
	*/
	public virtual void UpdateAnimations() {}
}
