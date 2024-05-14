/// <summary>
/// This component lets the player character act as a hittable entity.
/// </summary>
public class PlayerEntity : HittableEntity
{
    /*********************************** Static Settings ***********************************/
    public const float PLAYER_WIDTH = 1.0f;
    public const float PLAYER_HEIGHT = 1.0f;

    /*********************************** Ctor ***********************************/

    public PlayerEntity()
    {
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
    /// Constantly check if the player is dead.
    /// </summary>
    protected override void Update()
    {
        throw new System.NotImplementedException("Constantly check if the player is dead.");
    }
}