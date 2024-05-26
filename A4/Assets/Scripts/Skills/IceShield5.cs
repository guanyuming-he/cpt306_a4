using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class IceShield5 : InstantaneousSkill
{

    protected IceShield5Data shieldData;

    /// <summary>
    /// Release the shield and have the damage reduction effect on target.
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    private IEnumerator setShieldCoro(GameObject target)
    {
        var shield = GameObject.Instantiate(shieldData.iceShieldEffect, target.transform);
        MeshRenderer renderer = shield.GetComponent<MeshRenderer>();
        AudioSource iceshieldAudio = shield.GetComponent<AudioSource>();
        AudioManager.playEffect(iceshieldAudio);

        // make the shield semi-transparent.
        var temp = renderer.material.color;
        temp.a = .4f;
        renderer.material.color = temp;

        // set the damage reduction effect
        DamageableEntity de = target.GetComponent<DamageableEntity>();
        de.damageMultiplier = 1.0f - shieldData.damageReduction;

        yield return new WaitForSeconds(shieldData.duration);

        // destroy the shield and restore the damage multiplier
        de.damageMultiplier = 1.0f;
        if(!shield.IsDestroyed())
        {
            GameObject.Destroy(shield);
        }
    }

    /*********************************** From ConcreteSkill ***********************************/

    /// <summary>
    /// Overridden to set up the specific data
    /// </summary>
    protected override void setData()
    {
        base.setData();

        shieldData = skillData as IceShield5Data;
        Utility.MyDebugAssert(shieldData != null, "set the correct skill data type!");
        // unnecessary to check the fields of it here,
        // because they are checked in its Awake().
    }

    public override void onReleased(Vector3 position, GameObject target)
    {
        StartCoroutine(setShieldCoro(target));
    }
}
