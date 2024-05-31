class_name Attack
## Class for passing around damge

var damage: float
var type: Enums.MagicType

## reference to the HealthComponent of the game object that deals the damage CAN BE NULL
var attacker: HealthComponent # In the future we might want this not to be a reference to a HEalthCOmpontent but the entitity itself. REMEMBER TO CHANGE REFLECTION in healthcomponent

