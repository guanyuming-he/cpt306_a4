using UnityEngine;

/// <summary>
/// A level 2 skill that heals the caster for a duration.
/// </summary>
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/HealingSkill2Data", order = 1)]
public class HealingSkill2Data : SkillData
{
    [Header("HealingSkill Gameplay")]
    // how many healingPeriods does the skill have?
    public uint duration;
    // how much is healed in one healingPeriod?
    public float healingAmount;
    // how much time must elapse before the next healing is done?
    public float healingPeriod;

    public HealingSkill2Data()
    {
        level = ConcreteSkill.Level.LEVEL_2;
        skillName = "Healing Skill (Lv.2)";

        // some default values
        duration = 3;
        healingAmount = 1.0f;
        healingPeriod = .5f;
    }

    protected override void Awake()
    {
        base.Awake();

        Utility.MyDebugAssert(duration > 0, "duration must be positive.");
        Utility.MyDebugAssert(healingAmount > 0.0f, "healingAmount must be positive.");
        Utility.MyDebugAssert(healingPeriod > 0.0f, "healingPeriod must be positive.");
    }
}
