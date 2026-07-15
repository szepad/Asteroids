namespace Asteroids;

public class SaucerBomb : Entity
{
    private const float Speed = 500f;
    
    public SaucerBomb(World world, Ship ship, Vector2 pos)
        : base(world, pos)
    {
        Sprite = Assets.GetTex("circle");
        Scale = 0.15f;
        Tint = Color.Red;
        UseScreenWrap = false;
        HitboxRadius = 5f;

        Velocity = Vector2.Normalize(ship.Position - pos) * Speed;
    }

    public override void Update()
    {
        if (Position.Y > Game.Height + HitboxRadius)
            Destroy();
        
        base.Update();
    }
}