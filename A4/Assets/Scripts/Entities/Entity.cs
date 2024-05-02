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
    /*********************************** Mono ***********************************/
    protected abstract void Awake();

    protected abstract void Start();

    protected abstract void Update();
}
