using Godot;

/**
 * The EnemySpawn class represents a node in the room where enemies can be spawned.
 * These nodes are spread around the room and initialized by the RoomHandler.
 * They set the initial values for slimes when they are spawned.
 */
public partial class EnemySpawn : Node2D
{
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
			if (GD.Randf() < 0.25f)
			{
				slimeMagicType = magicType;
			}
			else
			{
				slimeMagicType = EntityTypeHelper.GetRandomMagicType();
			}
			SlimeSize size = GD.Randf() < 0.33f ? SlimeSize.LARGE : SlimeSize.SMALL;
			SlimeAttackRange range = (size == SlimeSize.LARGE || GD.Randf() < 0.5f) ? SlimeAttackRange.MELEE : SlimeAttackRange.RANGED;
			slimeInstance.SetSlimeProperties(slimeMagicType, size, range, GlobalPosition);
		}
		RoomHandler.CurrentRoomNode.AddChild(Slime);

		return Slime;
	}
}
