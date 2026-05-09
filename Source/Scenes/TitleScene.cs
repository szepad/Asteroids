namespace AsteroidsTutorial;

public class TitleScene : Scene
{
    private readonly ButtonHandler _buttons = new();
    
    protected override void OnLoad()
    {
        _buttons.Add(new Button("Play", Button_Play));
        _buttons.Add(new Button("Exit", Button_Exit));
        _buttons.PlaceVertically(new Vector2(Game.Width / 2f, Game.Height * 0.45f),
            20f);
    }
    
    public override void Update()
    {
        if (Raylib.IsKeyPressed(KeyboardKey.Escape))
            Game.Instance.Exit();

        _buttons.Update();
    }

    public override void Draw()
    {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.Black);

        const string Title = "Asteroids";
        Raylib.DrawText(Title, Game.Width / 2 - Raylib.MeasureText(Title, 50) / 2, 
            (int)(Game.Height * 0.25f), 50, Color.White);
        
        _buttons.Draw();
        
        Raylib.EndDrawing();
    }

    private void Button_Play(Button _)
    {
        Game.Instance.LoadScene(new GameScene());
    }

    private void Button_Exit(Button _)
    {
        Game.Instance.Exit();
    }
}