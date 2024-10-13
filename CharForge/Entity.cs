namespace CharForge
{

    /// <summary>
    /// Represents a game entity that can hold and manage various <see cref="GameSystem"/> objects.
    /// Each entity has a unique identifier (Id), and systems can be dynamically added or removed.
    /// The Entity class also allows for retrieving specific types of systems.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="Entity"/> class.
    /// </remarks>
    /// <param name="id">Optional ID. If not provided, a new GUID is generated. Explicitely set
    /// this if you need to keep track of the Object </param>
    public class Entity(string? id = null)
    {
        public Scene? Owner {get; private set;} = null;
        private readonly List<GameSystem> Systems = [];
        
        /// <summary>
        /// A unique identifier for the entity. If no ID is provided, a new GUID will be generated.
        /// </summary>
        public string Id = id == default ? Guid.NewGuid().ToString() : id;

        /// <summary>
        /// Gets all systems associated with this Entity 
        /// </summary>

        internal Entity SetOwner(Scene e) {
            Owner = e;
            return this;
        }

        internal List<GameSystem> GetAllSystems()
        {
            return Systems;
        }

        /// <summary>
        /// Retrieves the first <see cref="GameSystem"/> of the specified type <typeparamref name="T"/> from the entity's system collection.
        /// If no such system is found, it returns <see langword="null"/>.
        /// </summary>
        /// <typeparam name="T">The type of system to retrieve, must be a subtype of <see cref="GameSystem"/>.</typeparam>
        /// <returns>An instance of type <typeparamref name="T"/> if a matching system is found, or <see langword="null"/> if no system of the specified type exists.</returns>
        public T? GetSystem<T>() where T : GameSystem
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

        /// <summary>
        /// Adds a new <see cref="GameSystem"/> to the entity's collection of systems.
        /// </summary>
        /// <param name="system">The <see cref="GameSystem"/> to add.</param>
        /// <returns>The current <see cref="Entity"/> instance, allowing for method chaining.</returns>
        public Entity AddSystem(GameSystem system)
        {
            system.SetOwner(this);
            Systems.Add(system);
            return this;
        }

        /// <summary>
        /// Removes all systems of the specified type <typeparamref name="T"/> from the entity's system collection.
        /// The method continues to remove systems until no more systems of the specified type remain.
        /// </summary>
        /// <typeparam name="T">The type of systems to remove, must be a subtype of <see cref="GameSystem"/>.</typeparam>
        /// <returns>The current <see cref="Entity"/> instance, allowing for method chaining.</returns>
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
                else
                {
                    Systems.Remove(sys);
                }
            }
            return this;
        }

        public void OnInit() {
            while (true) {
                var uninitSystems = Systems.Where(s => !s.FinishedInitializing);
                if (uninitSystems.Any())
                {
                    uninitSystems.ToList().ForEach(s => s.OnInit());
                }
                else {
                    return;
                }
            }
        }

    }
}
