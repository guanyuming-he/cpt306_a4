using UnityEngine;

/// <summary>
/// A level 3 spell that release a rain of arrows (bullets)
/// from the sky above the target position.
/// </summary>
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/RainOfArrow3Data", order = 1)]
public class RainOfArrow3Data : SkillData
{
    // Every period, rains a bullet randomly in a circle of radius.
    // Lasts for duration periods.
    [Header("RainOfArrow Gameplay")]
    public float damage;
    public float radius;
    public uint duration;
    public float period;
    // from how high is the skill released.
    public float height;
    public GameObject bulletPrefab;

    public RainOfArrow3Data()
    {
        level = ConcreteSkill.Level.LEVEL_3;
        skillName = "Rain of arrows (Lv.3)";

        // some default values
        damage = 1.0f;
        duration = 15;
        radius = 3.0f;
        height = 10.0f;
        period = .1f;
    }

    protected override void Awake()
    {
        base.Awake();

        Utility.MyDebugAssert(damage > 0.0f, "damage must be positive.");
        Utility.MyDebugAssert(radius > 0.0f, "radius must be positive.");
        Utility.MyDebugAssert(height > 0.0f, "height must be positive.");
        Utility.MyDebugAssert(period > 0.0f, "period must be positive.");
        Utility.MyDebugAssert(bulletPrefab != null, "bullet prefab cannot be null.");
    }

}