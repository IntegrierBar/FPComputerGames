using Godot;
using System;
using System.Collections;


/**
The basic spell object of each element.
Setting the element will also change the color.

Note that since we use an Area2D which we manually move, if the spells moves ultra fast it can move over the enemies and thus miss them.
*/
public partial class BasicSpell : Spell
{
	[Export]
	public float SPEED = 600; ///< Speed of the spell. Do not set to high or else it might not hit enemies

	private Vector2 _direction; ///< Direction in which the spell moves

	CpuParticles2D _trailParticles;
	CpuParticles2D _collisionParticles;
	Light2D _light;
	Light2D _flash;

	[Export]
	public Color SunTrailColor = new Color(1, 0.8f, 0);
	[Export]
	public Color SunCollisionColor = new Color(1, 0.6f, 0);
	[Export]
	public Color CosmicTrailColor = new Color(0.3f, 0.7f, 1);
	[Export]
	public Color CosmicCollisionColor = new Color(0, 0.5f, 1);
	[Export]
	public Color DarkTrailColor = new Color(0.5f, 0, 0.5f);
	[Export]
	public Color DarkCollisionColor = new Color(0.3f, 0, 0.3f);

	/**
	Call after instantiating the base spell scene in order to set the Attack of the spell and change the animation depending on the magic type.
	*/
	public override void Init(Attack attack, Vector2 playerPosition, Vector2 targetPosition)
	{
		base.Init(attack, playerPosition, targetPosition);

		Position = playerPosition;
		_direction = (targetPosition-playerPosition).Normalized();

		LookAt(targetPosition); // make spell look in the correct direction

		// change the color depending on the magic type
		AnimatedSprite2D sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		String animations_name = EntityTypeHelper.GetMagicTypeAsString(_attack.magicType) + "_basic_spell";
		sprite.Play(animations_name);

		InitParticles();


		if (_direction.X < 0) // flip sprite vertically if the projectile flies to the left (so that down remains down in the image)
		{
			sprite.FlipV = true;
		}
	}


	private void InitParticles()
	{
		_trailParticles = GetNode<CpuParticles2D>("TrailParticles");
		_collisionParticles = GetNode<CpuParticles2D>("CollisionParticles");
		_light = GetNode<Light2D>("Light");
		_flash = GetNode<Light2D>("Flash");

		// Set colors based on magic type
		Color trailColor;
		Color collisionColor;
		switch (_attack.magicType)
		{
			case MagicType.SUN:
				trailColor = SunTrailColor; // Bright yellow
				collisionColor = SunCollisionColor; // Orange
				break;
			case MagicType.COSMIC:
				trailColor = CosmicTrailColor; // Light blue
				collisionColor = CosmicCollisionColor; // Blue
				break;
			case MagicType.DARK:
				trailColor = DarkTrailColor; // Purple
				collisionColor = DarkCollisionColor; // Dark purple
				break;
			default:
				trailColor = new Color(1, 1, 1); // White (fallback)
				collisionColor = new Color(1, 1, 1); // White (fallback)
				break;
		}

		_trailParticles.Color = trailColor;
		_collisionParticles.Color = collisionColor;
		_light.Color = trailColor;
		_flash.Color = trailColor;
	}

	/**
	Move the spell in _direction
	Count down the max life time of the spell and remove the spell once the time is up
	*/
	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		Position += (float)delta * SPEED * _direction;
	}


	/**
	Gets called when the spell hits a Health component since health components use area2Ds.
	Since the spells mask layer is set to the enemies layer, it cannot hit the player
	*/
	public override void OnAreaEntered(Area2D area)
	{
		if (area is HealthComponent healthComponent) // check if area is a health component and if true cast it as a healthcomponent under the name healthComponent
		{
			healthComponent.TakeDamage(_attack);
			PlayCollisionParticles();
			StopTrailParticles();
			StartFlash();
			QueueFree();
		}
	}

	/**
	Since parts of the tilemap that have a collision layer are not area2D nodes, body entered is necessary to use.
	This function detects collisions with all types of 2D nodes.
	Check if the projectile entered a part of the tilemap, which means a wall or object, and remove the projectile. 
	This requires mask 1 (Collision) to be set!
	*/
	public void OnBodyEntered(Node2D body)
	{
		if (body is TileMap)
		{
			PlayCollisionParticles();
			StopTrailParticles();
			StartFlash();
			QueueFree();
		}
	}

	private void StartFlash()
	{
		AnimationPlayer flashAnimationPlayer = _flash.GetNode<AnimationPlayer>("AnimationPlayer");
		flashAnimationPlayer.Active = true;
		flashAnimationPlayer.Play("flashAnimation");

		Vector2 globalPos = _flash.GlobalPosition;
	
		// Remove the flash from its parent to prevent it from being freed with the spell
		_flash.GetParent().RemoveChild(_flash);
		GetParent().AddChild(_flash);
		_flash.GlobalPosition = globalPos;

		// Use a weak reference to avoid potential memory leaks
		WeakReference<Light2D> weakFlash = new WeakReference<Light2D>(_flash);

		// Explanation of WeakReference:
		// A WeakReference allows the garbage collector to collect the referenced object
		// if there are no other strong references to it. This is useful in callback scenarios
		// like this one, where we want to avoid keeping the _flash object alive indefinitely
		// if the spell and its components are destroyed before the animation finishes.
		// By using a WeakReference, we ensure that:
		// 1. If the _flash object is still alive when the animation finishes, we can access and free it.
		// 2. If the _flash object has been garbage collected, we won't cause a null reference exception.
		// If we omit the WeakReference, the _flash object will not be freed and we will have a memory leak.

		// Queue free the flash after the animation is done
		flashAnimationPlayer.AnimationFinished += (StringName animName) =>
		{
			if (weakFlash.TryGetTarget(out Light2D flash))
			{
				flash.QueueFree();
			}
		};
	}

	private void StopTrailParticles()
	{
		Vector2 globalPos = _trailParticles.GlobalPosition;
		_trailParticles.GetParent().RemoveChild(_trailParticles);
		GetParent().AddChild(_trailParticles);
		_trailParticles.GlobalPosition = globalPos;
		_trailParticles.Emitting = false;
		CreateTimer(_trailParticles.Lifetime, _trailParticles);
	}

	private void PlayCollisionParticles()
	{
		_collisionParticles.Emitting = true;
		Vector2 globalPos = _collisionParticles.GlobalPosition;
		_collisionParticles.GetParent().RemoveChild(_collisionParticles);
		GetParent().AddChild(_collisionParticles);
		_collisionParticles.GlobalPosition = globalPos;
		CreateTimer(_collisionParticles.Lifetime, _collisionParticles);
	}

	private void CreateTimer(double duration, CpuParticles2D particles)
	{
		GetTree().CreateTimer(duration).Timeout += () =>
		{
			particles.QueueFree();
		};
	}
}
