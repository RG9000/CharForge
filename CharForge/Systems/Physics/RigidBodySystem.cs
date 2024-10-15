using System.Runtime.InteropServices;

namespace CharForge.Systems.Physics;

public class RigidBodySystem(float angle = 0, float speed = 0)
    : GameSystem(new Type[] { typeof(PositionSystem), typeof(CollisionSystem) }, runUpdateAfter: new Type[] { typeof(CollisionSystem) })
{

    public float Angle { get; private set; } = angle;
    public float Speed { get; private set; } = speed;

    public void MoveForward(float lambda = 1f)
    {
        float aRads = (float)(Angle * (Math.PI / 180));
        float vX = (float)(Speed * Math.Cos(aRads));
        float vY = (float)(Speed * Math.Sin(aRads));
        var positionSystem = GetDependentSystem<PositionSystem>();
        var x = positionSystem.X;
        var y = positionSystem.Y;
        positionSystem.SetPosition(x + (vX * lambda), y + (vY * lambda));
    }

    public override void OnUpdate()
    {
        var collisionSystem = GetDependentSystem<CollisionSystem>();
        var otherRigidBodies = GetSystemsInScene<RigidBodySystem>(false);
        MoveForward();
        foreach (var c in collisionSystem.CurrentCollisions)
        {
            var wasARigidBody = false;
            foreach (var o in otherRigidBodies)
            {
                if (c.OtherCollider == o.GetDependentSystem<CollisionSystem>())
                {
                    o.Angle = GetBouncedAngle(o.Angle, c.CollisionSide);
                    Angle = GetBouncedAngle(Angle, c.CollisionSide);
                    //give them some 'oomph' to escape each other's colliders
                    o.MoveForward(2f);
                    MoveForward(2f);
                    wasARigidBody = true;
                }
            }
            // we collided, but not with another rigid body, so this is simppler
            if (!wasARigidBody)
            {
                Angle = GetBouncedAngle(Angle, c.CollisionSide);
                //give it some 'oomph' to escape other colliders
                MoveForward(2f);
            }

        }

    }

    private static float GetBouncedAngle(float oA, CollisionSide side)
    {
        if (side == CollisionSide.Left || side == CollisionSide.Right)
        {
            return 180 - oA;
        }
        else
        {
            return oA * -1;
        }

    }
}