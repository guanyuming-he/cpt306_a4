using Unity.VisualScripting;

/// <summary>
/// A timer is an encapsulation of the commonly implemented logic of:
///     1. having a elapsed time
///     2. elapsed time += delta in Update
///     3. if elapsed time >= some value, then ...
///     4. may reset the elapsed time to 0.0f after that.
///     
/// A timer has three states.
///     -> IDLE.
///     IDLE -- start() --> RUNNING.
///     RUNNING -- time is up --> FIRED.
/// FIRED is the accepting state.
/// 
/// A timer's life ends when FIRED. However, one can reset() 
/// a timer to IDLE at any moment in its life. A reset timer 
/// acts as if it is newly created.
/// 
/// Alternatively, set loop to true, and the timer will automatically call reset() when FIRED.
/// For timers that have loop = true, the only way for others to know it has fired is through the onFire() callback.
/// 
/// </summary>
public class Timer
{
    /*********************************** Static ***********************************/
    // Called when the timer is fired.
    public delegate void onFire();

    public enum State
    {
        // the initial state
        IDLE,
        // transferred from IDLE when start() is called.
        RUNNING,
        // transferred from RUNNING when fired. The accepting state.
        FIRED
    }

    /*********************************** Fields ***********************************/

    private State state;
    private float timeElapsed;

    //// Settings variables. They are readonly once set, hence are public.
    
    // true -> reset() + start() are called IMMEDIATELY (after the callback) when fired.
    // false -> no effect.
    public readonly bool loop;
    // how many second does it take for a started timer to fire?
    public readonly float fireTime;
    // this is called back when the timer fires.
    public readonly onFire onFireCallback;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="fireTime">
    /// how many second it takes for a started timer to fire
    /// </param>
    /// <param name="onFireCallback">
    /// what is called back when the timer fires. can be null if loop = false.
    /// </param>
    /// <param name="loop">
    /// if the timer automatically resets itself to enter the next life cycle when it fires
    /// </param>
    /// 
    /// <exception cref="System.ArgumentException">
    /// 1. if fireTime is less than or equal to 0.0f
    /// 2. if loop = true && onFireCallback = null
    /// </exception>
    public Timer(float fireTime, onFire onFireCallback, bool loop = false)
    {
        if(fireTime <= 0.0f)
        {
            throw new System.ArgumentException("fireTime must be positive.");
        }
        if(loop == true && onFireCallback == null)
        {
            throw new System.ArgumentException("If loop = true, " +
                "then onFireCallback is the only way for someone to know it has fired. Hence, that cannot be null.");
        }

        // The initial state.
        state = State.IDLE;
        timeElapsed = 0.0f;

        // Settings variables.
        this.fireTime = fireTime;
        this.loop = loop;
        this.onFireCallback = onFireCallback;
    }

    /*********************************** Observers ***********************************/

    public float getTimeElapsed() { return timeElapsed; }

    public State getState() { return state; }

    /*********************************** Mutators ***********************************/

    /// <summary>
    /// This is not automatically called.
    /// I must explicitly call the method in some Mono script's Update().
    /// </summary>
    /// <param name="dt"></param>
    public void update(float dt)
    {
        // only update when running.
        if (state != State.RUNNING) return;

        timeElapsed += dt;
        if (timeElapsed >= fireTime)
        {
            state = State.FIRED;
            onFireCallback();

            if (loop)
            {
                reset();
                start();
            }
        }
    }

    /// <summary>
    /// Resets the state of the timer so that
    /// it goes back to the state when it was created.
    /// </summary>
    public void reset()
    {
        state = State.IDLE;
        timeElapsed = 0.0f;
    }
    
    /// <summary>
    /// Starts the timer.
    /// </summary>
    /// <exception cref="System.InvalidOperationException">
    /// if state != IDLE
    /// </exception>
    public void start()
    {
        if (state != State.IDLE)
        {
            throw new System.InvalidOperationException("Can only start a timer when it is IDLE.");
        }

        state = State.RUNNING;
    }
}
