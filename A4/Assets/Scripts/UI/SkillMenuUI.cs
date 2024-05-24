using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillMenuUI : MonoBehaviour
{
    // displays how many skill coins the player has, etc.
    public TMP_Text statusText;
    // go back.
    public Button backButton;
    public RectTransform skillsPanel;

    public GameObject skillItemPrefab;

    /// <summary>
    /// Update other elements that are not the skill items
    /// </summary>
    private void updateOtherElements()
    {
        statusText.text = string.Format("Skill Coins: {0}", Game.gameSingleton.stateMgr.getNumSkillCoins());
        // set the prepared skills text
    }

    /// <summary>
    /// Fill in skills and set up all the ui elements
    /// </summary>
    void Start()
    {
        // fill in the skills.
        {
            Utility.MyDebugAssert(skillsPanel != null, "assign this panel in editor.");
            Utility.MyDebugAssert(skillItemPrefab != null, "assign this item prefab in editor.");

            for (int i = 0; i < Game.gameSingleton.skillsMgr.playerSkills.Count; ++i)
            {
                var ps = Game.gameSingleton.skillsMgr.playerSkills[i];
                var skillItem = GameObject.Instantiate(skillItemPrefab, skillsPanel);
                SkillStoreItem skillItemScript = skillItem.GetComponent<SkillStoreItem>();
                skillItemScript.bindToSkill(i, ps);
            }
        }

        // other UI elements
        {
            updateOtherElements();
        }
    }

    // Update is called once per frame
    void Update()
    {
        updateOtherElements();
    }
}
