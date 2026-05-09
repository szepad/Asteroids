namespace AsteroidsTutorial;

public class Game
{
    public const int Width = 800;
    public const int Height = 600;
    private const string Title = "Asteroids";
    
    public static Vector2 Center => new Vector2(Width, Height) / 2f;
    
    private Scene? _currentScene;

    private bool _exited;
    
    public static Game Instance => _instance ??= new Game();
    private static Game? _instance;
    private Game(){}

    public void Run()
    {
        Init();
        Load();

        while (!Raylib.WindowShouldClose() && !_exited)
        {
            Update();
            Draw();
        }

        Unload();
    }

    public void Exit()
    {
        _exited = true;
    }

    public void LoadScene(Scene? scene)
    {
        _currentScene?.Unload();
        _currentScene = scene;
        _currentScene?.Load();
    }
    
    private void Init()
    {
        Raylib.SetTraceLogLevel(TraceLogLevel.Warning);
        Raylib.InitAudioDevice();
        Raylib.InitWindow(Width, Height, Title);
        Raylib.SetExitKey(KeyboardKey.Null);
        Raylib.SetTargetFPS(144);
    }

    private void Load()
    {
        Assets.Load();
        LoadScene(new GameScene());
    }

    private void Unload()
    {
        Assets.Unload();
        Raylib.CloseWindow();
        Raylib.CloseAudioDevice();
    }

    private void Update()
    {
        Time.Progress();
        SoundManager.Update();
        _currentScene?.Update();
    }

    private void Draw()
    {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.Black);
        _currentScene?.Draw();
        Raylib.EndDrawing();
    }
}