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
                AssertObject(augment._augmentEffects[i]).IsNotNull();
            }
            else
            {
                AssertObject(augment._augmentEffects[i]).IsNull();
            }
        }
    }
}