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
*/
// might have to redo this in GDScript if functionality is missing

[TestSuite]
public partial class TestPlayer
{
    private ISceneRunner _player;

    [BeforeTest]
    public void SetupTest()
    {
        _player = ISceneRunner.Load("res://modules/entities/player/player.tscn");
    }

    /**
	Test the transitions between states
	*/
    [TestCase]
    public async Task TestStateMachineStateTransistionsAsync()
    {
        StateMachine stateMachine = _player.GetProperty("StateMachine");

        AssertBool(stateMachine is not null).IsTrue();  // assert that stateMachine is not null

        AssertObject(stateMachine.GetState()).IsInstanceOf<PlayerIdle>();  // check if starting state is Idle


        // Use SimulateKeyPress and SimulateKeyRelease in order to do manually decide when key is released.
        // IMPORTANT: Technically we should be using _player.AwaitIdleFrame() and not SimulateFrames.
        // But for some magic reason, if you use AwaitIdleFrame() the test can sometimes fail.
        // But if we use SimulateFrames, with 100ms as delta. Then it always seems to work.
        // I believe this is because SimulateFrames(1, 100) does not actually siimulate 1 frame but more like 10. (Yeah it is basically bugged)
        // and I think something weird with async happens if not enough time passes. Not sure why or whatever, but now it seems to work, so don't change!!!!!


        // Check transitions from Idle to all other states /////////////////////////////////////////////////////////////////////////////////////////////////
        _player.SimulateKeyPress(Key.A);
        await _player.SimulateFrames(1, 100);
        AssertObject(stateMachine.GetState()).IsInstanceOf<PlayerMoving>();
        // stop movement and check that we are back in Idle state
        _player.SimulateKeyRelease(Key.A);
        await _player.SimulateFrames(1, 100);
        AssertObject(stateMachine.GetState()).IsInstanceOf<PlayerIdle>();

        // transition to dash state and back
        _player.SimulateKeyPress(Key.Space);
        await _player.SimulateFrames(1, 100);
        AssertObject(stateMachine.GetState()).IsInstanceOf<PlayerDashing>();
        _player.SimulateKeyRelease(Key.Space);
        await _player.SimulateFrames(10, 200);  // TODO if dash is longer then 2 seconds, need to change numbers here
        AssertObject(stateMachine.GetState()).IsInstanceOf<PlayerIdle>();

        // transition to spellcast state and back
        _player.SimulateKeyPress(Key.Key1);
        await _player.SimulateFrames(1, 100);
        AssertObject(stateMachine.GetState()).IsInstanceOf<PlayerSpellCasting>();
        _player.SimulateKeyRelease(Key.Key1);
        await _player.SimulateFrames(10, 200); // TODO if attack is longer then 2 seconds, need to change numbers here
        AssertObject(stateMachine.GetState()).IsInstanceOf<PlayerIdle>();


        // Check transitions from Moving to all other states /////////////////////////////////////////////////////////////////////////////////////////////////
        // first switch to MovementState by manually 
        _player.SimulateKeyPress(Key.A);
        await _player.SimulateFrames(1, 100);
        AssertObject(stateMachine.GetState()).IsInstanceOf<PlayerMoving>();

        // transition to dash state and back
        _player.SimulateKeyPress(Key.Space);
        await _player.SimulateFrames(1, 100);
        AssertObject(stateMachine.GetState()).IsInstanceOf<PlayerDashing>();
        _player.SimulateKeyRelease(Key.Space);
        await _player.SimulateFrames(10, 200);  // TODO if dash is longer then 2 seconds, need to change numbers here
        AssertObject(stateMachine.GetState()).IsInstanceOf<PlayerMoving>();

        // transition to spellcast state and back
        _player.SimulateKeyPress(Key.Key1);
        await _player.SimulateFrames(1, 100);
        AssertObject(stateMachine.GetState()).IsInstanceOf<PlayerSpellCasting>();
        _player.SimulateKeyRelease(Key.Key1);
        await _player.SimulateFrames(10, 200); // TODO if attack is longer then 2 seconds, need to change numbers here
        AssertObject(stateMachine.GetState()).IsInstanceOf<PlayerMoving>();

        // release A key, go to Idle again
        _player.SimulateKeyRelease(Key.A);
        await _player.SimulateFrames(1, 100);
        AssertObject(stateMachine.GetState()).IsInstanceOf<PlayerIdle>();
    }
}
