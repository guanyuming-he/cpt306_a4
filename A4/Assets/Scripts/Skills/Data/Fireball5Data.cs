using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Fireball5Data", order = 1)]
public class Fireball5Data : SkillData
{
    
    [Header("Fireball Gameplay")]
    public float damage;
    public GameObject fireballPrefab;

    public Fireball5Data()
    {
        level = ConcreteSkill.Level.LEVEL_5;
        skillName = "Fire ball (Lv.5)";

        // some default values
        damage = 20.0f;
    }

    protected override void Awake()
    {
        base.Awake();

        //Utility.MyDebugAssert(fireballPrefab != null, "assign the prefab.");
        Utility.MyDebugAssert(damage > 0.0f, "damage must be positive.");
    }
}
