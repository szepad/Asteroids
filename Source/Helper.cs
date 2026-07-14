namespace Asteroids;

public static class Helper
{
    private static readonly Random Rng = new();

    public static Vector2 Center => new Vector2(Game.Width, Game.Height) / 2f;
    
    public static int RandomInt(int min, int max) =>
        Rng.Next(min, max);

    public static float RandomFloat(float min, float max, float precision = 1000f) =>
        Rng.Next((int)(min * precision), (int)(max * precision) + 1) / precision;

    public static Color Alpha(this Color color, float alpha)
        => color with { A = (byte)(alpha * 255f) };

    extension(float val)
    {
        public float Clamp(float min, float max)
        {
            if (val < min) val = min;
            else if (val > max) val = max;
            return val;
        }

        public float Clamp01() => val.Clamp(0f, 1f);
    }
}