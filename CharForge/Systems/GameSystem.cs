namespace CharForge;

public abstract class GameSystem {

    internal bool FinishedInitializing {get; private set;}
    private readonly List<Type> DependentTypes = [];
    public Entity? Owner = null;
    private int InitializationAttempts = 0;

    private readonly List<GameSystem> Dependencies = [];

    public T GetDependentSystem<T>() where T : GameSystem
    {
        foreach (var s in Dependencies)
        {
            if (s is T ts)
            {
                return ts;
            }
        }

        throw new Exception("Tried to get a dependent system for '"+ this.GetType().ToString() + "' and failed. Have you referenced it in the system's constructor?");
    }

    internal GameSystem SetOwner(Entity e) {
        Owner = e;
        return this;
    }
    
    public abstract void OnUpdate();

    public void OnInit()
    {
        if (Owner == null) return;

        if (InitializationAttempts >= 50)
        {
            throw new Exception("Unable to initialize system '" + this.GetType() + "'. It looks like it has a dependency that you didn't add to the Owner.");
        }

        //These match, so everything must have been initialized
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
        InitializationAttempts += 1;
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
