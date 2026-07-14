namespace Asteroids;

public class World
{
    public IReadOnlyList<Entity> Entities => _entities;
    private readonly List<Entity> _entities = [];
    private readonly List<Effect> _effects = [];

    private int _score;
    private int _livesLeft = 3;
    private int _nextAsteroidCount = 4;

    private const float RespawnTime = 2f;
    private float _respawnTimer;
    
    public World()
    {
        _entities.Add(new Ship(this, Game.Center)); 
        StartWave();
    }

    public T SpawnEntity<T>(T entity) where T : Entity
    {
        _entities.Add(entity);
        return entity;
    }

    public void LoseLife()
    {
        _livesLeft--;
        _respawnTimer = RespawnTime;
    }

    public void GetScore(int score)
    {
        _score += score;
    }

    public void SpawnEffect(EffectData data, Vector2 pos)
    {
        _effects.Add(new Effect(data, pos));
    }
    
    public void Update()
    {
        HandleEntities();
        HandleRespawning();
        HandleEffects();
    }

    private void HandleEntities()
    {
        for (int i = _entities.Count - 1; i >= 0; i--)
        {
            _entities[i].Update();
        }

        _entities.RemoveAll(e => e.IsDestroyed);
        if (!_entities.Any(e => e is Asteroid))
        {
            SoundManager.PlaySound(SoundName.ExtraShip);
            _livesLeft++;
            StartWave();
        }
    }

    private void HandleRespawning()
    {
        if (_respawnTimer <= 0f)
            return;
        
        _respawnTimer -= Time.DeltaTime;
        if (_respawnTimer <= 0f)
        {
            if (_livesLeft > 0)
            {
                Ship ship = SpawnEntity(new Ship(this, Game.Center));
                ship.SetInvincible();
            }
            else
            {
                // Game Over
                Game.Instance.LoadScene(new GameOverScene(_score));
            }
        }
    }

    private void HandleEffects()
    {
        _effects.ForEach(e => e.Update());
        _effects.RemoveAll(e => e.LifeTimer <= 0f);
    }

    public void Draw()
    {
        _entities.ForEach(e => e.Draw());
        _effects.ForEach(e => e.Draw());
        
        DrawHud();
    }

    private void DrawHud()
    {
        Raylib.DrawText(_score.ToString(), 10, 10, 30, Color.White);
        
        Texture2D shipTex = Assets.GetTex("ship");
        const float Scale = 0.05f;
        for (int i = 0; i < _livesLeft; i++)
        {
            Raylib.DrawTexturePro(shipTex, 
                new Rectangle(Vector2.Zero, shipTex.Dimensions),
                new Rectangle(20 + i * 30, 60, shipTex.Width * Scale, shipTex.Height * Scale),
                shipTex.Dimensions / 2f * Scale, -90f, Color.Yellow);
        }
    }

    private void StartWave()
    {
        for (int i = 0; i < _nextAsteroidCount; i++)
        {
            if (!TryFindPosition(out Vector2 pos))
                continue;
            
            float angle = Helper.RandomFloat(0f, MathF.PI / 2f);
            Vector2 dir = new Vector2(MathF.Cos(angle), MathF.Sin(angle));
            
            _entities.Add(new Asteroid(this, pos, AsteroidData.Big, dir));
        }
        
        _nextAsteroidCount++;
    }

    private bool TryFindPosition(out Vector2 pos)
    {
        int iterCount = 0;
        const int MaxIter = 1000;
        do
        {
            pos = new Vector2(Helper.RandomInt(0, Game.Width), Helper.RandomInt(0, Game.Height));
            if (iterCount++ > MaxIter)
                return false;
        } 
        while (IsEntityNear(pos, AsteroidData.Big.HitboxRadius * 2f));

        return true;
    }

    private bool IsEntityNear(Vector2 pos, float radius)
    {
        foreach (Entity entity in _entities)
            if (Vector2.Distance(entity.Position, pos) <= (entity is Ship ? radius * 4 : radius))
                return true;
        
        return false;
    }
}