namespace CharForge;

public abstract class GameSystem {

    public bool HasRun {get; private set;}

    protected abstract void OnInit();
    
    protected abstract void OnUpdate();
    
}
