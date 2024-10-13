using CharForge;

public class PositionSystem : GameSystem
{
    public PositionSystem(float x = 0f, float y = 0f)
    {
        X = x;
        Y = y;
    }
    public float X {get; private set;}
    public float Y {get; private set;}
    public override void OnUpdate()
    {
        //pass;
    }
}