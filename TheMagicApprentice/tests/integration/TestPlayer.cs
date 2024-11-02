namespace Tests;

using GdUnit4;
using Godot;
using System;
using static GdUnit4.Assertions;
using GdUnit4.Executions;
using GdUnit4.Executions.Monitors;
using System.Threading.Tasks;

/**
Integration test for the player scene.
Uses the main_game.tscn since there the player is autoloaded and not disabled.
*/
// might have to redo this in GDScript if functionality is missing

[TestSuite]
public partial class TestPlayer
{
	private ISceneRunner _mainGameScene;
	private Player _player;

	[BeforeTest]
    public void SetupTest()
    {
		// we use 
        _mainGameScene = ISceneRunner.Load("res://main_game.tscn");

        // since player is now an autoload we need some cursed way to access it, since SceneRunner does not support autoloads.
		var roomHandler = _mainGameScene.FindChild("RoomHandler");
		System.Diagnostics.Debug.Assert(roomHandler is not null, "RoomHandler is null");
		_player = roomHandler.GetNode<Player>("/root/Player");
		System.Diagnostics.Debug.Assert(_player is not null, "Player is null");
    }

	/**
	Test the transitions between states
	*/
    [TestCase]
    public async Task TestStateMachineStateTransistionsAsync()
    {
        StateMachine stateMachine = _player.StateMachine;

        AssertBool(stateMachine is not null).IsTrue();  // assert that stateMachine is not null

        AssertObject(stateMachine.GetState()).IsInstanceOf<PlayerIdle>();  // check if starting state is Idle


		// Use SimulateKeyPress and SimulateKeyRelease in order to do manually decide when key is released.
		// IMPORTANT: Technically we should be using _player.AwaitIdleFrame() and not SimulateFrames.
		// But for some magic reason, if you use AwaitIdleFrame() the test can sometimes fail.
		// But if we use SimulateFrames, with 20ms as delta. Then it always seems to work.
		// I believe this is because SimulateFrames(1, 20) does not actually siimulate 1 frame but more like 3. (Yeah it is basically bugged)
		// and I think something weird with async happens if not enough time passes. Not sure why or whatever, but now it seems to work, so don't change!!!!!


		// Check transitions from Idle to all other states /////////////////////////////////////////////////////////////////////////////////////////////////
		_mainGameScene.SimulateKeyPress(Key.A);
		await _mainGameScene.SimulateFrames(1, 20);
		AssertObject(stateMachine.GetState()).IsInstanceOf<PlayerMoving>();
		// stop movement and check that we are back in Idle state
		_mainGameScene.SimulateKeyRelease(Key.A);
		await _mainGameScene.SimulateFrames(1, 20);
		AssertObject(stateMachine.GetState()).IsInstanceOf<PlayerIdle>();

		// transition to dash state and back
		_mainGameScene.SimulateKeyPress(Key.Space);
		await _mainGameScene.SimulateFrames(1, 20);
		AssertObject(stateMachine.GetState()).IsInstanceOf<PlayerDashing>();
		_mainGameScene.SimulateKeyRelease(Key.Space);
		await _mainGameScene.SimulateFrames(5, 200);	// TODO if dash is longer then 1 seconds, need to change numbers here
		AssertObject(stateMachine.GetState()).IsInstanceOf<PlayerIdle>();

		// transition to spellcast state and back
		_mainGameScene.SimulateKeyPress(Key.Key1);
		await _mainGameScene.SimulateFrames(1, 20);
		AssertObject(stateMachine.GetState()).IsInstanceOf<PlayerSpellCasting>();
		_mainGameScene.SimulateKeyRelease(Key.Key1);
		await _mainGameScene.SimulateFrames(5, 200); // TODO if attack is longer then 1 seconds, need to change numbers here
		AssertObject(stateMachine.GetState()).IsInstanceOf<PlayerIdle>();


		// Check transitions from Moving to all other states /////////////////////////////////////////////////////////////////////////////////////////////////
		// first switch to MovementState by manually 
		_mainGameScene.SimulateKeyPress(Key.A);
		await _mainGameScene.SimulateFrames(1, 20);
		AssertObject(stateMachine.GetState()).IsInstanceOf<PlayerMoving>();
		
		// transition to dash state and back
		_mainGameScene.SimulateKeyPress(Key.Space);
		await _mainGameScene.SimulateFrames(1, 20);
		AssertObject(stateMachine.GetState()).IsInstanceOf<PlayerDashing>();
		_mainGameScene.SimulateKeyRelease(Key.Space);
		await _mainGameScene.SimulateFrames(5, 200);	// TODO if dash is longer then 1 seconds, need to change numbers here
		AssertObject(stateMachine.GetState()).IsInstanceOf<PlayerMoving>();

		// transition to spellcast state and back
		_mainGameScene.SimulateKeyPress(Key.Key1);
		await _mainGameScene.SimulateFrames(1, 20);
		AssertObject(stateMachine.GetState()).IsInstanceOf<PlayerSpellCasting>();
		_mainGameScene.SimulateKeyRelease(Key.Key1);
		await _mainGameScene.SimulateFrames(5, 200); // TODO if attack is longer then 1 seconds, need to change numbers here
		AssertObject(stateMachine.GetState()).IsInstanceOf<PlayerMoving>();

		// release A key, go to Idle again
		_mainGameScene.SimulateKeyRelease(Key.A);
		await _mainGameScene.SimulateFrames(1, 20);
		AssertObject(stateMachine.GetState()).IsInstanceOf<PlayerIdle>();
    }

	/**
	Test SetPlayerSkill
	*/
	[TestCase]
    public void TestSetPlayerSkill()
    {
        // equip 3 different skills in the different slots
        _player.SetPlayerSkill(0, SpellName.SunBasic);
        _player.SetPlayerSkill(1, SpellName.CosmicBasic);
        _player.SetPlayerSkill(2, SpellName.DarkBasic);
        // check that equipping worked
        AssertObject(_player.GetPlayerSkill(0)._spellName).Equals(SpellName.SunBasic);
		AssertObject(_player.GetPlayerSkill(1)._spellName).Equals(SpellName.CosmicBasic);
		AssertObject(_player.GetPlayerSkill(2)._spellName).Equals(SpellName.DarkBasic);

		// unequip all spells
		for (int i = 0; i < 3; i++)
		{
			_player.SetPlayerSkill(i, null);
		}
		// check that playerskills are null now
        for (int i = 0; i < 3; i++)
        {
            AssertObject(_player.GetPlayerSkill(i)).IsNull();
        }
    }
}
