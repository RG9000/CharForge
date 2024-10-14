using CharForge;
using CharForge.Systems.Graphics;

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
    }
}

public class PlayerPaddle : Paddle {
    public PlayerPaddle() : base()
    {
        AddSystem(new PlayerPaddleControlSystem());
    }
}

public class PlayerPaddleControlSystem(ConsoleKey upKey = ConsoleKey.W, ConsoleKey downKey = ConsoleKey.S) : GameSystem(typeof(PositionSystem))
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



