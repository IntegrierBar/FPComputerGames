extends GutTest

var HealthComponent = load("res://modules/entities/HealthComponent.gd")
var _health_component = null;
var _attacker = null;

# create different parameters to test
var parameters = ParameterFactory.named_parameters(
	['MAX_HP', 'armor', 'damage', 'damage_type', 'result_hp', 'attacker_hp'],	# names
	[									# values
		[100., {Enums.MagicType.SUN: 0., Enums.MagicType.COSMIC: 0., Enums.MagicType.DARK: 0.}, 10., Enums.MagicType.SUN, 90., 100.], # default no armor
		[123., {Enums.MagicType.SUN: 50., Enums.MagicType.COSMIC: 20., Enums.MagicType.DARK: 0.}, 30., Enums.MagicType.SUN, 123. - 15., 100.], # armor SUN
		[50., {Enums.MagicType.SUN: 40., Enums.MagicType.COSMIC: 20., Enums.MagicType.DARK: 10.}, 10., Enums.MagicType.COSMIC, 50. - 8., 100.], # armor COSMIC
		[10., {Enums.MagicType.SUN: 120., Enums.MagicType.COSMIC: 30., Enums.MagicType.DARK: 90.}, 5., Enums.MagicType.DARK, 10. - 0.5, 100.], # armor DARK
		[10., {Enums.MagicType.SUN: 120., Enums.MagicType.COSMIC: 30., Enums.MagicType.DARK: 90.}, 100., Enums.MagicType.SUN, 10., 100. - 20.], # reflect damage
	]
)


func before_each():
	_health_component = HealthComponent.new()
	_attacker = HealthComponent.new()
	_attacker.MAX_HP = 100.
	_attacker.armor = {Enums.MagicType.SUN: 0., Enums.MagicType.COSMIC: 0., Enums.MagicType.DARK: 0.}
	add_child_autofree(_attacker)

func after_each():
	_health_component.free()

func test_take_damage_function(params = use_parameters(parameters)):
	_health_component.MAX_HP = params.MAX_HP
	#_health_component.current_HP = 100	# set current HP cause I am not sure if onready is called
	_health_component.armor = params.armor
	
	# add the node so that current HP gets initialized
	add_child_autofree(_health_component)
	var attack: Attack = Attack.new()
	attack.damage = params.damage
	attack.type = params.damage_type
	attack.attacker = _attacker
	
	_health_component.take_damage(attack)
	assert_eq(params.result_hp, _health_component.current_HP, "take_damage miscalculated")
	# now check that attackers HP are correct
	assert_eq(params.attacker_hp, _attacker.current_HP, "reflection was wrong")
