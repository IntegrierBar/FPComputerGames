extends Node2D

var dungeon_handler
var player
var entrance_points = []
var exit_points = []
var room
var tilemap

func _ready():
	init()

func init():
	dungeon_handler = get_tree().get_first_node_in_group("dungeonhandler")
	player = get_tree().get_first_node_in_group("player")
	entrance_points = get_tree().get_nodes_in_group("EntrancePoints")
	exit_points = get_tree().get_nodes_in_group("ExitPoints")
	room = get_tree().get_first_node_in_group("room")
	tilemap = room.get_node("TileMap")

func _on_dungeon_generated(event):
	if event == Enums.DungeonEvent.GENERATED:
		init()
		player.position = Vector2(0, 0)

		for exit_point in exit_points:
			var position = tilemap.local_to_map(exit_point.position)
			var tile_id = tilemap.get_cell_source_id(0, position)
			print("Tile ID at exit point: ", tile_id, " at position: ", position)
		var player_map_pos = tilemap.local_to_map(player.position)
		print("Tile ID at point: ", tilemap.get_cell_source_id(0, player_map_pos), " at position: ", player_map_pos)
		print(tilemap.layers)
