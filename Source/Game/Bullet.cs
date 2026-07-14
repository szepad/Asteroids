namespace Asteroids;

public class Bullet : Entity
{
    private const float ShootSpeed = 750f;
    private float _lifeTimer = 0.5f;
    
    public Bullet(World world, Vector2 pos, Vector2 dir)
        : base(world, pos)
    {
        Sprite = Assets.GetTex("bullet");
        Velocity = dir * ShootSpeed;
        Rotation = MathF.Atan2(dir.Y, dir.X) + MathF.PI / 2f;
        Tint = Color.Green;
        HitboxRadius = 8f;
    }

    public override void Update()
    {
        _lifeTimer -= Time.DeltaTime;
        if (_lifeTimer <= 0f)
            Destroy();
            
        base.Update();
    }

    protected override void OnEntityOverlap(Entity other)
    {
        if (other is Asteroid asteroid)
        {
            asteroid.Hit();
            Destroy();
            World.SpawnEffect(EffectData.Bullet, Position);
        }
    }
}