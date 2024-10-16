namespace CharForge.Systems.Graphics;

public class ConsoleSpriteRenderSystem(Func<List<string>> sprite, ConsoleColor fgColor = ConsoleColor.White, ConsoleColor bgColor = ConsoleColor.Black) 
    : GameSystem (new Type[]{typeof(PositionSystem), typeof(CameraSystem)}, [])
{
    public Func<List<string>>? Sprite { get; set; } = sprite;
    public ConsoleColor FGColor { get; set; } = fgColor;
    public ConsoleColor BGColor { get; set; } = bgColor;

    public override void OnRender()
    {
        var position = GetDependentSystem<PositionSystem>();
        var drawnX = (int)Math.Floor(position.X);
        var drawnY = (int)Math.Floor(position.Y);
        Console.ForegroundColor = FGColor;
        Console.BackgroundColor = BGColor;
        foreach (var c in GetSystemsInScene<CameraSystem>())
        {
            c.Draw(Sprite, drawnX, drawnY);
        }

    }
}