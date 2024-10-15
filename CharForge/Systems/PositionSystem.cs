using CharForge;

namespace CharForge.Systems;

public class PositionSystem(float x = 0f, float y = 0f) : GameSystem([],[])
{
    public float X { get; private set; } = x;
    public float Y { get; private set; } = y;

    public void SetPosition(float x, float y)
    {
        X = x;
        Y = y;
    }

    public override void OnUpdate()
    {
        //pass;
    }
}