using UnityEngine;

/// <summary>
/// Plants a timer bomb.
/// </summary>
public class PlantBomb4 : InstantaneousSkill
{
    /*********************************** Fields ***********************************/
    // for convenience, I convert the skillData field to the more specific type.
    protected PlantBomb4Data bombData;

    /*********************************** From ConcreteSkill ***********************************/

    /// <summary>
    /// Overridden to set up shootingSKillData
    /// </summary>
    protected override void setData()
    {
        base.setData();

        bombData = skillData as PlantBomb4Data;
        Utility.MyDebugAssert(bombData != null, "set the correct skill data type!");
        // unnecessary to check the fields of it here,
        // because they are checked in its Awake().
    }

    /// <summary>
    /// Spawn a bomb at position.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="caster">not used</param>
    public override void onReleased(Vector3 position, GameObject caster)
    {
        // just spawn the bomb, and it will do the rest of the job.
        // a bomb is about 1.4 units high.
        position.y = .7f;
        // keep the position out of the boss's body.
        if(position.x > -.5f * BossEntity.BOSS_WIDTH && position.x < .5f * BossEntity.BOSS_WIDTH)
        {
            position.x = Mathf.Sign(position.x) * (.5f * BossEntity.BOSS_WIDTH + .5f);
        }
        if(position.z > -.5f * BossEntity.BOSS_WIDTH && position.z < .5f * BossEntity.BOSS_WIDTH)
        {
            position.z = Mathf.Sign(position.z) * (.5f * BossEntity.BOSS_WIDTH + .5f);
        }

        var bomb = GameObject.Instantiate(bombData.bombPrefab, position, Quaternion.identity);
        TimerBomb bombScript = bomb.GetComponent<TimerBomb>();
        Utility.MyDebugAssert(bombScript != null, "did I attach the script?");

        // set up the bomb and start the countdown
        bombScript.setBombData(bombData);
        bombScript.startCountDown();
    }
}
