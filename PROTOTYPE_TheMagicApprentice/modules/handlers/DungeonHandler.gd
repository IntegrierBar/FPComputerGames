extends Node2D

const MIN_ROOMS = 5
const MAX_ROOMS = 10
const MIN_DISTANCE_TO_BOSS = 2
const GRID_SIZE = 2 * MAX_ROOMS + 1

var dungeon_layout = []
var entrance_pos = Vector2()
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
	queue_redraw()
	update_info_label()


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

func _draw():
	var cell_size = 20
	for i in range(GRID_SIZE):
		for j in range(GRID_SIZE):
			var pos = Vector2(i, j)
			var color = Color()
			if pos == entrance_pos:
				color = Color(0, 1, 0)  # Green for start room
			elif pos == boss_pos:
				color = Color(1, 0, 0)  # Red for boss room
			else:
				color = Color(1, 1, 1) if dungeon_layout[i][j] else Color(0, 0, 0)
			draw_rect(Rect2(i * cell_size, j * cell_size, cell_size, cell_size), color)

func update_info_label():
	var info_label = $UI/Control/InfoLabel
	info_label.text = "Start Room: (%d, %d)\nBoss Room: (%d, %d)\nMax Number of Rooms: %d\nNumber of Rooms: %d" % [
		entrance_pos.x, entrance_pos.y, boss_pos.x, boss_pos.y, MAX_ROOMS, num_rooms
	]
