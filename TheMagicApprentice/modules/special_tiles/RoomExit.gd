# RoomExit.gd
extends Area2D

func _ready():
	var body_error = self.body_entered.connect(_on_body_entered)

func _on_body_entered(body):
	get_tree().change_scene_to_file("res://modules/ui/main_hub/main_hub.tscn")
