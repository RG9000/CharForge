using CharForge;
using CharForge.Systems;

namespace CharForge.Systems.Graphics;

public class CameraSystem() : GameSystem(dependencies: new Type[] {typeof(PositionSystem)})
{
    public void Draw(Func<List<string>>? fSprite, int x, int y, ConsoleColor fg = ConsoleColor.White, ConsoleColor bg = ConsoleColor.Black)
    {
        var positionSystem = GetDependentSystem<PositionSystem>();

        //HERE WE SHOULD FIGURE OUT IF THE SPRITE IS OUT OF BOUNDS ENTIRELY

        Console.ForegroundColor = fg;
        Console.BackgroundColor = bg;

        Console.SetCursorPosition(x, y);
        if (fSprite != default && fSprite().Count > 0)
        {
            var lines = fSprite();
            int yIndex = 0;
            foreach (var line in lines)
            {
                Console.SetCursorPosition(x, y+yIndex);
                Console.Write(line);
                yIndex += 1;
            }
        }
    }

    public override void OnRender()
    {

    }
}