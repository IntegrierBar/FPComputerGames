class_name MainMenu
extends Control


func _on_new_game_button_pressed():
	print("New game")


func _on_continue_button_pressed():
	print("Continue")


# Note: the settings menu is rendered after the main menu and therefore visisble
# without the ColorRect as background, the main menu would be visisble behind the settings menu 
func _on_settings_button_pressed():
	var settings = load("res://modules/ui/settings_menu/settings_menu.tscn").instantiate()
	get_tree().current_scene.add_child(settings) 


func _on_exit_button_pressed():
	get_tree().quit()
