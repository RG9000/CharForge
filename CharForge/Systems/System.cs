namespace CharForge.Systems;

public abstract class GameSystem()
{
    public GameObject? Owner { get; private set; }

    public abstract void OnInit();
    public abstract void OnUpdate();

    public void SetOwner(GameObject owner)
    {
        Owner = owner;
    }

}
