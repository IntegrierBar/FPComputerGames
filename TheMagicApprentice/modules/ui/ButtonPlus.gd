extends Button
class_name ButtonPlus
# Extension of Button Class that gives all buttons an audiostreamplayer that plays the button sound
# if the button is pressed
# All buttons therefore need to be of type ButtonPlus to make a sound
# Note: This is not an elegant solution, see https://www.youtube.com/watch?v=QgBecUl_lFs for an 
# alternative and hopefully more elegant solution for the real game
# Bug: Buttons that remove a scene from the tree (exit, continue etc.) do not emit the sound because 
# they are removed from the tree before emitting it
# This is not a problem at the moment but should be taken into consideration for next time

 
@onready var click_sound = AudioStreamPlayer.new()


# Called when the node enters the scene tree for the first time.
func _ready():
	click_sound.stream = load("res://example_assets/sound_effects/button.ogg")
	click_sound.bus = "sfx"
	pressed.connect(func(): click_sound.play())
	add_child(click_sound)


