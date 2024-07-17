using GdUnit4;
using Godot;
using System;
using static GdUnit4.Assertions;

[TestSuite]
public partial class TestRoom
{
    [TestCase]
    public void TestRoomInitialization()
    {
        // Arrange
        RoomType type = RoomType.Normal;
        string scenePath = "res://modules/rooms/Room3.tscn";

        Room room = new Room(type, scenePath);

        AssertThat(room.Type).IsEqual(type);
        AssertThat(room.ScenePath).IsEqual(scenePath);
        AssertThat(room.IsVisited).IsFalse();
        AssertThat(room.IsCleared).IsFalse();
    }

    [TestCase]
    public void TestRoomTypeEnum()
    {
        AssertThat(Enum.IsDefined(typeof(RoomType), RoomType.Normal)).IsTrue();
        AssertThat(Enum.IsDefined(typeof(RoomType), RoomType.Boss)).IsTrue();
    }

    [TestCase]
    public void TestRoomProperties()
    {
        Room room = new Room(RoomType.Boss, "res://modules/rooms/Room4.tscn");

        room.IsVisited = true;
        room.IsCleared = true;

        AssertThat(room.Type).IsEqual(RoomType.Boss);
        AssertThat(room.ScenePath).IsEqual("res://modules/rooms/Room4.tscn");
        AssertThat(room.IsVisited).IsTrue();
        AssertThat(room.IsCleared).IsTrue();
    }
}
