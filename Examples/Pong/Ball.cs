using CharForge;
using CharForge.Systems;
using CharForge.Systems.Graphics;

namespace Pong;

public class Ball : Entity
{
    public Ball()
    {
        AddSystem(new ConsoleSpriteRenderSystem(() => ["0"]));
        AddSystem(new PositionSystem(15,10));
    }

}