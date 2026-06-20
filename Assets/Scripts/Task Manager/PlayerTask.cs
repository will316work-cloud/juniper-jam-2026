public abstract class PlayerTask
{
    protected GameContext _ctx;
    public void Initialize(GameContext ctx) => _ctx = ctx;

    public string TaskName { get; protected set; }
    public string TaskDescription { get; protected set; }

    public abstract void AnnounceTask();
    public abstract void StartTask();
    public abstract void OnTaskFail();
    public abstract void OnTaskSuccess();
    public abstract void Tick();
}