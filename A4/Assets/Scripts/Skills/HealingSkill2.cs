using System.Collections;
using UnityEngine;

public class HealingSkill2 : InstantaneousSkill
{
    /*********************************** Private helpers ***********************************/

    protected virtual void spawnHealingEffect(DamageableEntity target)
    {
        var healingEffect = GameObject.Instantiate
        (
            healingData.healingSkillEffect, 
            target.gameObject.transform
        );
        // Don't forget to destroy it when the healing is done.
        GameObject.Destroy(healingEffect, healingData.duration * healingData.healingPeriod);
    }
    
    /// <summary>
    /// Use this coroutine to easily heal over time.
    /// </summary>
    /// <returns></returns>
    private IEnumerator healingCoro(DamageableEntity target)
    {
        spawnHealingEffect(target);

        for(int i = 0; i < healingData.duration; ++i)
        {
            target.onHealed(healingData.healingAmount);
            yield return new WaitForSeconds(healingData.healingPeriod);
        }
    }

    /*********************************** Fields ***********************************/
    // for convenience, I convert the skillData field to the more specific type.
    protected HealingSkill2Data healingData;

    /*********************************** From ConcreteSkill ***********************************/

    /// <summary>
    /// Overridden to set up shootingSKillData
    /// </summary>
    protected override void setData()
    {
        base.setData();

        healingData = skillData as HealingSkill2Data;
        Utility.MyDebugAssert(healingData != null, "set the correct skill data type!");
        // unnecessary to check the fields of it here,
        // because they are checked in its Awake().
    }

    /// <summary>
    /// Released by target to heal target.
    /// </summary>
    /// <param name="position">not used</param>
    /// <param name="target">whom to heal</param>
    public override void onReleased(Vector3 position, GameObject target)
    {
        DamageableEntity de = target.GetComponent<DamageableEntity>();
        Utility.MyDebugAssert(de != null, "attach this script in the editor.");

        this.StartCoroutine(healingCoro(de));
    }
}