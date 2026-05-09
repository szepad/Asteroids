namespace AsteroidsTutorial;

public static class Assets
{
    private static readonly Dictionary<string, Texture2D> Tex = [];
    private static Texture2D _placeHolderTex;

    private static readonly Dictionary<SoundName, Sound> Sounds = [];
    
    public static void Load()
    {
        _placeHolderTex = Raylib.LoadTextureFromImage(
            Raylib.GenImageColor(16, 16, Color.Pink));
            
        TryLoadTex("ship", "res/graphics/ship.png");
        TryLoadTex("asteroid", "res/graphics/asteroid.png");
        TryLoadTex("bullet", "res/graphics/bullet.png");
        TryLoadTex("circle", "res/graphics/circle.png");
        TryLoadTex("star", "res/graphics/star.png");
        
        TryLoadSound(SoundName.Shoot, "res/sounds/shoot.wav");
        TryLoadSound(SoundName.ExplosionBig, "res/sounds/explosion_big.wav");
        TryLoadSound(SoundName.ExplosionMedium, "res/sounds/explosion_medium.wav");
        TryLoadSound(SoundName.ExplosionSmall, "res/sounds/explosion_small.wav");
    }

    private static void TryLoadTex(string name, string path)
    {
        if (Tex.ContainsKey(name))
        {
            Console.WriteLine($"Texture '{name}' tried to load twice.");
            return;
        }
        
        Texture2D tex = Raylib.LoadTexture(path);
        if (Raylib.IsTextureValid(tex))
        {
            Tex[name] = tex;
        }
        else
        {
            Console.WriteLine($"Couldn't open file '{path}'");
        }
    }

    private static void TryLoadSound(SoundName soundName, string path)
    {
        if (Sounds.ContainsKey(soundName))
        {
            Console.WriteLine($"Sound '{soundName}' tried to load twice.");
            return;
        }
        
        Sound sound = Raylib.LoadSound(path);
        if (Raylib.IsSoundValid(sound))
        {
            Sounds[soundName] = sound;
        }
        else
        {
            Console.WriteLine($"Couldn't open file '{path}'");
        }
    }
    
    public static void Unload()
    {
        Raylib.UnloadTexture(_placeHolderTex);
        foreach (Texture2D tex in Tex.Values)
            Raylib.UnloadTexture(tex);
        
        foreach (Sound sound in Sounds.Values)
            Raylib.UnloadSound(sound);
    }

    public static Texture2D GetTex(string name)
        => Tex.GetValueOrDefault(name, _placeHolderTex);

    public static bool TryGetSound(SoundName soundName, out Sound sound)
        => Sounds.TryGetValue(soundName, out sound);
}