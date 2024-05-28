extends State

@export var idle_state: State
@export var attack_state: State

@export var SPEED: float = 100

var player

func _ready():
	player = get_tree().get_first_node_in_group("player")
	if not player:
		print("player not found")



func process_physics(delta: float) -> State:	
	if player:
		var vector_to_player: Vector2 = player.position - parent.position
		
		# if we can attack the player, change to attack state
		if vector_to_player.length() <= parent.attack_range:
			return attack_state
		
		# if the player moves outside the view range, change back to idle state
		if vector_to_player.length() > parent.view_range:
			return idle_state
		
		# else move towards the player
		parent.velocity = vector_to_player.normalized() * SPEED * delta
		parent.move_and_slide()
	
	return null
