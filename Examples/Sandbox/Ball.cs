using System.Runtime.CompilerServices;
using CharForge;
using CharForge.Systems;
using CharForge.Systems.Graphics;
using CharForge.Systems.Physics;

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