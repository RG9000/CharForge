using CharForge;
using CharForge.Systems;
using CharForge.Systems.Graphics;
using CharForge.Systems.Physics;

namespace Pong;

public class Paddle : Entity {
    public Paddle(bool isLeft = true) {
        AddSystem(new ConsoleSpriteRenderSystem(() => [
            "XX",
            "XX",
            "XX",
            "XX",
            "XX",
            "XX",
        ]));
        if (isLeft)
        {
            AddSystem(new PositionSystem(2, 10));
        }
        else 
        {
            AddSystem(new PositionSystem(30, 10));
        }
        AddSystem(new CollisionSystem(2,6));
    }
}

public class Player1Paddle : Paddle {
    public Player1Paddle() : base()
    {
        AddSystem(new PlayerPaddleControlSystem());
    }
}
public class Player2Paddle : Paddle {
    public Player2Paddle() : base(false)
    {
        AddSystem(new PlayerPaddleControlSystem(ConsoleKey.UpArrow, ConsoleKey.DownArrow));
    }
}

public class PlayerPaddleControlSystem(ConsoleKey upKey = ConsoleKey.W, ConsoleKey downKey = ConsoleKey.S) 
    : GameSystem(new Type[] {typeof(PositionSystem)}, [])
{

    public ConsoleKey UpKey { get; set; } = upKey;
    public ConsoleKey DownKey { get; set; } = downKey;

    public override void OnUpdate()
    {
        var positionSystem = GetDependentSystem<PositionSystem>();
        if (GetCurrentKeyPressed() == UpKey){
            positionSystem.SetPosition(positionSystem.X, positionSystem.Y - 1);
        }
        if (GetCurrentKeyPressed() == DownKey){
            positionSystem.SetPosition(positionSystem.X, positionSystem.Y + 1);
        }

    }
}



