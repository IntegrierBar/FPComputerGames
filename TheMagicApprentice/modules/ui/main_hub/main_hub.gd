class_name MainHub
extends Control


func _process(delta):
	testEsc()


func _on_dungeon_button_pressed():
	get_tree().change_scene_to_file("res://example_scenes/dungeon.tscn")


func _on_exit_button_pressed():
	open_pause_menu()


func testEsc():
	if Input.is_action_just_pressed("esc"):
		open_pause_menu()


func open_pause_menu():
	var pause = load("res://modules/ui/pause_menu/pause_menu.tscn").instantiate()
	get_tree().current_scene.add_child(pause)
