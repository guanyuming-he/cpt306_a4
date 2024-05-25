using UnityEngine;

public class Fireball5 : InstantaneousSkill
{
    /*********************************** Fields ***********************************/
    // for convenience, I convert the skillData field to the more specific type.
    protected Fireball5Data fireballData;

    /*********************************** From ConcreteSkill ***********************************/

    /// <summary>
    /// Overridden to set up fireball data
    /// </summary>
    protected override void setData()
    {
        base.setData();

        fireballData = skillData as Fireball5Data;
        Utility.MyDebugAssert(fireballData != null, "set the correct skill data type!");
        // unnecessary to check the fields of it here,
        // because they are checked in its Awake().
    }

    /// <summary>
    /// Shoots a fireball along the direction: caster -> position
    /// Here the caster is abused to point to the one that shoots the bullet.
    /// </summary>
    /// <param name="position">where the mouse points to</param>
    /// <param name="caster">he who shoots the bullet</param>
    public override void onReleased(Vector3 position, GameObject caster)
    {
        // make the bullet the same height as the player.
        position.y = 1.0f;
        Vector3 casterNewPos = caster.transform.position;
        casterNewPos.y = 1.0f;

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
                fireballData.fireballPrefab,
                casterNewPos,
                Quaternion.LookRotation(direction)
            );
        }


        // set up the fireball
        {
            Fireball fireballScript = bullet.GetComponent<Fireball>();

            Utility.MyDebugAssert(fireballScript != null, "should have the fireball1 script.");

            fireballScript.damage = fireballData.damage;
            fireballScript.direction = direction.normalized;
        }
    }

}
