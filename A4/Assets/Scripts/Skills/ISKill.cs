/// <summary>
/// The interface for all skills.
/// </summary>
public interface ISkill
{
    /// <summary>
    /// Called when the skill is released.
    /// Some skill may call it as soon as a button is clicked,
    /// while some others may call it when a button is finally released.
    /// 
    /// This performs the effect of the skill.
    /// </summary>
    public void onSkillReleased();
}
