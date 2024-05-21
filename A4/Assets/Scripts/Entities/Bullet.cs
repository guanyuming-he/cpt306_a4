/// <summary>
/// 
/// </summary>
public class Bullet : Entity
{
    /*********************************** Fields ***********************************/
    public float damage;
    public float speed;
    public Entity.Side whichSide;

    /*********************************** Mono ***********************************/
    /// <summary>
    /// Check its data
    /// </summary>
    protected override void Awake()
    {
        Utility.MyDebugAssert(damage > 0.0f, "a bullet must have positive damage.");
        Utility.MyDebugAssert(speed > 0.0f, "a bullet must have a positive speed.");
    }

    protected override void Start()
    {
        throw new System.NotImplementedException();
    }

    protected override void Update()
    {
        throw new System.NotImplementedException();
    }
}