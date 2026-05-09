namespace AsteroidsTutorial;

public class GameOverScene(int score) : Scene
{
    private readonly int _score = score;
    private readonly ButtonHandler _buttons = new();
    
    protected override void OnLoad()
    {
        _buttons.Add(new Button("Retry", Button_Retry));
        _buttons.Add(new Button("Exit", Button_Exit));
        _buttons.PlaceVertically(new Vector2(Game.Width / 2f, Game.Height * 0.525f),
            20f);
    }
    
    public override void Update()
    {
        if (Raylib.IsKeyPressed(KeyboardKey.Escape))
            Game.Instance.LoadScene(new TitleScene());

        _buttons.Update();
    }

    public override void Draw()
    {
        const string Title = "Game Over!";
        Raylib.DrawText(Title, Game.Width / 2 - Raylib.MeasureText(Title, 50) / 2, 
            (int)(Game.Height * 0.25f), 50, Color.White);
        
        string scoreText = $"Score: {_score}";
        Raylib.DrawText(scoreText, Game.Width / 2 - Raylib.MeasureText(scoreText, 30) / 2, 
            (int)(Game.Height * 0.365f), 30, Color.White);
        
        _buttons.Draw();
    }

    private void Button_Retry(Button _)
    {
        Game.Instance.LoadScene(new GameScene());
    }

    private void Button_Exit(Button _)
    {
        Game.Instance.LoadScene(new TitleScene());
    }
}