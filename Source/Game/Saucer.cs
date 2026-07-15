namespace Asteroids;

public class Saucer : Entity
{
     private Vector2 _targetPosition;
     private readonly Ship _ship;
     
     private const float MoveSpeed = 130f;
     private const float ShootTime = 0.5f;
     private float _shootTimer = ShootTime;
     
     public Saucer(World world, Ship ship, Vector2 position) 
          : base(world, position)
     {
          _ship = ship;
          _ship.OnDestroy += _ => CalculateTarget();
          
          Sprite = Assets.GetTex("saucer");
          Scale = 0.085f;
          HitboxRadius = 28f;
          UseScreenWrap = false;
     }

     public void Hit()
     {
          World.GetScore(1000);
          World.SpawnEffect(EffectData.ShipExplosion, Position);
          SoundManager.PlaySound(SoundName.ExplosionBig);
          Destroy();
     }
     
     private void CalculateTarget()
     {
          if (_ship.IsDestroyed)
          {
               _targetPosition = new Vector2(Game.Width / 2f, -Game.Height);
               return;
          }
          
          _targetPosition = new Vector2(
               Position.X < Game.Width / 2f ? Game.Width - HitboxRadius * 2f : HitboxRadius * 2f,
               MathF.Max(HitboxRadius * 2f, _ship.Position.Y - Helper.RandomFloat(120f, 200f)));
     }

     public override void Update()
     {
          Velocity = Vector2.Normalize(_targetPosition - Position) * MoveSpeed;
          if (Vector2.DistanceSquared(Position, _targetPosition) <= 10f)
          {
               CalculateTarget();
          }

          _shootTimer -= Time.DeltaTime;
          if (_shootTimer <= 0f)
          {
               _shootTimer = ShootTime;
               Shoot();
          }

          if (Position.Y <= -HitboxRadius * 2f)
               Destroy();
          
          base.Update();
     }

     private void Shoot()
     {
          if (_ship.IsDestroyed || _ship.Position.Y < Position.Y + HitboxRadius * 2f)
               return;
          
          if (_ship.Position.Y < Position.Y + HitboxRadius * 4f)
               return;

          World.SpawnEntity(new SaucerBomb(World, _ship, Position));
     }
}