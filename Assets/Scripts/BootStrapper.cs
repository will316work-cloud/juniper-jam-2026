using System.Collections.Generic;
using UnityEngine;

public class BootStrapper : MonoBehaviour
{
    [Header("System references")]
    public GameContext GameContext;

    [Space]
    [Header("Dependency references")]
    public MoneyControllerData MoneyControllerData;
    public List<TaskTriggerEntry> TaskTriggerObjects;

    void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        GameContext.TaskManager.Initialize(GameContext,TaskTriggerObjects);
        GameContext.MoneyController.Initialize(MoneyControllerData);
        GameContext.PrinterCrank.Initialize(GameContext);
    }
}