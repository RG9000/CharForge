namespace CharForge;

public abstract class GameSystem {

    internal bool FinishedInitializing {get; private set;}
    private readonly List<Type> DependentTypes = [];
    protected Entity? Owner = null;

    private readonly List<GameSystem> Dependencies = [];

    protected T GetDependentSystem<T>() where T : GameSystem
    {
        foreach (var s in Dependencies)
        {
            if (s is T ts)
            {
                return ts;
            }
        }
        throw new Exception("Tried to get a dependent system that does not exist. Have you declared it in the constructor and added it to the Entity in question?");
    }

    internal GameSystem SetOwner(Entity e) {
        Owner = e;
        return this;
    }
    
    public abstract void OnUpdate();

    public void OnInit()
    {
        Console.WriteLine("running base constructor");
        if (Owner == null) return;
        Console.WriteLine("Owner was initted");
        Console.WriteLine("dependenttypes: " + DependentTypes.Count);

        //These match, so everything must have been initialized
        Console.WriteLine("Types count " + DependentTypes.Count);
        Console.WriteLine("actual count " + Dependencies.Count);
        if (DependentTypes.Count == Dependencies.Count)
        {
            FinishedInitializing = true;
            return;
        }

        //Otherwise, try and initialize all the dependencies
        foreach(var d in DependentTypes)
        {
            var method = Owner.GetType().GetMethod("GetSystem");
            if (method != null)
            {
                var genericMethod = method.MakeGenericMethod(d);
                var system = genericMethod.Invoke(Owner, null);
                if (system != null)
                {
                    Dependencies.Add((GameSystem)system);
                }
            }
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GameSystem"/> class with a list of dependent systems.
    /// When inheriting from <see cref="GameSystem"/>, declare your dependencies by passing them to this constructor.
    /// </summary>
    /// <param name="dependencies">
    /// The system types that this system depends on. Pass them as arguments like:
    /// <code>
    /// public MySystem() : base(owner, typeof(SystemA), typeof(SystemB)) {}
    /// </code>
    /// </param>

    protected GameSystem(params Type[] dependencies)
    {
        DependentTypes.AddRange(dependencies);
    }
    
    
}
