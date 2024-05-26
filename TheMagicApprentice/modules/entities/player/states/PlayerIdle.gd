extends State


@export var move_state: State
@export var dash_state: State
@export var spellcasting_state: State

func enter() -> void:
	super()
	parent.velocity = Vector2.ZERO

func process_input(event: InputEvent) -> State:
	if event.is_action_pressed("dash"):
		return dash_state
	if Input.get_vector("left", "right", "up", "down") != Vector2.ZERO:
		return move_state
	return null

func process_physics(delta: float) -> State:
	return null
