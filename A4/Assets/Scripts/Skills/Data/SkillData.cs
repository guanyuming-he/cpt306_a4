using UnityEngine;

/// <summary>
/// Inherits from ScriptableObject so that it can be instantiated as an asset
/// and maintain immutable data.
/// 
/// The class SkillData provides all the data settable for a ConcreteSkill.
/// To provide extra data for more specific skills, extend this class.
/// </summary>
public class SkillData : ScriptableObject
{
    /*********************************** Fields ***********************************/
    public float cooldown;
}