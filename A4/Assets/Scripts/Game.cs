using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The only object that's put in the scene.
/// Handles everything else.
/// </summary>
public sealed class Game : MonoBehaviour
{
    /*********************************** Fields ***********************************/
    public readonly StateManager stateMgr;
    // assigned in editor
    public CameraManager cameraMgr;
    // assigned in editor
    public MapManager mapMgr;

    // Will always be available before all's ctor
    // (because Game creates all, and in its ctor, the singleton var is assigned first).
    // Game acts as the mediator. All objects talk to it.
    public static Game gameSingleton = null;

    /*********************************** Ctor ***********************************/
    public Game()
    {
        // can only have one instance per game
        Utility.MyDebugAssert(gameSingleton == null);
        gameSingleton = this;

        // Create the managers that don't need prefabs
        stateMgr = new StateManager();

        // all that can't be inited here are inited in Awake().
    }

    /*********************************** Mono ***********************************/

    /// <summary>
    /// Init managers and spawners
    /// </summary>
    private void Awake()
    {
        cameraMgr = GameObject.Instantiate(cameraMgr);
        mapMgr = GameObject.Instantiate(mapMgr);
        Utility.MyDebugAssert(cameraMgr != null, "check this in the editor.");
        Utility.MyDebugAssert(mapMgr != null, "check this in the editor.");
    }
}
