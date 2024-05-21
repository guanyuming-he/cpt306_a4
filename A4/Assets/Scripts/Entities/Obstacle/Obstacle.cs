/// <summary>
/// For now I guess nothing to do.
/// Everything can be adjusted from the editor.
/// 
/// Perhaps I just put some constants here.
/// </summary>
public sealed class Obstacle : Entity
{
    /*********************************** Static ***********************************/
    // A normal obs is a cube of such length on each direction.
    public const float OBS_LENGTH = 1.0f;
    // A high obs is a normal obs streched in the Y direction to this height.
    public const float HIGH_OBS_HEIGHT = 2.0f;

    /*********************************** From Entity ***********************************/

    public override Side getSide()
    {
        return Side.ENVIRONMENT;
    }

    protected override void Awake()
    {
    }

    protected override void Start()
    {
    }

    protected override void Update()
    {
    }
}