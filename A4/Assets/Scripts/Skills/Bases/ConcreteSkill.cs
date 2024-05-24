using UnityEngine;

/// <summary>
/// A ConcreteSkill is a concrete (opposed to the most abstract form, an interface) 
/// implementation of ISkill that defines a few necessary fields and the basic logic.
/// 
/// It does inherit from MonoBehaviour because it has to read the input and perform various effects.
/// 
/// A subclass MUST call the base's version when overriding any virtual method.
/// </summary>
public abstract class ConcreteSkill : MonoBehaviour, ISkill
{
    public enum Level
    {
        LEVEL_1,
        LEVEL_2,
        LEVEL_3,
        LEVEL_4,
        LEVEL_5,
    }

    /*********************************** Fields ***********************************/
    private bool selected;
    protected Timer cooldownTimer;
    // set in the editor.
    public SkillData skillData;

    /*********************************** Ctor ***********************************/

    public ConcreteSkill()
    {
        selected = false;
        // Need to wait for Awake()
        cooldownTimer = null;
    }

    /*********************************** Observers ***********************************/

    /// <summary>
    /// A skill is in cooldown iff the cooldown timer is running.
    /// </summary>
    /// <returns>true iff the skill is in cooldown.</returns>
    public bool isInCooldown()
    {
        return cooldownTimer.getState() == Timer.State.RUNNING;
    }

    /// <returns>
    /// the progress of the cooldown
    /// 0: just started.
    /// 1: finished.
    /// </returns>
    public float getCooldownProgress()
    {
        return (cooldownTimer != null && isInCooldown()) ?
           cooldownTimer.getProgress() : 1.0f;
    }

    /*********************************** Mutators ***********************************/
    /// <summary>
    /// Init the skill with the data set in editor.
    /// Must only be called in Awake()
    /// </summary>
    protected virtual void setData()
    {
        Utility.MyDebugAssert(skillData != null, "Set the skill data in the editor.");

        // Timer
        {
            Utility.MyDebugAssert(skillData.cooldown >= 0.0f, "cooldown cannot be negative.");
            // does not loop and does not call back when it fires.
            cooldownTimer = new Timer(skillData.cooldown, null, false);
        }
    }

    /// <summary>
    /// Release the skill. For instantaneous skills, call this directly.
    /// For those that are not instantaneous, call this when the releasing is finished.
    /// </summary>
    /// <param name="position">
    /// the position where the skill is to be released.
    /// may or may not be used.
    /// </param>
    /// <param name="target">the target of the skill, can be null if not used.</param>
    /// <exception cref="System.InvalidOperationException">
    /// If it cannot be released now.
    /// </exception>
    /// 
    public void release(Vector3 position, GameObject target)
    {
        if (!canReleaseSkill())
        {
            throw new System.InvalidOperationException();
        }

        // perform the skill's effect
        onReleased(position, target);

        // restart the cooldown timer
        cooldownTimer.restart();
    }

    /*********************************** Interface ISkill ***********************************/

    /// <returns>true if it is not in cooldown, or if it is not current releasing a skill.</returns>
    public abstract bool canReleaseSkill();

    public bool isSelected()
    {
        return selected;
    }

    public virtual void onDeselected()
    {
        Utility.MyDebugAssert(selected == true, "previously should have been selected.");
        selected = false;
    }
    public virtual void onSelected()
    {
        Utility.MyDebugAssert(selected == false, "previously should not have been selected.");
        selected = true;
    }

    public abstract void onReleased(Vector3 position, GameObject target);

    /*********************************** Mono ***********************************/

    /// <summary>
    /// Set the skill's data.
    /// </summary>
    protected virtual void Awake()
    {
        setData();
    }

    protected virtual void Update()
    {
        cooldownTimer.update(Time.deltaTime);
    }
}