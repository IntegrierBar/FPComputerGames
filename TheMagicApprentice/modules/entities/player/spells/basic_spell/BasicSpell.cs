using Godot;
using System;


/**
The base spell object of each element.
Setting the element will also change the color.

Note that since we use an Area2D which we manually move, if the spells moves ultra fast it can move over the enemies and thus miss them.
*/
public partial class BasicSpell : Spell
{
	private Vector2 _direction; ///< Direction in which to spell moves

	public float SPEED = 600; ///< Speed of the spell. Do not set to high or else it might not hit enemies


	/**
	Call after instantiating the base spell scene in order to set the Attack of the spell and change the color.
	*/
	public override void Init(Attack attack, Vector2 playerPosition, Vector2 targetPosition)
	{
		Position = playerPosition;
		_attack = attack;
		_direction = (targetPosition-playerPosition).Normalized();

		LookAt(targetPosition); // make spell look in the correct direction

		// change the color depending on the magic type
		Sprite2D sprite = GetNode<Sprite2D>("Sprite2D");
		switch (_attack.magicType)
		{
			case MagicType.SUN:
			{
				sprite.Modulate = new Color("YELLOW");
				break;
			}
			case MagicType.COSMIC:
			{
				sprite.Modulate = new Color("BLUE");
				break;
			}
			default:
			{
				sprite.Modulate = new Color("BLACK");
				break;
			}
		}
	}

	/**
	Change position of the spell.
	Count down the max life time of the spell and remove the spell once the time is up
	*/
    public override void _PhysicsProcess(double delta)
    {
		base._PhysicsProcess(delta);
        Position += (float)delta * SPEED * _direction;
    }


    /**
	Gets called when the spell hits a Health component.
	Since the spells mask layer is set to the enemies layer, it cannot hit the player
	*/
    public void OnAreaEntered(Area2D area)
	{
		GD.Print("hit area");
		GD.Print(area);
		if (area is HealthComponent healthComponent) // check if area is a health component and if true cast it as a healthcomponent under the name healthComponent
		{
			healthComponent.TakeDamage(_attack);
			GD.Print("dealt Damage");
			GD.Print(_attack.damage);
			GD.Print(healthComponent.GetCurrentHP());
			// once the spell has hit something we delete it
			QueueFree();
		}
		// TODO: Spell should also disappear when hitting a wall
	}



	

}
