extends TextureProgressBar

var health_component
var target_value: float
var current_value: float
var smoothing_speed: float = 10.0

func _ready():
	var player = get_tree().get_nodes_in_group("player")[0]
	health_component = player.get_node("HealthComponent")
	target_value = health_component.current_HP
	current_value = target_value
	value = round(current_value)

func _process(delta):
	target_value = health_component.current_HP
	current_value = lerp(current_value, target_value, smoothing_speed * delta)
	value = round(current_value)

func update_health_bar(current_health):
	target_value = current_health
