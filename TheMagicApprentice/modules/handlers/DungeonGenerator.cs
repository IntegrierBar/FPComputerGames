using Godot;
using System;
using System.Collections.Generic;

public partial class DungeonGenerator : Node2D
{
	private const int MIN_DISTANCE_TO_BOSS = 2;
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

		while (Math.Abs(entrancePos.X - bossPos.X) + Math.Abs(entrancePos.Y - bossPos.Y) < MIN_DISTANCE_TO_BOSS + 1)
		{
			for (int i = 0; i < GRID_SIZE; i++)
			{
				for (int j = 0; j < GRID_SIZE; j++)
				{
					dungeonLayout[i, j] = false;
				}
			}

			dungeonLayout[(int)entrancePos.X, (int)entrancePos.Y] = true;

			List<Vector2> visitedTiles = new List<Vector2> { entrancePos };
			Vector2 currentPos = entrancePos;

			for (int i = 0; i < numRooms - 1; i++)
			{
				Vector2[] directions = new Vector2[]
				{
					new Vector2(1, 0),
					new Vector2(-1, 0),
					new Vector2(0, 1),
					new Vector2(0, -1)
				};

				for (int j = directions.Length - 1; j > 0; j--)
				{
					int k = new Random().Next(j + 1);
					Vector2 temp = directions[j];
					directions[j] = directions[k];
					directions[k] = temp;
				}

				bool moved = false;
				foreach (Vector2 direction in directions)
				{
					Vector2 nextPos = currentPos + direction;
					nextPos.X = Mathf.Clamp(nextPos.X, 0, GRID_SIZE - 1);
					nextPos.Y = Mathf.Clamp(nextPos.Y, 0, GRID_SIZE - 1);

					if (!dungeonLayout[(int)nextPos.X, (int)nextPos.Y])
					{
						dungeonLayout[(int)nextPos.X, (int)nextPos.Y] = true;
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
		infoLabel.Text = $"Start Room: ({entrancePos.X}, {entrancePos.Y})\nBoss Room: ({bossPos.X}, {bossPos.Y})\nMax Number of Rooms: {MAX_ROOMS}\nNumber of Rooms: {numRooms}";
	}
}
