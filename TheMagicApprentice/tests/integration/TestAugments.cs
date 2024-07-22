namespace Tests;

using GdUnit4;
using Godot;
using System;
using static GdUnit4.Assertions;
using GdUnit4.Executions;
using GdUnit4.Executions.Monitors;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

/**
Integration test for augments
Contains an integration test for every augment class but not all augments since that would be too much effort
*/
[TestSuite]
public partial class TestAugments
{
    private ISceneRunner _mainGameScene; ///< the player scene so that we can manipulate it with augments
    private Player _player; ///< reference to the player node inside the player scene


    private readonly string _folderPath = "res://modules/augments/augment_effects/resources/"; ///< hardcoded path to access the augmenteffect resources
    private static Random _random = new(); ///< random number generator

    private readonly double eps = 1e-8; ///< used for comparing doubles as arror margin

    /**
    Setup the scene and get the reference to the player
    */
    [BeforeTest]
    public void SetupTest()
    {
        _mainGameScene = ISceneRunner.Load("res://main_game.tscn");

        _player = _mainGameScene.FindChild("Player") as Player;

        AssertObject(_player).IsNotNull();
    }


    /**
    Test equiping augments in random slots
    */
    [TestCase]
    public void TestAugmentEquiping()
    {
        // iterate over all augmenteffect.
        // put randonly 1-3 effect into one augment and equip the augment in a random slot
        using var directory = DirAccess.Open(_folderPath);
        if (directory != null)
        {
            int augmentEffectIndex = _random.Next(3); // generate a random number for the index of the next effect inside the augment. Everytime this number gets below 0 we calculate a new number
            Augment augment = new Augment();
            directory.ListDirBegin();
            string fileName = directory.GetNext();
            while (fileName != "")
            {
                if (!directory.CurrentIsDir()) // make sure we skip directories (even though there should not be any)
                {
                    AugmentEffect augmentEffect = GD.Load<AugmentEffect>(_folderPath + fileName);
                    augment._augmentEffects[augmentEffectIndex] = augmentEffect;
                    AssertObject(augment._augmentEffects[augmentEffectIndex]).IsNotNull();
                    augmentEffectIndex -= 1;
                }
                // if augmentEffectIndex is below 0 then the augment is filled and we can move on to the next augment
                if (augmentEffectIndex < 0)
                {
                    _player.EquipAugmentInSlot(augment, _random.Next(5));
                    augmentEffectIndex = _random.Next(3);
                }
                fileName = directory.GetNext();
            }
        }
        else // in case it fails we print an error message.
        {
            GD.Print("Could not open augment effect directory. Maybe wrong name?");
            AssertBool(true).IsEqual(false);    // show that test failed
        }
    }

    /********************************************************************************************************************************************/
    /*                     ALL OTHER TEST ARE INDIVIDUAL AUGMENT EFFECT TESTS AND ONLY USE SLOT 0 TO MAKE THINGS EASIER                         */
    /********************************************************************************************************************************************/

    /**
    Test the Augment additional_stars
    */
    [TestCase]
    public void TestAdditionalStars()
    {
        InventoryStarRain inventoryStarRain = _player.GetTree().GetFirstNodeInGroup(Globals.StarRainSpellGroup) as InventoryStarRain;

        double startAmountStars = inventoryStarRain.AmountStarsToSpawn;

        EquipEffect("additional_stars.tres");

        AssertFloat(inventoryStarRain.AmountStarsToSpawn).IsGreater(startAmountStars); // Check if it increased

        _player.UnEquipAugmentFromSlot(0);

        AssertFloat(inventoryStarRain.AmountStarsToSpawn).IsEqualApprox(startAmountStars, eps); // Check if it is back to normal amount
    }


    /**
    Test that AugmentEffect cast_star_rain_on_moonlight works
    */
    [TestCase]
    public void TestCastAdditionalSpell()
    {
        // first clear spell1 group
        foreach (Node node in _player.GetTree().GetNodesInGroup(Globals.Spell1))
        {
            node.RemoveFromGroup(Globals.Spell1);
        }

        // first make sure that moonlight is in spell1 group and star rain is not
        InventorySpell moonLight = _player.GetTree().GetFirstNodeInGroup(Globals.MoonLightSpellGroup) as InventorySpell;
        InventorySpell starRain = _player.GetTree().GetFirstNodeInGroup(Globals.StarRainSpellGroup) as InventorySpell;
        moonLight.AddToGroup(Globals.Spell1);
        starRain.RemoveFromGroup(Globals.Spell1);
        AssertBool(moonLight.IsInGroup(Globals.Spell1)).IsTrue();
        AssertBool(starRain.IsInGroup(Globals.Spell1)).IsFalse();

        // Then equip the augment
        EquipEffect("cast_star_rain_on_moonlight.tres");

        // Check if StarRain is in spell1 group
        AssertBool(moonLight.IsInGroup(Globals.Spell1)).IsTrue();
        AssertBool(starRain.IsInGroup(Globals.Spell1)).IsTrue();

        // Check that unequipping works
        _player.UnEquipAugmentFromSlot(0);
        AssertBool(moonLight.IsInGroup(Globals.Spell1)).IsTrue();
        AssertBool(starRain.IsInGroup(Globals.Spell1)).IsFalse();
    }


    /**
    Check that the extra_armor_sun effect and extra_armor_of_all_types work
    */
    [TestCase]
    public void TestExtraArmor()
    {
        // check extra armor sun
        HealthComponent healthComponent = _player.GetNode("HealthComponent") as HealthComponent;
        AssertObject(healthComponent).IsNotNull();

        double startArmor = healthComponent.GetArmorOfType(MagicType.SUN);

        EquipEffect("extra_armor_sun.tres");

        AssertFloat(healthComponent.GetArmorOfType(MagicType.SUN)).IsGreater(startArmor); // check that the augment increased the armor

        _player.UnEquipAugmentFromSlot(0);

        AssertFloat(healthComponent.GetArmorOfType(MagicType.SUN)).IsEqualApprox(startArmor, eps); // check that unequipping returns the armor back down

        // check extra armor all types
        EquipEffect("extra_armor_all_types.tres");

        AssertFloat(healthComponent.GetArmorOfType(MagicType.SUN)).IsGreater(startArmor); // check that the augment increased the armor

        _player.UnEquipAugmentFromSlot(0);

        AssertFloat(healthComponent.GetArmorOfType(MagicType.SUN)).IsEqualApprox(startArmor, eps); // check that unequipping returns the armor back down
    }


    /**
    Check the flat_damage_dark effect and the percent_damage_dark effect
    */
    [TestCase("flat_damage_dark.tres", TestName = "flat damage")]
    [TestCase("percent_damage_dark.tres", TestName = "percent damage")]
    public void TestDamageIncreaseDark(string effectName)
    {
        // first get the damage from all dark spells
        Dictionary<SpellName, double> damageOfDarkSpells = new()
        {
            {SpellName.DarkBasic, 0},
            {SpellName.DarkEnergyWave, 0},
            {SpellName.BlackHole, 0}
        };
        foreach (var item in damageOfDarkSpells)
        {
            damageOfDarkSpells[item.Key] = (_player.GetTree().GetFirstNodeInGroup(Globals.GetGroupNameOfSpell(item.Key)) as InventorySpell).Damage;
        }
        AssertFloat(damageOfDarkSpells[SpellName.DarkBasic]).IsNotZero(); // check that the values are not zero

        EquipEffect(effectName);

        // Check that we increased the damage
        foreach (var item in damageOfDarkSpells)
        {
            AssertFloat((_player.GetTree().GetFirstNodeInGroup(Globals.GetGroupNameOfSpell(item.Key)) as InventorySpell).Damage).IsGreater(damageOfDarkSpells[item.Key]);
        }

        _player.UnEquipAugmentFromSlot(0);

        // check that after unequipping we get orignial values back
        foreach (var item in damageOfDarkSpells)
        {
            AssertFloat((_player.GetTree().GetFirstNodeInGroup(Globals.GetGroupNameOfSpell(item.Key)) as InventorySpell).Damage).IsEqualApprox(damageOfDarkSpells[item.Key], eps);
        }
    }


    /**
    Check flat_damage_slot1 effect
    */
    [TestCase]
    public void TestFlatDamageSlot1()
    {
        // first make sure only spell SunBasic is in Slot1
        foreach (Node node in _player.GetTree().GetNodesInGroup(Globals.Spell1))
        {
            node.RemoveFromGroup(Globals.Spell1);
        }
        InventorySpell sunBasic = _player.GetTree().GetFirstNodeInGroup(Globals.SunBasicSpellGroup) as InventorySpell;
        sunBasic.AddToGroup(Globals.Spell1);

        double startDamage = sunBasic.Damage;

        EquipEffect("flat_damage_slot1.tres");

        AssertFloat(sunBasic.Damage).IsGreater(startDamage); // check that increase worked

        _player.UnEquipAugmentFromSlot(0);

        AssertFloat(sunBasic.Damage).IsEqualApprox(startDamage, eps); // check that unequipping worked
    }


    /**
    Test HP_increase effect
    */
    [TestCase]
    public void TestHPIncrease()
    {
        // check get current MaxHP
        HealthComponent healthComponent = _player.GetNode("HealthComponent") as HealthComponent;
        AssertObject(healthComponent).IsNotNull();

        double startMaxHP = healthComponent.GetMaxHP();
        AssertFloat(healthComponent.GetCurrentHP()).IsEqualApprox(startMaxHP, eps); // check that at start currentHP and MaxHP are the Same

        EquipEffect("HP_increase.tres");
    
        AssertFloat(healthComponent.GetMaxHP()).IsGreater(startMaxHP); // check that the augment increased the MaxHP
        AssertFloat(healthComponent.GetCurrentHP()).IsGreater(startMaxHP); // check that the augment increased the CurrentHP

        _player.UnEquipAugmentFromSlot(0);

        AssertFloat(healthComponent.GetMaxHP()).IsEqualApprox(startMaxHP, eps); // check that MaxHP is back to old value
        AssertFloat(healthComponent.GetCurrentHP()).IsEqualApprox(startMaxHP, eps); // check that CurrentHP is back to old value
    }


    /**
    Test increase_radius_black_hole effect
    */
    [TestCase]
    public void TestIncreaseRadiusBlackHole()
    {
        InventorySpell blackHole = _player.GetTree().GetFirstNodeInGroup(Globals.BlackHoleSpellGroup) as InventorySpell;

        AssertInt(blackHole.GetOnCastAugmentEffects().Count).IsZero(); // check that there are now OnCastAugmentEffects

        // Get the size of the normal Cast BlackHole
        blackHole.Cast(Vector2.Zero, Vector2.Down); // Cast the spell so that it is instanciated

        Spell blackHoleSpell = _player.GetTree().Root.GetNode("BlackHole") as Spell; // right now spells get added as children of Root. This might change later!
        AssertObject(blackHoleSpell).IsNotNull();

        Vector2 startScale = blackHoleSpell.Scale;

        // free the blackHole 
        blackHoleSpell.Free();


        EquipEffect("increase_radius_black_hole.tres");

        AssertInt(blackHole.GetOnCastAugmentEffects().Count).IsEqual(1);

        // check if it is the correct augment effect
        IncreasedRadiusOfSpell effect = blackHole.GetOnCastAugmentEffects()[0] as IncreasedRadiusOfSpell;
        AssertObject(effect).IsNotNull();

        // Cast the spell and check that the scale increased
        blackHole.Cast(Vector2.Zero, Vector2.Down); // Cast the spell so that it is instanciated

        Spell blackHoleSpell2 = _player.GetTree().Root.GetNode("BlackHole") as Spell; // right now spells get added as children of Root. This might change later!
        AssertObject(blackHoleSpell2).IsNotNull();

        Vector2 newScale = blackHoleSpell2.Scale;

        // check that Scale has increased
        AssertFloat(newScale.X).IsGreater(startScale.X);
        AssertFloat(newScale.Y).IsGreater(startScale.Y);

        // finally unequip and check that the list of OnCastAugmentEffects is empty
        _player.UnEquipAugmentFromSlot(0);
        AssertInt(blackHole.GetOnCastAugmentEffects().Count).IsZero();
    }


    /**
    Test increase_duration_black_hole effect
    */
    [TestCase]
    public void TestIncreaseDurationBlackHole()
    {
        InventorySpell blackHole = _player.GetTree().GetFirstNodeInGroup(Globals.BlackHoleSpellGroup) as InventorySpell;

        AssertInt(blackHole.GetOnCastAugmentEffects().Count).IsZero(); // check that there are now OnCastAugmentEffects

        // Get the size of the normal Cast BlackHole
        blackHole.Cast(Vector2.Zero, Vector2.Down); // Cast the spell so that it is instanciated

        Spell blackHoleSpell = _player.GetTree().Root.GetNode("BlackHole") as Spell; // right now spells get added as children of Root. This might change later!
        AssertObject(blackHoleSpell).IsNotNull();

        double startTimeLeft = blackHoleSpell._timeLeftUntilDeletion;

        // free the blackHole 
        blackHoleSpell.Free();

        EquipEffect("increase_duration_black_hole.tres");

        AssertInt(blackHole.GetOnCastAugmentEffects().Count).IsEqual(1);

        // check if it is the correct augment effect
        IncreaseDurationOfSpell effect = blackHole.GetOnCastAugmentEffects()[0] as IncreaseDurationOfSpell;
        AssertObject(effect).IsNotNull();

        // Cast the spell and check that the scale increased
        blackHole.Cast(Vector2.Zero, Vector2.Down); // Cast the spell so that it is instanciated

        Spell blackHoleSpell2 = _player.GetTree().Root.GetNode("BlackHole") as Spell; // right now spells get added as children of Root. This might change later!
        AssertObject(blackHoleSpell2).IsNotNull();

        double newTimeLeft = blackHoleSpell2._timeLeftUntilDeletion;

        // check that Scale has increased
        AssertFloat(newTimeLeft).IsGreater(startTimeLeft);

        // finally unequip and check that the list of OnCastAugmentEffects is empty
        _player.UnEquipAugmentFromSlot(0);
        AssertInt(blackHole.GetOnCastAugmentEffects().Count).IsZero();
    }


    /**
    Test percent_damage_sun_basic
    */
    [TestCase]
    public void TestPercentDamageSunBasic()
    {
        InventorySpell sunBasic = _player.GetTree().GetFirstNodeInGroup(Globals.SunBasicSpellGroup) as InventorySpell;

        double startDamage = sunBasic.Damage;

        EquipEffect("percent_damage_sun_basic.tres");

        AssertFloat(sunBasic.Damage).IsGreater(startDamage); // check that increase worked

        _player.UnEquipAugmentFromSlot(0);

        AssertFloat(sunBasic.Damage).IsEqualApprox(startDamage, eps); // check that unequipping worked
    }


    /**
    Create augment from string path to augmenteffect resource 
    */
    private Augment CreateAugmentWithAugmenteffect(string pathToAugmentEffectResource)
    {
        Augment augment = new();

        AugmentEffect augmentEffect = GD.Load<AugmentEffect>(pathToAugmentEffectResource);
        augment._augmentEffects[0] = augmentEffect;

        AssertObject(augmentEffect).IsNotNull(); // check that loading worked

        return augment;
    }


    /**
    Equips the resouce effect to the player

    @param effectName is the name of the resource inside the folder
    */
    private void EquipEffect(string effectName)
    {
        string path = _folderPath + effectName;
        _player.EquipAugmentInSlot(CreateAugmentWithAugmenteffect(path), 0);
    }
}