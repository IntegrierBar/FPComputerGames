extends State

@export var idle_state: State
@export var move_state: State
@export var dash_state: State

# time until spell is cast
var time_left: float = 0

## When entering spellcaste state we cast the spell and at the end automatically exit
func enter() -> void:
	time_left = 2

func process_physics(delta: float) -> State:
	if time_left <= 0:
		if Input.get_vector("left", "right", "up", "down") == Vector2.ZERO:
			return idle_state
		return move_state
	time_left -= delta
	
	return null
