namespace CharForge.Systems.Physics;
public class ConstantAcceleration : GameSystem
{
    public ConstantAcceleration(float x, float y)
    {
        X = x;
        Y = y;
    }

    public float X {get; set;}
    public float Y {get; set;}

    public RigidBody? RigidBody {get; set;}

    public override void OnInit()
    {
        if (Owner == null) return;
        if (Owner.Systems == null) return;
        RigidBody = Owner.Systems.FirstOrDefault(e => e is RigidBody) as RigidBody;
    }

    public override void OnUpdate()
    {
        if (Owner == null) return;
        if (Owner.Scene == null) return;
        if (RigidBody == null) return;

        RigidBody.VX += X;
        RigidBody.VY += Y;
    }
}