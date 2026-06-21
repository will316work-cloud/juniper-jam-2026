using System.Collections.Generic;
using UnityEngine;

public class BootStrapper : MonoBehaviour
{
    [Header("System references")]
    public GameContext GameContext;

    [Space(10)]
    [Header("Dependency references")]
    public MoneyControllerData MoneyControllerData;
    public List<TaskTriggerEntry> TaskTriggerObjects;
    public UiManagerContext UiManagerContext;
    
    [Space(10)]
    [Header("Player references")]
    public Rigidbody playerRigidbody;
    public GameObject playerCollisionObject;

    void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        GameContext.TaskManager.Initialize(GameContext,TaskTriggerObjects);
        GameContext.MoneyController.Initialize(MoneyControllerData);
        GameContext.PrinterCrank.Initialize(GameContext);
        GameContext.GameInput.Initialize();
        GameContext.PlayerControl.Instantiate(playerRigidbody, GameContext.GameInput, playerCollisionObject);
        GameContext.UiManager.Initialize(GameContext, UiManagerContext);
    }
}