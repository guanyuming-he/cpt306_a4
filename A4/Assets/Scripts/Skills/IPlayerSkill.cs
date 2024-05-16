/// <summary>
/// A player's skill must provide some kind of
/// indication to help the player release the skill.
/// 
/// The player is able to switch between a list of skills.
/// So a skill must handle the situations when it is selected and deselected.
/// (Perhaps a skill will change the mouse icon on being selected)
/// </summary>
public interface IPlayerSkill : ISkill
{
    /// <summary>
    /// Called when the skill is selected by the player.
    /// (Perhaps a skill will change the mouse icon on being selected)
    /// </summary>
    public void onSkillSelected();

    /// <summary>
    /// Called when the skill is deselected by the player.
    /// (Perhaps a skill will restore the mouse icon on being deselected)
    /// </summary>
    public void onSkillDeselected();
}