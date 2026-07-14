namespace Asteroids;

public record EffectData(
    Texture2D Texture, float TextureScale, 
    int MinCount, int MaxCount, 
    float MinSpeed, float MaxSpeed, 
    float LifeTime, Color[] Colors)
{
    public static readonly EffectData Asteroid = new(
        Assets.GetTex("circle"), 0.25f,
        20, 30, 64f, 128f, 0.25f, [new Color(0, 0, 255)]);
    
    public static readonly EffectData Bullet = new(
        Assets.GetTex("circle"), 0.15f,
        10, 15, 32f, 80f, 0.25f, [new Color(0, 255, 0)]);
    
    public static readonly EffectData ShipExplosion = new(
        Assets.GetTex("star"), 2f,
        15, 25, 128f, 196f, 0.5f, [Color.Red, Color.Orange, Color.Yellow]);
}

public record EffectEntity(Vector2 Position, Vector2 Dir, float Speed, Color Color)
{
    public Vector2 Position = Position;
    public float Scale { get; set; } = 1f;
}

public class Effect
{
    public EffectData Data { get; }
    public float LifeTimer { get; private set; }
    private readonly List<EffectEntity> _effects = [];
    
    public Effect(EffectData data, Vector2 pos)
    {
        Data = data;
        LifeTimer = data.LifeTime;

        int count = Helper.RandomInt(data.MinCount, data.MaxCount);
        Vector2 dir = Vector2.UnitX;
        float angleInc = MathF.PI * 2f / count;
        for (int i = 0; i < count; i++)
        {
            _effects.Add(new EffectEntity(pos, dir, 
                Helper.RandomFloat(data.MinSpeed, data.MaxSpeed),
                Data.Colors[Helper.RandomInt(0, Data.Colors.Length)]));
            
            dir = Raymath.Vector2Rotate(dir, angleInc);
        }
    }
    
    public void Update()
    {
        LifeTimer -= Time.DeltaTime;

        foreach (EffectEntity effect in _effects)
        {
            effect.Position += effect.Dir * effect.Speed * Time.DeltaTime;
            effect.Scale = Raymath.Lerp(0f, 1f, (LifeTimer / Data.LifeTime).Clamp01());
        }
    }

    public void Draw()
    {
        foreach (EffectEntity effect in _effects)
            Raylib.DrawTexturePro(Data.Texture, new Rectangle(Vector2.Zero, Data.Texture.Dimensions),
                new Rectangle(effect.Position, Data.Texture.Dimensions * Data.TextureScale * effect.Scale),
                Data.Texture.Dimensions / 2f * Data.TextureScale * effect.Scale, 0f, effect.Color);
    }
}