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
	private ISceneRunner _mainGameScene; ///< scene runner of the main game
	private AugmentInventory _augmentInventory; ///< Reference to the AugmentInventory inside the scene tree

    /**
    Load the main game scene and get the reference to the AugmentInventory.
    Also Checks that the AugmentInventory was created
    */
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

    /**
    Test the function AddAugmentToInventory
    */
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

    /**
    Tests that active inventorySlots work and the augments are equiped.
    */
    [TestCase(0)]
    [TestCase(1)]
    [TestCase(2)]
    [TestCase(3)]
    [TestCase(4)]
    public void TestActiveInventorySlots(int slotIndex)
    {
        InventoryStarRain inventoryStarRain = _augmentInventory.GetTree().GetFirstNodeInGroup(Globals.StarRainSpellGroup) as InventoryStarRain;
        double startAmountStars = inventoryStarRain.AmountStarsToSpawn;

        // Create augment and equip it in active slot slotIndex
        Augment augment = TestAugments.CreateAugmentWithAugmenteffect("res://modules/augments/augment_effects/resources/additional_stars.tres");
        InventoryItem inventoryItem = new InventoryItem
        {
            Augment = augment
        };
        // Put it in the first slot of the grid so we dont get any null errors
        _augmentInventory.GetInactiveAugments().GetChild(0).AddChild(inventoryItem);

        // convert it to Variant in order to use _DropData
        Variant inventoryItemAsVariant = Variant.CreateFrom(inventoryItem);

        HBoxContainer activeSlots = _augmentInventory.GetActiveAugments();
        InventorySlot activeSlot = activeSlots.GetChild(slotIndex) as InventorySlot;
        AssertObject(activeSlot).IsNotNull();
        activeSlot._DropData(Vector2.Zero, inventoryItemAsVariant); // drop it into the slot

        AssertFloat(inventoryStarRain.AmountStarsToSpawn).IsGreater(startAmountStars); // Check if the augment effect is active

        // now unequip the augment by putting it back into the grid
        (_augmentInventory.GetInactiveAugments().GetChild(0) as InventorySlot)._DropData(Vector2.Zero, inventoryItemAsVariant);

        AssertFloat(inventoryStarRain.AmountStarsToSpawn).IsEqualApprox(startAmountStars, 1e-8); // Check if the augment effect is inactive
    }

}
