using UnityEngine;

/// <summary>
/// Base class for all bullets.
/// </summary>
public class Bullet : Entity
{
    /*********************************** Fields ***********************************/
    public float damage;
    public float speed;
    public Entity.Side whichSide;

    /// <summary>
    /// Side is set by the one that spawns the bullet.
    /// </summary>
    /// <returns></returns>
    public override Side getSide()
    {
        return whichSide;
    }

    /*********************************** Mono ***********************************/
    /// <summary>
    /// Check its data
    /// </summary>
    protected override void Awake()
    {
        Utility.MyDebugAssert(damage > 0.0f, "a bullet must have positive damage.");
        Utility.MyDebugAssert(speed > 0.0f, "a bullet must have a positive speed.");
        Utility.MyDebugAssert
        (
            whichSide != Side.ENVIRONMENT,
            "a bullet must be either from a player's or the boss's side"
        );
    }

    /// <summary>
    /// Do nothing for now.
    /// </summary>
    protected override void Start()
    {

    }

    /// <summary>
    /// Do nothing for now.
    /// </summary>
    protected override void Update()
    {

    }

    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        GameObject other = collision.gameObject;

        var entity = other.GetComponent<Entity>();
        if(entity == null)
        {
            return;
        }

        // entity != null.

        // If it's on the same side as the one hit by it
        if(getSide() == entity.getSide())
        {
            // basically it should not happen, as I have assigned
            // different collision layers
            Utility.MyDebugAssert(false, "collision layer is incorrect.");
            return;
        }

        // If I hit the environment.
        if(entity.getSide() == Side.ENVIRONMENT)
        {
            // destroy myself
            GameObject.Destroy(gameObject);
        }

        // Now I have hit a opponent.
        var damageable = entity as IDamageable;
        // If the entity can be damaged
        if(damageable != null)
        {
            damageable.onTakenDamage(damage);
        }
        // destroy myself regardless of whether the target can be hit (damaged) or not.
        GameObject.Destroy(gameObject);
    }
}