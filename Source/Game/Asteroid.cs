namespace Asteroids;

public enum AsteroidSize
{
    Big,
    Medium,
    Small
}

public record AsteroidData(
    AsteroidSize Size, float Scale, float HitboxRadius, float Speed, int Score)
{
    public static readonly AsteroidData Big = new(
        AsteroidSize.Big, 0.15f, 40f, 110f, 100);
    
    public static readonly AsteroidData Medium = new(
        AsteroidSize.Medium, 0.11f, 28f, 150f, 200);
    
    public static readonly AsteroidData Small = new(
        AsteroidSize.Small, 0.075f, 20f, 200f, 500);
}

public class Asteroid : Entity
{
    public AsteroidData Data { get; }
    public Vector2 Dir { get; }
    
    public Asteroid(World world, Vector2 pos, AsteroidData data, Vector2 dir)
        : base(world, pos)
    {
        Data = data;
        Dir = dir;
        Sprite = Assets.GetTex("asteroid");
        Scale = data.Scale;
        HitboxRadius = data.HitboxRadius;
        Rotation = Helper.RandomFloat(0f, MathF.PI * 2f);
        
        Velocity = dir * Data.Speed;
    }

    public void Hit()
    {
        Destroy();
        World.GetScore(Data.Score);
        World.SpawnEffect(EffectData.Asteroid, Position);
        SoundManager.PlaySound(Data.Size switch
        {
            AsteroidSize.Small => SoundName.ExplosionSmall,
            AsteroidSize.Medium => SoundName.ExplosionMedium,
            _ => SoundName.ExplosionBig
        });
        
        if (Data.Size == AsteroidSize.Small)
            return;

        AsteroidData data = Data.Size == AsteroidSize.Big ? AsteroidData.Medium : AsteroidData.Small;
        Vector2 dir = Raymath.Vector2Rotate(Vector2.UnitX, Helper.RandomFloat(0f, MathF.PI * 2f));
        World.SpawnEntity(new Asteroid(World, Position, data, dir));
        World.SpawnEntity(new Asteroid(World, Position, data, Raymath.Vector2Rotate(dir, MathF.PI)));
    }
}