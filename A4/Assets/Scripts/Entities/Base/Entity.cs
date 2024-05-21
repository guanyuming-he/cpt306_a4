using UnityEngine;

/// <summary>
/// An entity is one of:
///     The player, 
///     the boss, 
///     the obstacles.
///     
/// It is attached to the entity's GameObject.
/// Make the three mono methods virtual so that they can be overridden.
/// </summary>
public abstract class Entity : MonoBehaviour
{
    /*********************************** Static ***********************************/

    /// <summary>
    /// On which side an entity is.
    /// </summary>
    public enum Side
    {
        PLAYER,
        BOSS,
        ENVIRONMENT
    }

    /*********************************** Methods ***********************************/
    /// <returns>the side that the entity is on.</returns>
    public abstract Side getSide();

    /*********************************** Mono ***********************************/
    protected abstract void Awake();

    protected abstract void Start();

    protected abstract void Update();
}
