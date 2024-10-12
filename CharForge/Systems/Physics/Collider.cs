using CharForge;
using CharForge.Systems;

namespace CharForge.Systems.Physics;
public class Collider(int w, int h) : GameSystem
{
    public int Width { get; set; } = w;
    public int Height { get; set; } = h;
    public event EventHandler<CollisionEventArgs>? CollisionDetected;

    public override void OnUpdate()
    {
        if (Owner == null || Owner.Scene == null) return;

        Owner.Scene.Entities.ForEach(o =>
        {
            if (o != this.Owner)
            {
                Collider? c = o.GetSystem<Collider>();
                if (c == null) return;
                if (c.Owner == null) return;


                float ownerRight = Owner.X + Width;
                float ownerBottom = Owner.Y + Height;
                float otherRight = c.Owner.X + c.Width;
                float otherBottom = c.Owner.Y + c.Height;

                // Check if rectangles overlap
                if (Owner.X < otherRight && ownerRight > c.Owner.X &&
                    Owner.Y < otherBottom && ownerBottom > c.Owner.Y)
                {
                    // Calculate penetration depths for each side
                    float leftPenetration = otherRight - Owner.X;
                    float rightPenetration = ownerRight - c.Owner.X;
                    float topPenetration = ownerBottom - c.Owner.Y; // Inverted Y-axis: top is lower value
                    float bottomPenetration = otherBottom - Owner.Y; // Inverted Y-axis: bottom is higher value

                    // Find the smallest penetration depth
                    float minPenetration = Math.Min(Math.Min(leftPenetration, rightPenetration),
                                                  Math.Min(topPenetration, bottomPenetration));

                    // Determine which side the collision occurred based on the smallest penetration
                    if (minPenetration == leftPenetration)
                    {
                        OnCollisionDetected(new CollisionEventArgs(Owner, c.Owner, CollisionSide.Left));
                    }
                    else if (minPenetration == rightPenetration)
                    {
                        OnCollisionDetected(new CollisionEventArgs(Owner, c.Owner, CollisionSide.Right));
                    }
                    else if (minPenetration == topPenetration)
                    {
                        OnCollisionDetected(new CollisionEventArgs(Owner, c.Owner, CollisionSide.Top));
                    }
                    else
                    {
                        OnCollisionDetected(new CollisionEventArgs(Owner, c.Owner, CollisionSide.Bottom));
                    }
                }
            }
        });
    }



    protected virtual void OnCollisionDetected(CollisionEventArgs e)
    {
        CollisionDetected?.Invoke(this, e);
    }

    public override void OnInit()
    {
        //pass;
    }

}

public enum CollisionSide
{
    Left,
    Top,
    Right,
    Bottom
}

public class CollisionEventArgs : EventArgs
{
    public Entity EntityA { get; }
    public Entity EntityB { get; }
    public CollisionSide Side { get; set; }

    public CollisionEventArgs(Entity entityA, Entity entityB, CollisionSide side)
    {
        EntityA = entityA;
        EntityB = entityB;
        Side = side;
    }
}