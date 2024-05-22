using System;
using System.IO;
using UnityEditor.Playables;

using UnityEngine.SocialPlatforms.Impl;

/// <summary>
/// The state manager manages the state of the game.
/// It is a singleton class.
/// 
/// Its various methods provide means of state transitions.
/// </summary>
public class StateManager
{
    /*********************************** Static ***********************************/
    /// <summary>
    /// States of the game
    /// </summary>
    public enum States
    {
        // when the game is at the main UI
        MAIN_UI,
        // when the game is playing
        PLAYING,
        // when the player has won (the boss is dead)
        VICTORY,
        // when the game is over (the player is dead)
        GAME_OVER
    }

    // Path to the file the records the game's state.
    public const String STATE_FILE_PATH = "game_state.txt";
    // Format:
    // line 1: number of skill coins.

    /*********************************** CTOR ***********************************/
    public StateManager()
    {
        state = States.MAIN_UI;
    }

    /*********************************** FIELDS ***********************************/

    private States state;
    private uint numSkillCoins;

    /*********************************** METHODS ***********************************/

    public States getState() { return state; }

    public uint getNumSkillCoins() { return numSkillCoins; }

    /// <summary>
    /// Call this to use some of the skill coins.
    /// </summary>
    /// <param name="num">
    /// number of skill coins to use. 
    /// </param>
    /// <returns>
    /// true if num is less than or equal to the actual number, and the use succeeds.
    /// false if num is bigger than the actual number.
    /// </returns>
    public bool useSkillCoins(uint num)
    {
        if (num > numSkillCoins)
        {
            return false;
        }

        numSkillCoins -= num;

#if UNITY_EDITOR
        UnityEngine.Debug.Log(String.Format("Used {0} skill coin(s), and {1} remains.", num, numSkillCoins));
#endif

        return true;
    }

    /// <summary>
    /// Called when the player picks up some skill coins.
    /// </summary>
    /// <param name="num">number of coins picked up.</param>
    public void pickUpSkillCoins(uint num)
    {
        numSkillCoins += num;

#if UNITY_EDITOR
        UnityEngine.Debug.Log(String.Format("Picked up {0} skill coin(s), and now we have {1}.", num, numSkillCoins));
#endif
    }

    /// <summary>
    /// Called when the game starts to load the state from the file.
    /// </summary>
    public void loadStateFromFile()
    {
        // If the file exists, then load the state from the file.
        if (File.Exists(STATE_FILE_PATH))
        {
            using (StreamReader sr = File.OpenText(STATE_FILE_PATH))
            {
                String numCoinsLine = sr.ReadLine();
                try
                {
                    numSkillCoins = uint.Parse(numCoinsLine);
                }
                catch (Exception)
                {
                    // The save file is corrupted.
                    numSkillCoins = 0;
                }
            }
        }
        // Otherwise, load the default state.
        else
        {
            numSkillCoins = 0;
        }
    }

    /// <summary>
    /// Called when the game terminates to save the state into the file.
    /// </summary>
    public void saveStateToFile()
    {
        String numCoinsLine = String.Format("{0}", numSkillCoins);
        // Creates or opens a file for writing UTF-8 encoded text. If the file already exists, its contents are replaced.
        using (StreamWriter sw = File.CreateText(STATE_FILE_PATH))
        {
            sw.WriteLine(numCoinsLine);
        }
    }

    /*********************************** STATE TRANSITIONS ***********************************/
    // states are abbreviated in the following comments.

    /// <summary>
    /// From MU to P
    /// </summary>
    public void startGame()
    {
        Utility.MyDebugAssert(state == States.MAIN_UI);

        state = States.PLAYING;
    }

    /// <summary>
    /// When P, and the player played the Exit button.
    /// </summary>
    public void exitGame()
    {
        Utility.MyDebugAssert(state == States.PLAYING);

        state = States.MAIN_UI;
    }

    /// <summary>
    /// When the boss has died
    /// </summary>
    public void win()
    {
        Utility.MyDebugAssert(state == States.PLAYING);

        state = States.VICTORY;
    }

    /// <summary>
    /// When the player has died
    /// </summary>
    public void gameOver()
    {
        Utility.MyDebugAssert(state == States.PLAYING);

        state = States.GAME_OVER;
    }

    /// <summary>
    /// Return to main ui from the victory/lost menu
    /// </summary>
    public void goHome()
    {
        Utility.MyDebugAssert(state == States.VICTORY || state == States.GAME_OVER);

        state = States.MAIN_UI;
    }

}
