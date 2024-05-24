using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Playables;

using UnityEngine.SocialPlatforms.Impl;

/// <summary>
/// The state manager manages the state of the game.
/// It is a singleton class.
/// 
/// Its various methods provide means of state transitions
/// and ways to change the game's states.
/// It also handles saving and loading the game states to/from a local save file.
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

        playerOwnedSkills = new HashSet<int>();
        playerPreparedSkills = new HashSet<int>();

        // load the game from save file.
        loadStateFromFile();
    }

    /*********************************** FIELDS ***********************************/

    private States state;
    private uint numSkillCoins;
    // set of indices into playerSkills.
    public HashSet<int> playerOwnedSkills;
    // sync this to the player when the game starts
    public HashSet<int> playerPreparedSkills;

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

    /// <param name="skillInd"></param>
    /// <returns>true iff player has the skill</returns>
    public bool hasPlayerSkill(int skillInd)
    {
        return playerOwnedSkills.Contains(skillInd);
    }

    /// <summary>
    /// Purchases the skill for the player.
    /// </summary>
    /// <param name="skillInd">index into the playerSkills</param>
    /// <returns>
    /// 1. true if the purchase is successful or if the skill was already owned.
    /// 2. false if the purchase failed (because the number of skill coins is insufficient).
    /// </returns>
    public bool buySkill(int skillInd)
    {
        if(hasPlayerSkill(skillInd))
        {
            // already has the skill
            return true;
        }

        var skill = Game.gameSingleton.playerSkills[skillInd];
        if (useSkillCoins(skill.skillData.cost))
        // can purchase
        {
#if UNITY_EDITOR
            UnityEngine.Debug.Log(String.Format("Purchased skill {0}.", skillInd));
#endif
            playerOwnedSkills.Add(skillInd);
            return true;
        }
        else
        // cannot purchase
        {
            return false;
        }
    }

    /// <summary>
    /// Called in the skill menu
    /// </summary>
    /// <param name="skillInd"></param>
    public void prepareSkill(int skillInd)
    {
        Utility.MyDebugAssert(hasPlayerSkill(skillInd), "player is preparing a skill he has not learnt. Fix this.");
        
        // only 4 can be prepared.
        // 1. if it is already prepared, then do nothing.
        if(playerPreparedSkills.Contains(skillInd))
        {
            return;
        }
        // 2. not prepared.
        // 2.1 if less than 4 are prepared, then just add it.
        if(playerPreparedSkills.Count < 4)
        {
            playerPreparedSkills.Add(skillInd);
        }
        // 2.2 otherwise, remove the first one, and add it.
        else
        {
            playerPreparedSkills.Remove(playerPreparedSkills.First());
            playerPreparedSkills.Add(skillInd);
        }
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
                // skill coins (a line that contains the number)
                {
                    string numCoinsLine = sr.ReadLine();
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

                // owned skills (a line, indices separated by ,)
                {
                    string ownedSkillsLine = sr.ReadLine();
                    string[] ownedIndexStrings = ownedSkillsLine.Split(',');
                    foreach(string ownedIndexStr in ownedIndexStrings)
                    {
                        try
                        {
                            int ownedInd = int.Parse(ownedIndexStr);
                            playerOwnedSkills.Add(ownedInd);
                        }
                        catch (Exception)
                        {
                            // The save file is corrupted.
                            // do nothing
                        }
                    }
                }

                // prepared skills (a line, indices separated by ,)
                {
                    string preparedSkillsLine = sr.ReadLine();
                    string[] preparedIndexStrings = preparedSkillsLine.Split(',');
                    foreach (string preparedIndexStr in preparedIndexStrings)
                    {
                        try
                        {
                            int preparedInd = int.Parse(preparedIndexStr);
                            playerPreparedSkills.Add(preparedInd);
                        }
                        catch (Exception)
                        {
                            // The save file is corrupted.
                            // do nothing
                        }
                    }
                }
            }
        }
        // Otherwise, load the default state.
        else
        {
            numSkillCoins = 0;
            playerOwnedSkills = new HashSet<int>();
            playerPreparedSkills = new HashSet<int>();
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
            // skill coins line
            sw.WriteLine(numCoinsLine);
            // owned skills line
            string ownedSkillsLine = "";
            foreach(int i in playerOwnedSkills)
            {
                ownedSkillsLine += string.Format("{0},", i);
            }
            sw.WriteLine(ownedSkillsLine);
            // prepared skills line
            string preparedSkillsLine = "";
            foreach (int i in playerPreparedSkills)
            {
                preparedSkillsLine += string.Format("{0},", i);
            }
            sw.WriteLine(preparedSkillsLine);
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
    /// When P, and the player played the Exit key.
    /// </summary>
    public void exitGame()
    {
        Utility.MyDebugAssert(state == States.PLAYING);

        state = States.MAIN_UI;
        saveStateToFile();
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
        saveStateToFile();
    }

}
