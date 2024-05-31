class_name PauseMenu
extends Control


@onready var from_main_hub = false  # to remember where the pause menu is called from


func _ready(): 
	get_tree().paused = true
	if (get_parent() is MainHub):
		from_main_hub = true


func _process(delta):
	testEsc()


func testEsc():
	if Input.is_action_just_pressed("esc"):
		resume()


func _on_return_button_pressed():
	resume()


func resume():
	get_tree().paused = false
	queue_free()


func _on_settings_button_pressed():
	var settings = load("res://modules/ui/settings_menu/settings_menu.tscn").instantiate()
	add_child(settings) 


func _on_exit_button_pressed():
	get_tree().paused = false
	if from_main_hub:
		get_tree().change_scene_to_file("res://modules/ui/main_menu/main_menu.tscn")
	else:
		get_tree().change_scene_to_file("res://modules/ui/main_hub/main_hub.tscn")
