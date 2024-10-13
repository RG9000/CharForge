using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;

namespace CharForge;

public class Scene(int targetFps = 20, bool showFPS = false)
{
    private readonly FPSTracker fpsTracker = new();
    private readonly int TargetFPS = targetFps;

    private bool KillScene = false;

    private readonly bool ShowFPS = showFPS;

    private readonly List<Entity> Entities = [];

    public ConsoleKey LastKeyPressed { get; private set; }
    public ConsoleKey CurrentKeyPressed { get; private set; }
    private ConsoleKey? _keyBuffer;

    public async Task Activate()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.ForegroundColor = ConsoleColor.White;

        Entities.ForEach(e => e.OnInit());
        _ = Task.Run(ReadKeysAsync);
        await GameLoop();
    }
    private async Task GameLoop()
    {
        try {
        int delayPerFrame = 1000 / TargetFPS;

        while (true)
        {
            if (KillScene)
            {
                return;
            }

            // Record the start time of the frame
            var frameStartTime = DateTime.Now;

            // Capture the buffered input and store it in CurrentKeyPressed
            CurrentKeyPressed = _keyBuffer ?? ConsoleKey.None;

            // Gather all systems from entities and order them by name
            var allSystems = new List<GameSystem>();
            foreach (var o in Entities)
            {
                allSystems.AddRange(o.GetAllSystems());
            }

            allSystems = allSystems.OrderBy(e => e.GetType().Name).ToList();

            // Update each system with the current input
            foreach (var s in allSystems)
            {
                s.OnUpdate();
            }

            // After processing, clear the current key input for the next frame
            _keyBuffer = null;
            CurrentKeyPressed = ConsoleKey.None;

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

            // Clear screen, but do it after input is processed
            Console.Clear();
        }
        } catch(Exception e)
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

    private async Task ReadKeysAsync()
    {
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

            // Small delay to avoid high CPU usage in the loop
            await Task.Delay(50); // Adjust this delay as needed
        }
    }

}