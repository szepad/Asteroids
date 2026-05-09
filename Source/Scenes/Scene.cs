namespace AsteroidsTutorial;

public abstract class Scene
{
    private bool _isLoaded;
    
    public void Load()
    {
        if (_isLoaded) return;
        _isLoaded = true;

        OnLoad();
    }

    public void Unload()
    {
        if (!_isLoaded) return;
        _isLoaded = false;

        OnUnload();
    }
    
    protected abstract void OnLoad();
    protected virtual void OnUnload(){}
    
    public abstract void Update();
    public abstract void Draw();
}