namespace Asteroids;

public class GameScene : Scene
{
    private World _world = null!;
    
    protected override void OnLoad()
    {
        _world = new World();
    }
    
    public override void Update()
    {
        if (Raylib.IsKeyPressed(KeyboardKey.Escape))
            Game.Instance.LoadScene(new TitleScene());

        _world.Update();
    }

    public override void Draw()
    {
        _world.Draw();
    }
}