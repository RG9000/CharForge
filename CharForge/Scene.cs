using CharForge.Systems;

namespace CharForge;

public class Scene()
{
    private readonly List<Entity> SpawningEntities = [];
    public readonly List<Entity> Entities = [];
    private bool KillScene = false;

    public ConsoleKeyInfo? CurrentPressedKey;

    public void Spawn(Entity e)
    {
        SpawningEntities.Add(e);
    }

    public Scene AddObject(GameObject obj)
    {
        obj.SetScene(this);
        Entities.Add(obj);
        return this;
    }
    

    public async Task Activate()
    {
        Console.Clear();
        _ = Task.Run(ReadKeysAsync);
        Console.OutputEncoding = System.Text.Encoding.UTF8;  // UTF-8 support
        Console.ForegroundColor = ConsoleColor.White;
        //Console.Clear();

        foreach(var o in Entities)
        {
            if (o is GameObject gameObject)
            {
                gameObject.OnInit();
            }
        }

        await GameLoop();

    }
    private async Task GameLoop()
    {
        // Target FPS (frames per second)
        int targetFPS = 60;
        
        int delayPerFrame = 1000 / targetFPS; // Desired time per frame in milliseconds

        // Variables for FPS calculation
        int frameCount = 0;
        double totalFrameTime = 0;
        double realFPS = 0;
        var lastFPSUpdateTime = DateTime.Now;
        while (true)
        {
            if (KillScene)
            {
                return;
            }

            // Record the start time of the frame
            var frameStartTime = DateTime.Now;

            // Add newly spawned entities to the world
            Entities.AddRange(SpawningEntities);
            SpawningEntities.Clear(); // Clear after adding to the world

            var allSystems = new List<GameSystem>();
            foreach (var o in Entities)
            {
                allSystems.AddRange(o.Systems);
            }

            allSystems = [.. allSystems.OrderBy(e => e.GetType().Name)];

            foreach (var s in allSystems)
            {
                s.OnUpdate();
            }

            // Calculate how much time has passed for this frame
            var elapsedMilliseconds = (DateTime.Now - frameStartTime).TotalMilliseconds;

            // Calculate remaining time for this frame
            var remainingTime = delayPerFrame - elapsedMilliseconds;

            Console.SetCursorPosition(0,0);
            Console.Write($"Real FPS: {realFPS:F2}");

            // If processing took less than the frame duration, delay for the remaining time
            if (remainingTime > 0)
            {
                await Task.Delay((int)remainingTime);
            }

            // Increment frame count and total frame time
            frameCount++;
            totalFrameTime += (DateTime.Now - frameStartTime).TotalMilliseconds;

            // Update FPS every second
            if ((DateTime.Now - lastFPSUpdateTime).TotalMilliseconds >= 1000)
            {
                realFPS = frameCount / (totalFrameTime / 1000);
                frameCount = 0;                                // Reset frame count
                totalFrameTime = 0;                            // Reset total frame time
                lastFPSUpdateTime = DateTime.Now;              // Reset timer
            }

            // Optionally, clear the console to update the screen per frame
            CurrentPressedKey = null;
            Console.Clear();
        }
    }


    private async Task ReadKeysAsync()
    {
        while (true)
        {
            if (Console.KeyAvailable) // Check if a key has been pressed
            {
                var key = Console.ReadKey(intercept: true); // Read the key without showing it
                CurrentPressedKey = key;
                if (key.KeyChar == 'q'){
                    KillScene = true;
                }
            }

            // Small delay to avoid high CPU usage in the loop
            await Task.Delay(50); // Adjust this delay as needed
        }
    }

}
