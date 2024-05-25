using System;
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
    /// <param name="skillInds">A list of indices into playerSkills</param>
    public void prepareSkillsFromIndices(HashSet<int> skillInds)
    {
        List<ConcreteSkill> skillsToPrepare = new List<ConcreteSkill>();
        foreach (int i in skillInds)
        {
            skillsToPrepare.Add(Game.gameSingleton.playerSkills[i]);
        }

        prepareSkills(skillsToPrepare);
    }

    /*********************************** Mono ***********************************/

    // According to the user's input
    // 1. select and deselect skills.
    // 2. release the selected skill.
    private void Update()
    {
        // for each i, see if KeyCode.One + i is pressed.
        // if so, select that skill and break.
        // Breaking the loop is to prevent the situation when multiple keys are pressed.
        for(int i = 0; i < preparedSkills.Count; ++i)
        {
            if(Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                Debug.Log(string.Format("The player selects skill {0}", i+1));
                selectSkill(i);
                break;
            }
        }

        // see if the attack key is pressed
        // 0 is the LMB
        if(Input.GetMouseButtonDown(0))
        {
            if(isAnySkillSelected())
            {
                // get the mouse's position on the floor
                Vector3 position;
                try
                {
                    position = PlayerMovement.mouseClickToFloorPosition();
                }
                catch(ArgumentException)
                {
                    // the click is not on the floor.
                    return;
                }

                // For now, all skills' caster is self.
                try
                {
                    castSkill(position, gameObject);
                }
                catch(InvalidOperationException)
                {
                    // cannot cast the spell now. May be play some warning sound.
                }
            }
        }
    }

    private void OnDestroy()
    {
        // deselect the skills to avoid problems on replay
        deselectSkill();
    }
}
