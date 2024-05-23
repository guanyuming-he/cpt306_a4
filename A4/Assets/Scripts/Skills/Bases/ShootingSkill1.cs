using UnityEngine;

/// <summary>
/// Shoots a bullet in a straight line.
/// </summary>
public class ShootingSkill1 : InstantaneousSkill
{
    /*********************************** Fields ***********************************/
    // for convenience, I convert the skillData field to the more specific type.
    protected ShootSkill1Data shootingSKillData;

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
    /// Shoots a bullet along the direction: target -> position
    /// Here the target is abused to point to the one that shoots the bullet.
    /// </summary>
    /// <param name="position">where the mouse points to</param>
    /// <param name="target">he who shoots the bullet</param>
    public override void onReleased(Vector3 position, GameObject target)
    {
        GameObject bullet;
        Vector3 direction;
        // spawn a bullet at the position, with the correct direction.
        {
            direction = target.transform.position - position;
            // In case this happens...
            if (direction == Vector3.zero)
            {
                direction = Vector3.forward;
            }

            bullet = GameObject.Instantiate
            (
                shootingSKillData.bulletPrefab,
                position,
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
    }
}