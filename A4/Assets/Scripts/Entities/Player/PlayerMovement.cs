using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Controls the movement of the player only
/// </summary>
public sealed class PlayerMovement : MonoBehaviour
{
    /*********************************** Static ***********************************/

    // when the speed is less than this value, I treat the player as not moving.
    private const float staticMaxSpeed = 0.04f;

    /// <summary>
    /// A helper that does this calculation.
    /// </summary>
    /// <param name="mouseScreenPos">the screen position of the mouse</param>
    /// <returns>where on the floor the mouse is clicking at</returns>
    /// 
    /// <exception cref="System.ArgumentException">
    /// if the mouse click point is not on the plane
    /// </exception>
    public static Vector3 mouseClickToFloorPosition()
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
            // Don't need to handle when the click point is inside an obstacle 
            // or the boss, because NavMesh will handle that for me (won't go into there).
            return pointOnFloor;

            // TODO: If navmesh can't handle movement inside obstacles, then I will do it myself.
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
    // moving is controlled by a NavMesh Agent
    NavMeshAgent moveAgent;

    /*********************************** Ctor ***********************************/

    /// <summary>
    /// Nothing to do for now.
    /// </summary>
    public PlayerMovement()
    {
    }

    /*********************************** Observers ***********************************/
    public bool isMoving() 
    { 
        return 
            // do not use speed, as that name is misleading.
            // speed means the max speed actually.
            moveAgent.velocity.magnitude >= staticMaxSpeed; 
    }

    /*********************************** Mutators ***********************************/

    /// <summary>
    /// Called when the player clicks somewhere to move his character.
    /// </summary>
    private void startMoving()
    {
        try
        {
            moveDst = mouseClickToFloorPosition();
        }
        catch(ArgumentException)
        {
            // The position is not on the floor or is on some obstacle.
            // Do nothing.
            return;
        }

        // Notify the moveAgent about the new destination.
        moveAgent.destination = moveDst;

        // show the movement indicator
        moveDst.y = 1.5f;
        moveIndicator.transform.position = moveDst;
        moveIndicator.SetActive(true);
    }

    /// <summary>
    /// Called when the isMoving is true in Update.
    /// Should show the movement indicator here.
    /// </summary>
    private void onMoving()
    {
        moveIndicator.SetActive(true);
    }

    /// <summary>
    /// Called when the isMoving is false in Update.
    /// Should hide the movement indicator here.
    /// </summary>
    private void onNotMoving()
    {
        moveIndicator.SetActive(false);
    }

    /*********************************** From Entity ***********************************/

    private void Awake()
    {
        // Initially there is no move.
        moveDst = gameObject.transform.position;

        moveIndicator = GameObject.Instantiate(moveIndicator);
        Utility.MyDebugAssert(moveIndicator != null, "assign this in the editor.");
        // hide it first.
        moveIndicator.SetActive(false);
    }

    /// <summary>
    /// Init things that cannot be done in Awake()
    /// </summary>
    private void Start()
    {
        // Don't know why, but the doc puts the init of the navmesh agent in Start()
        moveAgent = gameObject.GetComponent<NavMeshAgent>();
        Utility.MyDebugAssert(moveAgent != null, "assign this in the editor.");
    }

    private void Update()
    {
        // handle movement input
        if(Input.GetMouseButtonDown(1))
        // 1 is right click for movement
        {
            startMoving();
        }

        // show or hide the movement indicator based on the state.
        if(isMoving())
        {
            onMoving();
        }
        else
        {
            onNotMoving();
        }
    }
}