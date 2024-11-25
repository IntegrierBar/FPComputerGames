namespace Tests;

using GdUnit4;
using Godot;
using System;
using static GdUnit4.Assertions;

/**
Integration test for the SkillTree.
*/
[TestSuite]
public partial class TestSkillTreeIntegration 
{
	private ISceneRunner _mainGameScene; ///< scene runner of the main game
	private SkillTree _skillTree; ///< Reference to the SkillTree inside the scene tree

	[BeforeTest]
	public void SetupTest()
	{
		_mainGameScene = ISceneRunner.Load("res://modules/entities/player/inventory/spells/skill_tree.tscn");
		var marginContainer = _mainGameScene.FindChild("MarginContainer");
		_skillTree = marginContainer.GetParent() as SkillTree;
		AssertObject(_skillTree).IsNotNull();
	}

	[TestCase(4, 5, 6, 1, 5, -1, TestName = "Select same skill again")]
	[TestCase(4, 5, 6, 1, 6, 2, TestName = "Skill in Slot 3 is dublicate")]
	[TestCase(4, 5, 6, 2, 5, 1, TestName = "Skill in Slot 2 is dublicate")]
	[TestCase(4, 4, 4, 2, 4, 1, TestName = "All Skills are dublicate")]
	[TestCase(1, -1, -1, 1, 2, -1, TestName = "No dublicate")]
	public void TestIsSkillCurrentlyEquipped(int index1, int index2, int index3, int nrSkillSlot, int newIndex, int result)
	{
		OptionButton node1 = _skillTree.GetNode<OptionButton>("%OptionsSkillSlot1");
		OptionButton node2 = _skillTree.GetNode<OptionButton>("%OptionsSkillSlot2");
		OptionButton node3 = _skillTree.GetNode<OptionButton>("%OptionsSkillSlot3");
		node1.Select(index1);
		node2.Select(index2);
		node3.Select(index3);

		int resultSkillSlot = _skillTree.IsSkillCurrentlyEquipped(nrSkillSlot, newIndex);
		AssertThat(resultSkillSlot).IsEqual(result);
	}

	/**
	tests if adding skill points works and then tests if spending the skill points to unlock the skills works
	*/
	[TestCase]
	public void TestAddingSkillPoints()
	{
		// first add skill points (3 Sun 2 Cosmix 1 Dark)
		_skillTree.AddSkillPointOfType(MagicType.SUN);
		_skillTree.AddSkillPointOfType(MagicType.SUN);
		_skillTree.AddSkillPointOfType(MagicType.SUN);
		_skillTree.AddSkillPointOfType(MagicType.COSMIC);
		_skillTree.AddSkillPointOfType(MagicType.COSMIC);
		_skillTree.AddSkillPointOfType(MagicType.DARK);

		// check that that worked
		AssertInt(_skillTree.GetSkillPointsOfType(MagicType.SUN)).IsEqual(3); 
		AssertInt(_skillTree.GetSkillPointsOfType(MagicType.COSMIC)).IsEqual(2);
		AssertInt(_skillTree.GetSkillPointsOfType(MagicType.DARK)).IsEqual(1);
	}
}
