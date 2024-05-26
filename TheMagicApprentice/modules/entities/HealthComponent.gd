class_name HealthComponent extends Area2D
## Health Component handels everything related to HP
## Always needs to be direct child of the object that is supposed to have HP

## Emitted when the Health component reaches 0 HP
signal death

@export var MAX_HP: float = 100
@onready var current_HP: float = MAX_HP

@export var armor: Dictionary = {
	Enums.MagicType.SUN: 30.,
	Enums.MagicType.COSMIC: 30.,
	Enums.MagicType.DARK: 30.,
}

func take_damage(attack: Attack) -> void:
	if armor[attack.type] > 100.:
		var reflected_damage = Attack.new()
		reflected_damage.damage = attack.damage * (armor[attack.type]/100. - 1.)
		reflected_damage.type = attack.type
		reflected_damage.attacker = get_parent()
		attack.attacker.take_damage(reflected_damage) # TODO need to think how we call take_damage function
		return
	
	current_HP -= attack.damage * (1. - armor[attack.type]/100.)
	if current_HP <= 0:
		emit_signal("death")
