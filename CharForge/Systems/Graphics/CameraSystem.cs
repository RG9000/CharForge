using System.Net;

namespace CharForge.Systems.Graphics;

public class CameraSystem(int xSize, int ySize, bool border = false) : GameSystem(dependencies: new Type[] { typeof(PositionSystem) })
{
    private readonly int XSize = xSize;
    private readonly int YSize = ySize;

    private readonly bool Border = border;

    public override void OnInit()
    {
        if (Console.WindowWidth < XSize || Console.WindowHeight < YSize)
        {
            throw new Exception("A camera tried to render off the screen. Make your terminal window bigger, or zoom out.");
        }
        base.OnInit();
    }

    public void Draw(Func<List<string>>? fSprite, int x, int y, ConsoleColor fg = ConsoleColor.White, ConsoleColor bg = ConsoleColor.Black)
    {
        var positionSystem = GetDependentSystem<PositionSystem>();

        int centerX = (int)Math.Floor(positionSystem.X);
        int centerY = (int)Math.Floor(positionSystem.Y);

        int minX = centerX - (XSize / 2);
        int minY = centerY - (YSize / 2);
        int maxX = minX + XSize;
        int maxY = minY + YSize;

        if (fSprite == null) return;

        var sprite = fSprite();

        if (sprite == null) return;

        if (x < minX || y < minY || x + sprite[0].Length > maxX || y + sprite.Count > maxY)
        {
            return;
        }

        Console.ForegroundColor = fg;
        Console.BackgroundColor = bg;

        int screenX = x - minX;
        int screenY = y - minY;

        int yIndex = 0;
        foreach (var line in sprite)
        {
            Console.SetCursorPosition(screenX, screenY + yIndex);
            Console.Write(line);
            yIndex += 1;
        }
    }

    public override void OnRender()
    {
        if (Console.WindowWidth < XSize || Console.WindowHeight < YSize)
        {
            throw new Exception("A camera tried to render off the screen. Make your terminal window bigger, or zoom out.");
        }

        if (Border)
        {
            var floor = new char[XSize];
            Array.Fill(floor, '=');

            // Draw the top and bottom borders in screen coordinates
            Console.SetCursorPosition(0, 0);
            Console.WriteLine(floor);
            Console.SetCursorPosition(0, YSize - 1);
            Console.WriteLine(floor);

            // Draw the side borders in screen coordinates
            for (int i = 1; i < YSize - 1; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write('|');
                Console.SetCursorPosition(XSize - 1, i);
                Console.Write('|');
            }
        }
    }
}