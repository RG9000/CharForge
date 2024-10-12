using CharForge.Systems;

namespace CharForge;
public class GameObject: Entity {
    public GameObject(string name, float x, float y, bool visible) : base(name)
    {
        X = x;
        Y = y;
        Visible = visible;
    }

    public float X { get; set; }
    public float Y { get; set; }
    public bool Visible { get; set; }

    private const float Epsilon = 0.001f;
    private const float Bias = 0.0001f; // Small bias to adjust the rounding behavior

    public int DrawnX
    {
        get
        {
            // If X is very close to a whole number, round it to the nearest integer
            if (Math.Abs(this.X - Math.Round(this.X)) < Epsilon)
            {
                return (int)Math.Round(this.X);
            }
            // Otherwise, use Math.Floor with a small bias
            return (int)Math.Floor(this.X + Bias);
        }
    }

    public int DrawnY
    {
        get
        {
            // If Y is very close to a whole number, round it to the nearest integer
            if (Math.Abs(this.Y - Math.Round(this.Y)) < Epsilon)
            {
                return (int)Math.Round(this.Y);
            }
            // Otherwise, use Math.Floor with a small bias
            return (int)Math.Floor(this.Y + Bias);
        }
    }


    public GameObject AddSystem(GameSystem system)
    {
        system.SetOwner(this); 
        Systems.Add(system);
        return this;
    }

    
    public override void OnInit() {
        foreach (var s in Systems)
        {
            s.OnInit();
        }
    }

    public override void OnUpdate() {
    }
}