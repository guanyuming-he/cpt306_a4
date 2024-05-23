using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Both the player and the boss have to carry a subset of 
/// a statically available set of skills.
/// 
/// Each skill has a big variety of data: 
/// the skill icon, audiovisual effects, names, cooldown, ...
/// 
/// These things are better stored and managed in a database.
/// This class, SkillsManager, is the database for the job.
/// </summary>
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SkillsManager", order = 1)]
public sealed class SkillsManager : ScriptableObject
{
    public List<ConcreteSkill> playerSkills;
    public List<ConcreteSkill> bossSkills;
}