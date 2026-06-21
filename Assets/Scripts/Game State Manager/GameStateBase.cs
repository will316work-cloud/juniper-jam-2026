using System.Collections;

public abstract class GameState
{
    protected GameContext _ctx;
    public void Initialize(GameContext ctx)
    {
        _ctx = ctx;
    }

    public abstract IEnumerator OnEnter();
    public abstract IEnumerator OnExit();
    public abstract void Tick();
}