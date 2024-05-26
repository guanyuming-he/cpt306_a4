//#define PERSISTENT_UI_LISTENERS

#if PERSISTENT_UI_LISTENERS
using UnityEditor.Events;
#endif
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    /*********************************** Static helpers ***********************************/


    /// <summary>
    /// At one time, there should only be one menu active.
    /// This helper hides the old menu and shows the new menu.
    /// </summary>
    /// <param name="oldMenu">the menu to be hidden</param>
    /// <param name="newMenu">the menu to be shown</param>
    public static void switchMenu(GameObject oldMenu, GameObject newMenu)
    {
        Utility.MyDebugAssert(oldMenu.activeSelf == true, "the old menu should be active.");
        Utility.MyDebugAssert(newMenu.activeSelf == false, "the new menu should be inactive.");

        oldMenu.SetActive(false);
        newMenu.SetActive(true);
    }

    /*********************************** Fields ***********************************/
    public GameObject mainMenu;
    public GameObject gameOverMenu;
    public GameObject inGameMenu;
    public GameObject optionsMenu;
    public GameObject skillsMenu;
    public GameObject difficultyMenu;

    /*********************************** Private helpers ***********************************/
    /// <summary>
    /// Spawn all UI from the prefabs.
    /// </summary>
    private void spawnAllUI()
    {
        this.mainMenu = GameObject.Instantiate(mainMenu);
        this.gameOverMenu = GameObject.Instantiate(gameOverMenu);
        this.inGameMenu = GameObject.Instantiate(inGameMenu);
        this.optionsMenu = GameObject.Instantiate(optionsMenu);
        this.skillsMenu = GameObject.Instantiate(skillsMenu);
        this.difficultyMenu = GameObject.Instantiate(difficultyMenu);

        Utility.MyDebugAssert(mainMenu != null, "check UIManager prefabs");
        Utility.MyDebugAssert(gameOverMenu != null, "check UIManager prefabs");
        Utility.MyDebugAssert(inGameMenu != null, "check UIManager prefabs");
        Utility.MyDebugAssert(optionsMenu != null, "check UIManager prefabs");
        Utility.MyDebugAssert(skillsMenu != null, "check UIManager prefabs");
        Utility.MyDebugAssert(difficultyMenu != null, "check UIManager prefabs");
    }

    /// <summary>
    /// Prepare the event callbacks for UI interactions
    /// (e.g. button clicks)s
    /// </summary>
    private void setUpUIEvents()
    {
        // https://docs.unity3d.com/Manual/InspectorOptions.html#reordering-components
        // states that
        // The component order you apply in the Inspector is
        // the same order that you need to use when you query components in your scripts.

// If I want to make the events persistent.
#if PERSISTENT_UI_LISTENERS
        // main menu
        {
            var btns = mainMenu.GetComponentsInChildren<Button>();

            // start btn
            UnityEventTools.AddVoidPersistentListener(btns[0].onClick, () => { Game.gameSingleton.startFullGame(); });
            // training btn
            UnityEventTools.AddVoidPersistentListener(btns[1].onClick, () => { Game.gameSingleton.startTrainingGround(); });
            // options btn
            UnityEventTools.AddVoidPersistentListener(btns[3].onClick, () => { switchMenu(mainMenu, optionsMenu); });
            // exit btn
            UnityEventTools.AddVoidPersistentListener(btns[4].onClick, () => { Game.gameSingleton.exitGame(); });
        }

        // in game menu.
        {
            // nothing. it is controlled by a dedicated script.
        }

        // options menu
        {
            // nothing. it is controlled by a dedicated script.
        }

        // game over menu
        {
            var btns = gameOverMenu.GetComponentsInChildren<Button>();
            // go back button
            UnityEventTools.AddVoidPersistentListener(btns[0].onClick, () => Game.gameSingleton.goHome());
        }

#else // outside of the editor, use xxx.AddListener
        // main menu
        {
            var btns = mainMenu.GetComponentsInChildren<Button>();

            // start btn
            btns[0].onClick.AddListener(() => { Game.gameSingleton.startFullGame(); });
            // training btn
            btns[1].onClick.AddListener(() => { Game.gameSingleton.startTrainingGround(); });
            // skills btn
            btns[2].onClick.AddListener(() => { switchMenu(mainMenu, skillsMenu); });
            // options btn
            btns[3].onClick.AddListener(() => { switchMenu(mainMenu, optionsMenu); });
            // exit btn
            btns[4].onClick.AddListener(() => { Game.gameSingleton.exitGame(); });
        }

        // in game menu.
        {
            // nothing. it is controlled by a dedicated script.
        }

        // options menu
        {
            // nothing. it is controlled by a dedicated script.
        }

        // game over menu
        {
            var btns = gameOverMenu.GetComponentsInChildren<Button>();
            // go back button
            btns[0].onClick.AddListener(() => Game.gameSingleton.goHome());
        }

        // skills menu
        {
            // nothing. it is controlled by a dedicated script.
        }

        // difficulty menu
        {
            var btns = difficultyMenu.GetComponentsInChildren<Button>();

            // easy
            btns[0].onClick.AddListener(() =>
            {
                Game.gameSingleton.difficulty = Game.Difficulty.EASY;
                switchMenu(difficultyMenu, optionsMenu);
            });
            // normal
            btns[1].onClick.AddListener(() =>
            {
                Game.gameSingleton.difficulty = Game.Difficulty.NORMAL;
                switchMenu(difficultyMenu, optionsMenu);
            });
        }
#endif
    }

    /*********************************** Mutators (, which change UI states) ***********************************/

    /// <summary>
    /// Hide all except the in game menu.
    /// </summary>
    public void hideAllExceptInGameUI()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        skillsMenu.SetActive(false);
        difficultyMenu.SetActive(false);
        // inGameMenu.SetActive(false);
    }

    /// <summary>
    /// Hide all menus.
    /// </summary>
    public void hideAllUI()
    {
        hideAllExceptInGameUI();
        inGameMenu.SetActive(false);
    }

    /// <summary>
    /// Hide the main UI and show the in game UI.
    /// </summary>
    public void onGameStart()
    {
        switchMenu(mainMenu, inGameMenu);

        // init the in game menu
        InGameUIScript inGameUIScript = inGameMenu.GetComponent<InGameUIScript>();
        inGameUIScript.init();

        // populate the skill icons.
        inGameUIScript.populateSkillIcons();
    }

    /// <summary>
    /// When either the boss or the player has died.
    /// </summary>
    /// <param name="whoDied">
    /// true: player
    /// false: boss
    /// </param>
    public void onGameOver(bool whoDied)
    {
        // set the title accordingly.
        TMP_Text title = gameOverMenu.GetComponentInChildren<TMP_Text>();
        Utility.MyDebugAssert(title != null, "should have a title.");
        if (whoDied)
        // the player died.
        {
            title.text = "You Lost.";
        }
        else
        // the boss died.
        {
            title.text = "You Won.";
        }

        // switch the menu
        switchMenu(inGameMenu, gameOverMenu);
    }

    /*********************************** Mono ***********************************/
    
    /// <summary>
    /// Spawn and hide all UIs
    /// </summary>
    private void Awake()
    {
        spawnAllUI();
        hideAllUI();
    }

    /// <summary>
    /// Set up UI events
    /// </summary>
    void Start()
    {
        setUpUIEvents();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        GameObject.Destroy(mainMenu);
        GameObject.Destroy(optionsMenu);
        GameObject.Destroy(skillsMenu);
        GameObject.Destroy(optionsMenu);
        GameObject.Destroy(inGameMenu);
    }
}
