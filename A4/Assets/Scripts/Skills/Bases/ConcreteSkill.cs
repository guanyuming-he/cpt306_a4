using UnityEngine;

/// <summary>
/// A ConcreteSkill is a concrete (opposed to the most abstract form, an interface) 
/// implementation of ISkill that defines a few necessary fields and the basic logic.
/// 
/// It inherits from MonoBehaviour so that it can be attached to an object.
/// 
/// A subclass MUST call the base's version when overriding any virtual method.
/// </summary>
public abstract class ConcreteSkill : MonoBehaviour, ISkill
{
    /*********************************** Fields ***********************************/
    private bool selected;
    protected Timer cooldownTimer;

    protected SkillData skillData;

    /*********************************** Ctor ***********************************/
    public ConcreteSkill()
    {
        selected = false;
        // inited later as the skill information is not available at construction for a Unity script.
        cooldownTimer = null;
        // set later in setSkillData()
        skillData = null;
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

    /*********************************** Mutators ***********************************/
    public void setSkillData(SkillData data)
    {
        Utility.MyDebugAssert(skillData == null, "can only set skill data once.");

        skillData = data;
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

    public abstract void onReleased();

    /*********************************** Mono ***********************************/

    /// <summary>
    /// Check if an instance of SkillData has been provided to it.
    /// And init the skill with the skill data.
    /// </summary>
    protected virtual void Start()
    {
        Utility.MyDebugAssert(skillData != null, "need data for this skill.");

        // Timer
        {
            Utility.MyDebugAssert(skillData.cooldown >= 0.0f, "cooldown cannot be negative.");
            // does not loop and does not call back when it fires.
            cooldownTimer = new Timer(skillData.cooldown, null, false);
        }        
    }

    /// <summary>
    /// Any action that the skill needs to perfrom per frame.
    /// </summary>
    protected virtual void Update()
    {
        cooldownTimer.update(Time.deltaTime);
    }
}