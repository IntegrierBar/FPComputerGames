class_name MainHub
extends Control



func _on_exit_button_pressed():
	var pause = load("res://modules/ui/pause_menu/pause_menu.tscn").instantiate()
	get_tree().current_scene.add_child(pause) 
