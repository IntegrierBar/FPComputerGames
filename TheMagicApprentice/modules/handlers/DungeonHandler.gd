extends Node2D

signal dungeon_generated

const MIN_ROOMS = 5
const MAX_ROOMS = 10
const MIN_DISTANCE_TO_BOSS = 2
const GRID_SIZE = 2 * MAX_ROOMS + 1

var dungeon_layout = []
var entrance_pos = Vector2()
@export var player_pos = Vector2()
var boss_pos = Vector2()
var num_rooms = 0


func _ready():
	dungeon_layout = []
	for i in range(GRID_SIZE):
		var row = []
		for j in range(GRID_SIZE):
			row.append(false)
		dungeon_layout.append(row)

	regenerate()

func regenerate():
	generate_dungeon_layout()
	player_pos = entrance_pos
	dungeon_generated.emit(Enums.DungeonEvent.GENERATED)


func generate_dungeon_layout():
	num_rooms = randi() % (MAX_ROOMS - MIN_ROOMS + 1) + MIN_ROOMS
	entrance_pos = Vector2(int(GRID_SIZE / 2), int(GRID_SIZE / 2))
	boss_pos = entrance_pos

	while abs(entrance_pos.x - boss_pos.x) + abs(entrance_pos.y - boss_pos.y) < MIN_DISTANCE_TO_BOSS+1:

		# Initialize the dungeon layout as a grid of false values
		for i in range(GRID_SIZE):
			for j in range(GRID_SIZE):
				dungeon_layout[i][j] = false

		# Set the entrance and boss positions to true
		dungeon_layout[entrance_pos.x][entrance_pos.y] = true

		# Generate the rest of the rooms
		var visited_tiles = [entrance_pos]
		var current_pos = entrance_pos
		for i in range(num_rooms - 1):
			var next_pos = current_pos
			var directions = [
				Vector2(1, 0),  # Move right
				Vector2(-1, 0),  # Move left
				Vector2(0, 1),  # Move down
				Vector2(0, -1)  # Move up
			]
			directions.shuffle()

			var moved = false
			for direction in directions:
				next_pos = current_pos + direction
				next_pos.x = clamp(next_pos.x, 0, GRID_SIZE - 1)
				next_pos.y = clamp(next_pos.y, 0, GRID_SIZE - 1)
				if not dungeon_layout[next_pos.x][next_pos.y]:
					dungeon_layout[next_pos.x][next_pos.y] = true
					visited_tiles.append(next_pos)
					current_pos = next_pos
					moved = true
					break

			if not moved:
				# If no move was possible, pick a random visited tile and try again
				current_pos = visited_tiles[randi() % visited_tiles.size()]

		boss_pos = current_pos

# Helper method to get possible directions based on the current position in the layout
func get_possible_directions(pos: Vector2) -> Array:
	var directions = []
	if pos.x > 0 and not dungeon_layout[pos.x - 1][pos.y]:
		directions.append(Enums.Direction.LEFT)  # Move left
	if pos.x < GRID_SIZE - 1 and not dungeon_layout[pos.x + 1][pos.y]:
		directions.append(Enums.Direction.RIGHT)  # Move right
	if pos.y > 0 and not dungeon_layout[pos.x][pos.y - 1]:
		directions.append(Enums.Direction.UP)  # Move up
	if pos.y < GRID_SIZE - 1 and not dungeon_layout[pos.x][pos.y + 1]:
		directions.append(Enums.Direction.DOWN)  # Move down
	return directions
