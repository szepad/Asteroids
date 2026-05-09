namespace AsteroidsTutorial;

public enum SoundName
{
    Thrust,
    Shoot,
    ExtraShip,
    
    ExplosionBig,
    ExplosionMedium,
    ExplosionSmall,
    Saucer,
    
    Hover,
    Select
}

public static class SoundManager
{
    private static readonly List<Sound> Aliases = []; 
    
    public static void PlaySound(SoundName soundName, bool asAlias = true)
    {
        if (Assets.TryGetSound(soundName, out Sound sound))
        {
            if (asAlias)
            {
                Sound alias = Raylib.LoadSoundAlias(sound);
                Aliases.Add(alias);
                Raylib.PlaySound(alias);
            }
            else
            {
                Raylib.PlaySound(sound);
            }
        }
        else
        {
            Console.WriteLine($"Sound '{soundName}' is not loaded.");
        }
    }

    public static void Update()
    {
        for (int i = Aliases.Count - 1; i >= 0; i--)
        {
            Sound alias = Aliases[i];
            if (!Raylib.IsSoundPlaying(alias))
            {
                Raylib.UnloadSoundAlias(alias);
                Aliases.RemoveAt(i);
            }
        }
    }
}