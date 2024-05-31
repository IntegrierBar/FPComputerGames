extends State

func enter() -> void:
	print("player died")
	get_tree().paused = true
