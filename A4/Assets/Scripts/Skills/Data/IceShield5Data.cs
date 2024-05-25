using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/IceShield5Data", order = 1)]
public class IceShield5Data : SkillData
{
    [Header("IceShield Gameplay")]
    public float damageReduction;
    public float duration;
    public GameObject iceShieldEffect;
    public IceShield5Data()
    {
        level = ConcreteSkill.Level.LEVEL_5;
        skillName = "IceShield (Lv.5)";

        // some default values
        damageReduction = .5f;
        duration = 20.0f;
    }

    protected override void Awake()
    {
        base.Awake();

        Utility.MyDebugAssert(damageReduction > 0.0f && damageReduction <= 1.0f,
            "damage reduction must be in (0, 1]");
        Utility.MyDebugAssert(duration > 0.0f, "duration must be positive.");
    }

}
