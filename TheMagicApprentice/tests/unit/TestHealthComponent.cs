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

    [TestCase]
    public void TestReadyMethod()
    {
        _target.SetMaxHP(150);
        
        // Call _Ready manually since we're not in the Godot scene tree
        _target._Ready();

        AssertThat(_target.GetCurrentHP()).IsEqual(150);
    }

    [TestCase]
    public void TestSetMaxHP()
    {
        _target.SetMaxHP(200);
        AssertThat(_target.GetCurrentHP()).IsEqual(200);

        // Test that setting MaxHP resets CurrentHP
        _target.TakeDamage(new Attack(50, MagicType.SUN, _attackingNode));
        AssertThat(_target.GetCurrentHP()).IsEqual(150);

        _target.SetMaxHP(300);
        AssertThat(_target.GetCurrentHP()).IsEqual(300);
    }

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

        
        AssertThat(_target.GetCurrentHP()).IsEqual(resultHP);
        AssertThat(_attackingNode.GetCurrentHP()).IsEqual(attackerHP);

        AssertBool(died).IsEqual(resultHP<=0);
    }

    [TestCase]
    public void TestDamageReflection()
    {
        // Arrange
        double initialAttackerHP = 100;
        double initialTargetHP = 100;
        double attackDamage = 50;
        double armorValue = 150; // More than 100 to trigger reflection

        HealthComponent attacker = AutoFree(new HealthComponent());
        attacker.SetMaxHP(initialAttackerHP);
        attacker._Ready();

        _target.SetMaxHP(initialTargetHP);
        _target.SetArmor(armorValue, 0, 0);
        _target._Ready();

        Attack attack = new Attack(attackDamage, MagicType.SUN, attacker);

        // Act
        _target.TakeDamage(attack);

        // Assert
        double expectedReflectedDamage = attackDamage * (armorValue / 100.0 - 1.0);
        AssertThat(attacker.GetCurrentHP()).IsEqual(initialAttackerHP - expectedReflectedDamage);
        AssertThat(_target.GetCurrentHP()).IsEqual(initialTargetHP);
    }
}