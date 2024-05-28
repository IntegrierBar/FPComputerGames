# RoomExit.gd
extends Area2D

func _ready():
	print("connecting")
	var body_error = self.body_entered.connect(_on_body_entered)
	if body_error != OK:
		print("Failed to connect body_entered signal:", body_error)

func _on_body_entered(body):
	print("test")
	get_tree().change_scene_to_file("res://modules/ui/main_hub/main_hub.tscn")
