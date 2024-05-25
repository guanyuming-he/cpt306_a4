/// <summary>
/// Represents the damageable and physical part of the player.
/// </summary>
public class PlayerEntity : DamageableEntity
{
    /*********************************** Static Settings ***********************************/
    public const float PLAYER_WIDTH = 1.0f;
    public const float PLAYER_HEIGHT = 1.0f;

    /*********************************** Fields ***********************************/
    HealthBar healthBar;

    public float initialHealth;

    /*********************************** Ctor ***********************************/

    public PlayerEntity()
    {
    }

    /*********************************** From Entity ***********************************/

    public override Side getSide()
    {
        return Side.PLAYER;
    }

    /*********************************** From DamageableEntity ***********************************/

    public override void onDeath()
    {
        Game.gameSingleton.gameOver(true);

        UnityEngine.GameObject.Destroy(gameObject);
    }

    /*********************************** Mono ***********************************/

    protected override void Awake()
    {
        // Do nothing for now.
    }

    protected override void Start()
    {
        // 1. set the inital health
        Utility.MyDebugAssert(initialHealth > 0.0f, "health must be positive.");
        setInitialHealth(initialHealth);
    }

    /// <summary>
    /// Nothing for now.
    /// </summary>
    protected override void Update()
    {
        
    }

}