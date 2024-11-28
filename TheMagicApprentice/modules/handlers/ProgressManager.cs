using Godot;
using System;
using System.Collections.Generic;

public partial class ProgressManager : Node
{
    private MagicType _playerStartMagicType; // The magic type the player chose at start
    private List<bool> _storyDungeonsCompleted; // Tracks which story dungeons are completed
    private int _currentStoryDungeonIndex; // Index of the next story dungeon to play
    private bool _introDungeonCompleted;

    /**
	 * Called when the node is added to the scene tree, adds this node to the progress_manager group.
	 */
	public override void _EnterTree()
	{
		AddToGroup(Globals.ProgressManagerGroup);
	}

    public override void _Ready()
    {
        _storyDungeonsCompleted = new List<bool> { false, false, false, false, false }; // 5 story dungeons
        _currentStoryDungeonIndex = 0;
        _introDungeonCompleted = false;
    }

    public void SetPlayerStartMagicType(MagicType magicType)
    {
        _playerStartMagicType = magicType;
    }

    public bool IsIntroDungeonCompleted()
    {
        return _introDungeonCompleted;
    }

    public void CompleteIntroDungeon()
    {
        _introDungeonCompleted = true;
        // Add skill point of the intro dungeon's magic type (which is the type the player is strong against)
        var skillTree = GetTree().GetFirstNodeInGroup(Globals.PlayerGroup).GetNode<SkillTree>("SkillTree");
        skillTree.AddSkillPointOfType(EntityTypeHelper.GetWeakerMagicType(_playerStartMagicType));
        
        // Unlock the basic skill of that magic type
        skillTree.SetStartBasic(EntityTypeHelper.GetWeakerMagicType(_playerStartMagicType));
    }

    public MagicType GetNextStoryDungeonType()
    {
        // First story dungeon is same type as intro dungeon
        if (_currentStoryDungeonIndex == 0)
        {
            return EntityTypeHelper.GetWeakerMagicType(_playerStartMagicType);
        }

        // For subsequent dungeons, get the type that is weak against the previous dungeon's type
        MagicType previousType = GetStoryDungeonType(_currentStoryDungeonIndex - 1);
        return EntityTypeHelper.GetWeakerMagicType(previousType);
    }

    public MagicType GetStoryDungeonType(int index)
    {
        if (index == 0)
        {
            return EntityTypeHelper.GetWeakerMagicType(_playerStartMagicType);
        }

        // Calculate the sequence: if player chose Sun -> Cosmic -> Dark -> Sun -> Cosmic -> Dark
        int cycleLength = 3;
        int baseIndex = (index + 1) % cycleLength; // +1 because first dungeon is same as intro

        return baseIndex switch
        {
            0 => EntityTypeHelper.GetWeakerMagicType(_playerStartMagicType),
            1 => EntityTypeHelper.GetWeakerMagicType(EntityTypeHelper.GetWeakerMagicType(_playerStartMagicType)),
            2 => _playerStartMagicType,
            _ => MagicType.SUN // Should never happen
        };
    }

    public void CompleteStoryDungeon(int index)
    {
        if (index >= 0 && index < _storyDungeonsCompleted.Count)
        {
            _storyDungeonsCompleted[index] = true;
            _currentStoryDungeonIndex = index + 1;

            // Add skill point of the completed dungeon's type
            var skillTree = GetTree().GetFirstNodeInGroup(Globals.PlayerGroup).GetNode<SkillTree>("SkillTree");
            skillTree.AddSkillPointOfType(GetStoryDungeonType(index));
        }
    }

    public bool IsStoryDungeonCompleted(int index)
    {
        return index >= 0 && index < _storyDungeonsCompleted.Count && _storyDungeonsCompleted[index];
    }

    public int GetCurrentStoryDungeonIndex()
    {
        return _currentStoryDungeonIndex;
    }

    public bool AreAllStoryDungeonsCompleted()
    {
        return _storyDungeonsCompleted.TrueForAll(completed => completed);
    }
}
