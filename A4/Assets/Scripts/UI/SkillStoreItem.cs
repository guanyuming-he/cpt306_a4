using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillStoreItem : MonoBehaviour
{
    // The skill that binds to it.
    private ConcreteSkill skill;
    // index into SkillMgr.playerSkills.
    private int skillInd;

    // These are set in the editor.
    public Image skillIconImg;
    public TMP_Text titleText;
    public TMP_Text descriptionText;
    public TMP_Text priceText;
    public Button purchaseBtn;
    public TMP_Text purchaseBtnText;

    /// <summary>
    /// Binds this item to a skill
    /// </summary>
    /// <param name="skill"></param>
    public void bindToSkill(int skillInd, ConcreteSkill skill)
    {
        Utility.MyDebugAssert(skill != null, "can't pass in a null.");
        Utility.MyDebugAssert(skill.skillData != null, "can't pass in null skill data.");

        this.skill = skill;
        this.skillInd = skillInd;

        skillIconImg.sprite = skill.skillData.skillIconSprite;
        titleText.text = skill.skillData.skillName;
        descriptionText.text = skill.skillData.description;
        priceText.text = string.Format("{0} coin(s)", skill.skillData.cost);
        purchaseBtnText.text =
            Game.gameSingleton.stateMgr.hasPlayerSkill(skillInd) ?
            "Prepare" : "Buy";
        purchaseBtn.onClick.AddListener(() =>
        {
            if(purchaseBtnText.text == "Buy")
            // Not bought yet
            {
                bool purchased = Game.gameSingleton.stateMgr.buySkill(skillInd);
                purchaseBtnText.text =
                    purchased ? "Prepare" : "Buy";
            }
            else if(purchaseBtnText.text == "Prepare")
            // bought. Prepare the skill
            {
                Game.gameSingleton.stateMgr.prepareSkill(skillInd);
            }

        });

    }

    // Not used for now.
    void Start()
    {
        
    }
}
