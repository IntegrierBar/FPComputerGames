extends CanvasLayer


func _on_pause_button_pressed():
	var pause = load("res://modules/ui/pause_menu/pause_menu.tscn").instantiate()
	get_tree().current_scene.add_child(pause) 
