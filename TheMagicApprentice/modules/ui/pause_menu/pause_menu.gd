class_name PauseMenu
extends Control



func _on_return_button_pressed():
	queue_free()


func _on_settings_button_pressed():
	var settings = load("res://modules/ui/settings_menu/settings_menu.tscn").instantiate()
	get_tree().current_scene.add_child(settings) 


func _on_exit_button_pressed():
	get_tree().change_scene_to_file("res://modules/ui/main_menu/main_menu.tscn")
