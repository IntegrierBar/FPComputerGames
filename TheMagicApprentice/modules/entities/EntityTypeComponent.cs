using Godot;
using System;

public partial class EntityTypeComponent : Node
{
	/**
	\enum EntityType
	\brief Enum for the different types of entities

	The different types are None, Player, Enemy, Boss, Projectile, Slime, and Unicorn.
	This enum uses flags, meaning an entity can be a combination of types. For example, a Slime is both a Slime and an Enemy.
	*/
	[Flags]
	public enum EntityType
	{
		None = 0,
		Player = 1 << 0,
		Enemy = 1 << 1,
		Boss = 1 << 2,
		Projectile = 1 << 3,

		// Specific enemy types
		Slime = Enemy | (1 << 4),
		Unicorn = Enemy | Boss | (1 << 5)
	}

	[Export(PropertyHint.Enum, "Player,Enemy,Boss,Projectile,Slime,Unicorn")]
	public EntityType Type { get; set; } = EntityType.None;

	/**
	\brief Checks if the entity is of a specific type

	\param type The type to check against
	\return True if the entity is of the specified type, false otherwise
	*/
	public bool IsOfType(EntityType type)
	{
		return (Type & type) == type;
	}

	/**
	\brief Called when the node enters the scene tree for the first time
	*/
	public override void _Ready()
	{
		GD.Print($"Entity type: {Type}");
	}
}
