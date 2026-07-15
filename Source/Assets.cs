namespace Asteroids;

public static class Assets
{
    private static readonly Dictionary<string, Texture2D> Tex = [];
    private static Texture2D _placeHolderTex;

    private static readonly Dictionary<SoundName, Sound> Sounds = [];

    private static readonly string[] TextureFormats = [".png", ".jpg", ".jpeg", ".bmp"];
    private static readonly string[] SoundFormats = [".wav", ".ogg", ".flac"];
    
    public static void Load()
    {
        _placeHolderTex = Raylib.LoadTextureFromImage(
            Raylib.GenImageColor(16, 16, Color.Pink));
            
        const string Root = "res";
        List<string> files = SearchDir(Root);

        foreach (string file in files)
        {
            string ext = Path.GetExtension(file);

            if (TextureFormats.Contains(ext)) TryLoadTex(file);
            else if (SoundFormats.Contains(ext)) TryLoadSound(file);
        }
    }

    private static void TryLoadTex(string path)
    {
        string name = Path.GetFileNameWithoutExtension(path);
        
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

    private static void TryLoadSound(string path)
    {
        string stringName = Path.GetFileNameWithoutExtension(path);
        stringName = stringName.Replace("_", "");
        if (!Enum.TryParse(stringName, true, out SoundName soundName))
            return;
        
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

    private static List<string> SearchDir(string root)
    {
        List<string> files = [];

        foreach (string entry in Directory.EnumerateFileSystemEntries(root))
        {
            if (Directory.Exists(entry))
            {
                files.AddRange(SearchDir(entry));
            }
            else
            {
                files.Add(entry);
            }
        }
        
        return files;
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