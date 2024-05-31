extends State


@export var idle_state: State
@export var dash_state: State
@export var spellcasting_state: State

@export var SPEED: float = 100


func process_input(event: InputEvent) -> State:
	if event.is_action_pressed("dash"):
		return dash_state
	if event.is_action_pressed("cast"):
		return spellcasting_state
	return null

func process_physics(delta: float) -> State:	
	var movement: Vector2 = Input.get_vector("left", "right", "up", "down")
	if movement == Vector2.ZERO:
		return idle_state
	
	parent.velocity = movement * SPEED
	parent.move_and_slide()
	
	return null
