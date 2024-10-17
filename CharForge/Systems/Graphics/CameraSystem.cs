using CharForge;
using CharForge.Systems;

namespace CharForge.Systems.Graphics;

public class CameraSystem(int xSize, int ySize) : GameSystem(dependencies: new Type[] {typeof(PositionSystem)})
{
    private readonly int XSize = xSize;
    private readonly int YSize = ySize;

    public void Draw(Func<List<string>>? fSprite, int x, int y, ConsoleColor fg = ConsoleColor.White, ConsoleColor bg = ConsoleColor.Black)
    {
        var positionSystem = GetDependentSystem<PositionSystem>();

        int minX = (int)Math.Floor(positionSystem.X);
        int minY = (int)Math.Floor(positionSystem.Y);
        var maxX = minX + XSize;
        var maxY = minY + YSize;

        if (fSprite == null) return;

        var sprite = fSprite();

        if (sprite == null) return;

        if (x < minX || y < minY || x + sprite[0].Length > maxX || y + sprite.Count > maxY)
        {
            return;
        }

        Console.ForegroundColor = fg;
        Console.BackgroundColor = bg;

        Console.SetCursorPosition(x, y);
        if (fSprite != default && fSprite().Count > 0)
        {
            var lines = fSprite();
            int yIndex = 0;
            foreach (var line in lines)
            {
                Console.SetCursorPosition(x + minX, y+yIndex);
                Console.Write(line);
                yIndex += 1;
            }
        }
    }

    public override void OnRender()
    {

    }
}