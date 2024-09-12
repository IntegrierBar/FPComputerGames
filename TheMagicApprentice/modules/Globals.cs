

/**
This static class contains all Global constants.
In particular it conatins all string names for the groups
*/
public static class Globals
{
    public const string PlayerGroup = "player"; ///< string name of the group for player
    public const string RoomHandlerGroup = "room_handler"; ///< string name of the group of the room handler

    // group for all InventorySpells
    public const string InventorySpellGroup = "inventory_spell";  ///< string name of the group that contains all inventory spells

    // the groups for the different spell slots
    public const string Spell1 = "spell1";  ///< string name of the group that contains the spells for slot one
    public const string Spell2 = "spell2";  ///< string name of the group that contains the spells for slot tow
    public const string Spell3 = "spell3";  ///< string name of the group that contains the spells for slot three


    // The spell groups for the different magic types
    public const string SunSpellGroup = "sun_spells";   ///< string name of the group for all sun spells
    public const string CosmicSpellGroup = "cosmic_spells";   ///< string name of the group for all cosmic spells
    public const string DarkSpellGroup = "dark_spells";   ///< string name of the group for all dark spells

    // The spell groups to acces the individual InventorySpell
    public const string SunBasicSpellGroup = "sun_basic_spell"; ///< string name of the group containing the Sun Basic Spell
    public const string CosmicBasicSpellGroup = "cosmic_basic_spell"; ///< string name of the group containing the Cosmic Basic Spell
    public const string DarkBasicSpellGroup = "dark_basic_spell"; ///< string name of the group containing the Dark Basic Spell
    public const string SunBeamSpellGroup = "sun_beam_spell"; ///< string name of the group containing the Sun Beam Spell
    public const string SummonSunSpellGroup = "summon_sun_spell"; ///< string name of the group containing the Summon Sun Spell
    public const string MoonLightSpellGroup = "moon_light_spell"; ///< string name of the group containing the Moon Light Spell
    public const string StarRainSpellGroup = "star_rain_spell"; ///< string name of the group containing the Star Rain Spell
    public const string DarkEnergyWaveSpellGroup = "dark_energy_wave_spell"; ///< string name of the group containing the Dark Energy Wave Spell
    public const string BlackHoleSpellGroup = "black_hole_spell"; ///< string name of the group containing the Black Hole Spell


    // Groups for the spell slot of the UI
    public const string SpellSlot1 = "spell_slot1";
    public const string SpellSlot2 = "spell_slot2";
    public const string SpellSlot3 = "spell_slot3";


    /**
    Converts the enum SpellName to the string group names
    */
    public static string GetGroupNameOfSpell(SpellName spell) => spell switch
    {
        SpellName.SunBasic => SunBasicSpellGroup,
        SpellName.CosmicBasic => CosmicBasicSpellGroup,
        SpellName.DarkBasic => DarkBasicSpellGroup,
        SpellName.SunBeam => SunBeamSpellGroup,
        SpellName.SummonSun => SummonSunSpellGroup,
        SpellName.MoonLight => MoonLightSpellGroup,
        SpellName.StarRain => StarRainSpellGroup,
        SpellName.DarkEnergyWave => DarkEnergyWaveSpellGroup,
        SpellName.BlackHole => BlackHoleSpellGroup,
        _ => "", // Need to have a default value to make C# happy
    };


    /**
    Returns the group name of the spells of the MagicType
    */
    public static string GetGroupNameOfSpellsOfType(MagicType magicType) => magicType switch
    {
        MagicType.SUN => SunSpellGroup,
        MagicType.COSMIC => CosmicSpellGroup,
        MagicType.DARK => DarkSpellGroup,
        _ => "", // Need to have a default value to make C# happy
    };

    /**
    Returns the group name of the spellslot
    (Currently the uint slot starts at 1 put the spellslots start at 1 so its a bit awkward)
    */
    public static string GetGroupNameOfSpellsInSlot(uint slot) => slot switch
    {
        0 => Spell1,
        1 => Spell2,
        2 => Spell3,
        _ => "", // Need to have a default value to make C# happy
    };

}