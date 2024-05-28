class_name MainMenu
extends Control


func _on_new_game_button_pressed():
	print("New game")


# Once a dungeon exists, this has to be changed to Root and Main hub needs to be the one that is visible
func _on_continue_button_pressed():
	get_tree().change_scene_to_file("res://modules/ui/main_hub/main_hub.tscn")


# Note: the settings menu is rendered after the main menu and therefore visisble
# without the ColorRect as background, the main menu would be visisble behind the settings menu 
func _on_settings_button_pressed():
	var settings = load("res://modules/ui/settings_menu/settings_menu.tscn").instantiate()
	get_tree().current_scene.add_child(settings) 


func _on_exit_button_pressed():
	get_tree().quit()
