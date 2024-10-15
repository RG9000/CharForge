namespace CharForge.Systems.Graphics;

public class ConsoleSpriteRenderSystem(Func<List<string>> sprite, ConsoleColor fgColor = ConsoleColor.White, ConsoleColor bgColor = ConsoleColor.Black) 
    : GameSystem (new Type[]{typeof(PositionSystem)}, [])
{
    public Func<List<string>>? Sprite { get; set; } = sprite;
    public ConsoleColor FGColor { get; set; } = fgColor;
    public ConsoleColor BGColor { get; set; } = bgColor;

    public override void OnUpdate()
    {
        var position = GetDependentSystem<PositionSystem>();
        var drawnX = (int)Math.Floor(position.X);
        var drawnY = (int)Math.Floor(position.Y);
        Console.SetCursorPosition(drawnX, drawnY);
        Console.ForegroundColor = FGColor;
        Console.BackgroundColor = BGColor;
        if (Sprite != default && Sprite().Count > 0)
        {
            var lines = Sprite();
            int yIndex = 0;
            foreach (var line in lines)
            {
                Console.SetCursorPosition(drawnX, drawnY+yIndex);
                Console.Write(line);
                yIndex += 1;
            }
        }
    }
}