using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the floor and everything on it.
/// </summary>
public class MapManager : MonoBehaviour
{
    /*********************************** Static Settings ***********************************/
    // width is defined in the specification.
    public const float mapWidth = 31.0f;
    // these are defined by myself
    public const float mapHeight = 1.0f;
    // Designed in such a way that the center of the top surface is the origin.
    public static readonly Vector3 floorCenterPos =
        new Vector3(.0f, -.5f * mapHeight, .0f);
    public static readonly Vector3 playerStart =
        new Vector3(0.0f, .5f, -12.0f);
    public static readonly Vector3 bossStart =
        new Vector3(.0f, .5f * BossEntity.bossWidth, .0f);

    // Useful constants
    public const float mapHalfWidth = .5f * mapWidth;
    public const float mapMinX = -mapHalfWidth;
    public const float mapMaxX = mapHalfWidth;
    public const float mapMinZ = -mapHalfWidth;
    public const float mapMaxZ = mapHalfWidth;

    /// <param name="l">y is ignored</param>
    /// <returns>
    /// true iff the (x,z) of the location is within the map, 
    /// and is not on the boundary
    /// </returns>
    public static bool isLocationInMap(Vector3 l)
    {
        return
            l.x > mapMinX && l.x < mapMaxX &&
            l.z > mapMinZ && l.z < mapMaxZ;
    }

    /// <param name="p">the position of the object</param>
    /// <returns>true if an object at position p with width will not overlap with the boss</returns>
    public static bool isPointNotOverlapWithBoss(Vector3 p, float width)
    {
        return
            p.x < -BossEntity.bossWidth - .5f * width || p.x > BossEntity.bossWidth + .5f * width ||
            p.z < -BossEntity.bossWidth - .5f * width || p.z > BossEntity.bossWidth + .5f * width;
    }

    /// <param name="p">the position of the object</param>
    /// <returns>true if an object at position p with width will not overlap with the player start</returns>
    public static bool isPointNotOverlapWithPlayerStart(Vector3 p, float width)
    {
        return
            p.x < playerStart.x - 1.0f - .5f * width || p.x > playerStart.x + 1.0f + .5f * width ||
            p.z < playerStart.z - 1.0f - .5f * width || p.z > playerStart.z + 1.0f + .5f * width;
    }

    /*********************************** Fields ***********************************/
    //// These are public because they may need to be accessed.
    public GameObject floor;
    public GameObject player;
    public GameObject boss;
    public List<GameObject> obstacles;

    //// These are public because they are prefabs assigned in the editor.
    public GameObject floorPrefab;
    public GameObject playerPrefab;
    public GameObject bossPrefab;
    public GameObject shortObstaclePrefab;
    public GameObject highObstaclePrefab;

    // the plane of the up surface
    public Plane floorSurfacePlane;

    /*********************************** Ctor ***********************************/
    /// <summary>
    /// Nothing to do for now.
    /// </summary>
    public MapManager()
    {
    }

    /*********************************** Mutators ***********************************/

    /// <summary>
    /// Randomly generate obstacles on the map in the way specified in the specification.
    /// Called by the game logic when the user chooses to start the game from the UI.
    /// </summary>
    public void createObstacles()
    {
        // Create (m,n) for each integer -15 <= m,n <= 15,
        // and exclude the boss area.
        Vector2[] possibleObsPoints = new Vector2[31 * 31];
        List<int> indicesExcludeBossAndPlayer = new List<int>();

        int i = 0;
        for(int m = -15; m <= 15; ++m)
        {
            for(int n = -15; n <= 15; ++n)
            {
                // populate the list
                Vector2 element = new Vector2(m, n);
                possibleObsPoints[i] = element;

                // add the index to the list if it is not on the boss or the player
                var temp = new Vector3(m, 0.0f, n);
                if(isPointNotOverlapWithBoss(temp, 1.0f) && isPointNotOverlapWithPlayerStart(temp, 1.0f))
                {
                    indicesExcludeBossAndPlayer.Add(i);
                }

                // Don't forget this.
                ++i;
            }
        }

        // randomly pick 20 from the indices.
        // the first 5 of the 20 will be high obstacles.
        int[] pickedIndices = new int[20];
        int count = indicesExcludeBossAndPlayer.Count;
        for(int j = 0; j < 20; ++j)
        {
            // pick one from the rest of the array
            int r = UnityEngine.Random.Range(j, count);
            pickedIndices[j] = indicesExcludeBossAndPlayer[r];

            // swap the ith and the picked one so that 
            // it will be as if the ith was picked.
            int temp = indicesExcludeBossAndPlayer[j];
            indicesExcludeBossAndPlayer[j] = indicesExcludeBossAndPlayer[r];
            indicesExcludeBossAndPlayer[r] = temp;
        }

        // The first 5 high obstacles
        for(int j = 0; j < 5; ++j)
        {
            var p = possibleObsPoints[pickedIndices[j]];
            obstacles.Add(GameObject.Instantiate(highObstaclePrefab, new Vector3(p.x, 1.0f, p.y), Quaternion.identity));
        }
        // The rest are short obstacles
        for (int j = 5; j < 20; ++j)
        {
            var p = possibleObsPoints[pickedIndices[j]];
            obstacles.Add(GameObject.Instantiate(shortObstaclePrefab, new Vector3(p.x, .5f, p.y), Quaternion.identity));
        }
    }

    /*********************************** Mono ***********************************/
    /// <summary>
    /// Init the floor, player, and boss.
    /// </summary>
    public void Awake()
    {
        // the surface is centered at the origin and facing up.
        floorSurfacePlane = new Plane(Vector3.up, Vector3.zero);

        // place the floor
        floor = GameObject.Instantiate(floorPrefab, floorCenterPos, Quaternion.identity);
        Utility.MyDebugAssert(floor != null, "assign the prefab in the editor.");

        // place the boss
        boss = GameObject.Instantiate(bossPrefab, bossStart, Quaternion.identity);
        Utility.MyDebugAssert(boss != null, "assign the prefab in the editor.");

        // place the player
        player = GameObject.Instantiate(playerPrefab, playerStart, Quaternion.identity);
        Utility.MyDebugAssert(player != null, "assign the prefab in the editor.");

        // Do not init obstacles here, as in the training ground it is not needed.
    }

    /// <summary>
    /// Nothing to do for now.
    /// </summary>
    public void Update()
    {
        
    }

}
