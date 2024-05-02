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
    /*********************************** STATES ***********************************/
    /// <summary>
    /// States of the game
    /// </summary>
    public enum State
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

    /*********************************** CTOR ***********************************/
    public StateManager()
    {
        state = State.MAIN_UI;
    }

    /*********************************** FIELDS ***********************************/

    private State state;

    /*********************************** STATE TRANSITIONS ***********************************/
    // states are abbreviated in the following comments.

    /// <summary>
    /// From MU to P
    /// </summary>
    public void startGame()
    {
        Utility.MyDebugAssert(state == State.MAIN_UI);

        state = State.PLAYING;
    }

    /// <summary>
    /// When P, and the player played the Exit button.
    /// </summary>
    public void exitGame()
    {
        Utility.MyDebugAssert(state == State.PLAYING);

        state = State.MAIN_UI;
    }

    /// <summary>
    /// When the boss has died
    /// </summary>
    public void win()
    {
        Utility.MyDebugAssert(state == State.PLAYING);

        state = State.VICTORY;
    }

    /// <summary>
    /// When the player has died
    /// </summary>
    public void gameOver()
    {
        Utility.MyDebugAssert(state == State.PLAYING);

        state = State.GAME_OVER;
    }

    /// <summary>
    /// Return to main ui from the victory/lost menu
    /// </summary>
    public void goHome()
    {
        Utility.MyDebugAssert(state == State.VICTORY || state == State.GAME_OVER);

        state = State.MAIN_UI;
    }

}
