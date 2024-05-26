using UnityEngine;

/// <summary>
/// Shoots a bullet in a straight line.
/// </summary>
public class ShootingSkill1 : InstantaneousSkill
{
    /*********************************** Fields ***********************************/
    // for convenience, I convert the skillData field to the more specific type.
    protected ShootSkill1Data shootingSKillData;

    private AudioSource shootingAudio;

    /*********************************** From ConcreteSkill ***********************************/

    /// <summary>
    /// Overridden to set up shootingSKillData
    /// </summary>
    protected override void setData()
    {
        base.setData();

        shootingSKillData = skillData as ShootSkill1Data;
        Utility.MyDebugAssert(shootingSKillData != null, "set the correct skill data type!");
        // unnecessary to check the fields of it here,
        // because they are checked in its Awake().
    }

    /// <summary>
    /// Shoots a bullet along the direction: caster -> position
    /// Here the caster is abused to point to the one that shoots the bullet.
    /// </summary>
    /// <param name="position">where the mouse points to</param>
    /// <param name="caster">he who shoots the bullet</param>
    public override void onReleased(Vector3 position, GameObject caster)
    {
        // make the bullet the same height as the player.
        position.y = .7f;
        Vector3 casterNewPos = caster.transform.position;
        casterNewPos.y = .7f;

        // the instantiated bullet.
        GameObject bullet;
        Vector3 direction;
        // spawn a bullet at the position, with the correct direction.
        {
            direction = position - casterNewPos;
            // In case this happens...
            if (direction == Vector3.zero)
            {
                direction = Vector3.forward;
            }

            bullet = GameObject.Instantiate
            (
                shootingSKillData.bulletPrefab,
                casterNewPos,
                Quaternion.LookRotation(direction)
            );
        }

        // set up the bullet
        {
            Bullet bulletScript = bullet.GetComponent<Bullet>();

            Utility.MyDebugAssert(bulletScript != null, "should have the bullet script.");

            bulletScript.damage = shootingSKillData.damage;
            bulletScript.direction = direction.normalized;
        }

        // play the sound effect
        {
            AudioManager.playEffect(shootingAudio);
        }
    }

    protected override void Awake()
    {
        base.Awake();
        shootingAudio = GetComponent<AudioSource>();
    }
}