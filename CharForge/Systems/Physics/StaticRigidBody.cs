namespace CharForge.Systems.Physics;

public class StaticRigidBody : GameSystem
{
    public StaticRigidBody() {
    }
    internal float? prevX;
    internal float? prevY;
    private Collider? Collision;
    protected virtual void HandleCollision(object? sender, CollisionEventArgs? e)
    {
        if (Owner == null) return;

        // If the derived class has already handled the collision, don't reset the position
        if (Math.Abs(Owner.X - (prevX ?? Owner.X)) > 0.01f || 
            Math.Abs(Owner.Y - (prevY ?? Owner.Y)) > 0.01f)
        {
            // The position has changed significantly, so let the derived class changes persist
            return;
        }

        // Otherwise, revert to the previous position
        Owner.X = prevX ?? Owner.X;
        Owner.Y = prevY ?? Owner.Y;
        Console.SetCursorPosition(0, 0);
    }

        public override void OnInit()
        {
            if (Owner == null) return;
            Collision = Owner.Systems.FirstOrDefault(e => e is Collider) as Collider;
            if (Collision != null)
            {
                Collision.CollisionDetected += HandleCollision;
            }
        }

    public override void OnUpdate()
    {
        if (Owner == null) return;
        prevX = Owner.X;
        prevY = Owner.Y;

    }
}

