using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateController : MonoBehaviour
{
    public bool IsDebugOn;

    GameContext _ctx;
    Dictionary<StateType, GameState> _states = new();
    GameState _currentState; public GameState CurrentState => _currentState;    
    GameState _previousState;
    public LooseReason LooseReason = LooseReason.None;

    public bool IsPlayerDead = true;
    public void Initialize(GameContext ctx)
    {
        _ctx = ctx;

        CreateDictionary();
        InitializeStates(_ctx);
    }
    void CreateDictionary()
    {
        _states.Add(StateType.Gameplay, new GameplayState());
        _states.Add(StateType.PlayerTask, new PlayerTaskState());
        _states.Add(StateType.GameOver, new GameOverState());
        _states.Add(StateType.DayChange, new DayChangeState());
        _states.Add(StateType.MainMenu, new MainMenuState());
    }
    void InitializeStates(GameContext ctx)
    {
        foreach (GameState state in _states.Values) state.Initialize(ctx);
    }

    void Update()
    {
        _currentState?.Tick();
    }

    public void ChangeState(StateType stateType, bool hasTransition = false, string transitionText = "")
    {
        StartCoroutine(ChangeStateRoutine(stateType, hasTransition, transitionText));
        if(IsDebugOn) Debug.Log($"Change state to [{stateType}]");
    }


    IEnumerator ChangeStateRoutine(StateType stateType, bool hasTransition = false, string transitionText = "")
    {
        if(_currentState != null)
        {
            // if(hasTransition)
            //     yield return StartCoroutine(_ctx.TransitionController.TransitionFadeIn(transitionText));

            yield return StartCoroutine(_currentState.OnExit());
        }

        _previousState = _currentState;
        _currentState = _states[stateType];

        if(_currentState != null)
        {   
            yield return StartCoroutine(_currentState.OnEnter());
        
            // if(hasTransition)
            // {
            //     yield return new WaitForSeconds(3.5f);
            //     yield return StartCoroutine(_ctx.TransitionController.TransitionFadeOut());
            // }
                
        }
    }
}
