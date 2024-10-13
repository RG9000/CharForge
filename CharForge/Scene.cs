namespace CharForge;

public class Scene
{
    private readonly FPSTracker fpsTracker = new FPSTracker();
    private readonly int TargetFPS;

    private bool KillScene = false; 
    private bool ShowFPS = false; 

    private List<Entity> Entities = [];

    public Scene(int targetFps = 20, bool showFPS = false)
    {
        TargetFPS = targetFps;
        ShowFPS = showFPS;
    }

    public async Task Activate()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.ForegroundColor = ConsoleColor.White;

        Entities.ForEach(e => e.OnInit());

        await GameLoop();
    }
    
    private async Task GameLoop()
    {
        int delayPerFrame = 1000 / TargetFPS; 

        while (true)
        {
            if (KillScene)
            {
                return;
            }

            // Record the start time of the frame
            var frameStartTime = DateTime.Now;

            // Gather all systems from entities and order them by name
            var allSystems = new List<GameSystem>();
            foreach (var o in Entities)
            {
                allSystems.AddRange(o.GetAllSystems());
            }

            allSystems = [.. allSystems.OrderBy(e => e.GetType().Name)];

            // Update each system
            foreach (var s in allSystems)
            {
                s.OnUpdate();
            }

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

            //Console.Clear();
        }
    }

    public Scene AddEntity(Entity e)
    {
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


}