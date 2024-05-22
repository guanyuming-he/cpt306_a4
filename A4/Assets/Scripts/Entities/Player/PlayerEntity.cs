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
        throw new System.NotImplementedException();
    }

    /*********************************** Mono ***********************************/

    protected override void Awake()
    {
        // Do nothing for now.
    }

    protected override void Start()
    {
        // Do nothing for now.
    }

    /// <summary>
    /// Nothing for now.
    /// </summary>
    protected override void Update()
    {
        
    }
}