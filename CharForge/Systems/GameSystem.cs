namespace CharForge;

public abstract class GameSystem
{

    internal bool FinishedInitializing { get; private set; }
    internal readonly List<Type> DependentTypes = [];
    internal List<Type> RunUpdateAfter = [];
    public Entity? Owner = null;
    private int InitializationAttempts = 0;

    internal readonly List<GameSystem> Dependencies = [];

    /// <summary>
    /// Gets all the systems in the current scene of the provided type, ordered by Entity ID.
    /// </summary>
    /// <param name="includeSelf">
    /// Pass this in to return systems associated with the same object as the caller
    /// </param>
    public List<T> GetSystemsInScene<T>(bool includeSelf = false) where T : GameSystem
    {
        if (Owner?.Owner == null) throw new Exception("Entity " + Owner?.Id + " not attached to a Scene. Only call 'GetSystemsInScene' during OnUpdate calls.");
        var retSystems = new List<T>();
        foreach (var e in Owner.Owner.Entities)
        {
            if (!includeSelf && e == Owner) continue;
            foreach (var s in e.Systems)
            {
                if (s is T ts)
                {
                    retSystems.Add(ts);
                }
            }
        }

        return [.. retSystems.OrderBy(e => e.Owner?.Id)];
    }

    /// <summary>
    /// Gets the system matching the provided type on the same Entity as the caller
    /// </summary>
    public T GetDependentSystem<T>() where T : GameSystem
    {
        foreach (var s in Dependencies)
        {
            if (s is T ts)
            {
                return ts;
            }
        }

        throw new Exception("Tried to get a dependent system for '" + this.GetType().ToString() + "' and failed. Have you referenced it in the system's constructor?");
    }

    internal GameSystem SetOwner(Entity e)
    {
        Owner = e;
        return this;
    }

    public virtual void OnUpdate(){}

    public virtual void OnRender(){}

    public float GetDeltaTime()
    {
        if (Owner == null) throw new Exception("Owner was null while attempting to get Delta");
        return Owner.GetDeltaTime();
    }

    public virtual void OnInit()
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
        foreach (var d in DependentTypes)
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

    protected GameSystem(Type[]? dependencies = null, Type[]? runUpdateAfter = null)
    {
        DependentTypes.AddRange(dependencies ?? []);
        RunUpdateAfter.AddRange(runUpdateAfter ?? []);
    }

    protected ConsoleKey GetCurrentKeyPressed()
    {
        if (Owner?.Owner == null) return ConsoleKey.None;
        return Owner.Owner.CurrentKeyPressed;
    }


}
