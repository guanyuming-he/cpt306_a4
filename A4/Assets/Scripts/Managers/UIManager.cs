using UnityEditor.Callbacks;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    /*********************************** Fields ***********************************/
    public GameObject mainMenu;
    public GameObject gameOverMenu;
    public GameObject inGameMenu;
    public GameObject optionsMenu;

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

        Utility.MyDebugAssert(mainMenu != null, "check UIManager prefabs");
        Utility.MyDebugAssert(gameOverMenu != null, "check UIManager prefabs");
        Utility.MyDebugAssert(inGameMenu != null, "check UIManager prefabs");
        Utility.MyDebugAssert(optionsMenu != null, "check UIManager prefabs");
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

        // Dynamic event binding isn't visible in the editor.
        // To make them visible, in the editor, use UnityEventTools.AddVoidPersistentListener
        #if UNITY_EDITOR

        // outside of the editor, use xxx.AddListener
        #else

        #endif
    }

    /*********************************** Mutators (, which change UI states) ***********************************/

    /// <summary>
    /// At one time, there should only be one menu active.
    /// This helper hides the old menu and shows the new menu.
    /// </summary>
    /// <param name="oldMenu">the menu to be hidden</param>
    /// <param name="newMenu">the menu to be shown</param>
    public void switchMenu(GameObject oldMenu, GameObject newMenu)
    {
        Utility.MyDebugAssert(oldMenu.activeSelf == true, "the old menu should be active.");
        Utility.MyDebugAssert(newMenu.activeSelf == false, "the new menu should be inactive.");

        oldMenu.SetActive(false);
        newMenu.SetActive(true);
    }

    /// <summary>
    /// Hide all except the in game menu.
    /// </summary>
    public void hideAllExceptInGameUI()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(false);
        gameOverMenu.SetActive(false);
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

    /*********************************** Mono ***********************************/

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
