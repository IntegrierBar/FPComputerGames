class_name SettingsMenu
extends Control


func _ready():
	process_mode = Node.PROCESS_MODE_ALWAYS


# Removes the settings menu from the scene tree 
# This means: when the settings menu is called, it always needs to be added to the current scene as a child!
# Otherwise this part does not make sense...
# There is probably a smarter way to do this
func _on_exit_button_pressed():
	queue_free()


# Note: there are two fullscreen options in Godot. I chose this one at random, because I dont see a difference.
func _on_fullscreen_check_toggled(toggled_on):
	if toggled_on:
		DisplayServer.window_set_mode(DisplayServer.WINDOW_MODE_FULLSCREEN)
	else:
		DisplayServer.window_set_mode(DisplayServer.WINDOW_MODE_WINDOWED)
