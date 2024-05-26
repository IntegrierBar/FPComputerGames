class_name MeleeSlime
extends CharacterBody2D


@onready var state_machine = $StateMachine

## If the player is closer then this, then the slime can see the player and moves towards him
@export var view_range: float = 100.

## If the player is closer then this, then the slime can attack the player
@export var attack_range: float = 10.


func _ready() -> void:
	# Initialize the state machine, passing a reference of the player and animation tree to the states,
	# that way they can move and react accordingly
	state_machine.init(self)

func _unhandled_input(event: InputEvent) -> void:
	state_machine.process_input(event)

func _physics_process(delta: float) -> void:
	state_machine.process_physics(delta)

func _process(delta: float) -> void:
	state_machine.process_frame(delta)
	
