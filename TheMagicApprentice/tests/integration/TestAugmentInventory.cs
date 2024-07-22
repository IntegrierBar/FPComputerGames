namespace Tests;

using GdUnit4;
using Godot;
using System;
using static GdUnit4.Assertions;
using GdUnit4.Executions;
using GdUnit4.Executions.Monitors;
using System.Threading.Tasks;

/**
Integration test for the AugmentInventory.
*/

[TestSuite]
public partial class TestAugmentInventory
{
	private ISceneRunner _mainGameScene;
	private AugmentInventory _augmentInventory;


	[BeforeTest]
	public void SetupTest()
	{
        _mainGameScene = ISceneRunner.Load("res://main_game.tscn");
        _augmentInventory = _mainGameScene.FindChild("AugmentInventory") as AugmentInventory;
        AssertObject(_augmentInventory).IsNotNull();

        // Assert that there are exactly 5 active slots and at least 10 inactive
        AssertInt(_augmentInventory.GetActiveAugments().GetChildCount()).IsEqual(5);
        AssertInt(_augmentInventory.GetInactiveAugments().GetChildCount()).IsGreater(10);

	}


    [TestCase]
    public void TestAddAugmentToInventory()
    {
        // check that first augment slot is empty
        GridContainer grid = _augmentInventory.GetInactiveAugments();
        AssertInt(grid.GetChild(0).GetChildCount()).IsEqual(0);

        // create an augment and add it to the inventory
        Augment augment = AugmentManager.Instance.CreateRandomAugment(3);
        _augmentInventory.AddAugmentToInventory(augment);

        AssertInt(grid.GetChild(0).GetChildCount()).IsEqual(1);
    }


    /**
    Test that when adding an augment and all slots are full, new slots are created
    */
    [TestCase]
    public void TestInventoryOverflow()
    {
        // check that all augment slots are empty
        GridContainer grid = _augmentInventory.GetInactiveAugments();
        foreach (var slot in grid.GetChildren())
        {
            AssertInt(slot.GetChildCount()).IsEqual(0);
        }

        // get the amount of slots at the start
        int startSlotCount = grid.GetChildCount();

        // create and add as many augments to the inventory as there are slots
        for (int i = 0; i < startSlotCount; i++)
        {
            Augment augment = AugmentManager.Instance.CreateRandomAugment(3);
            _augmentInventory.AddAugmentToInventory(augment);
        }

        // check that all augment slots are full
        foreach (var slot in grid.GetChildren())
        {
            AssertInt(slot.GetChildCount()).IsEqual(1);
        }

        // add one more augment
        Augment extraAugment = AugmentManager.Instance.CreateRandomAugment(3);
        _augmentInventory.AddAugmentToInventory(extraAugment);

        // check that the amount of slots has increased
        AssertInt(grid.GetChildCount()).IsGreater(startSlotCount);
    }

}
