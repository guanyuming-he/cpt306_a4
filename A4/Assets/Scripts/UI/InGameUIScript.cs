using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUIScript : MonoBehaviour
{
    /*********************************** Fields ***********************************/
    // this menu needs to monitor the state of the player and the boss.

    // physical state of the player
    PlayerEntity playerEntity;
    // skill state of the player
    PlayerSkills playerSkills;
    // physical state of the boss.
    BossEntity bossEntity;

    HealthBar playerHealthBar;
    HealthBar bossHealthBar;

    public RectTransform skillIconsPanel;
    public GameObject skillInGameUIPrefab;
    
    /*********************************** Methods ***********************************/

    /// <summary>
    /// Populate a skill icon UI for each prepared skill of the player.
    /// Called by the UI manager when a game has started.
    /// </summary>
    public void populateSkillIcons()
    {
        // clear the panel's previous children first.
        Utility.DestroyAllChildren(skillIconsPanel.gameObject);

        var ps = Game.gameSingleton.mapMgr.player.GetComponent<PlayerSkills>();
        var preparedSkills = ps.getPreparedSkills();

        for(int i = 0; i < preparedSkills.Count; ++i)
        {
            // skill number starts from 1.
            int skillNum = i + 1;

            GameObject skillIcon = GameObject.Instantiate(skillInGameUIPrefab, skillIconsPanel);
            SkillInGameIcon skillIconScript = skillIcon.GetComponent<SkillInGameIcon>();

            skillIconScript.bindToSkill(skillNum, preparedSkills[i], ps);
        }
    }

    /// <summary>
    /// Bind to in game objects.
    /// </summary>
    public void init()
    {
        playerEntity = Game.gameSingleton.mapMgr.player.GetComponent<PlayerEntity>();
        playerSkills = Game.gameSingleton.mapMgr.player.GetComponent<PlayerSkills>();
        bossEntity = Game.gameSingleton.mapMgr.boss.GetComponent<BossEntity>();
        Utility.MyDebugAssert(playerEntity != null, "check if the player entity script is attached.");
        Utility.MyDebugAssert(playerSkills != null, "check if the player skills script is attached.");
        Utility.MyDebugAssert(bossEntity != null, "check if the boss entity script is attached.");

        HealthBar[] healthBars = gameObject.GetComponentsInChildren<HealthBar>();
        Utility.MyDebugAssert(healthBars.Length == 2, "Should have two health bars in the menu.");
        playerHealthBar = healthBars[0];
        bossHealthBar = healthBars[1];

        Utility.MyDebugAssert(skillInGameUIPrefab != null, "assign this in the editor.");
    }

    /// <summary>
    /// do nothing.
    /// </summary>
    void Start()
    {
    }

    /// <summary>
    /// Update the UI based on the player's and the boss's states.
    /// </summary>
    void Update()
    {
        // Only update when playing.
        if (Game.gameSingleton.stateMgr.getState() != StateManager.States.PLAYING)
        {
            return;
        }

        // health bars
        {
            playerHealthBar.updateValues(playerEntity);
            bossHealthBar.updateValues(bossEntity);
        }
    }
}
