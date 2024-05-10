using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    // so long as the player is within this distance to the move destination,
    // we treat the move as finished.
    public const float moveDstTolerance = .001f;

    /// <summary>
    /// A helper that does this calculation.
    /// </summary>
    /// <param name="mouseScreenPos">the screen position of the mouse</param>
    /// <returns>where on the floor the mouse is clicking at</returns>
    /// 
    /// <exception cref="System.ArgumentException">
    /// if the mouse click point is not on the plane
    /// </exception>
    private static Vector3 mouseClickToFloorPosition()
    {
        Camera cam = Game.gameSingleton.cameraMgr.getMainCamera();
        Utility.MyDebugAssert(cam != null);

        // the ray from the eye pointing to where the mouse is clicking at.
        Ray mouseRay = cam.ScreenPointToRay(Input.mousePosition);

        // This is passed out from Plane.Raycast that is the value of
        // the distance between the intersection and the ray origin
        float distance = 0.0f;
        if (Game.gameSingleton.mapMgr.floorSurfacePlane.Raycast(mouseRay, out distance))
        // returns true if there is an intersection
        {
            // the intersection is this far from the origin.
            var pointOnFloor = mouseRay.GetPoint(distance);
            // TODO: if it's on the plane, decide if the intersection is under an obstacle.
            throw new System.NotImplementedException();

            return pointOnFloor;
        }
        else
        // there is no intersection
        {
            throw new System.ArgumentException("The mouse click is not on the floor.");
        }
    }

    /*********************************** Fields ***********************************/

    //// Movement
    // the move destination indicator, assigned in the editor.
    public GameObject moveIndicator;
    // the current move destination
    private Vector3 moveDst;
    // Moving is implemented using a coroutine.
    Coroutine moveCoroVar;

    //// States
    private bool isMoving;
    private bool isReleasingSkill;

    //// TODO: A collection of skills.

    /*********************************** Ctor ***********************************/

    /// <summary>
    ///  Inits the player
    /// </summary>
    public Player(float initialHealth) : base(initialHealth)
    {
        moveCoroVar = null;
        isMoving = false;
        isReleasingSkill = false;
    }

    /*********************************** Methods ***********************************/

    /// <summary>
    /// Called when a mouse move click is received.
    /// </summary>
    private void startMoving()
    {
        try
        {
            moveDst = mouseClickToFloorPosition();
        }
        catch(ArgumentException)
        {
            // The mouse click cannot intersect with the plane. Do nothing.
            return;
        }

        // show the movement indicator
        moveIndicator.transform.position = moveDst;
        moveIndicator.SetActive(true);

        isMoving = true;
        moveCoroVar = StartCoroutine(moveCoro());
    }

    /// <summary>
    /// Called inside the move coroutine to end the move.
    /// </summary>
    private void endMovement()
    {
        // now pos is at or very near the destination.
        // just put it in the destination anyway.
        transform.position = moveDst;

        isMoving = false;
        moveIndicator.SetActive(false);
    }

    /// <summary>
    /// The move coroutine.
    /// </summary>
    /// <returns></returns>
    private IEnumerator moveCoro()
    {
        // In each frame:
        // 1. check if the player has arrived at the dst
        // 2. if not, move towards it
        // 3. otherwise, complete the movement.

        float dist;
        do
        {
            // 2.
            dist = Vector3.Distance(transform.position, moveDst);
            float moveDist = moveSpeed * Time.deltaTime;
            // if the move distance is bigger than the actual distance,
            // then don't over move.
            if (moveDist > dist)
            {
                transform.position = moveDst;
            }
            else
            {
                var moveDirection = moveDst - transform.position;
                transform.position += moveDist * moveDirection.normalized;
            }

            yield return null;
        }
        // 1.
        while (dist > moveDstTolerance);

        // 3.
        endMovement();
        // Doing nothing will exit the coroutine.
    }

    /*********************************** From Entity ***********************************/

    protected override void Awake()
    {
        // Initially there is no move.
        moveDst = gameObject.transform.position;

        moveIndicator = GameObject.Instantiate(moveIndicator);
        Utility.MyDebugAssert(moveIndicator != null, "assign this in the editor.");
        // hide it first.
        moveIndicator.SetActive(false);

        throw new System.NotImplementedException("Skills");
    }

    protected override void Start()
    {
        throw new System.NotImplementedException();
    }

    protected override void Update()
    {
        // 1. handle movement input
        // 2. handle skill release input

        // 1.
        if(Input.GetMouseButtonDown(1))
        // 1 is right click for movement
        {
            // check if the click is on any obstacle first.
        }

        throw new System.NotImplementedException("2.");
    }
}