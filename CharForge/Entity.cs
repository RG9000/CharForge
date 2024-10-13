namespace CharForge;

public class Entity 
{
    private readonly List<GameSystem> Systems = [];

    public T? GetSystem<T> () where T : GameSystem
    {
        foreach (var s in Systems)
        {
            if (s is T ts)
            {
                return ts;
            }
        }
        return default;
    }
    
    public Entity AddSystem(GameSystem system) 
    {
        Systems.Add(system);
        return this;
    }
    public Entity RemoveSystemsOfType<T>() where T : GameSystem 
    {
        bool noMoreSystemsOfType = false;
        while (!noMoreSystemsOfType)
        {
            var sys = GetSystem<T>();
            if (sys == default)
            {
                noMoreSystemsOfType = true;
            }
        }
        return this;
    }
}
