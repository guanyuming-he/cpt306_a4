using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PlantBomb4Data", order = 1)]
public class PlantBomb4Data : SkillData
{
    [Header("Explosion Gameplay")]
    public float explosionDamage;
    public GameObject bombPrefab;
    public float timeToExplode;
    public float explosionRadius;
    public GameObject explosionEffect;
    public float explosionAttenuation;

    public PlantBomb4Data()
    {
        level = ConcreteSkill.Level.LEVEL_4;
        skillName = "Plant Bomb (Lv.4)";

        // some default values
        explosionDamage = 10.0f;
        timeToExplode = 2.0f;
        explosionRadius = 5.0f;
        explosionAttenuation = .3f;
    }

    protected override void Awake()
    {
        base.Awake();

        Utility.MyDebugAssert(bombPrefab != null, "bomb prefab must not be null.");
        Utility.MyDebugAssert(explosionDamage > 0.0f, "explosion damage must be positive.");
        Utility.MyDebugAssert(timeToExplode > 0.0f, "time to explode must be positive.");
        Utility.MyDebugAssert(explosionRadius > 0.0f, "explosion radius must be positive.");
        Utility.MyDebugAssert(explosionEffect != null, "explosion effect must not be null.");
    }
}
