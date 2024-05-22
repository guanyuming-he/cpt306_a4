
/// <summary>
/// Represents the damageable and physical part of the boss.
/// </summary>
public class BossEntity : DamageableEntity
{
    /*********************************** Static ***********************************/
    // decided by myself so long as it's no bigger than 5.0f
    public const float BOSS_WIDTH = 4.0f;

    /*********************************** Fields ***********************************/
    // Set in the editor
    public float gameHealth;
    public float trainingGroundHealth;

    /*********************************** From Entity ***********************************/

    public override Side getSide()
    {
        return Side.BOSS;
    }

    /*********************************** From DamageableEntity ***********************************/

    public override void onDeath()
    {
        throw new System.NotImplementedException();
    }

    /*********************************** Mutators ***********************************/
    /// <summary>
    /// Called by map manager
    /// </summary>
    public void onEnterGame()
    {
        setInitialHealth(gameHealth);

        throw new System.NotImplementedException("TODO: set up skills.");
    }

    /// <summary>
    /// Called by map manager
    /// </summary>
    public void onEnterTraningGround()
    {
        setInitialHealth(trainingGroundHealth);
        // no skills in the training ground.
    }

    /*********************************** Mono ***********************************/
    protected override void Awake()
    {
        // Check if the health values are correctly set.
        Utility.MyDebugAssert(gameHealth > 0.0f, "Must have a positive health.");
        Utility.MyDebugAssert(trainingGroundHealth > 0.0f, "Must have a positive health.");
        Utility.MyDebugAssert(trainingGroundHealth > gameHealth, "Would want the training ground's boss to have more health.");
    }

    protected override void Start()
    {
        //throw new System.NotImplementedException();
    }

    protected override void Update()
    {
        //throw new System.NotImplementedException();
    }
}