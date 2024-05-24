using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles the display of a skill in the in game menu.
/// </summary>
public class SkillInGameIcon : MonoBehaviour
{
 
    // The image of the skill.
    public Image skillImage;
    // The image the controls the cooldown fill.
    public Image coolDownImage;
    // The text that displays the number of the skill.
    public TMP_Text numberText;

    // The skill that it binds to.
    private ConcreteSkill skill;
    // The number of the skill in the in game UI.
    // Note: this is DIFFERENT from the skill index into the skillMgr's list.
    private int skillNumber;

    public void bindToSkill(int skillNumber, ConcreteSkill skill)
    {
        Utility.MyDebugAssert(this.skill == null, "can only bind to a skill once.");
        Utility.MyDebugAssert(skillNumber >= 1 && skillNumber <= 4, "Should get the skill number right.");

        this.skill = skill;
        this.skillNumber = skillNumber;
        numberText.text = string.Format("{0}", skillNumber);
        skillImage.sprite = this.skill.skillData.skillIconSprite;

        updateUIElements();
    }

    private void updateUIElements()
    {
        coolDownImage.fillAmount = 1.0f - skill.getCooldownProgress();
    }

    private void Start()
    {
        Utility.MyDebugAssert(coolDownImage != null, "assign this in the editor");
        Utility.MyDebugAssert(numberText != null, "assign this in the editor");
    }

    private void Update()
    {
        updateUIElements();
    }
}