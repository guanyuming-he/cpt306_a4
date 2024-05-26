using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Manages the floor and everything on it.
/// </summary>
public class MapManager : MonoBehaviour
{
    /*********************************** Static Settings ***********************************/
    // Spawn heights
    public const float BOSS_SPAWN_H = .5f * BossEntity.BOSS_WIDTH;
    public const float PLAYER_SPAWN_H = .5f * PlayerEntity.PLAYER_HEIGHT;
    public const float LOW_OBS_SPAWN_H = .5f * Obstacle.OBS_LENGTH;
    public const float HIGH_OBS_SPAWN_H = .5f * Obstacle.HIGH_OBS_HEIGHT;
    public const float SKILL_COIN_SPAWN_H = .5f * SkillCoinMovement.SKILL_COIN_WIDTH;

    // width is defined in the specification.
    public const float mapWidth = 31.0f;
    // these are defined by myself
    public const float mapHeight = 1.0f;
    // Designed in such a way that the center of the top surface is the origin.
    public static readonly Vector3 floorCenterPos =
        new Vector3(.0f, -.5f * mapHeight, .0f);
    public static readonly Vector3 playerStart =
        new Vector3(0.0f, PLAYER_SPAWN_H, -12.0f);
    public static readonly Vector3 bossStart =
        new Vector3(.0f, BOSS_SPAWN_H, .0f);

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
            p.x < -BossEntity.BOSS_WIDTH - .5f * width || p.x > BossEntity.BOSS_WIDTH + .5f * width ||
            p.z < -BossEntity.BOSS_WIDTH - .5f * width || p.z > BossEntity.BOSS_WIDTH + .5f * width;
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
    // Track them just from removal. So I don't remove picked up coins from the list.
    public List<GameObject> skillCoins;

    //// These are public because they are prefabs assigned in the editor.
    public GameObject floorPrefab;
    public GameObject playerPrefab;
    public GameObject bossPrefab;
    public GameObject shortObstaclePrefab;
    public GameObject highObstaclePrefab;
    public GameObject skillCoinPrefab;

    // the plane of the up surface
    public Plane floorSurfacePlane;
    
    //// Needed for generation of obstacle and other objects.
    
    // Will randomly choose from these points.
    private Vector2[] possibleObsPoints;
    // each generate chooses one random index from the rest of the list
    // and swap it to the front.
    private List<int> indicesExcludeBossAndPlayer;
    // number of indices that have been randomly chosen.
    private uint numGeneratedIndices;
    // those that are left in the queue are unused.
    // pop the one as you use it.
    private Queue<int> generatedIndices;

    // The specification says that I can only generate at most 3 per game.
    private uint numGeneratedSkillCoins;

    /*********************************** Ctor ***********************************/
    /// <summary>
    /// Fill in the object generation data structure
    /// </summary>
    public MapManager()
    {
        // Create (m,n) for each integer -15 <= m,n <= 15,
        // and exclude the boss and player area.
        possibleObsPoints = new Vector2[31 * 31];
        indicesExcludeBossAndPlayer = new List<int>();
        numGeneratedIndices = 0;
        generatedIndices = new Queue<int>();

        int i = 0;
        for (int m = -15; m <= 15; ++m)
        {
            for (int n = -15; n <= 15; ++n)
            {
                // populate the list
                Vector2 element = new Vector2(m, n);
                possibleObsPoints[i] = element;

                // add the index to the list if it is not on the boss or the player
                var temp = new Vector3(m, 0.0f, n);
                if (isPointNotOverlapWithBoss(temp, 1.0f) && isPointNotOverlapWithPlayerStart(temp, 1.0f))
                {
                    indicesExcludeBossAndPlayer.Add(i);
                }

                // Don't forget this.
                ++i;
            }
        }

        numGeneratedSkillCoins = 0;
    }

    /*********************************** Private Helpers ***********************************/

    /// <summary>
    /// Randomly choose one from [numGeneratedIndices, size) in indicesExcludeBossAndPlayer
    /// and swap the chosen one with the one at numGeneratedIndices.
    /// Put the chosen one in the queue.
    /// Then, advance numGeneratedIndices.
    /// 
    /// The chosen one is an index into possibleObsPoints.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// if numGeneratedIndices >= size
    /// </exception>
    private void generateNext()
    {
        // convert it to int first as List.Count is int, not uint (Why?)
        int i = (int)numGeneratedIndices;
        Utility.MyDebugAssert(i >= 0, "bad logical error happened.");

        int count = indicesExcludeBossAndPlayer.Count;
        if (i >= indicesExcludeBossAndPlayer.Count)
        {
            throw new InvalidOperationException("Already used all indices.");
        }

        // pick one from the rest of the array,
        // that is, [numGeneratedIndices, size) in indicesExcludeBossAndPlayer
        int r = UnityEngine.Random.Range(i, count);
        int chosenIndex = indicesExcludeBossAndPlayer[r];
        // swap the picked one with the one at numGeneratedIndices
        // it will be as if the latter was picked.
        indicesExcludeBossAndPlayer[r] = indicesExcludeBossAndPlayer[i];
        indicesExcludeBossAndPlayer[i] = chosenIndex;

        // put the chosen on in the queue
        generatedIndices.Enqueue(chosenIndex);
        // and don't forget to advance numGeneratedIndices
        ++numGeneratedIndices;
    }

    /// <returns>
    /// the next generated indices (first in the queue) if the queue is not empty.
    /// Asserts that the queue is not empty.
    /// </returns>
    private int getNextGenerated()
    {
        Utility.MyDebugAssert(generatedIndices.Count != 0, "Generate before use!");
        return generatedIndices.Dequeue();
    }

    /// <summary>
    /// Randomly generate obstacles on the map in the way specified in the specification.
    /// Called by the game logic when the user chooses to start the game 
    /// (not the training ground) from the UI.
    /// 
    /// My masterful modularity made it possible to call this and any other generation methods
    /// in ANY order, so long as they are not called simultaneously in different threads.
    /// </summary>
    private void createObstacles()
    {
        // generate 20 random indices into possibleObsPoints
        for (int j = 0; j < 20; ++j)
        {
            try
            {
                generateNext();
            }
            catch(InvalidOperationException)
            {
                Utility.MyDebugAssert(false, "I have used all places. This should not happen.");
            }
        }

        // The first 5 high obstacles
        for(int j = 0; j < 5; ++j)
        {
            var p = possibleObsPoints[getNextGenerated()];
            obstacles.Add
            (
                GameObject.Instantiate(highObstaclePrefab, 
                new Vector3(p.x, HIGH_OBS_SPAWN_H, p.y), Quaternion.identity)
            );
        }
        // The rest are short obstacles
        for (int j = 5; j < 20; ++j)
        {
            var p = possibleObsPoints[getNextGenerated()];
            obstacles.Add
            (
                GameObject.Instantiate(shortObstaclePrefab, new Vector3(p.x, LOW_OBS_SPAWN_H, p.y), 
                Quaternion.identity)
            );
        }
    }

    /// <summary>
    /// Sets up the player when the game starts.
    /// This is used twice --- once for game start, and the other for training ground.
    /// </summary>
    private void setUpPlayer()
    {
        // skills
        {
            PlayerSkills ps = player.GetComponent<PlayerSkills>();
            Utility.MyDebugAssert(ps != null, "did I forget to attach this script?");
            ps.prepareSkillsFromIndices(Game.gameSingleton.stateMgr.playerPreparedSkills);
        }
    }

    /// <summary>
    /// Sets up the boss
    /// This is used twice --- once for game start, and the other for training ground.
    /// </summary>
    /// <param name="fullGame">
    /// ture: full game.
    /// false: training ground.
    /// </param>
    private void setUpBoss(bool fullGame)
    {
        // physical body
        {
            BossEntity bossEntity = boss.GetComponent<BossEntity>();
            Utility.MyDebugAssert(bossEntity != null, "did I forget to attach this script?");
            if(fullGame)
            {
                bossEntity.onEnterGame();
            }
            else
            {
                bossEntity.onEnterTraningGround();
            }
        }

        // skills
        {
            BossSkills bs = boss.GetComponent<BossSkills>();
            Utility.MyDebugAssert(bs != null, "did I forget to attach this script?");

            // prepare the boss's skills
            if(fullGame)
            {
                bs.prepareSkillsOnGameStart((int)Game.gameSingleton.difficulty);
            }
            else
            // training ground
            {
                bs.prepareSkillsOnGameStart(-1);
            }
        }
    }

    /// <summary>
    /// Init the basic elements (floor, player, boss) on the map.
    /// </summary>
    private void init()
    {
        // place the floor
        floor = GameObject.Instantiate(floorPrefab, floorCenterPos, Quaternion.identity);
        Utility.MyDebugAssert(floor != null, "assign the prefab in the editor.");

        // place the boss
        boss = GameObject.Instantiate(bossPrefab, bossStart, Quaternion.identity);
        Utility.MyDebugAssert(boss != null, "assign the prefab in the editor.");

        // place the player
        player = GameObject.Instantiate(playerPrefab, playerStart, Quaternion.identity);
        Utility.MyDebugAssert(player != null, "assign the prefab in the editor.");
    }

    /*********************************** Mutators ***********************************/

    /// <summary>
    /// Randomly generate a skill coin specified by the specification.
    /// Called by the game logic 
    ///     1. when the user chooses to start the game (not the training ground) from the UI.
    ///     2. when a skill coin is picked up.
    /// 
    /// My masterful modularity made it possible to call this and any other generation methods
    /// in ANY order, so long as they are not called simultaneously in different threads.
    /// </summary>
    public void createSkillCoin()
    {
        // can generate at most 3
        if (numGeneratedSkillCoins >= 3)
        {
            return;
        }

        // generate 1 random indices into possibleObsPoints
        try
        {
            generateNext();
        }
        catch (InvalidOperationException)
        {
            Utility.MyDebugAssert(false, "I have used all places. This should not happen.");
        }

        // instantiate the skill coin at the location
        var p = possibleObsPoints[getNextGenerated()];
        var coin = GameObject.Instantiate
        (
            skillCoinPrefab,
            new Vector3(p.x, SKILL_COIN_SPAWN_H, p.y), Quaternion.identity
        );
        skillCoins.Add(coin);

        // Don't forget to do this.
        ++numGeneratedSkillCoins;
    }

    /// <summary>
    /// Called by the game logic when the user chooses to start the the training ground
    /// from the UI.
    /// </summary>
    public void enterTrainingGround()
    {
        init();

        // set up the boss
        setUpBoss(false);

        // set up the player
        setUpPlayer();
    }

    /// <summary>
    /// Called by the game logic when the user chooses to start the game 
    /// (not the training ground) from the UI.
    /// </summary>
    public void enterFullGame()
    {
        init();

        // the objects used in the full game.
        createObstacles();
        createSkillCoin();

        // set up the boss
        setUpBoss(true);

        // set up the player
        setUpPlayer();
    }

    /// <summary>
    /// Destroys everything on the map, including the floor.
    /// </summary>
    public void clear()
    {
        GameObject.Destroy(player);
        GameObject.Destroy(boss);

        foreach (var obs in obstacles)
        {
            GameObject.Destroy(obs);
        }
        obstacles.Clear();
        foreach(var coin in skillCoins)
        {
            GameObject.Destroy(coin);
        }
        skillCoins.Clear();

        GameObject.Destroy(floor);
    }

    /*********************************** Mono ***********************************/
    /// <summary>
    /// Init the what cannot be done in the ctor
    /// </summary>
    private void Awake()
    {
        // the surface is centered at the origin and facing up.
        floorSurfacePlane = new Plane(Vector3.up, Vector3.zero);

        // Do not init anything here, as they are controlled by other methods.
        //init();
        //createObstacles();
        //createSkillCoin();
    }

    /// <summary>
    /// Nothing to do for now.
    /// </summary>
    private void Update()
    {
        
    }

    private void OnDestroy()
    {
        clear();
    }
}
