namespace Asteroids;

public static class Time
{
    public static float DeltaTime { get; private set; }
    public static float TimeScale { get; set; } = 1f;

    public static void Progress()
    {
        DeltaTime = Raylib.GetFrameTime() * TimeScale;
    }
}