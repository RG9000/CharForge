/// <summary>
/// Helper class for keeping FPS on track in simple scenes.
/// This code was generated via LLM.
/// </summary>
public class FPSTracker
{
    private int frameCount = 0;
    private double totalFrameTime = 0;
    private double realFPS = 0;
    private DateTime lastFPSUpdateTime = DateTime.Now;

    /// <summary>
    /// Records the time taken for a frame and calculates the real FPS.
    /// </summary>
    /// <param name="elapsedMilliseconds">Time taken for the current frame in milliseconds.</param>
    /// <returns>The current real FPS.</returns>
    public double TrackFrame(double elapsedMilliseconds)
    {
        // Increment frame count and add frame time to the total
        frameCount++;
        totalFrameTime += elapsedMilliseconds;

        // Update FPS every second
        if ((DateTime.Now - lastFPSUpdateTime).TotalMilliseconds >= 1000)
        {
            realFPS = frameCount / (totalFrameTime / 1000);
            frameCount = 0;                                // Reset frame count
            totalFrameTime = 0;                            // Reset total frame time
            lastFPSUpdateTime = DateTime.Now;              // Reset timer
        }

        return realFPS;
    }
}
