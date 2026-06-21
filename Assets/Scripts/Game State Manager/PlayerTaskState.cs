using System.Collections;

public class PlayerTaskState : GameState
{
    public override IEnumerator OnEnter()
    {
        _ctx.TaskManager.BeginTask();
        yield return null;
    }

    public override IEnumerator OnExit()
    {
        _ctx.TaskManager.OnTaskEnd();
        yield return null;
    }

    public override void Tick()
    {
    }
}
