using Godot;
using System;
using System.Collections.Generic;

public partial class DungeonHandler : Node2D
{
	private const int MIN_ROOMS = 5;
	private const int MAX_ROOMS = 10;
	private const int GRID_SIZE = 2 * MAX_ROOMS + 1;

	private bool[,] dungeonLayout;
	private Vector2 entrancePos;
	private Vector2 bossPos;
	private int numRooms;

	public override void _Ready()
	{
		dungeonLayout = new bool[GRID_SIZE, GRID_SIZE];
		GD.Print("Test");
		Regenerate();
	}

	public void Regenerate()
	{
		GenerateDungeonLayout();
		QueueRedraw();
		UpdateInfoLabel();
	}

	private void GenerateDungeonLayout()
	{
		// Start Generation Here
		GD.Print("Dungeon Generation Started");

		numRooms = new Random().Next(MIN_ROOMS, MAX_ROOMS + 1);
		entrancePos = new Vector2(GRID_SIZE / 2, GRID_SIZE / 2);
		bossPos = entrancePos;

		while (Math.Abs(entrancePos.x - bossPos.x) + Math.Abs(entrancePos.y - bossPos.y) < MIN_DISTANCE_TO_BOSS + 1)
		{
			for (int i = 0; i < GRID_SIZE; i++)
			{
				for (int j = 0; j < GRID_SIZE; j++)
				{
					dungeonLayout[i, j] = false;
				}
			}

			dungeonLayout[(int)entrancePos.x, (int)entrancePos.y] = true;

			List<Vector2> visitedTiles = new List<Vector2> { entrancePos };
			Vector2 currentPos = entrancePos;

			for (int i = 0; i < numRooms - 1; i++)
			{
				List<Vector2> directions = new List<Vector2>
				{
					new Vector2(1, 0),
					new Vector2(-1, 0),
					new Vector2(0, 1),
					new Vector2(0, -1)
				};

				directions.Shuffle();

				bool moved = false;
				foreach (Vector2 direction in directions)
				{
					nextPos = currentPos + direction;
					nextPos.x = Mathf.Clamp(nextPos.x, 0, GRID_SIZE - 1);
					nextPos.y = Mathf.Clamp(nextPos.y, 0, GRID_SIZE - 1);

					if (!dungeonLayout[(int)nextPos.x, (int)nextPos.y])
					{
						dungeonLayout[(int)nextPos.x, (int)nextPos.y] = true;
						visitedTiles.Add(nextPos);
						currentPos = nextPos;
						moved = true;
						break;
					}
				}

				if (!moved)
				{
					currentPos = visitedTiles[new Random().Next(visitedTiles.Count)];
				}
			}

			bossPos = currentPos;
		}
	}

	public override void _Draw()
	{
		int cellSize = 20;
		for (int i = 0; i < GRID_SIZE; i++)
		{
			for (int j = 0; j < GRID_SIZE; j++)
			{
				Vector2 pos = new Vector2(i, j);
				Color color = new Color();

				if (pos == entrancePos)
				{
					color = new Color(0, 1, 0);
				}
				else if (pos == bossPos)
				{
					color = new Color(1, 0, 0);
				}
				else
				{
					color = dungeonLayout[i, j] ? new Color(1, 1, 1) : new Color(0, 0, 0);
				}

				DrawRect(new Rect2(i * cellSize, j * cellSize, cellSize, cellSize), color);
			}
		}
	}

	private void UpdateInfoLabel()
	{
		Label infoLabel = GetNode<Label>("UI/Control/InfoLabel");
		infoLabel.Text = $"Start Room: ({entrancePos.x}, {entrancePos.y})\nBoss Room: ({bossPos.x}, {bossPos.y})\nMax Number of Rooms: {MAX_ROOMS}\nNumber of Rooms: {numRooms}";
	}
}
