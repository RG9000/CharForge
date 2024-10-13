using CharForge;
using CharForge.Systems.Graphics;

namespace Pong;

public class Paddle : Entity {
    public Paddle() {
        AddSystem(new ConsoleSpriteRenderSystem(() => [
            "XX",
            "XX",
            "XX",
            "XX",
            "XX",
            "XX",
        ]));
        AddSystem(new PositionSystem(25, 30));
    }
}

public class PlayerPaddle : Paddle {
    public PlayerPaddle() : base()
    {
        AddSystem(new PlayerPaddleControlSystem());
    }
}

public class PlayerPaddleControlSystem(ConsoleKey upKey = ConsoleKey.W, ConsoleKey downKey = ConsoleKey.S, bool isLeft = true) : GameSystem(typeof(PositionSystem))
{

    public ConsoleKey UpKey { get; set; } = upKey;
    public ConsoleKey DownKey { get; set; } = downKey;
    public bool IsLeft { get; set; } = isLeft;

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



