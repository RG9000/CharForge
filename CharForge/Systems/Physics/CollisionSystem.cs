namespace CharForge.Systems.Physics;

public record CollisionInfo(CollisionSystem otherCollider, CollisionSide collisionSide)
{
    readonly CollisionSystem otherCollider = otherCollider;
    readonly CollisionSide CollisionSide = collisionSide;
}

public enum CollisionSide
{
    Left,
    Top,
    Right,
    Bottom
}

public class CollisionSystem(float width, float height)
    : GameSystem(typeof(PositionSystem))
{
    public float Width { get; private set; } = width;
    public float Height { get; private set; } = height;

    private List<CollisionInfo> currentCollisions = [];
    public IList<CollisionInfo> CurrentCollisions { 
        get { return currentCollisions.AsReadOnly(); }
    }

    public override void OnUpdate()
    {
        List<CollisionSystem> otherColliders = GetSystemsInScene<CollisionSystem>(includeSelf: false);
        List<PositionSystem> otherPositions = GetSystemsInScene<PositionSystem>(includeSelf: false);
        PositionSystem myPosition = GetDependentSystem<PositionSystem>();

        currentCollisions = [];

        float myX = myPosition.X;
        float myY = myPosition.Y;

        for (int i = 0; i < otherColliders.Count - 1; i++)
        {
            float otherX = otherPositions[i].X;
            float otherY = otherPositions[i].Y;
            float otherW = otherColliders[i].Width;
            float otherH = otherColliders[i].Height;

            float ownerRight = myX + Width;
            float ownerBottom = myY + Height;
            float otherRight = otherX + otherW;
            float otherBottom = otherY + otherH;

            if (myX < otherRight && ownerRight > otherX &&
                myY < otherBottom && ownerBottom > otherY)
            {
                float leftPenetration = otherRight - myX;
                float rightPenetration = ownerRight - otherX;
                float topPenetration = ownerBottom - otherY; 
                float bottomPenetration = otherBottom - myY;

                // Find the smallest penetration depth
                float minPenetration = Math.Min(Math.Min(leftPenetration, rightPenetration),
                                              Math.Min(topPenetration, bottomPenetration));

                // Determine which side the collision occurred based on the smallest penetration
                if (minPenetration == leftPenetration)
                {
                    currentCollisions.Add(new CollisionInfo(otherColliders[i], CollisionSide.Left));
                }
                else if (minPenetration == rightPenetration)
                {
                    currentCollisions.Add(new CollisionInfo(otherColliders[i], CollisionSide.Right));
                }
                else if (minPenetration == topPenetration)
                {
                    currentCollisions.Add(new CollisionInfo(otherColliders[i], CollisionSide.Top));
                }
                else
                {
                    currentCollisions.Add(new CollisionInfo(otherColliders[i], CollisionSide.Bottom));
                }
            }
        }
    }
}