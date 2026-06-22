using System.Collections;

public class DayChangeState : GameState
{
    public override IEnumerator OnEnter()
    {
        // start transition and get to dark
        _ctx.WorldHealthMeter.SetTimerState(false);
        _ctx.DayTimeController.SetIsTimerOn(false);
        yield return _ctx.TransitionController.TransitionFadeIn();
        _ctx.GameStateController.ChangeState(StateType.Gameplay);
        yield return null;
    }

    public override IEnumerator OnExit()
    {
        _ctx.DayTimeController.IncrementDay();
        _ctx.DayTimeController.ResetTime();
        _ctx.WorldHealthMeter.ResetHealth();
        yield return _ctx.TransitionController.TransitionFadeOut();
        // end transition
        yield return null;
    }

    public override void Tick()
    {

    }
}