using System.Net.Mail;
using CharForge;
using CharForge.Systems;

public class CameraSystem() : GameSystem(dependencies: new Type[] {typeof(PositionSystem)})
{
    protected List<List<List<char>>> Projection {get; set;}

    public void Draw(Func<List<string>>? fSprite, int x, int y, ConsoleColor fg = ConsoleColor.White, ConsoleColor bg = ConsoleColor.Black)
    {
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