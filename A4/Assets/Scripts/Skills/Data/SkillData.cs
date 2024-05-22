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
    public float cooldown;

    [Header("Skill Properties")]
    public uint cost;
    public ConcreteSkill.Level level;

    // A UI element that displays the skill's icon.
    public GameObject skillIconUI;
    public string description;

}