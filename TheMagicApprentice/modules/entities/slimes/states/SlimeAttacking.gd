extends State

@export var idle_state: State

# time until spell is cast
var time_left: float = 0

var player

func _ready():
	player = get_tree().get_first_node_in_group("player")
	if not player:
		print("player not found")

## Here we immediately call attack when we enter the state and then wait out the animation and then return to Idle
func enter() -> void:
	attack()

func process_physics(delta: float) -> State:
	if time_left <= 0:
		return idle_state
	time_left -= delta
	return null

func attack() -> void:
	# play attack animation and create attack
	time_left = 2
	pass
