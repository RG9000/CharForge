using CharForge;
using CharForge.Systems.Graphics;

public class Ball : Entity
{
    public Ball()
    {
        AddSystem(new PositionSystem(2,2))
        .AddSystem(new ConsoleSpriteRenderSystem(() =>
            {
                return ["/@@@\\",
                "@@1@@",
                "\\@@@/"
                        ];
            }));
    }
}