using System.Collections;

public class PlayerTaskState : GameState
{
    public override IEnumerator OnEnter()
    {
        _ctx.UiManager.IngameMenuHandler.SetCanOpenInGameMenueState(false);
        CursorHandler.SetCursorVisible(true);
        _ctx.CameraController.SetIsDepthOfFieldEnabled(true);
        _ctx.TaskManager.BeginTask();
        yield return null;
    }

    public override IEnumerator OnExit()
    {
        CursorHandler.SetCursorVisible(false);
        _ctx.CameraController.SetIsDepthOfFieldEnabled(false);
        _ctx.TaskManager.OnTaskEnd();
        _ctx.PlayerControl.ActivateStunProtection();
        yield return null;
    }

    public override void Tick()
    {
    }
}
