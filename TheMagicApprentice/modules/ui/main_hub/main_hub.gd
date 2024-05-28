class_name MainHub
extends Control

func _on_dungeon_button_pressed():
	get_tree().change_scene_to_file("res://example_scenes/dungeon.tscn")

func _on_exit_button_pressed():
	var pause = load("res://modules/ui/pause_menu/pause_menu.tscn").instantiate()
	get_tree().current_scene.add_child(pause) 
