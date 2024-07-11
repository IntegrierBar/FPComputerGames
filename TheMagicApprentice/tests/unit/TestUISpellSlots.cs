namespace Tests;

using GdUnit4;
using Godot;
using System;
using static GdUnit4.Assertions;

/**
Unit tests for all Spell classes
*/
[TestSuite]
public class TestUISpellSlots
{

	[TestCase(10, "10.0s", TestName = "integer number")]
	[TestCase(3.21439, "3.2s", TestName = "round down")]
	[TestCase(3.26439, "3.3s", TestName = "round up")]
	[TestCase(-1.201, "0.0s", TestName = "simpel numbers")]
	public void TestConvertTimeToStringInSeconds(double cooldown, string s)
	{
		AssertString(UISpellSlot.ConvertTimeToStringInSeconds(cooldown)).IsEqual(s);
	}
}
