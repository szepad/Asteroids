namespace Asteroids;

public abstract class Entity(World world, Vector2 position)
{
    public World World { get; } = world;
    
    public Vector2 Position = position;
    public Vector2 Velocity = Vector2.Zero;

    public float HitboxRadius { get; set; }
    public Texture2D Sprite { get; set; }
    public float Rotation { get; set; }
    public float Scale { get; set; } = 1f;
    public Color Tint { get; set; } = Color.White;
    public bool UseScreenWrap { get; set; } = true;

    public bool IsDestroyed { get; private set; }
    
    public void Destroy()
    {
        IsDestroyed = true;
    }
    
    public virtual void Update()
    {
        Position += Velocity * Time.DeltaTime;
        if (UseScreenWrap)
        {
            HandleScreenWrap();
        }

        HandleCollisions();
    }

    private void HandleScreenWrap()
    {
        Vector2 size = Sprite.Dimensions * Scale;
        
        if (Position.X + size.X / 2f < 0f) Position.X = Game.Width + size.X / 2f;
        else if (Position.X - size.X / 2f > Game.Width) Position.X = -size.X / 2f;
        
        if (Position.Y + size.Y / 2f < 0f) Position.Y = Game.Height + size.Y / 2f;
        else if (Position.Y - size.Y / 2f > Game.Height) Position.Y = -size.Y / 2f;
    }

    private void HandleCollisions()
    {
        for (int i = World.Entities.Count - 1; i >= 0; i--)
        {
            var other = World.Entities[i];
            if (other == this)
                continue;

            float distance = Vector2.Distance(Position, other.Position);
            if (distance <= HitboxRadius + other.HitboxRadius)
            {
                OnEntityOverlap(other);
            }
        }
    }

    public virtual void Draw()
    {
        Raylib.DrawTexturePro(Sprite,
            new Rectangle(Vector2.Zero, Sprite.Dimensions),
            new Rectangle(Position, Sprite.Dimensions * Scale),
            Sprite.Dimensions * Scale / 2f, Rotation / MathF.PI * 180f, Tint);
        
        // Raylib.DrawCircleV(Position, HitboxRadius, Color.Red.Alpha(0.5f));
    }

    protected virtual void OnEntityOverlap(Entity other)
    {
        
    }
}