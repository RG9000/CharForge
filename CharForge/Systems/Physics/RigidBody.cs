using CharForge;
using CharForgeExample;

public class RigidBody(float bounciness, float roughness) : StaticRigidBody
{
    public float VX { get; set; } = 0;
    public float VY { get; set; } = 0;
    public float Bounciness { get; private set; } = bounciness;
    public float Roughness { get; private set; } = roughness;

    public override void OnInit()
    {
        base.OnInit();
    }

    public override void OnUpdate()
    {
        if (Owner == null) return;
        base.OnUpdate();

        Owner.X += VX;
        Owner.Y += VY;
    }

    protected override void HandleCollision(object? sender, CollisionEventArgs? e)
    {
        if (Owner == null || e == null) return;

        const float minVelocityThreshold = 0.1f;

        if (e.EntityB is not GameObject Other) return;
        var c = Other.GetSystem<Collider>();
        var c2 = Owner.GetSystem<Collider>();
        var orb = Other.GetSystem<RigidBody>();

        if (c == null) return;
        if (c2 == null) return;

        if (e.Side == CollisionSide.Left || e.Side == CollisionSide.Right)
        {
            VX *= -1 * Bounciness;
            VY *= (1 - Roughness);

            if (c.Owner == null) return;
            if (c.Owner.Systems == null) return;

            if (orb is not null)
            {
                orb.VX *= -1 * Bounciness;
                orb.VY *= 1 - Roughness;
            }


            // Push the object out of the collision
            if (e.Side == CollisionSide.Right)
            {
                Owner.X = Other.X - c2.Width; // Place the object just to the left of the other object
            }
            else
            {
                Owner.X = Other.X + c.Width; // Place the object just to the right of the other object
            }

            // Check if the velocity is below the threshold, and stop bouncing if it is
            if (Math.Abs(VX) < minVelocityThreshold)
            {
                VX = 0;
            }
        }
        else
        {
            VX *= (1 - Roughness);
            VY *= -1 * Bounciness;

            if (orb is not null)
            {
                orb.VY *= -1 * Bounciness;
                orb.VX *= 1 - Roughness;
            }

            // Push the object out of the collision
            if (e.Side == CollisionSide.Top)
            {
                Owner.Y = Other.Y - c2.Height; // Place the object above the other object
            }
            else
            {
                Owner.Y = Other.Y + c.Height; // Place the object below the other object
            }

            // Check if the velocity is below the threshold, and stop bouncing if it is
            if (Math.Abs(VY) < minVelocityThreshold)
            {
                VY = 0;
                // Snap to the nearest integer to prevent jittering
                Owner.Y = (float)Math.Round(Owner.Y);
            }
        }

        base.HandleCollision(sender, e);
    }

}