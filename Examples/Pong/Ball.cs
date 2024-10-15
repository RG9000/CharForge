using CharForge;
using CharForge.Systems;
using CharForge.Systems.Graphics;
using CharForge.Systems.Physics;

namespace Pong;

public class Ball : Entity
{
    public Ball()
    {
        AddSystem(new ConsoleSpriteRenderSystem(() => ["O"]));
        AddSystem(new PositionSystem(15,10));
        AddSystem(new CollisionSystem(1,1));
        AddSystem(new RigidBodySystem(170, 0.5f));
    }

}