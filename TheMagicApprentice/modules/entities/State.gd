class_name State extends Node
## This is the base state class. Every state extends this class.



#@export var animation_name: String

## Hold a reference to the parent so that it can be controlled by the state
var parent: CharacterBody2D

## Hold a reference to the animation player so that we can play the correct animation based on the state
#var animation_tree: AnimationTree

## Called when we enter the state
func enter() -> void:
	pass

## Called when we exit
func exit() -> void:
	pass

## Called when imput is processed
func process_input(event: InputEvent) -> State:
	return null

## Called each frame
func process_frame(delta: float) -> State:
	return null

## Called each physics frame
func process_physics(delta: float) -> State:
	return null
