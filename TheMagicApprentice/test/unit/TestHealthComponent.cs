namespace Tests;

using System.Collections.Generic;
using System.Threading.Tasks;
using GdUnit4;
using Godot;
using static GdUnit4.Assertions;


/**
Unit tests for the HealthComponent
Tests the TakeDamage function
*/
[TestSuite]
public class TestHealthComponent
{    
    private HealthComponent _target;
    private HealthComponent _attackingNode;

    private bool died = false;

    // This gets called before each test
    [BeforeTest]
    public void SetupTest()
    {
        // use AutoFree so that data gets automatically freed
        _target = AutoFree(new HealthComponent());
        _attackingNode = AutoFree(new HealthComponent());
        _attackingNode.SetMaxHP(100.0);
        _attackingNode.SetArmor(50.0, 50.0, 50.0);
        _target.Death += () => died = true; // Add custom method to check if Death Signal got emitted
    }

/*
    [TestCase]
    public void StringToLower() 
    {
        AssertString("AbcD".ToLower()).IsEqual("abcd");
    }

    [TestCase]
    public void FailTest() 
    {
        AssertThat(false).IsTrue();
    }*/

    [TestCase(100.0, 0.0, 0.0, 0.0, 10.0, MagicType.SUN, 90.0, 100.0, TestName = "No Armor")]
    [TestCase(123.0, 50.0, 20.0, 0.0, 30.0, MagicType.SUN, 123.0 - 15.0, 100.0, TestName = "Armor Sun")]
    [TestCase(50.0, 40.0, 20.0, 10.0, 10.0, MagicType.COSMIC, 50.0 - 8.0, 100.0, TestName = "Armor Cosmic")]
    [TestCase(10.0, 120.0, 30.0, 90.0, 5.0, MagicType.DARK, 10.0 - 0.5, 100.0, TestName = "Armor Dark")]
    [TestCase(10.0, 120.0, 30.0, 90.0, 100.0, MagicType.SUN, 10.0, 100.0 - 10.0, TestName = "Test Reflection")]
    [TestCase(10.0, 0.0, 30.0, 90.0, 10.0, MagicType.SUN, 10.0-10.0, 100.0, TestName = "Death Emitted when Health=0")]
    [TestCase(10.0, 0.0, 30.0, 90.0, 20.0, MagicType.SUN, 10.0-20.0, 100.0, TestName = "Death Emitted when Health<0")]
    public void TestTakeDamageFunctionAsync(double maxHP, double armorSun, double armorCosmic, double armorDark, double damage, MagicType magicType, double resultHP, double attackerHP)
    {
        Attack attack = new Attack(damage, magicType, _attackingNode);
        _target.SetMaxHP(maxHP);
        _target.SetArmor(armorSun, armorCosmic, armorDark);

        _target.TakeDamage(attack);

        
        // IMPORTANT!!!!!!!!!!!!!!! DO NOT USE .Equals!!!!!! instead us .IsEqual!!!!!!!!!!!!!
        AssertThat(_target.GetCurrentHP()).IsEqual(resultHP);
        AssertThat(_attackingNode.GetCurrentHP()).IsEqual(attackerHP);

        AssertBool(died).IsEqual(resultHP<=0);
    }
}