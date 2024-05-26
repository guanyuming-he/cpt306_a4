using System.Collections;
using UnityEngine;

/// <summary>
/// A level 3 spell that release a rain of arrows (bullets) from the sky above the target position.
/// </summary>
public class RainOfArrow3 : InstantaneousSkill
{

    private AudioSource audioEffect;

    /// <summary>
    /// use this coroutine to rain the bullets over time.
    /// </summary>
    /// <returns></returns>
    private IEnumerator rainCoro(Vector3 position)
    {
        for(int i = 0; i < rainData.duration; ++i)
        {
            // Spawn in the circle randomly.
            float angle = Random.Range(0.0f, Utility.TWO_PI);
            float dist = Random.Range(0.0f, rainData.radius);

            float x = dist * Mathf.Cos(angle);
            float z = dist * Mathf.Sin(angle);

            GameObject bullet = GameObject.Instantiate
            (
                rainData.bulletPrefab,
                position + new Vector3(x, 0.0f, z),
                Quaternion.LookRotation(Vector3.down)
            );
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.direction = Vector3.down;
            bulletScript.damage = rainData.damage;

            yield return new WaitForSeconds(rainData.period);
        }
    }

    /*********************************** Fields ***********************************/
    // for convenience, I convert the skillData field to the more specific type.
    protected RainOfArrow3Data rainData;

    /*********************************** From ConcreteSkill ***********************************/

    /// <summary>
    /// Overridden to set up shootingSKillData
    /// </summary>
    protected override void setData()
    {
        base.setData();

        rainData = skillData as RainOfArrow3Data;
        Utility.MyDebugAssert(rainData != null, "set the correct skill data type!");
        // unnecessary to check the fields of it here,
        // because they are checked in its Awake().
    }

    /// <summary>
    /// Every period, rains a bullet randomly in a circle of radius.
    /// Lasts for duration periods.
    /// </summary>
    /// <param name="position">the skill is to be released somewhere above position.</param>
    /// <param name="target">not used</param>
    public override void onReleased(Vector3 position, GameObject target)
    {
        position.y = rainData.height;

        AudioManager.playEffect(audioEffect);

        StartCoroutine(rainCoro(position));
    }

    protected override void Awake()
    {
        base.Awake();

        audioEffect = GetComponent<AudioSource>();
    }
}