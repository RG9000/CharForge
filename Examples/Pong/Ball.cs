using CharForge;
using CharForge.Systems;
using CharForge.Systems.Graphics;
using CharForge.Systems.Physics;

namespace Pong;

public class Ball : Entity
{
    public Ball() {
        AddSystem(new ConsoleSpriteRenderSystem(() => ["O"]));
        AddSystem(new PositionSystem(15,10));
        AddSystem(new CollisionSystem(1,1));
        AddSystem(new RigidBodySystem(190, 0.5f));
        AddSystem(new BallLoggingSystem());
    }

}

public class BallLoggingSystem() 
    : GameSystem(dependencies: new Type[] {typeof(PositionSystem)},
    runUpdateAfter: new Type[]{typeof(PositionSystem)}) {
    public override void OnUpdate()
    {
        Console.SetCursorPosition(0,0);
        var p = GetDependentSystem<PositionSystem>();
        Console.WriteLine("Ball x = " + p.X);
        Console.WriteLine("Ball y = " + p.Y);
    }
}