public abstract class PlayerTask
{
    protected GameContext _ctx;

    public void Initialize(GameContext ctx) => _ctx = ctx;

    public abstract void AnnounceTask();
    public abstract void StartTask();
    public abstract void OnTaskFail();
    public abstract void OnTaskSuccess();
}