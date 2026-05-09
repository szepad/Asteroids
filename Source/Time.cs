namespace AsteroidsTutorial;

public static class Time
{
    public static float DeltaTime { get; private set; }

    public static void Progress()
    {
        DeltaTime = Raylib.GetFrameTime();
    }
}