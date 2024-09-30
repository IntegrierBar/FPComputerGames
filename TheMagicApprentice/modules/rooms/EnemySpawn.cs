using Godot;

public enum SpawnType {
	RANDOM,
	MELEE_SLIME,
	BIG_SLIME,
	RANGED_SLIME,
	UNICORN
	
}

/**
 * The EnemySpawn class represents a node in the room where enemies can be spawned.
 * These nodes are spread around the room and initialized by the RoomHandler.
 * They set the initial values for slimes when they are spawned.
 */
public partial class EnemySpawn : Node2D
{

	[Export]
	public SpawnType SpawnType = SpawnType.RANDOM;

	/**
	 * Spawns a slime enemy at the position of this EnemySpawn node.
	 * 
	 * @return The spawned slime enemy as a Node2D.
	 */
	public Node2D Spawn()
	{
		DungeonHandler dungeonHandler = GetTree().GetFirstNodeInGroup("dungeon_handler") as DungeonHandler;
		MagicType magicType = dungeonHandler.GetMagicType();

		Node2D Slime = ResourceLoader.Load<PackedScene>("res://modules/entities/slimes/slime.tscn").Instantiate() as Node2D;
		Slime.GlobalPosition = GlobalPosition;
		RoomHandler RoomHandler = GetTree().GetFirstNodeInGroup("room_handler") as RoomHandler;
		Slime slimeInstance = Slime as Slime;
		if (slimeInstance != null)
		{
			MagicType slimeMagicType;
			SlimeSize size;
			SlimeAttackRange range;

			if (SpawnType == SpawnType.RANDOM)
			{
				if (GD.Randf() < 0.25f)
				{
					slimeMagicType = magicType;
				}
				else
				{
					slimeMagicType = EntityTypeHelper.GetRandomMagicType();
				}
				size = GD.Randf() < 0.33f ? SlimeSize.LARGE : SlimeSize.SMALL;
				range = (size == SlimeSize.LARGE || GD.Randf() < 0.5f) ? SlimeAttackRange.MELEE : SlimeAttackRange.RANGED;
			}
			else
			{
				slimeMagicType = magicType;
				switch (SpawnType)
				{
					case SpawnType.MELEE_SLIME:
						size = SlimeSize.SMALL;
						range = SlimeAttackRange.MELEE;
						break;
					case SpawnType.BIG_SLIME:
						size = SlimeSize.LARGE;
						range = SlimeAttackRange.MELEE;
						break;
					case SpawnType.RANGED_SLIME:
						size = SlimeSize.SMALL;
						range = SlimeAttackRange.RANGED;
						break;
					case SpawnType.UNICORN:
						size = SlimeSize.LARGE;
						range = SlimeAttackRange.RANGED;
						break;
					default:
						size = SlimeSize.SMALL;
						range = SlimeAttackRange.MELEE;
						break;
				}
			}
			slimeInstance.SetSlimeProperties(slimeMagicType, size, range, GlobalPosition);
		}
		RoomHandler.CurrentRoomNode.AddChild(Slime);

		return Slime;
	}
}
