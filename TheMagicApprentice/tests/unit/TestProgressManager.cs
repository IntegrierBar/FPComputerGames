namespace Tests;

using GdUnit4;
using Godot;
using static GdUnit4.Assertions;

[TestSuite]
public class TestProgressManager
{
    private ProgressManager _progressManager;

    [BeforeTest]
    public void SetupTest()
    {
        _progressManager = AutoFree(new ProgressManager());
        _progressManager._Ready(); // Initialize the lists
    }

    [TestCase(MagicType.SUN, MagicType.COSMIC, TestName = "Sun player gets Cosmic intro")]
    [TestCase(MagicType.COSMIC, MagicType.DARK, TestName = "Cosmic player gets Dark intro")]
    [TestCase(MagicType.DARK, MagicType.SUN, TestName = "Dark player gets Sun intro")]
    public void TestIntroDungeonType(MagicType playerType, MagicType expectedDungeonType)
    {
        _progressManager.SetPlayerStartMagicType(playerType);
        AssertThat(_progressManager.GetNextStoryDungeonType()).IsEqual(expectedDungeonType);
    }

    [TestCase]
    public void TestStoryDungeonSequence()
    {
        // If player chose Sun:
        // Intro: Cosmic
        // Story 1: Cosmic
        // Story 2: Dark
        // Story 3: Sun
        // Story 4: Cosmic
        // Story 5: Dark
        _progressManager.SetPlayerStartMagicType(MagicType.SUN);

        var expectedTypes = new MagicType[]
        {
            MagicType.SUN, // Story 1
            MagicType.COSMIC,   // Story 2
            MagicType.DARK,    // Story 3
            MagicType.SUN, // Story 4
            MagicType.COSMIC    // Story 5
        };

        for (int i = 0; i < expectedTypes.Length; i++)
        {
            GD.Print($"Testing story dungeon {i}, expected {expectedTypes[i]}, got {_progressManager.GetStoryDungeonType(i)}");
            AssertThat(_progressManager.GetStoryDungeonType(i)).IsEqual(expectedTypes[i]);
        }
    }

    [TestCase]
    public void TestDungeonCompletionTracking()
    {
        _progressManager.SetPlayerStartMagicType(MagicType.SUN);
        
        // Initially no dungeons are completed
        AssertThat(_progressManager.IsIntroDungeonCompleted()).IsFalse();
        AssertThat(_progressManager.IsStoryDungeonCompleted(0)).IsFalse();
        
        // Complete intro dungeon
        _progressManager.CompleteIntroDungeon();
        AssertThat(_progressManager.IsIntroDungeonCompleted()).IsTrue();
        
        // Complete first story dungeon
        _progressManager.CompleteStoryDungeon(0);
        AssertThat(_progressManager.IsStoryDungeonCompleted(0)).IsTrue();
        AssertThat(_progressManager.GetCurrentStoryDungeonIndex()).IsEqual(1);
        
        // Other dungeons should still be incomplete
        AssertThat(_progressManager.IsStoryDungeonCompleted(1)).IsFalse();
        AssertThat(_progressManager.AreAllStoryDungeonsCompleted()).IsFalse();
    }

    [TestCase]
    public void TestAllDungeonsCompletion()
    {
        _progressManager.SetPlayerStartMagicType(MagicType.COSMIC);
        
        // Complete all dungeons
        _progressManager.CompleteIntroDungeon();
        for (int i = 0; i < 5; i++)
        {
            _progressManager.CompleteStoryDungeon(i);
        }
        
        // Verify all are completed
        AssertThat(_progressManager.IsIntroDungeonCompleted()).IsTrue();
        AssertThat(_progressManager.AreAllStoryDungeonsCompleted()).IsTrue();
        AssertThat(_progressManager.GetCurrentStoryDungeonIndex()).IsEqual(5);
    }

    [TestCase]
    public void TestInvalidDungeonCompletion()
    {
        _progressManager.SetPlayerStartMagicType(MagicType.DARK);
        
        // Try to complete invalid dungeon indices
        _progressManager.CompleteStoryDungeon(-1);
        _progressManager.CompleteStoryDungeon(5);
        
        // Verify nothing changed
        AssertThat(_progressManager.GetCurrentStoryDungeonIndex()).IsEqual(0);
        AssertThat(_progressManager.AreAllStoryDungeonsCompleted()).IsFalse();
    }

    [TestCase]
    public void TestDungeonSequenceProgression()
    {
        _progressManager.SetPlayerStartMagicType(MagicType.SUN);
        
        // Initially should get Cosmic (type player is strong against) - same as intro dungeon
        AssertThat(_progressManager.GetNextStoryDungeonType()).IsEqual(MagicType.COSMIC);
        
        // Complete first dungeon and check next type
        _progressManager.CompleteStoryDungeon(0);
        // After completing first story dungeon (which is SUN type), next should be COSMIC
        AssertThat(_progressManager.GetNextStoryDungeonType()).IsEqual(MagicType.COSMIC);
        
        // Complete second dungeon and check next type
        _progressManager.CompleteStoryDungeon(1);
        // After completing second story dungeon (which is COSMIC type), next should be DARK
        AssertThat(_progressManager.GetNextStoryDungeonType()).IsEqual(MagicType.DARK);
    }
} 