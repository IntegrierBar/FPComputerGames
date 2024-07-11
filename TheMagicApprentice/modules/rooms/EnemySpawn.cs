using Godot;

public partial class EnemySpawn : Node2D
{
	public Node2D Spawn()
	{
		Node2D Slime = ResourceLoader.Load<PackedScene>("res://modules/entities/slimes/slime.tscn").Instantiate() as Node2D;
		Slime.GlobalPosition = GlobalPosition;
		RoomHandler RoomHandler = GetTree().GetFirstNodeInGroup("room_handler") as RoomHandler;
		Slime slimeInstance = Slime as Slime;
		if (slimeInstance != null)
		{
			slimeInstance.SetSlimeProperties(MagicType.SUN, SlimeSize.SMALL, SlimeAttackRange.MELEE, GlobalPosition, 100);
		}
		RoomHandler.CurrentRoomNode.AddChild(Slime);

		return Slime;
	}
}
