using UnityEngine;

/// <summary>
/// The player.
/// It reacts to inputs to move and release skills.
/// It can be hit and killed.
/// It can move while releasing a skill.
/// </summary>
public sealed class Player : HittableEntity
{
    /*********************************** Static ***********************************/

    // game units per second.
    public const float moveSpeed = 1.0f;

    /*********************************** Fields ***********************************/

    // the current move destination
    public Vector3 moveDst;
    // Moving is implemented using a coroutine.
    Coroutine moveCoro;

    // TODO: A collection of skills.

    /*********************************** Ctor ***********************************/
    public Player(float initialHealth) : base(initialHealth)
    {
    }

    /*********************************** Methods ***********************************/

    /*********************************** From Entity ***********************************/

    protected override void Awake()
    {
        // Initially there is no move.
        moveDst = gameObject.transform.position;

        throw new System.NotImplementedException();
    }

    protected override void Start()
    {
        throw new System.NotImplementedException();
    }

    protected override void Update()
    {
        throw new System.NotImplementedException();
    }
}