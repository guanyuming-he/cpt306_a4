using UnityEngine;

/// <summary>
/// Data for a level 1 skill that shoots a bullet.
/// </summary>
/// 
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ShootSkill1Data", order = 1)]
public class ShootSkill1Data : SkillData
{
    public float damage;
}