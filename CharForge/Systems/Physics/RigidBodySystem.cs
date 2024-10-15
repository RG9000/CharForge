namespace CharForge.Systems.Physics;

public class RigidBodySystem()
    : GameSystem(new Type[] { typeof(PositionSystem), typeof(CollisionSystem) }, runUpdateAfter: new Type[] { typeof(CollisionSystem) })
{
    public float vX {get; private set;}
    public float vY {get; private set;}
    public override void OnUpdate()
    {
        //pass;
    }
}