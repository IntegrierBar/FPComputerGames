extends State


@export var idle_state: State
@export var move_state: State

@export var SPEED: float = 400
@export var DASH_TIME: float = 0.3

var time_left: float = 0
var direction: Vector2 = Vector2.ZERO

## When entering dash state, get the current direction we want to dash in and set it as direction
func enter() -> void:
	direction = Input.get_vector("left", "right", "up", "down")
	time_left = DASH_TIME
	parent.velocity = direction * SPEED

func process_physics(delta: float) -> State:
	if time_left <= 0:
		if Input.get_vector("left", "right", "up", "down") == Vector2.ZERO:
			return idle_state
		return move_state
	time_left -= delta
	
	parent.move_and_slide()
	
	return null
