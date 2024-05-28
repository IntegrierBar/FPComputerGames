extends Area2D

var magic_type: Enums.MagicType = Enums.MagicType.SUN
var damage: float = 50.
var attacker: HealthComponent
var attack: Attack

var speed: float = 100
var direction: Vector2

func _ready():
	attack = Attack.new()
	attack.type = magic_type
	attack.damage = damage
	attack.attacker = attacker

func cast(pos: Vector2, target: Vector2):
	position = pos
	direction = (target-pos).normalized()
	look_at(target)

func _physics_process(delta):
	position += speed * delta * direction

func _on_area_entered(area):
	if area is HealthComponent:
		area.take_damage(attack)
		queue_free()
