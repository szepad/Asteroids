namespace AsteroidsTutorial;

public class Ship : Entity
{
    public Vector2 Dir => Raymath.Vector2Normalize(
        new Vector2(MathF.Cos(Rotation), MathF.Sin(Rotation)));
    
    private const float ThrustForce = 500f;
    private const float MaxSpeed = 1500f;
    private const float DragFactor = 0.1608f;

    private const float ShootCooldown = 0.25f;
    private float _shootCooldownTimer;

    private const float InvincibleTime = 3f;
    private float _invincibleTimer = 0f;

    private bool _isThrusting;
    private float _thrustTimer;
    
    public Ship(World world, Vector2 pos) 
        : base(world, pos)
    {
        Sprite = Assets.GetTex("ship");
        Scale = 0.075f;
        Tint = Color.Yellow;
        HitboxRadius = 16f;
    }

    public void SetInvincible()
    {
        _invincibleTimer = InvincibleTime;
    }
    
    public override void Update()
    {
        _invincibleTimer -= Time.DeltaTime;
        
        HandleMovement();
        HandleShooting();
        
        base.Update();
    }

    private void HandleMovement()
    {
        Vector2 dirToMouse = Raymath.Vector2Normalize(Raylib.GetMousePosition() - Position);
        Rotation = MathF.Atan2(dirToMouse.Y, dirToMouse.X);
        
        if (Raylib.IsKeyDown(KeyboardKey.W) && Velocity.Length() < MaxSpeed)
        {
            Velocity += Dir * ThrustForce * Time.DeltaTime;
            _isThrusting = true;
            _thrustTimer += Time.DeltaTime;
        }
        else
        {
            Velocity *= MathF.Pow(DragFactor, Time.DeltaTime);
            _isThrusting = false;
            _thrustTimer = 0f;
        }
    }
    
    private void HandleShooting()
    {
        _shootCooldownTimer -= Time.DeltaTime;

        if (Raylib.IsMouseButtonPressed(MouseButton.Left))
            _shootCooldownTimer = 0f;
        
        if (Raylib.IsMouseButtonDown(MouseButton.Left) && _shootCooldownTimer <= 0f)
        {
            _shootCooldownTimer = ShootCooldown;
            Vector2 pos = Position + Dir * HitboxRadius;
            World.SpawnEntity(new Bullet(World, pos, Dir));
            SoundManager.PlaySound(SoundName.Shoot, false);
        }
    }

    protected override void OnEntityOverlap(Entity other)
    {
        if (other is Asteroid asteroid && _invincibleTimer <= 0f)
        {
            asteroid.Hit();
            Destroy();
            World.LoseLife();
            World.SpawnEffect(EffectData.ShipExplosion, Position);
        }
    }

    public override void Draw()
    {
        if (_invincibleTimer > 0f)
        {
            if (_invincibleTimer % 0.2f > 0.1f)
                DoDraw();
        }
        else
        {
            DoDraw();
        }
    }

    private void DoDraw()
    {
        base.Draw();
        if (_isThrusting && _thrustTimer % 0.2f <= 0.1f)
        {
            Vector2 size = Sprite.Dimensions * Scale * 0.65f;
            Vector2 pos = Position - Dir * 30f;
            Raylib.DrawTexturePro(Sprite, new Rectangle(Vector2.Zero, Sprite.Dimensions),
                new Rectangle(pos, size), size / 2f,
                (Rotation + MathF.PI) / MathF.PI * 180f, Color.Orange);
        }
    }
}