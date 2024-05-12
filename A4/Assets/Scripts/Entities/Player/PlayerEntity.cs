/// <summary>
/// This component lets the player character act as a hittable entity.
/// </summary>
public class PlayerEntity : HittableEntity
{
    public PlayerEntity()
    {
    }

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