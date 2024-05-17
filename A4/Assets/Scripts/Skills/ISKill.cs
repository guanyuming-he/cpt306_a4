/// <summary>
/// The interface for all skills.
/// 
/// If defines how two major things about a skill is done.
/// 
/// A. The releasing of a skill.
    /// Although some skills are released instantaneously and some are not,
    /// I define two methods anyway:
    ///     1. startReleasing();
    ///     2. stopReleasing();
    /// If a skill is released instantaneously, then only 1 has effect and 2 should be empty.
    /// If a skill is not released instantaneously, then both have effects.
    /// 
/// B. The selection of a skill.
    /// In this game, both the boss and the player can own a number of skills,
    /// but only one or two are selected at a time.
    /// Whether a skill is selected have these effects:
    ///     1. Those that are selected are active and respond to the events of the game (player inputs, boss decisions).
    ///     2. Those that are not selected are not active and remains dormant until being selected.
/// </summary>
public interface ISkill
{
    /// <summary>
    /// Called to start releasing the skill.
    /// For skills that are released instantaneously,
    /// this method performs the action of the skill.
    /// </summary>
    public void startReleasing();

    /// <summary>
    /// Called to end releasing the skill.
    /// For skills that are NOT released instantaneously,
    /// this method performs the action of the skill.
    /// </summary>
    public void stopReleasing();

    /// <returns>
    /// true iff releaseSkill can be called now.
    /// Mostly, this is connected with the cooldown.
    /// </returns>
    public bool canReleaseSkill();

    /// <summary>
    /// Called when the skill is selected by the entity that owns the skill.
    /// </summary>
    public void onSelected();

    /// <summary>
    /// Called when the skill is deselected by the entity that owns the skill.
    /// </summary>
    public void onDeselected();

    /// <returns>true iff the skill is currently selected.</returns>
    public bool isSelected();
}
