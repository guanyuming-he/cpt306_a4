using TMPro;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Events;
#endif
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// handle the sliders.
/// </summary>
public class OptionsUIScript : MonoBehaviour
{
    Slider[] optionsMenuSliders;
    TMP_Text chDifficultyBtnText;

    private void Start()
    {
// If I want to make the events persistent.
#if PERSISTENT_UI_LISTENERS
        // sliders
        {
            optionsMenuSliders = gameObject.GetComponentsInChildren<Slider>();
            Utility.MyDebugAssert(optionsMenuSliders.Length == 3);

            // master volume slider
            UnityEventTools.AddVoidPersistentListener
            (
                optionsMenuSliders[0].onValueChanged,
                () => AudioManager.masterVolume = optionsMenuSliders[0].value
            );
            // music volume slider
            UnityEventTools.AddVoidPersistentListener
            (
                optionsMenuSliders[1].onValueChanged,
                () => AudioManager.musicVolume = optionsMenuSliders[1].value
            );
            // effects volume slider
            UnityEventTools.AddVoidPersistentListener
            (
                optionsMenuSliders[2].onValueChanged,
                () => AudioManager.effectsVolume = optionsMenuSliders[2].value
            );
        }

        // buttons
        {
            var btns = gameObject.GetComponentsInChildren<Button>();
            Utility.MyDebugAssert(btns.Length == 1);
            // go back button
            UnityEventTools.AddVoidPersistentListener
            (
                btns[0].onClick, 
                () => UIManager.switchMenu(gameObject, Game.gameSingleton.uiMgr.mainMenu)
            );
        }
#else
        // sliders
        {
            optionsMenuSliders = gameObject.GetComponentsInChildren<Slider>();
            Utility.MyDebugAssert(optionsMenuSliders.Length == 3);

            // master volume slider
            optionsMenuSliders[0].onValueChanged.AddListener
            (
                (value) => { AudioManager.masterVolume = value; }
            );

            // music volume slider
            optionsMenuSliders[1].onValueChanged.AddListener
            (
                (value) => { AudioManager.musicVolume = value; }
            );
            // effects volume slider
            optionsMenuSliders[2].onValueChanged.AddListener
            (
                (value) => { AudioManager.effectsVolume = value; }
            );
        }

        // buttons
        {
            var btns = gameObject.GetComponentsInChildren<Button>();
            Utility.MyDebugAssert(btns.Length == 2);

            // ch difficulty btn
            btns[0].onClick.AddListener(() =>
            {
                UIManager.switchMenu(gameObject, Game.gameSingleton.uiMgr.difficultyMenu);
            });
            chDifficultyBtnText = btns[0].gameObject.GetComponentInChildren<TMP_Text>();
            
            // go back button
            btns[1].onClick.AddListener
            (
                () => UIManager.switchMenu(gameObject, Game.gameSingleton.uiMgr.mainMenu)
            );


        }
#endif

    }

    private void Update()
    {
        // change the difficulty text 
        chDifficultyBtnText.text = string.Format
        (
            "Change difficulty.\n Current: {0}",
            Game.gameSingleton.difficulty.ToString()
        );
    }
}