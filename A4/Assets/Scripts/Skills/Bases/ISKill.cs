using UnityEngine;

/// <summary>
/// The interface for all skills.
/// 
/// If defines how two major things about a skill is done.
/// 
/// A. The releasing of a skill.
/// Although some skills are released instantaneously and some are not,
/// I define only one method in this interface anyway:
///     1. onReleased()
/// , because I believe handling the releasing process of a skill is too specific to be defined here.
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
    /// Called when the skill is just released.
    /// 
    /// In this method, perform (activate) the effect of the skill.
    /// </summary>
    /// <param name="position">
    /// the position where the skill is to be released.
    /// may or may not be used.
    /// </param>
    /// <param name="caster">the caster of the skill, can be null if not used.</param>
    public void onReleased(Vector3 position, GameObject caster);

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
