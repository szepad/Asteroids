namespace AsteroidsTutorial;

public record Button(string Label, Action<Button> OnClick)
{
    public Vector2 Position { get; set; }
    public const int RegularFontSize = 30;
    public const int HoveredFontSize = 35;
    public int FontSize => IsHovered ? HoveredFontSize : RegularFontSize;

    public Vector2 Size => new(Raylib.MeasureText(Label, FontSize), FontSize);
    public Rectangle Rect => new(Position - Size / 2f, Size);
    
    public bool IsHovered { get; private set; }

    public void Update()
    {
        bool prevHovered = IsHovered;
        IsHovered = Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), Rect);
        if (!prevHovered && IsHovered)
            SoundManager.PlaySound(SoundName.Hover);
        
        if (Raylib.IsMouseButtonReleased(MouseButton.Left)
            && IsHovered)
        {
            SoundManager.PlaySound(SoundName.Select);
            OnClick.Invoke(this);
        }
    }
    
    public void Draw()
    {
        Raylib.DrawText(Label, (int)Rect.X, (int)Rect.Y,
            FontSize, IsHovered ? Color.Yellow : Color.White);
        
        // Raylib.DrawRectangleV(Rect.Position, Rect.Size, Color.Red.Alpha(0.5f));
    }
}

public class ButtonHandler
{
    private readonly List<Button> _buttons = [];

    public void Add(Button button)
    {
        _buttons.Add(button);
    }

    public void PlaceVertically(Vector2 startPos, float gap)
    {
        Vector2 pos = startPos;
        foreach (var button in _buttons)
        {
            button.Position = pos;
            pos.Y += button.FontSize + gap;
        }
    }

    public void Update()
    {
        _buttons.ForEach(btn => btn.Update());
    }

    public void Draw()
    {
        _buttons.ForEach(btn => btn.Draw());
    }
}