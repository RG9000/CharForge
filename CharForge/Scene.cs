using System.Diagnostics;

namespace CharForge;

public class Scene(int targetFps = 20, bool showFPS = false)
{
    private readonly FPSTracker fpsTracker = new();
    private readonly int TargetFPS = targetFps;

    private bool KillScene = false;

    private readonly bool ShowFPS = showFPS;

    internal readonly List<Entity> Entities = [];

    public ConsoleKey LastKeyPressed { get; private set; }
    public ConsoleKey CurrentKeyPressed { get; private set; }
    private ConsoleKey? _keyBuffer;

    public float DeltaTime { get; private set; }

    public async Task Activate()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.ForegroundColor = ConsoleColor.White;

        Entities.ForEach(e => e.OnInit());
        _ = Task.Run(RenderLoop);
        await GameLoop();
    }

    private async Task GameLoop()
    {
        long previousElapsedTime = 0;
        Stopwatch stopwatch = new();
        stopwatch.Start();

        while (true)
        {
            long currentElapsedTime = stopwatch.ElapsedMilliseconds;
            long delta = currentElapsedTime - previousElapsedTime;
            DeltaTime = delta/1000f;
            previousElapsedTime = currentElapsedTime;

            if (KillScene)
            {
                return;
            }

            CurrentKeyPressed = _keyBuffer ?? ConsoleKey.None;
            UpdateSystems(render: false);
            _keyBuffer = null;
            CurrentKeyPressed = ConsoleKey.None;

            await Task.Delay(50);
        }
    }

    private async Task RenderLoop()
    {
        try
        {
            int delayPerFrame = 1000 / TargetFPS;

            while (true)
            {
                if (Console.KeyAvailable) // Check if a key has been pressed
                {
                    var key = Console.ReadKey(intercept: true);
                    LastKeyPressed = key.Key;
                    _keyBuffer = key.Key; // Store the input in a buffer

                    if (key.KeyChar == 'q')
                    {
                        KillScene = true;
                    }
                }

                // Record the start time of the frame
                var frameStartTime = DateTime.Now;

                UpdateSystems(render: true);
                // After processing, clear the current key input for the next frame

                var elapsedMilliseconds = (DateTime.Now - frameStartTime).TotalMilliseconds;
                var remainingTime = delayPerFrame - elapsedMilliseconds;
                var realFPS = fpsTracker.TrackFrame(elapsedMilliseconds);

                if (ShowFPS)
                {
                    Console.SetCursorPosition(0, 0);
                    Console.Write($"Real FPS: {realFPS:F2}");
                }

                if (remainingTime > 0)
                {
                    await Task.Delay((int)remainingTime);
                }

                Console.Clear();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine(e.InnerException?.Message);
            KillScene = true;
        }
    }


    public Scene AddEntity(Entity e)
    {
        e.SetOwner(this);
        Entities.Add(e);
        return this;
    }

    public Scene RemoveEntity(Entity e)
    {
        Entities.Remove(e);
        return this;
    }

    public Scene RemoveEntityById(string id)
    {
        var thisEntity = Entities.FirstOrDefault(e => e.Id == id);
        if (thisEntity != null)
        {
            Entities.Remove(thisEntity);
        }
        return this;
    }

    public void UpdateSystems(bool render)
    {
        var systems = new List<GameSystem>();

        foreach (var entity in Entities)
        {
            systems.AddRange(entity.Systems);
        }

        var sortedSystems = TopologicalSort(systems);

        foreach (var system in sortedSystems)
        {
            if (render)
            {
                system.OnRender();
            }
            else
            {
                system.OnUpdate();
            }
        }
    }

    #region (LLM Generated)
    // The following code was generated via chat-gpt
    private List<GameSystem> TopologicalSort(List<GameSystem> systems)
    {
        var sorted = new List<GameSystem>();
        var visited = new HashSet<GameSystem>();  // Tracks fully visited system instances
        var inProcess = new HashSet<GameSystem>(); // Tracks system instances currently being processed

        foreach (var system in systems)
        {
            Visit(system, visited, inProcess, sorted, systems);
        }

        return sorted;
    }

    private void Visit(GameSystem system, HashSet<GameSystem> visited, HashSet<GameSystem> inProcess, List<GameSystem> sorted, List<GameSystem> allSystems)
    {
        if (visited.Contains(system))
        {
            return; // Already fully visited and processed
        }

        if (inProcess.Contains(system))
        {
            throw new Exception($"Cyclic dependency detected in system {system.GetType()} for entity {system.Owner?.Id}.");
        }

        // Mark the system as being in process
        inProcess.Add(system);

        // Visit dependencies
        foreach (var dependencyType in system.DependentTypes)
        {
            var dependency = system.Owner?.Systems.FirstOrDefault(s => s.GetType() == dependencyType);
            if (dependency != null)
            {
                Visit(dependency, visited, inProcess, sorted, allSystems);
            }
        }

        // Visit systems that must execute after this system
        foreach (var mustExecuteAfter in allSystems.Where(s => s.RunUpdateAfter.Contains(system.GetType()) && s.Owner != system.Owner))
        {
            Visit(mustExecuteAfter, visited, inProcess, sorted, allSystems);
        }

        // Mark the system as fully processed
        inProcess.Remove(system);
        visited.Add(system);

        // Add to sorted list after all dependencies are visited
        sorted.Add(system);
    }

    #endregion

}