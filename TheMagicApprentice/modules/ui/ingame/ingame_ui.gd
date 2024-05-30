class_name InGameUI
extends CanvasLayer


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	testEsc()


func testEsc():
	if Input.is_action_just_pressed("esc"):
		open_pause_menu()


func _on_pause_button_pressed():
	open_pause_menu()


func open_pause_menu():
	print(get_tree().current_scene)
	var pause = load("res://modules/ui/pause_menu/pause_menu.tscn").instantiate()
	add_child(pause)
