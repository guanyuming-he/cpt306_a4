﻿using System.Collections.Generic;
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
/// 
/// Note: the data here are immutable once the game is finished.
/// Therefore, they are NOT part of the game's states and are not handled by stateMgr.
/// </summary>
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SkillsManager", order = 1)]
public sealed class SkillsManager : ScriptableObject
{
}