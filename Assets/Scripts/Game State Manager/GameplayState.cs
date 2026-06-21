using System.Collections;

public class GameplayState : GameState
{
    public override IEnumerator OnEnter()
    {
        _ctx.PlayerControl.enabled = true;
        _ctx.UiManager.InGameUiHandler.SetPanelState(true);
        yield return null;
    }

    public override IEnumerator OnExit()
    {
        _ctx.PlayerControl.enabled = false;
        _ctx.UiManager.InGameUiHandler.SetPanelState(false);
        yield return null;
    }

    public override void Tick()
    {
        
    }
}