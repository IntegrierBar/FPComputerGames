namespace Tests;

using GdUnit4;
using Godot;
using System;
using static GdUnit4.Assertions;

/**
Unit tests for all Spell classes
*/
[TestSuite]
public class TestInventorySpells
{
    [TestCase(10)]
    [TestCase(1839.102)]
    [TestCase(0)]
    public void TestResetDamage(double baseDamage)
    {
        var inventorySpell = AutoFree(new InventorySpell());
        inventorySpell.BaseDamage = baseDamage;
        inventorySpell.ResetDamage();
        AssertFloat(inventorySpell.Damage).IsEqual(baseDamage);
    }
}
