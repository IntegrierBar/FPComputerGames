using Godot;
using System.Collections.Generic;

public partial class CurseHandler : Node
{
    private static CurseHandler _instance;
    
    /*!
    \brief Boolean properties representing the state of each curse.
    
    We use bools even though we have a Curse enum in @enums.cs because this lets us set them active or disabled in the editor very easily.
    */
    [Export]
    public bool Skill3Disabled { get; set; } = false;
    [Export]
    public bool Skill1Only { get; set; } = false;
    [Export]
    public bool MoreVulnerable { get; set; } = false;
    [Export]
    public bool MonsterBuff { get; set; } = false;
    [Export]
    public bool MoreMonsters { get; set; } = false;
    [Export]
    public bool TwoBosses { get; set; } = false;

    // Add the curse descriptions dictionary
    private static readonly Dictionary<Curse, string> CurseDescriptions = new Dictionary<Curse, string>
    {
        { Curse.SKILL_3_DISABLED, "Third skill slot disabled" },
        { Curse.SKILL_1_ONLY, "Only first skill available" },
        { Curse.MORE_VULNERABLE, "Take more damage" },
        { Curse.MONSTER_BUFF, "Stronger monsters" },
        { Curse.MORE_MONSTERS, "More monsters" },
        { Curse.TWO_BOSSES, "Two bosses" }
    };

    /**
    \brief Called when the node enters the scene tree for the first time
    */
    public override void _EnterTree()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            GD.PushWarning("Multiple CurseHandler instances detected. Only one should exist.");
        }
    }
    
    /**
    \brief Called when the node is about to be removed from the scene tree
    */
    public override void _ExitTree()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }

    /**
    \brief Checks if a specific curse is active

    \param curse The curse to check
    \return True if the curse is active, false otherwise
    */
    public static bool IsActive(Curse curse)
    {
        if (_instance == null)
        {
            GD.PushError("CurseHandler instance not found. Ensure it's added to the scene.");
            return false;
        }

        return curse switch
        {
            Curse.SKILL_3_DISABLED => _instance.Skill3Disabled,
            Curse.SKILL_1_ONLY => _instance.Skill1Only,
            Curse.MORE_VULNERABLE => _instance.MoreVulnerable,
            Curse.MONSTER_BUFF => _instance.MonsterBuff,
            Curse.MORE_MONSTERS => _instance.MoreMonsters,
            Curse.TWO_BOSSES => _instance.TwoBosses,
            _ => false
        };
    }

    /**
    \brief Activates a specific curse

    \param curse The curse to activate
    */
    public static void ActivateCurse(Curse curse)
    {
        if (_instance == null)
        {
            GD.PushError("CurseHandler instance not found. Ensure it's added to the scene.");
            return;
        }

        switch (curse)
        {
            case Curse.SKILL_3_DISABLED: _instance.Skill3Disabled = true; break;
            case Curse.SKILL_1_ONLY: _instance.Skill1Only = true; break;
            case Curse.MORE_VULNERABLE: _instance.MoreVulnerable = true; break;
            case Curse.MONSTER_BUFF: _instance.MonsterBuff = true; break;
            case Curse.MORE_MONSTERS: _instance.MoreMonsters = true; break;
            case Curse.TWO_BOSSES: _instance.TwoBosses = true; break;
        }
    }

    /**
    \brief Deactivates a specific curse

    \param curse The curse to deactivate
    */
    public static void DeactivateCurse(Curse curse)
    {
        if (_instance == null)
        {
            GD.PushError("CurseHandler instance not found. Ensure it's added to the scene.");
            return;
        }

        switch (curse)
        {
            case Curse.SKILL_3_DISABLED: _instance.Skill3Disabled = false; break;
            case Curse.SKILL_1_ONLY: _instance.Skill1Only = false; break;
            case Curse.MORE_VULNERABLE: _instance.MoreVulnerable = false; break;
            case Curse.MONSTER_BUFF: _instance.MonsterBuff = false; break;
            case Curse.MORE_MONSTERS: _instance.MoreMonsters = false; break;
            case Curse.TWO_BOSSES: _instance.TwoBosses = false; break;
        }
    }

    /**
    \brief Deactivates all curses
    */
    public static void ClearAllCurses()
    {
        if (_instance == null)
        {
            GD.PushError("CurseHandler instance not found. Ensure it's added to the scene.");
            return;
        }

        _instance.Skill3Disabled = false;
        _instance.Skill1Only = false;
        _instance.MoreVulnerable = false;
        _instance.MonsterBuff = false;
        _instance.MoreMonsters = false;
        _instance.TwoBosses = false;
    }

    // Add a method to get the description
    public static string GetCurseDescription(Curse curse)
    {
        return CurseDescriptions.TryGetValue(curse, out string description) 
            ? description 
            : "Unknown curse";
    }
}