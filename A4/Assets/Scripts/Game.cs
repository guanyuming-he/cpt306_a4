using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The only object that's put in the scene.
/// Handles everything else.
/// </summary>
public sealed class Game : MonoBehaviour
{
    /*********************************** Statis ***********************************/
    public enum Difficulty
    {
        EASY = 0,
        NORMAL = 1,
    }

    /*********************************** Fields ***********************************/
    public Difficulty difficulty = Difficulty.EASY;

    /// <summary>
    /// Managers
    /// </summary>
    public readonly StateManager stateMgr;
    // assigned in editor
    public CameraManager cameraMgr;
    // assigned in editor
    public MapManager mapMgr;
    // assigned in editor
    public UIManager uiMgr;

    /// <summary>
    /// Skills
    /// </summary>
    public List<ConcreteSkill> playerSkills;
    public List<ConcreteSkill> bossSkills;

    /// <summary>
    /// Audio
    /// </summary>
    AudioSource bgMusic;

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
    /// or when the user presses the exit key during play
    /// </summary>
    public void exitGame()
    {
        // save the game
        stateMgr.saveStateToFile();

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
    /// When either the boss or the player has died.
    /// </summary>
    /// <param name="whoDied">
    /// true: player
    /// false: boss
    /// </param>
    public void gameOver(bool whoDied)
    {
        // UI
        uiMgr.onGameOver(whoDied);

        if(whoDied)
        // player death
        {
            stateMgr.gameOver();
        }
        else
        // boss death
        {
            stateMgr.win();
        }
    }

    /// <summary>
    /// Transition between the game over menu to the main menu.
    /// </summary>
    public void goHome()
    {
        stateMgr.goHome();

        // clear previous game objects and states
        mapMgr.clear();

        // reset the skill cooldowns
        {
            foreach (var s in playerSkills)
            {
                s.resetCooldown();
            }
            foreach (var s in bossSkills)
            {
                s.resetCooldown();
            }
        }
        
        UIManager.switchMenu(uiMgr.gameOverMenu, uiMgr.mainMenu);
    }

    /*********************************** Private helpers ***********************************/

    /// <summary>
    /// Skill prefabs must be instantiated before they can be used.
    /// Hence this method.
    /// </summary>
    private void initSkills()
    {
        // Can't use a prefab unless I instantiate them.
        for(int i = 0; i < playerSkills.Count; ++i)
        {
            playerSkills[i] = GameObject.Instantiate(playerSkills[i]);
            bossSkills[i] = GameObject.Instantiate(bossSkills[i]);
        }
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
        bgMusic = GetComponent<AudioSource>();
        Utility.MyDebugAssert(bgMusic != null, "should have the bg music.");
        AudioManager.playMusic(bgMusic);

        initSkills();
        uiMgr.mainMenu.SetActive(true);
    }

    /// <summary>
    /// 1. See if the player presses the exit key during play
    /// 2. bg music volume
    /// </summary>
    private void Update()
    {
        // 1.
        if (stateMgr.getState() == StateManager.States.PLAYING)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                exitGame();
            }
        }

        // 2.
        bgMusic.volume = AudioManager.musicStrength();
    }
}
