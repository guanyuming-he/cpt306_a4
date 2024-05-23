using UnityEngine;

/// <summary>
/// Inherits from ScriptableObject so that it can be instantiated as an asset
/// and maintain immutable data.
/// 
/// The class SkillData provides all the data settable for a ConcreteSkill.
/// To provide extra data for more specific skills, extend this class.
/// </summary>
public abstract class SkillData : ScriptableObject
{
    /*********************************** Fields ***********************************/
    [Header("Gameplay")]
    public float cooldown = 1.0f;

    [Header("Skill Properties")]
    public uint cost = 1;
    public ConcreteSkill.Level level;

    // An image of the skill.
    public Sprite skillIconSprite;
    public string skillName;
    public string description;

    /// <summary>
    /// Check the data assigned.
    /// </summary>
    protected virtual void Awake()
    {
        Utility.MyDebugAssert(cooldown >= 0.0f, "cooldown cannot be negative.");
        // the icon can be empty for boss.
    }

}