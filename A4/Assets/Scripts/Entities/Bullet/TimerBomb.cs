using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Make the attached GameObject act like a timer-bomb ---
/// it will explode after a set time,
/// damaging all objects not on its side.
/// </summary>
public class TimerBomb : Entity
{
    [Header("Entity")]
    public Entity.Side whichSide;

    [Header("Explosion")]
    public float explosionDamage;
    public float timeToExplode;
    public float explosionRadius;
    public GameObject explosionEffect;
    public float explosionAttenuation;

    public override Side getSide()
    {
        return whichSide;
    }

    private IEnumerator explodeCoro()
    {
        // explode after these seconds.
        yield return new WaitForSeconds(timeToExplode);

        // find all that are hit by the explosion.
        Collider[] objsHit = Physics.OverlapSphere
        (
            gameObject.transform.position,
            explosionRadius
        );

        // damage those that are damageable 
        foreach (var colliderHit in objsHit)
        {
            GameObject go = colliderHit.gameObject;
            DamageableEntity e = go.GetComponent<DamageableEntity>();

            // if not damageable, then skip.
            if (e == null)
            {
                continue;
            }

            // if on the same side as the bomb, then skip.
            if (e.getSide() == this.getSide())
            {
                continue;
            }

            // otherwise, damage it by the explosion.
            float dist = (gameObject.transform.position - go.transform.position).magnitude;
            // damage attenuates as the distance increases linearly.
            // max dmg: 0 dist. min dmg = (1-attenuation) * maxDmg: radius.
            float damage = (1.0f - explosionAttenuation * (dist / explosionRadius)) * explosionDamage;
            e.onTakenDamage(damage);
        }

        // play the explosion effect
        var expEff = GameObject.Instantiate(explosionEffect, gameObject.transform.position, Quaternion.identity);
        // scale it with the explosion radius.
        // the effect looks like having a 1.1 radius.
        expEff.transform.localScale = explosionRadius / 1.1f * Vector3.one;
        AudioSource expAudio = expEff.GetComponent<AudioSource>();
        AudioManager.playEffect(expAudio);

        // destroy myself
        GameObject.Destroy(gameObject);
        // delayed destroy the explosion effect
        GameObject.Destroy(expEff, 2.0f);
    }

    /// <summary>
    /// Called by the skill to set up the bomb.
    /// </summary>
    /// <param name="data"></param>
    public void setBombData(PlantBomb4Data data)
    {
        explosionDamage = data.explosionDamage;
        explosionRadius = data.explosionRadius;
        explosionAttenuation = data.explosionAttenuation;
        explosionEffect = data.explosionEffect;
        timeToExplode = data.timeToExplode;
    }

    /// <summary>
    /// Better let the skill call it than to call it in Start().
    /// This is to ensure data is set before this is called.
    /// </summary>
    public void startCountDown()
    {
        StartCoroutine(explodeCoro());
    }

    protected override void Awake()
    {
        // do nothing.
    }

    protected override void Start()
    {
        // do nothing.
    }

    protected override void Update()
    {
        // do nothing.
    }

}
