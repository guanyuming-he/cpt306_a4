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
    // assigned in editor
    public UIManager uiMgr;

    // Will always be available before all's ctor
    // (because Game creates all, and in its ctor, the singleton var is assigned first).
    // Game acts as the mediator. All objects talk to it.
    public static Game gameSingleton = null;

    /*********************************** Ctor ***********************************/
    public Game()
    {
        // Create the managers that don't need prefabs
        stateMgr = new StateManager();

        // all that can't be inited here are inited in Awake().
    }

    /*********************************** Mutators ***********************************/
    /// <summary>
    /// When the user selects "start" on the main UI
    /// </summary>
    public void startFullGame()
    {
        mapMgr.enterFullGame();
        stateMgr.startGame();
        uiMgr.onGameStart();
    }

    /// <summary>
    /// When the user selects "training" on the main UI
    /// </summary>
    public void startTrainingGround()
    {
        mapMgr.enterTrainingGround();
        stateMgr.startGame();
        uiMgr.onGameStart();
    }

    /// <summary>
    /// When the user selects "exit" on the main UI
    /// </summary>
    public void exitGame()
    {
        // Destroy all the Mono managers
        GameObject.Destroy(uiMgr);
        GameObject.Destroy(cameraMgr);
        GameObject.Destroy(mapMgr);

        // Destroy myself
        GameObject.Destroy(gameObject);

        // exit the game
        Application.Quit();   
    }

    /// <summary>
    /// Transition between the game over menu to the main menu.
    /// </summary>
    public void goHome()
    {
        stateMgr.goHome();
        mapMgr.clear();
        UIManager.switchMenu(uiMgr.gameOverMenu, uiMgr.mainMenu);
    }

    /*********************************** Mono ***********************************/

    /// <summary>
    /// Init managers and spawners
    /// </summary>
    private void Awake()
    {
        // can only have one instance per game
        Utility.MyDebugAssert(gameSingleton == null, "you should only put one instance per scene.");
        gameSingleton = this;

        cameraMgr = GameObject.Instantiate(cameraMgr);
        mapMgr = GameObject.Instantiate(mapMgr);
        uiMgr = GameObject.Instantiate(uiMgr);
        Utility.MyDebugAssert(cameraMgr != null, "check this in the editor.");
        Utility.MyDebugAssert(mapMgr != null, "check this in the editor.");
        Utility.MyDebugAssert(uiMgr != null, "check this in the editor.");
    }

    /// <summary>
    /// Init the game by bringing up the initial objects
    /// </summary>
    private void Start()
    {
        uiMgr.mainMenu.SetActive(true);
    }
}
