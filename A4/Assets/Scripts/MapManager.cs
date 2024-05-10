using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

/// <summary>
/// Manages the floor and everything on it.
/// </summary>
public class MapManager : MonoBehaviour
{
    /*********************************** Fields ***********************************/
    //// These are public because they need to be accessed.
    public Player player;
    // public Boss boss;
    public List<Obstacle> obstacles;

    //// These are public because they are prefabs assigned in the editor.
    public GameObject floorPrefab;
    public GameObject playerPrefab;
    public GameObject bossPrefab;
    public GameObject obstaclePrefab;

    // the plane of the up surface
    public Plane floorSurfacePlane;

    /*********************************** Static Settings ***********************************/
    // width is defined in the specification.
    public const float mapWidthX = 31.0f;
    public const float mapWidthZ = 31.0f;
    // these are defined by myself
    public const float mapHeight = 1.0f;
    // The center on the surface is the origin.
    public static readonly Vector3 floorCenterPos =
        new Vector3(.0f, -.5f * mapHeight, .0f);

    /*********************************** Ctor ***********************************/
    /// <summary>
    /// Nothing to do for now.
    /// </summary>
    public MapManager()
    { 
    }

    /*********************************** Mono ***********************************/
    /// <summary>
    /// Init the floor, player, boss, and the obstacles.
    /// </summary>
    public void Awake()
    {
        // the surface is centered at the origin and facing up.
        floorSurfacePlane = new Plane(Vector3.up, Vector3.zero);

        throw new System.NotImplementedException();
    }

    /// <summary>
    /// Nothing to do for now.
    /// </summary>
    public void Update()
    {
        
    }

}
