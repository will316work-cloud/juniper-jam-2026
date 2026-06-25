using System.Collections.Generic;
using UnityEngine;

public class BootStrapper : MonoBehaviour
{
    public StateType StartingState;

    [Header("System references")]
    public GameContext GameContext;

    [Space(10)]
    [Header("Dependency references")]
    public MoneyControllerData MoneyControllerData;
    public UiManagerContext UiManagerContext;
    
    [Space(10)]
    [Header("Player references")]
    public Rigidbody playerRigidbody;
    public GameObject playerCollisionObject;

    [Space(10)]
    [Header("Starting Settings")]
    public StartingSettings Settings;

    void Start()
    {
        Initialize();
    }

    void Initialize()
    {   

        GameContext.SettingsData = new(Settings);
        GameContext.PoolManager.Initialize(GameContext);
        GameContext.TaskManager.Initialize(GameContext);
        GameContext.MoneyController.Initialize(MoneyControllerData);
        GameContext.GameInput.Initialize();
        GameContext.PlayerControl.Instantiate(GameContext, playerRigidbody, playerCollisionObject);
        GameContext.GameStateController.Initialize(GameContext);
        GameContext.PlayerInteractor.Initialize(GameContext.GameInput);
        GameContext.WorldHealthMeter.Initialize(GameContext);
        GameContext.DayTimeController.Initialize(GameContext);
        GameContext.CoworkerManager.Initialize(GameContext);
        GameContext.TitleCoworkerManager.Initialize(GameContext);
        GameContext.BatteryDropoff.Initialize(GameContext);
        GameContext.Battery.Initialize(GameContext);
        GameContext.TransitionController.Initialize(GameContext);
        GameContext.DifficultyManager.Initialize(GameContext);
        GameContext.CameraController.Initialize();
        GameContext.UiManager.Initialize(GameContext, UiManagerContext);

        GameContext.GameStateController.ChangeState(StartingState);
        GameContext.DifficultyManager.SetDifficulty(1);

        if (StartingState != StateType.MainMenu)
            GameContext.PoolManager.FadeInGameplayMusic();
    }
}