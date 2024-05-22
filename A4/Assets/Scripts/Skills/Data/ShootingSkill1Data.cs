using UnityEngine;

/// <summary>
/// Data for a level 1 skill that shoots a bullet.
/// </summary>
/// 
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ShootSkill1Data", order = 1)]
public class ShootSkill1Data : SkillData
{
    [Header("ShootSkill Gameplay")]
    public float damage;
    public GameObject bulletPrefab;

    private void Awake()
    {
        Utility.MyDebugAssert(bulletPrefab != null, "bullet cannot be null.");
        Utility.MyDebugAssert(damage > 0.0f, "damage must be positive.");
    }
}