namespace Tests;

using System.Collections.Generic;
using System.Threading.Tasks;
using GdUnit4;
using Godot;
using static GdUnit4.Assertions;

[TestSuite]
public class TestDungeonGeneration
{
    private DungeonHandler _dungeonHandler;

    [BeforeTest]
    public void SetupTest()
    {
        _dungeonHandler = AutoFree(new DungeonHandler());
    }

    [TestCase(5, 10, TestName = "Dungeon Generation with 5 to 10 rooms")]
    [TestCase(7, 7, TestName = "Dungeon Generation with exactly 7 rooms")]
    [TestCase(10, 10, TestName = "Dungeon Generation with maximum 10 rooms")]
    public void TestDungeonGeneration(int minRooms, int maxRooms)
    {
        _dungeonHandler.MIN_ROOMS = minRooms;
        _dungeonHandler.MAX_ROOMS = maxRooms;
        _dungeonHandler.Regenerate();

        var dungeonLayout = _dungeonHandler.GetDungeonLayout();
        var numRooms = _dungeonHandler.GetNumRooms();
        var entrancePos = _dungeonHandler.GetEntrancePos();
        var bossPos = _dungeonHandler.GetBossPos();

        // Check the number of rooms
        AssertThat(numRooms).IsGreaterOrEqualTo(minRooms);
        AssertThat(numRooms).IsLessOrEqualTo(maxRooms);

        // Check the grid-like pattern and room selection
        int roomCount = 0;
        for (int i = 0; i < dungeonLayout.GetLength(0); i++)
        {
            for (int j = 0; j < dungeonLayout.GetLength(1); j++)
            {
                if (dungeonLayout[i, j])
                {
                    roomCount++;
                }
            }
        }
        AssertThat(roomCount).IsEqual(numRooms);

        // Check the distance between entrance and boss room
        var distance = CalculateDistance(entrancePos, bossPos);
        AssertThat(distance).IsGreaterOrEqualTo(2);
    }

    private int CalculateDistance(Vector2 entrancePos, Vector2 bossPos)
    {
        return Math.Abs((int)entrancePos.x - (int)bossPos.x) + Math.Abs((int)entrancePos.y - (int)bossPos.y);
    }
}