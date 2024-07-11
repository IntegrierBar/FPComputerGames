using Godot;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Linq;


public partial class StateMachine : Node
{
	[Export]
	public State StartingState; ///< Reference to the State in which we start the game
	[Export]
	public State DeathState; ///< Reference to tthe Death state

	private State _currentState; ///< Reference to the state in which we currently are
	
	/**
	Initialise all states by setting their Parent and Animations members and changes into the StartingState
	*/
	public void Init(CharacterBody2D parent, AnimationPlayer animationPlayer)
	{
		foreach (State state in GetChildren().Where(x => x is State).Cast<State>())
		{
			state.Parent = parent;
			state.Animations = animationPlayer;
		}
		ChangeState(StartingState);
	}

	/**
	Change from _currentState to newState
	*/
	private void ChangeState(State newState)
	{
		// If we cannot enter the new state, do nothing
		if (!newState.CanEnter())
		{
			return;
		}

		if (_currentState is not null)
		{
			_currentState.Exit();
		}
		_currentState = newState;
		_currentState.Enter();
	}

	/**
	direct the input from the entity to the current state
	*/
	public void ProcessInput(InputEvent @event)
	{
		State newState = _currentState.ProcessInput(@event);
		if (newState is not null)
		{
			ChangeState(newState);
		}
	}

	/**
	direct the _Process from the entity to the current state
	*/
	public void ProcessFrame(double delta)
	{
		State newState = _currentState.ProcessFrame(delta);
		if (newState is not null)
		{
			ChangeState(newState);
		}
	}

	/**
	direct the _PhysicsProcess from the entity to the current state
	*/
	public void ProcessPhysics(double delta)
	{
		State newState = _currentState.ProcessPhysics(delta);
		if (newState is not null)
		{
			ChangeState(newState);
		}
	}

	/**
	Getter for currentState. Is only used for testing
	*/
	public State GetState()
	{
		return _currentState;
	}

	public void OnDeath()
	{
		ChangeState(DeathState);
	}
}
