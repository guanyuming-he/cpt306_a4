using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillMenuUI : MonoBehaviour
{
    /*********************************** Fields ***********************************/
    // displays how many skill coins the player has, etc.
    public TMP_Text statusText;
    // go back.
    public Button backButton;
    // clear the prepared skills.
    public Button clearPreparedBtn;
    // panel of skills
    public RectTransform skillsPanel;
    // for each skill, spawn one this into the panel.
    public GameObject skillItemPrefab;
    // track those that are in the panel so that 
    // I won't create duplicate ones
    // skillInd -> skillItemUI
    private Dictionary<int, GameObject> skillItemsInPanel;
    // show which skills are prepared.
    public TMP_Text preparedSkillsText;

    /*********************************** Ctor ***********************************/
    public SkillMenuUI()
    {
        skillItemsInPanel = new Dictionary<int, GameObject>();
    }

    /*********************************** Private helpers ***********************************/
    /// <summary>
    /// Update other elements that are not the skill items
    /// </summary>
    private void updateOtherElements()
    {
        statusText.text = string.Format("Skill Coins: {0}", Game.gameSingleton.stateMgr.getNumSkillCoins());

        string preparedSkillsStr = "Prepared skills:\n" 
            + "(Only 4 can be prepared)\n";
        foreach(int pi in Game.gameSingleton.stateMgr.playerPreparedSkills)
        {
            preparedSkillsStr += string.Format("{0},", pi);
        }
        preparedSkillsText.text = preparedSkillsStr;
    }

    /*********************************** Mono ***********************************/

    /// <summary>
    /// Fill in skills and set up all the ui elements
    /// </summary>
    void Start()
    {
        // fill in the skills.
        {
            Utility.MyDebugAssert(skillsPanel != null, "assign this panel in editor.");
            Utility.MyDebugAssert(skillItemPrefab != null, "assign this item prefab in editor.");

            for (int i = 0; i < Game.gameSingleton.playerSkills.Count; ++i)
            {
                // If the item is already in the panel
                if(skillItemsInPanel.ContainsKey(i))
                {
                    continue;
                }

                // No. Add one to it.
                {
                    // get the corresponding skill.
                    var ps = Game.gameSingleton.playerSkills[i];
                    // init the item UI and bind it to the skill.
                    var skillItem = GameObject.Instantiate(skillItemPrefab, skillsPanel);
                    SkillStoreItem skillItemScript = skillItem.GetComponent<SkillStoreItem>();
                    skillItemScript.bindToSkill(i, ps);
                    // track the added UI item.
                    skillItemsInPanel.Add(i, skillItem);
                }
            }
        }

        // the buttons
        {
            // go back to the main UI.
            backButton.onClick.AddListener(() =>
            {
                UIManager.switchMenu(gameObject, Game.gameSingleton.uiMgr.mainMenu);
            });

            // clear the prepared skills
            clearPreparedBtn.onClick.AddListener(() =>
            {
                Game.gameSingleton.stateMgr.playerPreparedSkills.Clear();
            });
        }

        // other UI elements
        {
            updateOtherElements();
        }
    }

    // Update the status in real time.
    void Update()
    {
        updateOtherElements();
    }
}
