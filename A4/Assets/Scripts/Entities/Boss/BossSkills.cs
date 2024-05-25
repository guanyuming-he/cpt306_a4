using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Handles the skill casting of the boss
/// </summary>
public class BossSkills : SkillCaster
{
    private int lv5IceSkillInd = -1;

    public float timeToCastNextSkill = 2.0f;
    private Timer castSkillTimer;

    public BossSkills()
    {
        castSkillTimer = new Timer(timeToCastNextSkill, castNextSkill, true);
    }

    /// <summary>
    /// Randomly switches to one of the prepared skills and cast it.
    /// </summary>
    private void castNextSkill()
    {
        // No skill is prepared. Go back.
        if (getNumPreparedSkills() == 0)
        {
            return;
        }

        // find the first that can be cast
        for (int i = 0; i < getNumPreparedSkills(); ++i)
        {
            if (getPreparedSkill(i).canReleaseSkill())
            {
                selectSkill(i);
                // cast to the player's position
                // caster is always self in this game.
                castSkill(Game.gameSingleton.mapMgr.player.transform.position, gameObject);

                break;
            }
        }

    }
    /// <summary>
    /// Prepare these skills.
    /// Also see if the lv5 ice skill is prepared.
    /// </summary>
    /// <param name="skillInds">A list of indices into playerSkills</param>
    private void prepareSkillsFromIndices(HashSet<int> skillInds)
    {
        List<ConcreteSkill> skillsToPrepare = new List<ConcreteSkill>();

        // track the prepared skills indices
        int j = 0;
        foreach (int i in skillInds)
        {
            skillsToPrepare.Add(Game.gameSingleton.bossSkills[i]);
            // the lv 5 ice skill is the 5th (ind 4) boss skill.
            if (i == 4)
            {
                lv5IceSkillInd = j;
            }
        }

        prepareSkills(skillsToPrepare);
    }

    /// <summary>
    /// Prepare randomly this number of skills from bossSkills.
    /// Called by the map manager.
    /// </summary>
    /// <param name="difficulty">
    /// -1: training ground
    /// 0: easy
    /// 1: normal
    /// </param>
    public void prepareSkillsOnGameStart(int difficulty)
    {
        if(difficulty == -1)
        {
            return;
        }

        HashSet<int> skillsToPrepare = new HashSet<int>();
        List<int> skills = null;

        switch(difficulty)
        {
            case 0:
                // easy: only prepare 2 skills randomly from level 3 and below.
                skills = new List<int>{ 0, 1, 2 };

                // boss casts slowly.
                timeToCastNextSkill = 2.8f;
                break;
                
            case 1:
                // normal: prepare 2 skills randomly from the others
                skills = new List<int> { 0, 1, 2, 3, 5 };
                // and prepare the ice skill
                skillsToPrepare.Add(4);

                // boss casts faster.
                timeToCastNextSkill = 1.5f;
                break;
        }

        // first one.
        int indRandom = Random.Range(0, skills.Count);
        skillsToPrepare.Add(skills[indRandom]);

        // swap the selected with [0]
        int temp = skills[0];
        skills[0] = skills[indRandom];
        skills[indRandom] = temp;

        // second one.
        indRandom = Random.Range(1, skills.Count);
        skillsToPrepare.Add(skills[indRandom]);

        // prepare these skills.
        prepareSkillsFromIndices(skillsToPrepare);

        castSkillTimer = new Timer(timeToCastNextSkill, castNextSkill, true);
        castSkillTimer.start();
    }

    /// <summary>
    /// 1. Every a few time, randomly selects a skill and fires it.
    /// </summary>
    private void Update()
    {
        castSkillTimer.update(Time.deltaTime);
    }
}