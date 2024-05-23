using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This component controls the skills of the player.
/// </summary>
public class PlayerSkills : SkillCaster
{
    /*********************************** Methods ***********************************/
    /// <summary>
    /// Prepare these skills.
    /// </summary>
    /// <param name="skillInds">A list of indices into skillsMgr.playerSkills</param>
    public void prepareSkillsFromIndices(HashSet<int> skillInds)
    {
        List<ConcreteSkill> skillsToPrepare = new List<ConcreteSkill>();
        foreach (int i in skillInds)
        {
            skillsToPrepare.Add(Game.gameSingleton.skillsMgr.playerSkills[i]);
        }

        prepareSkills(skillsToPrepare);
    }

    /*********************************** Mono ***********************************/

    // According to the user's input
    // 1. select and deselect skills.
    // 2. release the selected skill.
    private void Update()
    {
        
    }
}