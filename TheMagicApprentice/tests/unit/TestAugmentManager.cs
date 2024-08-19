namespace Tests;

using GdUnit4;
using Godot;
using System;
using static GdUnit4.Assertions;
using GdUnit4.Executions;
using GdUnit4.Executions.Monitors;
using System.Threading.Tasks;
using System.Linq;

/**
Unit tests for the AugmentManager Class
*/
[TestSuite]
public partial class TestAugmentManager
{
    private AugmentManager _augmentManager;

    [BeforeTest]
    public void SetupTest()
    {
        _augmentManager = AutoFree(new AugmentManager());
    }

    /**
    Test that LoadAugmentEffects loads 
    */
    [TestCase]
    public void TestLoadAugmentEffects()
    {
        AssertThat(_augmentManager.GetAugmentEffects().Count == 0).IsEqual(true); // check that at start it is empty

        // find out how many different effects there are
        int amountFiles = 0;
        string folderPath = "res://modules/augments/augment_effects/resources/"; // sadly need to hardcode the path.
		using var directory = DirAccess.Open(folderPath); 
		if (directory != null)
		{
			directory.ListDirBegin();
			string fileName = directory.GetNext();
			while (fileName != "")
			{
				if (!directory.CurrentIsDir()) // make sure we skip directories (even though there should not be any)
				{
					amountFiles += 1;
				}
				fileName = directory.GetNext();
			}
		}
		else // in case it fails we print an error message. 
		{
			GD.Print("Could not open augment effect directory. Maybe wrong name?");
            AssertBool(true).IsEqual(false);    // show that test failed
		}

        _augmentManager._Ready(); // call ready which intern calls LoadAugmentEffects
        AssertInt(_augmentManager.GetAugmentEffects().Count).IsEqual(amountFiles); // check that it is now not empty
    }


    /**
    Test automatic generation of Augments
    */
    [TestCase(0, TestName = "too low number")]
    [TestCase(1, TestName = "1 effect")]
    [TestCase(2, TestName = "2 effects")]
    [TestCase(3, TestName = "3 effects")]
    [TestCase(4, TestName = "too large number")]
    public void TestCreateRandomAugment(int amountAugmentEffects)
    {
        _augmentManager._Ready();

        Augment augment = _augmentManager.CreateRandomAugment((uint)amountAugmentEffects);

        for (int i = 0; i < augment._augmentEffects.Count(); i++)
        {
            if (i < amountAugmentEffects)
            {
                AssertObject(augment.GetAugmentEffect(i)).IsNotNull();
            }
            else
            {
                AssertObject(augment.GetAugmentEffect(i)).IsNull();
            }
        }

        if (amountAugmentEffects > 0)
        {
            AssertString(augment.Description).IsNotEmpty();
        }
        else
        {
            AssertString(augment.Description).IsEmpty();
        }
    }


    /**
    Test fusing of augments
    */
    [TestCase]
    public void TestFuseAugments()
    {
        _augmentManager._Ready();

        Augment target = _augmentManager.CreateRandomAugment(2);
        Augment sacrifice = _augmentManager.CreateRandomAugment(1);

        // get description of target and the effect from sacrifice
        string targetDescriptionStart = target.Description;
        string effectToKeepDescription = sacrifice.GetAugmentEffect(0).Description();
        AugmentEffect effectToKeep = sacrifice.GetAugmentEffect(0);

        // first check that using wrong indices does nothing
        _augmentManager.FuseAugments(target, -1, sacrifice, 0);
        AssertString(target.Description).IsEqual(targetDescriptionStart);
        AssertObject(sacrifice).IsNotNull();

        _augmentManager.FuseAugments(target, 0, sacrifice, -2);
        AssertString(target.Description).IsEqual(targetDescriptionStart);
        AssertObject(sacrifice).IsNotNull();

        _augmentManager.FuseAugments(target, 2, sacrifice, 0);
        AssertString(target.Description).IsEqual(targetDescriptionStart);
        AssertObject(sacrifice).IsNotNull();

        _augmentManager.FuseAugments(target, 1, sacrifice, 1);
        AssertString(target.Description).IsEqual(targetDescriptionStart);
        AssertObject(sacrifice).IsNotNull();

        // now fuse the augments
        _augmentManager.FuseAugments(target, 0, sacrifice, 0);
        // check that augment was fused
        AssertObject(target.GetAugmentEffect(0)).IsSame(effectToKeep);
        AssertString(target.Description).IsNotEqual(targetDescriptionStart);
        AssertString(target.Description).Contains(effectToKeepDescription);
        AssertObject(sacrifice.GetAugmentEffect(0)).IsNull();
    }
}