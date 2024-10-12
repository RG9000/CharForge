using CharForge.Systems;
using CharForgeExample;

public class Impulse : GameSystem
{
    public Impulse(float x, float y)
    {
        X = x;
        Y = y;
    }

    public float X {get; set;}
    public float Y {get; set;}

    private bool impulseApplied = false;

    public RigidBody? RigidBody {get; set;}

    public override void OnInit()
    {
        if (Owner == null) return;
        RigidBody = Owner.GetSystem<RigidBody>();
    }

    public override void OnUpdate()
    {
        if (RigidBody == null) return;
        if (!impulseApplied)
        {
            RigidBody.VX += X;
            RigidBody.VY += Y;
            impulseApplied = true;
        }

    }
}