namespace CharForge.Systems.Graphics;

public class CameraSystem(int xSize, int ySize, bool border = false) : GameSystem(dependencies: new Type[] {typeof(PositionSystem)})
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

        int minX = (int)Math.Floor(positionSystem.X);
        int minY = (int)Math.Floor(positionSystem.Y);
        var maxX = minX + XSize;
        var maxY = minY + YSize;

        if (fSprite == null) return;

        var sprite = fSprite();

        if (sprite == null) return;

        if (x < minX+1 || y < minY+1 || x + sprite[0].Length > maxX || y + sprite.Count > maxY)
        {
            return;
        }

        Console.ForegroundColor = fg;
        Console.BackgroundColor = bg;

        Console.SetCursorPosition(x, y);
        if (fSprite != default && fSprite().Count > 0)
        {
            var lines = sprite;
            int yIndex = 0;
            foreach (var line in lines)
            {
                Console.SetCursorPosition(x + minX, y+yIndex + minY);
                Console.Write(line);
                yIndex += 1;
            }
        }
    }

    public override void OnRender()
    {
        if (Console.WindowWidth < XSize || Console.WindowHeight < YSize)
        {
            throw new Exception("A camera tried to render off the screen. Make your terminal window bigger, or zoom out.");
        }
        var positionSystem = GetDependentSystem<PositionSystem>();
        if (Border)
        {
            int minX = (int)Math.Floor(positionSystem.X);
            int minY = (int)Math.Floor(positionSystem.Y);
            var maxX = minX + XSize;
            var maxY = minY + YSize;
               
            var floor = new char[maxX];
            Array.Fill(floor, '=');

            Console.SetCursorPosition(0,0);
            Console.WriteLine(floor);
            Console.SetCursorPosition(0,maxY-1);
            Console.WriteLine(floor);

            for (int i = 0; i < YSize; i++)
            {
                Console.SetCursorPosition(minX, minY+i);
                Console.Write('|');
                Console.SetCursorPosition(maxX, minY+i);
                Console.Write('|');
            }
        }

    }
}