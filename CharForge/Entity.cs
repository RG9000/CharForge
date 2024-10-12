using CharForge.Systems;

namespace CharForge;
public class Entity
{
    public virtual void OnUpdate(){}
    public virtual void OnInit(){}

    public Entity(string name)
    {
        Name = name;
    }

    public string Name {get; set;}
    public Scene? Scene { get; set; }
    public List<GameSystem> Systems { get; set; } = [];
    public void SetScene(Scene scene)
    {
        Scene = scene;
    }

    public T? GetSystem<T>() {
        foreach (var s in Systems)
        {
            if (s is T ts)
            {
                return ts;
            }
        }
        return default;
    }

}