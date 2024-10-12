namespace CharForge.Systems;

public class SpriteRenderer : GameSystem
{
    public SpriteRenderer(Func<List<string>> sprite) : base()
    {
        Sprite = sprite;
    }
    
    public Func<List<string>>? Sprite { get; set; }

    public override void OnInit()
    {
    }

    public override void OnUpdate()
    {
        if (Owner == null) return;
        Console.SetCursorPosition(Owner.DrawnX, Owner.DrawnY);
        if (Sprite != default && Sprite().Count > 0)
        {
            var lines = Sprite();
            int yIndex = 0;
            foreach (var line in lines)
            {
                Console.SetCursorPosition(Owner.DrawnX, Owner.DrawnY+yIndex);
                Console.Write(line);
                yIndex += 1;
            }
        }
    }
}