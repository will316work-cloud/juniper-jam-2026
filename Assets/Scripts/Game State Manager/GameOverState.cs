using System.Collections;

public class GameOverState : GameState
{
    public override IEnumerator OnEnter()
    {
        _ctx.UiManager.GameOverUiHandler.SetPanelState(true);
        yield return null;
    }

    public override IEnumerator OnExit()
    {
        _ctx.UiManager.GameOverUiHandler.SetPanelState(false);
        yield return null;
    }

    public override void Tick()
    {
    }
}